# Claude Handoff: TaskFlow Project (C# + React)

## Decision Made
- **Stack**: C# + ASP.NET Core (backend) + React (frontend)
- **Why**: Modern, future-proof, building new things (not legacy maintenance)
- **IDE**: VS Code + .NET CLI (not Visual Studio yet)

## Project: TaskFlow
- **Type**: Project management application
- **Purpose**: Learn C# + React full-stack development
- **Timeline**: 2-4 weeks (1-2 hours daily)

## Phase 1 Scope
1. C# REST API (ASP.NET Core)
2. React dashboard
3. Database (SQL)
4. Simple authentication (email/password)
5. Features: Create projects, add tasks, manage status

## Teaching Approach
- Use a "guided independence" model (not answer-dumping, not sink-or-swim)
- Plain-language explanations ("why" before "what")
- Small, verified batches (plan → build → test → reflect)
- Interview-prep question after each major concept

## Research-Aligned Learning Protocol

### Session Structure (60-90 min)
1. Goal (5 min): Define one tiny outcome (for example: "Create GET /health endpoint").
2. Model (10-15 min): Explain concept and think aloud through decisions.
3. Build (20-30 min): Implement together in small steps.
4. Retrieval Check (5-10 min): User explains from memory what was done and why.
5. Solo Variation (10-20 min): User modifies one part independently.
6. Reflect (5 min): What worked, what failed, what to try next.

### Hint Policy (Critical)
- First response to implementation uncertainty: ask user to propose a plan in 2-4 steps.
- If blocked after ~10 minutes: give one focused hint.
- If still blocked: give a second hint plus a tiny scaffold (starter function/signature).
- Give full solution only when:
	- tooling/environment is broken, or
	- user explicitly asks for "implement mode".

### Learning Science Rules to Apply
- Prefer retrieval over rereading: quick recall questions after each task.
- Use spaced review: revisit prior concepts at the start of next session.
- Use desirable difficulty: tasks should feel slightly hard but achievable.
- Use worked-example + practice pairs: one demonstrated example, then one similar solo task.
- Use explicit metacognition prompts: plan, monitor, evaluate.

### Confidence and Motivation Handling
- Counter second-guessing with evidence: "what passed, what failed, what changed".
- Keep challenge calibrated: too easy = no growth; too hard = stall.
- Praise process quality (debug method, decomposition, verification), not just outcomes.

### Independence Progression
- Week 1: Copilot leads structure; user executes with guidance.
- Week 2: Shared control; user drafts plans first, Copilot reviews.
- Week 3+: User leads end-to-end implementation; Copilot acts as reviewer.

### Definition of "Actually Learned"
User can do all three without looking:
1. Explain the concept in plain language.
2. Implement a close variation.
3. Debug one broken version and recover.

## Tomorrow's First Tasks (when user returns)
1. Verify .NET SDK is installed
2. Verify Node.js is installed
3. Create TaskFlow project folder structure
4. Create C# backend project
5. Create first "Hello World" API

## Environment Requirements
- .NET SDK (latest)
- Node.js + npm
- VS Code + C# extension
- Git (optional but recommended)

## Key Interview Questions to Prepare
- "Why did you choose C# over PHP/Python/Java?"
- "What are the benefits of ASP.NET Core?"
- "How do REST APIs work?"
- "What's the difference between frontend and backend?"

## User Notes
- Prone to second-guessing; needs reassurance and clear direction
- Prefers teaching-first, step-by-step approach
- Wants to understand "why" behind decisions
- Has experience with TypeScript/JavaScript/Node.js