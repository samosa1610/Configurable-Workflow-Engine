
# Configurable Workflow Engine (State-Machine API)

A lightweight and extensible backend service that allows clients to define, trigger, and track workflows modeled as finite state machines.

🛠️ Built with: `.NET 8`, C#, ASP.NET Core (Minimal API)

---

# Features

-  Define custom workflows with states and transitions
-  Start independent workflow instances from definitions
-  Execute transitions with validation
-  Track current state and history of each instance
-  In-memory storage (no external DB needed)

---

## How to Run

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### ▶️ Run the API

```bash
dotnet run 
```

Expected output:

```
Now listening on: http://localhost:5016
```

---

## 🔁 Sample Workflow via cURL

### 1️⃣ Create a workflow definition

```bash
curl -X POST http://localhost:5016/workflow ^
  -H "Content-Type: application/json" ^
  -d "{\"id\":\"leave-approval\",\"states\":[{\"id\":\"applied\",\"isInitial\":true,\"isFinal\":false,\"enabled\":true},{\"id\":\"reviewed\",\"isInitial\":false,\"isFinal\":false,\"enabled\":true},{\"id\":\"approved\",\"isInitial\":false,\"isFinal\":true,\"enabled\":true}],\"actions\":[{\"id\":\"review\",\"enabled\":true,\"fromStates\":[\"applied\"],\"toState\":\"reviewed\"},{\"id\":\"approve\",\"enabled\":true,\"fromStates\":[\"reviewed\"],\"toState\":\"approved\"}]}"
```

### 2️⃣ Start a new workflow instance

```bash
curl -X POST http://localhost:5016/instance/leave-approval
```

Copy the `"id"` value from the response.

### 3️⃣ Execute actions (transitions)

```bash
curl -X POST http://localhost:5016/instance/YOUR_INSTANCE_ID/action/review
curl -X POST http://localhost:5016/instance/YOUR_INSTANCE_ID/action/approve
```

### 4️⃣ View current state and history

```bash
curl http://localhost:5016/instance/YOUR_INSTANCE_ID
```

---

## Assumptions & Notes

- Each workflow definition must include **exactly one** initial state.
- Final states cannot be transitioned out of.
- Transitions must be enabled and valid from the current state.
- The engine stores everything **in-memory** — restart = reset.

---

## 🗂️ Project Structure

```
WebApplication1/
├── Models/          # Domain models: State, Action, WorkflowDefinition, etc.
├── Services/        # Workflow orchestration and lifecycle management
├── Logic/           # Stateless logic (engine, validator, repository)
├── Endpoints/       # Minimal API endpoints
└── Program.cs       # Entry point for the app
```

---

## 🔧 Possible Extensions

- File-based persistence (JSON/YAML)
- Swagger / OpenAPI docs
- Role-based access or guard conditions
- Time-based or event-driven transitions
- Audit trail for compliance

---

## 📫 Contact

Built by **Nitin Kumar**  
As part of the **Infonetica Software Engineering Intern Assignment**
