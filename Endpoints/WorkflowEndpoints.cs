using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Endpoints;

/// <summary>
/// Minimal API route definitions for the workflow engine.
/// This class maps all HTTP endpoints for interacting with workflows and instances.
/// Designed to be clean, easy to extend, and focused.
/// </summary>
public static class WorkflowEndpoints
{
    /// <summary>
    /// Maps all routes related to workflows and instances.
    /// </summary>
    public static void MapWorkflowEndpoints(this WebApplication app)
    {
        // Endpoint to create a new workflow definition.
        // Expects states and actions in the body.
        app.MapPost("/workflow", (WorkflowDefinition def, WorkflowService svc) =>
        {
            try
            {
                var id = svc.CreateDefinition(def);
                return Results.Ok(new { message = "Created", id });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        // Endpoint to retrieve a workflow definition by ID.
        app.MapGet("/workflow/{id}", (string id, WorkflowService svc) =>
        {
            var def = svc.GetDefinition(id);
            return def != null ? Results.Ok(def) : Results.NotFound();
        });

        // Endpoint to start a new workflow instance for a given definition.
        app.MapPost("/instance/{defId}", (string defId, WorkflowService svc) =>
        {
            try
            {
                var inst = svc.StartInstance(defId);
                return Results.Ok(inst);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        // Endpoint to perform an action on a given workflow instance.
        // Validates current state, action, and transition rules before applying.
        app.MapPost("/instance/{id}/action/{actionId}", (string id, string actionId, WorkflowService svc) =>
        {
            try
            {
                svc.ExecuteAction(id, actionId);
                return Results.Ok(new { message = "Transition applied." });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        // Endpoint to retrieve the current state and transition history of a workflow instance.
        app.MapGet("/instance/{id}", (string id, WorkflowService svc) =>
        {
            var inst = svc.GetInstance(id);
            return inst != null ? Results.Ok(inst) : Results.NotFound();
        });
    }
}
