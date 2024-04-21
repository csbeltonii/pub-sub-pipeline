using ConcurrentPipeline.Models;
using MediatR;

namespace ConcurrentPipeline.Commands
{
    public abstract record BaseLoadModelCommand(int Count, Func<BaseModel, bool> PostFunction) : IRequest;
}