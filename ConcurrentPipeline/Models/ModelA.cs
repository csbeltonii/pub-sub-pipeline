namespace ConcurrentPipeline.Models;

public class ModelA : BaseModel
{
    public override string ToString() => $"{typeof(ModelA)} {Name}";
    public ModelA(string name) : base(name) { }
}