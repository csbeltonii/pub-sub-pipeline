using ConcurrentPipeline.Models;
using Microsoft.Extensions.Logging;

namespace ConcurrentPipeline.Batching.Services;

public class BaseModelBatchConsumer : BaseDataConsumer<BaseModel>
{
    public BaseModelBatchConsumer(ILogger<BaseDataConsumer<BaseModel>> logger) 
        : base(logger) { }
}