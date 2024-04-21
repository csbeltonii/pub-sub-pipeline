namespace ConcurrentPipeline.Models;

public class BaseModel : IDisposable
{
    protected BaseModel(string name) => Name = name;
    public string? Name { get; set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}