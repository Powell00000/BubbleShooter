using Assets.Code.Bubbles;
using Assets.Code.Bubbles.Connections;
using Assets.Code.Bubbles.Solving;
using Assets.Code.Gameplay;
using Assets.Code.Grid.Spawn;
using Assets.Code.Visuals;
using Unity.Entities;

namespace Assets.Code.DOTS
{
    internal static class Archetypes
    {
        public static ComponentType[] SpawnNextRow => new ComponentType[]
        {
            ComponentType.ReadOnly<SpawnNextRowTagCmp>(),
        };

        public static ComponentType[] RowSpawned => new ComponentType[]
        {
            ComponentType.ReadOnly<RowSpawnedTagCmp>(),
        };

        public static ComponentType[] VisualsInProgress => new ComponentType[]
        {
            ComponentType.ReadOnly<VisualsInProgressTagCmp>(),
        };

        public static ComponentType[] AllVisualsFinished => new ComponentType[]
        {
            ComponentType.ReadOnly<AllVisualsFinishedTagCmp>(),
        };

        public static ComponentType[] PopulateRowWithBubbles => new ComponentType[]
        {
            ComponentType.ReadOnly<PopulateRowWithBubbles>(),
        };

        public static ComponentType[] BubbleIsShooting => new ComponentType[]
        {
            ComponentType.ReadOnly<BubbleIsShootingTagCmp>(),
        };

        public static ComponentType[] RefreshConnections => new ComponentType[]
        {
            ComponentType.ReadOnly<RefreshConnectionsTagCmp>(),
        };

        public static ComponentType[] ConnectionsRefreshed => new ComponentType[]
        {
            ComponentType.ReadOnly<ConnectionsRefreshedTagCmp>(),
        };

        public static ComponentType[] SolverFinished => new ComponentType[]
        {
            ComponentType.ReadOnly<SolverFinishedTagCmp>(),
        };

        public static ComponentType[] DestroyAll => new ComponentType[]
        {
            ComponentType.ReadOnly<DestroyAllTagCmp>(),
        };
    }

}
