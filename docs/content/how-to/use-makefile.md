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
Utilise the repository's `Makefile` to execute common development, formatting, and testing tasks from a single, unified interface.

## Install Make (Windows)

Before running Makefile commands on Windows, install the `make` utility using one of the following methods:

### Method A: Install via Winget (Recommended)
Open an administrative PowerShell prompt and run:
```powershell
winget install GnuWin32.Make
```

### Method B: Install via Chocolatey
Run:
```powershell
choco install make
```

### Method C: Install via Scoop
Run:
```powershell
scoop install make
```

## Run development commands

Execute development and build pipelines from the repository root:

### Build the solution
To clean, restore, format, and compile the entire .NET solution:
```bash
make build
```

### Run unit and integration tests
To run all fast, local unit and integration tests:
```bash
make test
```

### Run end-to-end tests
To run Playwright browser automation user journey scenarios:
```bash
make test-e2e
```

### Run accessibility tests
To execute the axe-core accessibility compliance suite:
```bash
make test-a11y
```

## Run infrastructure commands

Validate and clean your cloud environment configurations:

### Format Terraform files
To recursively clean and format spacing in all Terraform files:
```bash
make tf-f
```

### Validate Terraform syntax
To verify configurations and resource reference parameters:
```bash
make tf-v
```

### Generate Terraform documentation
To generate markdown documentation from the Terraform configurations:
```bash
make tf-docs
```

### Clean documentation site
To remove the generated `docs/_site` directory:
```bash
make docs-c
```

### Build documentation site
To install dependencies and build the static site:
```bash
make docs-b
```

### Serve documentation site
To start a local preview server for the documentation:
```bash
make docs-s
```

### Flatten documentation files
To compile all separate documentation files into folder-specific notebooks for LLM ingestion:
```bash
make flatten-docs
```
