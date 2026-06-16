# Claude Handoff: TaskFlow Project (C# + ASP.NET Core)

## Current Status
- **Week 1: DONE**
- **Program.cs: FIXED and building**
- **Week 2: NOT STARTED** — ready to begin after Swagger verified

## First Thing Next Session
Run the app and verify Swagger still works:
```
dotnet run
```
Test the basic flow: create project → create task → update status → delete. Then start Week 2.

## Week 2 Plan
1. Add EF Core NuGet packages
2. Create DbContext
3. Create first migration
4. Swap in-memory lists → database
5. Test in Swagger (data should survive restart)

## Stack
- **Backend**: C# + ASP.NET Core minimal API, .NET 8
- **Models**: Classes (not records) — intentional for EF Core compatibility
- **Frontend**: Razor Pages (Week 5), React optional Week 7
- **Database**: SQLite (Week 2)
- **Roadmap**: `c-sharp-decision-summary.md`
- **Progress log**: `claude-code-progression.md`

## Teaching Approach
- Guided independence — user drives, Claude gives hints
- Default: guidance-only. User must say "implement mode" or "approve action" for Claude to edit files
- See `learning-protocol.md` for full protocol

## User Notes
- TypeScript/JavaScript background — use those analogies when helpful
- Prone to spiraling when stuck — redirect to the specific blocker, don't let direction debates take over
- In Utah — React is dominant locally, worth adding later for job market
