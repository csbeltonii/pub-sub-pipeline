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
        private readonly IBatchConsumerFactory<BaseModel> _batchConsumerFactory;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IBatchConsumerFactory<BaseModel> batchConsumerFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _batchConsumerFactory = batchConsumerFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("{ClassName}: Starting work...", nameof(Worker));
                var stopwatch = Stopwatch.StartNew();

                await using var scope = _serviceProvider.CreateAsyncScope();
                var linkOptions = new DataflowLinkOptions
                {
                    PropagateCompletion = true
                };

                var bufferBlock = new BufferBlock<BaseModel>();
                var batchOne = new BatchBlock<BaseModel>(50);
                var batchTwo = new BatchBlock<BaseModel>(50);
                var batchThree = new BatchBlock<BaseModel>(50);

                var batchConsumer = _batchConsumerFactory.CreateConsumer(batchOne);
                var batchConsumer2 = _batchConsumerFactory.CreateConsumer(batchTwo);
                var batchConsumer3 = _batchConsumerFactory.CreateConsumer(batchThree);
                var mediator = _serviceProvider.GetRequiredService<IMediator>();

                var consumeOneTask = batchConsumer!.ConsumeAsync();
                var consumeTwoTask = batchConsumer2!.ConsumeAsync();
                var consumeThreeTask = batchConsumer3!.ConsumeAsync();

                var loadModelA = new LoadModelA(1000, bufferBlock.Post);
                var loadModelB = new LoadModelB(1000, bufferBlock.Post);
                var loadModelC = new LoadModelC(1000, bufferBlock.Post);

                bufferBlock.LinkTo(batchOne, linkOptions);
                bufferBlock.LinkTo(batchTwo, linkOptions);
                bufferBlock.LinkTo(batchThree, linkOptions);

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
