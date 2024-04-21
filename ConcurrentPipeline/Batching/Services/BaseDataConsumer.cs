using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;

namespace ConcurrentPipeline.Batching.Services;

public interface IBatchConsumer<TModel>
{
    Task ConsumeAsync(IReceivableSourceBlock<TModel> sourceBlock);
}

public abstract class BaseDataConsumer<TModel> : IBatchConsumer<TModel>
{
    private readonly ILogger<BaseDataConsumer<TModel>> _logger;
    private readonly Guid Id;

    protected BaseDataConsumer(ILogger<BaseDataConsumer<TModel>> logger)
    {
        Id = Guid.NewGuid();
        _logger = logger;
    }

    public async Task ConsumeAsync(IReceivableSourceBlock<TModel> sourceBlock)
    {
        while (await sourceBlock.OutputAvailableAsync())
        {
            while (sourceBlock.TryReceive(out var entity))
            {
                _logger.LogWarning("Consumer {ID}: Consuming item #{Model}", Id, entity!.ToString());
            }
        }
    }

}