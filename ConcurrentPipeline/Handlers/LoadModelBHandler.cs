using ConcurrentPipeline.Batching.Interfaces;
using ConcurrentPipeline.Commands;
using ConcurrentPipeline.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConcurrentPipeline.Handlers;

public class LoadModelBHandler : BaseLoadModelHandler<ModelB>, IRequestHandler<LoadModelB>
{
    protected override string GetName(int number) => $"Model B {number}";
    public async Task Handle(LoadModelB request, CancellationToken cancellationToken) 
        => await base.Handle(request, cancellationToken);

    public LoadModelBHandler(ILogger<BaseLoadModelHandler<ModelB>> logger) : base(logger) { }
}