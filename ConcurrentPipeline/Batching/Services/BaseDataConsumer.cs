using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;

namespace ConcurrentPipeline.Batching.Services;

public interface IBatchConsumer<TModel>
{
    Task ConsumeAsync();
}

public abstract class BaseDataConsumer<TModel> : IBatchConsumer<TModel>
{
    private readonly ILogger<BaseDataConsumer<TModel>> _logger;
    private readonly ActionBlock<TModel[]> _consumerBlock;
    private int batchCount;
    private readonly Guid Id;

    protected BaseDataConsumer(ISourceBlock<TModel[]> sourceBlock, ILogger<BaseDataConsumer<TModel>> logger)
    {
        Id = Guid.NewGuid();
        _consumerBlock = CreateConsumerBlock();
        _logger = logger;

        sourceBlock.LinkTo(
            _consumerBlock,
            new DataflowLinkOptions
            {
                PropagateCompletion = true
            }
        );
    }

    public Task ConsumeAsync() => _consumerBlock.Completion;

    private ActionBlock<TModel[]> CreateConsumerBlock() => new ActionBlock<TModel[]>(
        entities =>
        {
            _logger.LogCritical("Consumer {ID}: Consuming batch {BatchNumber}", Id, ++batchCount);

            foreach (var entity in entities)
            {
                _logger.LogCritical("Consumer {ID}: Consuming item #{Model}", Id, entity!.ToString());
            }
        }
    );
}