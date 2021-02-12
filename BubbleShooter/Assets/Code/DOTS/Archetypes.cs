﻿using Assets.Code.Bubbles;
using Assets.Code.Grid.Spawn;
using Unity.Entities;

namespace Assets.Code.DOTS
{
    internal static class Archetypes
    {
        public static ComponentType[] RowSpawned => new ComponentType[]
        {
            ComponentType.ReadOnly<RowSpawnedTagCmp>(),
        };

        public static ComponentType[] VisualsInProgress => new ComponentType[]
        {
            ComponentType.ReadOnly<Visuals.VisualsInProgressTagCmp>(),
        };

        public static ComponentType[] PopulateRowWithBubbles => new ComponentType[]
        {
            ComponentType.ReadOnly<PopulateRowWithBubbles>(),
        };

        public static ComponentType[] BubbleIsShooting => new ComponentType[]
        {
            ComponentType.ReadOnly<BubbleIsShootingTagCmp>(),
        };
    }
}
