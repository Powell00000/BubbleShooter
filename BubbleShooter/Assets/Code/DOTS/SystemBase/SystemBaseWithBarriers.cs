using Unity.Entities;

public abstract class SystemBaseWithBarriers : SystemBase
{
    protected BeginInitializationEntityCommandBufferSystem beginInitializationBuffer;
    protected BeginSimulationEntityCommandBufferSystem beginSimulationBuffer;
    protected EndSimulationEntityCommandBufferSystem endSimulationBuffer;
    protected BeginPresentationEntityCommandBufferSystem beginPresentationBuffer;


    protected override void OnCreate()
    {
        beginInitializationBuffer = World.GetExistingSystem<BeginInitializationEntityCommandBufferSystem>();
        beginSimulationBuffer = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        endSimulationBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        beginPresentationBuffer = World.GetExistingSystem<BeginPresentationEntityCommandBufferSystem>();
    }
}
