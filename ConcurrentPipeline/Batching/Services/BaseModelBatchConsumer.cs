using System.Threading.Tasks.Dataflow;
using ConcurrentPipeline.Models;
using Microsoft.Extensions.Logging;

namespace ConcurrentPipeline.Batching.Services;

public class BaseModelBatchConsumer : BaseDataConsumer<BaseModel>
{
    public BaseModelBatchConsumer(ISourceBlock<BaseModel[]> sourceBlock, ILogger<BaseDataConsumer<BaseModel>> logger) 
        : base(sourceBlock, logger) { }
}