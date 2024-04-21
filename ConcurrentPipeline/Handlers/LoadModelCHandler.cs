using ConcurrentPipeline.Batching.Interfaces;
using ConcurrentPipeline.Commands;
using ConcurrentPipeline.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConcurrentPipeline.Handlers;

public class LoadModelCHandler : BaseLoadModelHandler<ModelC>, IRequestHandler<LoadModelC>
{
    protected override string GetName(int number) => $"Model C {number}";
    public async Task Handle(LoadModelC request, CancellationToken cancellationToken)
        => await base.Handle(request, cancellationToken);

    public LoadModelCHandler(ILogger<BaseLoadModelHandler<ModelC>> logger) : base(logger) { }
}