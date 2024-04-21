using ConcurrentPipeline.Batching.Interfaces;
using ConcurrentPipeline.Commands;
using ConcurrentPipeline.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConcurrentPipeline.Handlers;

public class LoadModelAHandler : BaseLoadModelHandler<ModelA>, IRequestHandler<LoadModelA>
{
    protected override string GetName(int number) => $"Model A {number}";
    public async Task Handle(LoadModelA request, CancellationToken cancellationToken) 
        => await base.Handle(request, cancellationToken);

    public LoadModelAHandler(ILogger<BaseLoadModelHandler<ModelA>> logger) : base(logger) { }
}