using System.Reflection;
using ConcurrentPipeline.Batching.Interfaces;
using ConcurrentPipeline.Commands;
using ConcurrentPipeline.Models;
using Microsoft.Extensions.Logging;

namespace ConcurrentPipeline.Handlers;

/// <summary>
/// Uses reflection to create models derived from <see cref="BaseModel"/> asynchronously.
/// Pushes the 
/// </summary>
/// <typeparam name="TBaseModel"></typeparam>
public abstract class BaseLoadModelHandler<TBaseModel>
{
    private readonly ILogger<BaseLoadModelHandler<TBaseModel>> _logger;

    protected BaseLoadModelHandler(ILogger<BaseLoadModelHandler<TBaseModel>> logger)
    {
        _logger = logger;
    }

    public async Task Handle(BaseLoadModelCommand request, CancellationToken cancellationToken)
    {
        await foreach (var model in GetModels(request.Count, typeof(TBaseModel)).WithCancellation(cancellationToken))
        {
            _logger.LogInformation("Loading {Model}", model.ToString());
            request.PostFunction(model);
        }
    }

    protected abstract string GetName(int number);

    private async IAsyncEnumerable<BaseModel> GetModels(int count, Type modelType)
    {
        var random = new Random();
        while (count > 0)
        {
            var constructor = modelType
                         .GetConstructor(
                             bindingAttr: BindingFlags.Public | BindingFlags.Instance, 
                             binder: null,
                             types: new [] { typeof(string) },
                             modifiers: null);

            if (constructor is null)
            {
                yield break;
            }

            yield return (BaseModel)constructor.Invoke(parameters: new object[] { GetName(count) });

            await Task.Delay((int)random.NextInt64(0, 500));

            count--;
        }
    }
}