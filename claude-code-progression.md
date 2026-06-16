# TaskFlow — Code Progression Log

## Session: 2026-06-16

### What We Fixed
Program.cs was broken after models and requests were split into separate files.

**Root cause:** `Project` and `TaskItem` were defined as `class` in their files, but Program.cs was still using record syntax (`with` expressions and positional constructors). Classes were intentional — better for EF Core mutability.

**Fixes applied:**
1. Added missing `using` statements at top of Program.cs:
   - `using TaskFlow.Api.Requests;`
   - `using TaskFlow.Api.Models;`
2. Replaced positional constructors with object initializer syntax:
   ```csharp
   // Before (record syntax)
   new TaskItem(Guid.NewGuid(), request.ProjectId, ...)
   // After (class syntax)
   new TaskItem { Id = Guid.NewGuid(), ProjectId = request.ProjectId, ... }
   ```
3. Replaced `with` expressions with direct property mutation:
   ```csharp
   // Before (record syntax)
   var updated = existing with { Status = normalizedStatus! };
   tasks[index] = updated;
   // After (class syntax)
   tasks[index].Status = normalizedStatus!;
   ```

**Result:** `dotnet build` passes.

### What Was Learned
- C# properties use PascalCase (not camelCase like JS/TS)
- `class` vs `record` — records auto-generate positional constructors and `with` support, classes don't
- Object initializer syntax: `new Foo { Property = value }`
- Classes are mutable so you can set properties directly instead of using `with`
- Server-set values (Status default, CreatedAtUtc) don't belong in request objects

### Still TODO Before Week 2
- [ ] Run `dotnet run` and verify Swagger flow works (create project → create task → update status → delete)

---

## Week 1 — Completed (~2026-06-14)
- Full CRUD for Projects and Tasks in Program.cs (minimal API)
- In-memory storage with `List<Project>` and `List<TaskItem>`
- Input validation returning 400s, missing resources returning 404s
- Swagger configured and working
- Helper functions for common validations
- Models and Requests split into separate files under `Models/` and `Requests/`
