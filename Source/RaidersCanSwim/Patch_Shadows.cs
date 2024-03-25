using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace Swimming;

[HarmonyPatch(typeof(Graphic_Shadow), "DrawWorker")]
internal static class Patch_Shadows
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionsList = instructions.ToList();
        for (var i = 0; i < instructionsList.Count; i++)
        {
            var instruction = instructionsList[i];
            yield return instruction;
            if (instruction.opcode != OpCodes.Brtrue || (MethodInfo)instructionsList[i - 1].operand !=
                typeof(RoofGrid).GetMethod("Roofed", [typeof(IntVec3)]))
            {
                continue;
            }

            //Start of injection
            yield return new CodeInstruction(OpCodes.Ldarg_1); //First argument for both our method and its own
            yield return
                new CodeInstruction(OpCodes.Ldarg_S,
                    (byte)4); //Second argument for our method, fourth argument for its own: Thing
            yield return
                new CodeInstruction(OpCodes.Call,
                    typeof(Patch_Shadows).GetMethod("SatisfiesNoShadow")); //Injected code
            yield return
                new CodeInstruction(OpCodes.Brtrue,
                    instruction.operand); //If true, break to exactly where the original instruction went
        }
    }

    public static bool SatisfiesNoShadow(IntVec3 loc, Thing thing)
    {
        try
        {
            var terrain = thing.PositionHeld.GetTerrain(thing.Map);
            return thing is Pawn
                   && (RaidersCanSwim.DeepWaterDefs.Contains(terrain)
                       || RaidersCanSwim.DeepWaterLabels.Contains(terrain.label.ToLower()));
        }
        //In rare cases sometimes pawn has misconfigured position or map.
        catch
        {
            return false;
        }
    }
}