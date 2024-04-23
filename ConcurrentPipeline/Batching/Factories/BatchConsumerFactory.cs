using System.Threading.Tasks.Dataflow;
using ConcurrentPipeline.Batching.Interfaces;
using ConcurrentPipeline.Batching.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ConcurrentPipeline.Batching.Factories;

public sealed class BatchConsumerFactory<TModel> : IBatchConsumerFactory<TModel>
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<BatchConsumerFactory<TModel>> _logger;

    public BatchConsumerFactory(ILoggerFactory loggerFactory, ILogger<BatchConsumerFactory<TModel>> logger)
    {
        _loggerFactory = loggerFactory;
        _logger = logger;
    }

    public IBatchConsumer<TModel>? CreateConsumer(ITargetBlock<TModel> targetBlock)
    {
        var consumerFactory = _loggerFactory.CreateLogger<BaseDataConsumer<TModel>>();

        var constructor = typeof(IBatchConsumer<TModel>)
                          .Assembly
                          .GetTypes()
                          .FirstOrDefault(_ => _.GetInterfaces()
                                                .Contains(typeof(IBatchConsumer<TModel>)))?
                          .GetConstructor(
                              bindingAttr: BindingFlags.Public | BindingFlags.Instance, 
                              binder: null, 
                              types: new[] { typeof(ISourceBlock<TModel[]>), typeof(ILogger<BaseDataConsumer<TModel>>) }, 
                              modifiers: null);

        if (constructor is null)
        {
            _logger.LogError("Could not find valid constructor for type {Type}", typeof(IBatchConsumer<TModel>));
            return null;
        }

        return (IBatchConsumer<TModel>)constructor.Invoke(parameters: new object[] { targetBlock, consumerFactory });
    }
}