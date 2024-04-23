using ConcurrentPipeline.Batching.Factories;
using ConcurrentPipeline.Batching.Interfaces;
using ConcurrentPipeline.Batching.Services;
using ConcurrentPipeline.Commands;
using ConcurrentPipeline.Models;
using WorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(BaseLoadModelCommand).Assembly);
    }
);
builder.Services.AddSingleton<IDataQueueService<BaseModel>>(DataQueueService<BaseModel>.Create(50));
//builder.Services.AddTransient<IBatchConsumer<BaseModel>, BaseModelBatchConsumer>();
builder.Services.AddTransient<IBatchProducer<BaseModel>>(_ => new DataProducer<BaseModel>(50));
builder.Services.AddSingleton<IBatchConsumerFactory<BaseModel>, BatchConsumerFactory<BaseModel>>();

var host = builder.Build();
host.Run();