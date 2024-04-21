using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using ConcurrentPipeline.Batching.Interfaces;
using ConcurrentPipeline.Batching.Services;
using ConcurrentPipeline.Commands;
using ConcurrentPipeline.Models;
using MediatR;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("{ClassName}: Starting work...", nameof(Worker));
                var stopwatch = Stopwatch.StartNew();

                await using var scope = _serviceProvider.CreateAsyncScope();
                var batchConsumer = _serviceProvider.GetRequiredService<IBatchConsumer<BaseModel>>();
                var batchConsumer2 = _serviceProvider.GetRequiredService<IBatchConsumer<BaseModel>>();
                var batchConsumer3 = _serviceProvider.GetRequiredService<IBatchConsumer<BaseModel>>();
                var mediator = _serviceProvider.GetRequiredService<IMediator>();

                var bufferBlock = new BufferBlock<BaseModel>();

                var consumeOneTask = batchConsumer.ConsumeAsync(bufferBlock);
                var consumeTwoTask = batchConsumer2.ConsumeAsync(bufferBlock);
                var consumeThreeTask = batchConsumer3.ConsumeAsync(bufferBlock);

                var loadModelA = new LoadModelA(100, bufferBlock.Post);
                var loadModelB = new LoadModelB(100, bufferBlock.Post);
                var loadModelC = new LoadModelC(100, bufferBlock.Post);

                await Task.WhenAll(
                    mediator.Send(loadModelA, stoppingToken),
                    mediator.Send(loadModelB, stoppingToken),
                    mediator.Send(loadModelC, stoppingToken)
                );

                bufferBlock.Complete();
                await Task.WhenAll(consumeThreeTask, consumeTwoTask, consumeOneTask);

                _logger.LogInformation("************");
                _logger.LogInformation("************");
                _logger.LogInformation("************");
                _logger.LogInformation("************");
                _logger.LogInformation("************");
                _logger.LogInformation("Finished loading in {Time}", stopwatch.ElapsedMilliseconds);
                _logger.LogInformation("************");
                _logger.LogInformation("************");
                _logger.LogInformation("************");
                _logger.LogInformation("************");
                _logger.LogInformation("************");

                await scope.DisposeAsync();
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
