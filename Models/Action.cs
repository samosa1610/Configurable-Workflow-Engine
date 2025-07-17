namespace WebApplication1.Models;

public class Action
{
    public string Id { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public List<string> FromStates { get; set; } = new();
    public string ToState { get; set; } = string.Empty;

    // In the future, we can support things like role-based restrictions,
    // action priorities, or automatic timeouts right here.
}
