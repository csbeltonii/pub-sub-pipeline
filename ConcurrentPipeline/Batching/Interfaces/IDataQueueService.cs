namespace ConcurrentPipeline.Batching.Interfaces;

public interface IDataQueueService<TModel>
{
    ValueTask QueueAsync(TModel entity);
    ValueTask<TModel> DequeueAsync(CancellationToken cancellationToken);
    IAsyncEnumerable<TModel> DequeueAllAsync(CancellationToken cancellationToken);
}