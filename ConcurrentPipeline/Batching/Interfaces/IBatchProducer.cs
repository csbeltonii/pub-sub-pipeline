using System.Threading.Tasks.Dataflow;

namespace ConcurrentPipeline.Batching.Interfaces;

public interface IBatchProducer<TModel> : IBatchPoster<TModel>
{
    Task LinkTo(ITargetBlock<TModel[]> targetBlock);
    Task LinkTo(IPropagatorBlock<TModel, TModel> sourceBlock);
    void Complete();
}

public interface IBatchPoster<in TModel>
{
    void Post(TModel TModel);
}