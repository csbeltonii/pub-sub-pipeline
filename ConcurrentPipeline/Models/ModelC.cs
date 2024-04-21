namespace ConcurrentPipeline.Models;

public class ModelC : BaseModel
{
    public override string ToString() => $"{typeof(ModelC)} {Name}";
    public ModelC(string name) : base(name) { }
}