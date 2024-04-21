namespace ConcurrentPipeline.Batching.Interfaces;

public interface IBatchProducerService<in TModel>
{
    Task PostAnyAsync(TModel model);
}