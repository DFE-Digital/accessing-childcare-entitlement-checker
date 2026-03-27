---
status: "proposed"
---

# Use GitHub runners for UI tests on PR push

## Context and Problem Statement

We'd like to run UI tests on PR pushes. But we don't need the expense or complexity of full ephemeral environments in Azure.

This ADR does *not* preclude also running tests against an Azure deployment for other kinds of changes. For example; we'll almost certainly run tests against a deployment to Azure whenever we merge to `main`)

## Decision Drivers

* Run tests as close to the change as possible.
* Prefer lower cost or free solutions - GitHub action usage is free for public repos.
* Prefer less complex solutions.
* Our application can be tested without external systems like databases etc.

## Considered Options

* Not running UI tests on PR pushes.
* Deploying to Azure for every PR push.
* Running the tests directly on the runner.

## Decision Outcome

Chosen option: "Running the tests directly on the runner" - because it's relatively simple; and cheap to do, and lets us run tests in CI.

### Consequences

* Good, because we can run tests in CI close to when change happens.
* Good, because simpler to implement and manage than an Azure deployment.
* Good, because cheaper running cost than an Azure deployment.
* Neutral, because it tests in an environment that's very different to live. But we can also run tests after merging to `main`.
* Bad, because if we need to introduce an external dependency like a database, we might prefer to deploy to Azure.
