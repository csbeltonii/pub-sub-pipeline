using System.Threading.Tasks.Dataflow;
using ConcurrentPipeline.Batching.Interfaces;

namespace ConcurrentPipeline.Batching.Services;

public class DataProducer<TModel> : IBatchProducer<TModel>
{
    private readonly BatchBlock<TModel> _batchBlock;

    public DataProducer(int capacity)
    {
        _batchBlock = new BatchBlock<TModel>(capacity);
    }

    public void Post(TModel model) => _batchBlock.Post(model);

    public Task LinkTo(ITargetBlock<TModel[]> targetBlock)
    {
        _batchBlock.LinkTo(
            targetBlock,
            new DataflowLinkOptions
            {
                PropagateCompletion = true
            }
        );

        return _batchBlock.Completion;
    }

    public Task LinkTo(IPropagatorBlock<TModel, TModel> sourceBlock)
    {
        sourceBlock.LinkTo(_batchBlock, new DataflowLinkOptions
        {
            PropagateCompletion = true
        });

        return sourceBlock.Completion;
    }

    public void Complete() => _batchBlock.Complete();
}