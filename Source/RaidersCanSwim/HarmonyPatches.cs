using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;
using RimWorld;
using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;

namespace Swimming
{
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnInternal", new Type[] { typeof(Vector3), typeof(float), typeof(bool), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(bool), typeof(bool), typeof(bool) }), StaticConstructorOnStartup]
    static class Patch_PawnRenderer
    {
        static Patch_PawnRenderer()
        {
            var harmonyInstance = new Harmony("com.spdskatr.swimming.patches");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message(
                "SS Raiders Can Swim Initialized. Patches:\n" +
                "(Prefix non-destructive) Verse.PawnRenderer.RenderPawnInternal Overload with 7 parameters\n" +
                "(Transpiler infix injection (brtrue)): Verse.Graphic_Shadow.DrawWorker\n" +
                "(Transpiler infix injection (ldc.r4 1))Verse.ShotReport.get_FactorFromPosture\n\n");

        }
        static void Prefix(ref bool renderBody, PawnRenderer __instance)
        {
            var pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if (pawn != null
                && !pawn.Dead
                && pawn.Map != null
                && pawn.RaceProps.Humanlike
                && pawn.Position.GetTerrain(pawn.Map) != null 
                && (pawn.Position.GetTerrain(pawn.Map).label == "deep water" || pawn.Position.GetTerrain(pawn.Map) == TerrainDefOf.WaterDeep))
            {
                renderBody = false;
            }
        }
    }
    [HarmonyPatch(typeof(Graphic_Shadow), "DrawWorker")]
    static class Patch_Shadows
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = instructions.ToList();
            for (var i = 0; i < instructionsList.Count; i++)
            {
                var instruction = instructionsList[i];
                yield return instruction;
                if (instruction.opcode == OpCodes.Brtrue 
                    && instructionsList[i - 1].operand == typeof(RoofGrid).GetMethod("Roofed", new Type[] { typeof(IntVec3) })) //Identifier for which IL line to inject to
                {
                    //Start of injection
                    yield return new CodeInstruction(OpCodes.Ldarg_1);//First argument for both our method and its own
                    yield return new CodeInstruction(OpCodes.Ldarg_S, (byte)4);//Second argument for our method, fourth argument for its own: Thing thing
                    yield return new CodeInstruction(OpCodes.Call, typeof(Patch_Shadows).GetMethod("SatisfiesNoShadow"));//Injected code
                    yield return new CodeInstruction(OpCodes.Brtrue, instruction.operand);//If true, break to exactly where the original instruction went
                }
            }
        }
        public static bool SatisfiesNoShadow(IntVec3 loc, Thing thing)
        {
            try
            {
                var terrain = thing.PositionHeld.GetTerrain(thing.Map);
                return thing is Pawn
                    && (terrain == TerrainDefOf.WaterDeep
                    || terrain.label.ToLower() == TerrainDefOf.WaterDeep.label.ToLower());
            }
            //In rare cases sometimes pawn has misconfigured position or map.
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
    [HarmonyPatch]
    static class Patch_ShotReport
    {
        static MethodInfo TargetMethod()
        {
            return typeof(ShotReport).GetProperty("FactorFromPosture", AccessTools.all).GetGetMethod(true);
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var instruction = list[i];
                if (instruction.opcode == OpCodes.Ret && list[i-1].operand is float f && f - 0.9f > 0f)//f should be 1f
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldflda, AccessTools.Field(typeof(ShotReport), "target"));
                    //Since reflection doesnt work for this, I'm manually loading the private variable "target" with IL
                    yield return new CodeInstruction(OpCodes.Call, typeof(TargetInfo).GetProperty("Thing").GetGetMethod());
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_ShotReport), nameof(Manual)));
                }
                yield return instruction;
            }
        }
        static float Manual(float result, Thing thing)
        {
            //0.2 factor for body size when in water
            if (Patch_Shadows.SatisfiesNoShadow(thing.Position, thing))
            {
                result = 0.2f;
            }
            return result;
        }
    }
}
