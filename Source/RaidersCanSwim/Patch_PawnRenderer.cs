using HarmonyLib;
using UnityEngine;
using Verse;

namespace Swimming
{
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnInternal", typeof(Vector3), typeof(float), typeof(bool), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(bool), typeof(bool), typeof(bool))]
    internal static class Patch_PawnRenderer
    {
        private static void Prefix(ref bool renderBody, PawnRenderer __instance)
        {
            var pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if (pawn == null || pawn.Dead || pawn.Map == null || !pawn.RaceProps.Humanlike)
            {
                return;
            }

            if (pawn.Position.GetTerrain(pawn.Map) == null)
            {
                return;
            }

            var terrain = pawn.Position.GetTerrain(pawn.Map);
            if (!RaidersCanSwim.DeepWaterDefs.Contains(terrain) && !RaidersCanSwim.DeepWaterLabels.Contains(terrain.label))
            {
                return;
            }

            renderBody = false;
        }
    }
}