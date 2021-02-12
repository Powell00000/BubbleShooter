using Unity.Entities;

namespace Assets.Code.Bubbles
{
    internal struct SpawnBubbleCmp : IComponentData
    {
        public bool SolveHere;
        public bool RandomizeNumber;
        public int Number;
    }
}
