using Unity.Collections;
using Unity.Entities;

namespace Assets.Code.Bubbles.Nodes
{
    [System.Serializable]
    internal struct NodeNeighboursCmp : IBufferElementData
    {
        public Entity Neighbour;
    }
}
