using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Swimming;

[StaticConstructorOnStartup]
public static class RaidersCanSwim
{
    public static readonly List<TerrainDef> DeepWaterDefs =
        [TerrainDefOf.WaterDeep, TerrainDefOf.WaterOceanDeep, TerrainDefOf.ToxicWaterDeep];

    public static readonly List<string> DeepWaterLabels = ["deep water", "deep ocean water"];

    static RaidersCanSwim()
    {
        new Harmony("com.spdskatr.swimming.patches").PatchAll(Assembly.GetExecutingAssembly());
    }
}