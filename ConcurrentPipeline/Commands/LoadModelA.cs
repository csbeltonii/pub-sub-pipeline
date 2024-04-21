using ConcurrentPipeline.Models;

namespace ConcurrentPipeline.Commands;

public record LoadModelA(int Count, Func<BaseModel, bool> PostFunction) : BaseLoadModelCommand(Count, PostFunction);