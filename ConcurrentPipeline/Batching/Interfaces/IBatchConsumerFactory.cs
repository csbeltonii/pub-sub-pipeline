using System.Threading.Tasks.Dataflow;
using ConcurrentPipeline.Batching.Services;

namespace ConcurrentPipeline.Batching.Interfaces;

public interface IBatchConsumerFactory<TModel>
{
    IBatchConsumer<TModel>? CreateConsumer(ITargetBlock<TModel> targetBlock);
}