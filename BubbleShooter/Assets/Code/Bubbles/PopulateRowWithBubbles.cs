using Unity.Entities;

namespace Assets.Code.Bubbles
{
    internal struct PopulateRowWithBubbles : IComponentData
    {
        public int Row;
        public bool RandomizeNumbers;
    }
}
