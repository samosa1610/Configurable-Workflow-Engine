namespace WebApplication1.Models;

public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DefinitionId { get; set; } = string.Empty;
    public string CurrentState { get; set; } = string.Empty;
    public List<(string ActionId, DateTime Timestamp)> History { get; set; } = new();
}
