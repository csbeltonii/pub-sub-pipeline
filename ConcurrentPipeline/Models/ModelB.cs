namespace ConcurrentPipeline.Models;

public class ModelB : BaseModel
{
    public override string ToString() => $"{typeof(ModelB)} {Name}";
    public ModelB(string name) : base(name) { }
}