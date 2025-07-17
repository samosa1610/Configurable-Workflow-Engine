using WebApplication1.Endpoints;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WorkflowService>();

var app = builder.Build();
app.MapWorkflowEndpoints();
app.Run();
