using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Swimming;

[StaticConstructorOnStartup]
public static class RaidersCanSwim
{
    public static readonly List<TerrainDef> DeepWaterDefs = new List<TerrainDef>
    {
        TerrainDefOf.WaterDeep, TerrainDefOf.WaterOceanDeep
    };

    public static readonly List<string> DeepWaterLabels = new List<string>
    {
        "deep water", "deep ocean water"
    };

    static RaidersCanSwim()
    {
        var harmonyInstance = new Harmony("com.spdskatr.swimming.patches");
        harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        Log.Message(
            "SS Raiders Can Swim Initialized. Patches:\n" +
            "(Prefix non-destructive) Verse.PawnRenderer.RenderPawnInternal Overload with 7 parameters\n" +
            "(Transpiler infix injection (brtrue)): Verse.Graphic_Shadow.DrawWorker\n" +
            "(Transpiler infix injection (ldc.r4 1))Verse.ShotReport.get_FactorFromPosture\n\n");
    }
}