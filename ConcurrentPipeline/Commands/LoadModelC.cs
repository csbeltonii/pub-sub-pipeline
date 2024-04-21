using ConcurrentPipeline.Models;

namespace ConcurrentPipeline.Commands;

public record LoadModelC(int Count, Func<BaseModel, bool> PostFunction) : BaseLoadModelCommand(Count, PostFunction);