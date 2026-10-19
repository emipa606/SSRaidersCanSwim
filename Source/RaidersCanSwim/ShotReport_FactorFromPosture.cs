using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace Swimming;

[HarmonyPatch]
internal static class ShotReport_FactorFromPosture
{
    public static MethodInfo TargetMethod()
    {
        return typeof(ShotReport).GetProperty("FactorFromPosture", AccessTools.all)?.GetGetMethod(true);
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = instructions.ToList();
        for (var i = 0; i < list.Count; i++)
        {
            var instruction = list[i];
            if (instruction.opcode == OpCodes.Ret && list[i - 1].operand is float f &&
                f - 0.9f > 0f) //f should be 1f
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldflda, AccessTools.Field(typeof(ShotReport), "target"));
                //Since reflection doesn't work for this, I'm manually loading the private variable "target" with IL
                yield return new CodeInstruction(OpCodes.Call,
                    typeof(TargetInfo).GetProperty(nameof(TargetInfo.Thing))?.GetGetMethod());
                yield return new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(ShotReport_FactorFromPosture), nameof(Manual)));
            }

            yield return instruction;
        }
    }

    private static float Manual(float result, Thing thing)
    {
        //0.2 factor for body size when in water
        if (Graphic_Shadow_DrawWorker.SatisfiesNoShadow(thing.Position, thing))
        {
            result = 0.2f;
        }

        return result;
    }
}