---
title: Use dev containers
layout: sub-navigation
sectionKey: How-to guides
order: 7
includeInBreadcrumbs: true
eleventyNavigation:
  parent: How-to guides
  key: Use dev containers
---
Use the pre-configured Dev Container (development container) to run a fully isolated development environment with all required SDKs and dependencies pre-installed.

## Install prerequisites

Before launching the container, ensure the following software is installed on the host machine:

- **Docker Desktop** (or Docker Engine on Linux)
- **Visual Studio Code**
- **Dev Containers Extension** for VS Code (extension ID: `ms-vscode-remote.remote-containers`)

## Open the repository in the container

To mount and run the workspace inside Docker:

1. **Launch VS Code** and open the cloned repository folder.
2. **Open the Command Palette** (`Ctrl+Shift+P` on Windows/Linux, `Cmd+Shift+P` on macOS).
3. **Run the command:**
   ```text
   Dev Containers: Reopen in Container
   ```
4. **Wait for the build to complete.** VS Code will download the base image, execute the `Dockerfile` instructions, and install local development tools. Once completed, the terminal inside VS Code will operate within the Linux container.

## Rebuild the container after changes

If the `.devcontainer/Dockerfile` or `.devcontainer/devcontainer.json` configuration is modified, rebuild the environment to apply the updates:

1. **Open the Command Palette.**
2. **Run the command:**
   ```text
   Dev Containers: Rebuild Container
   ```
