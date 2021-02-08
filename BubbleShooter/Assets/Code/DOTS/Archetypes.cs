using Assets.Code.Spawn;
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
    }
}
