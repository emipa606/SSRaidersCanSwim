using HarmonyLib;
using Verse;

namespace Swimming;

[HarmonyPatch(typeof(PawnRenderNodeWorker_Body), "CanDrawNow")]
internal static class Patch_PawnRenderNodeWorker_Body
{
    private static void Postfix(PawnDrawParms parms, ref bool __result)
    {
        if (!__result)
        {
            return;
        }

        var pawn = parms.pawn;
        if (pawn == null || pawn.Dead || pawn.Map == null || !pawn.RaceProps.Humanlike)
        {
            return;
        }

        if (pawn.Position.GetTerrain(pawn.Map) == null)
        {
            return;
        }

        var terrain = pawn.Position.GetTerrain(pawn.Map);
        if (!RaidersCanSwim.DeepWaterDefs.Contains(terrain) &&
            !RaidersCanSwim.DeepWaterLabels.Contains(terrain.label))
        {
            return;
        }

        __result = false;
    }
}