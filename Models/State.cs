namespace WebApplication1.Models;

public class State
{
    public string Id { get; set; } = string.Empty;
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
    public bool Enabled { get; set; } = true;

    // We could easily extend this to include descriptions, labels, or metadata
    // like who can enter this state, when, etc.
}
