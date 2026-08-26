---
title: Use makefile commands
layout: sub-navigation
sectionKey: How-to guides
order: 9
includeInBreadcrumbs: true
eleventyNavigation:
  parent: How-to guides
  key: Use makefile commands
---
Utilise the repository's `Makefile` to execute development, formatting, and testing tasks from a single, unified interface.

## Install prerequisites

Before running `Makefile` commands, you must install the `make` utility and other mandatory development utilities on your host machine.

### 1. Install Make (Windows)
Install the `make` utility using one of the following package managers:

#### Option A: Install via Winget (Recommended)
Open an administrative PowerShell prompt and run:
```powershell
winget install GnuWin32.Make
```

#### Option B: Install via Chocolatey
Open an administrative prompt and run:
```powershell
choco install make
```

#### Option C: Install via Scoop
Run:
```powershell
scoop install make
```

### 2. Install jq (Windows)
Deep static analysis commands parse analysis results using the `jq` utility. Install `jq` on your host machine:

#### Option A: Install via Winget (Recommended)
Open an administrative PowerShell prompt and run:
```powershell
winget install jqlang.jq
```

#### Option B: Install via Chocolatey
Open an administrative prompt and run:
```powershell
choco install jq
```

### 3. Restore local .NET tools
Verify and restore local development tools configured for this solution:
```bash
dotnet tool restore
```

## Run make commands

To execute any pipeline task, open your terminal at the repository root and run `make` followed by the target command name.

For example, to build and compile the entire .NET solution:
```bash
make build
```

## View available commands

For a comprehensive list of all configured build, static analysis, testing, Terraform, and documentation commands, refer to the [Makefile commands reference guide](/reference/makefile-commands/).
