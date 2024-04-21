using ConcurrentPipeline.Models;

namespace ConcurrentPipeline.Commands;


public record LoadModelB(int Count, Func<BaseModel, bool> PostFunction) : BaseLoadModelCommand(Count, PostFunction);