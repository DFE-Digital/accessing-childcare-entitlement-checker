---
title: Generate LLM notebook artefacts
layout: sub-navigation
sectionKey: How-to guides
order: 10
includeInBreadcrumbs: true
eleventyNavigation:
  parent: How-to guides
  key: Generate LLM notebook artefacts
---
Generate folder-specific, flattened Markdown files of the repository's technical documentation to serve as compact, high-context knowledge sources for Large Language Model (LLM) notebooks (such as Copilot Notebook, Gemini Notebook, or NotebookLM).

## Generate the flattened documentation

Compile and flatten the Markdown files by folder, automatically removing YAML metadata frontmatter and converting the `title` attribute into standard `H1` headings.

1. **Navigate to the repository root** in your terminal.
2. **Execute the generator target:**
   ```bash
   make flatten-docs
   ```
3. **Verify the output:**
   The script traverses the subfolders of `docs/content/` (ignoring the `assets` folder and any Markdown files in the root `content` directory itself) and outputs the compiled Markdown files directly into a local `notebook/` folder.

## Locate the generated notebook files

The following compiled documents are generated in the `notebook/` directory for lookup:

| Notebook File | Source Content | Purpose |
| :--- | :--- | :--- |
| `tutorials.md` | `docs/content/tutorials/` | Step-by-step onboarding walkthroughs. |
| `how-to.md` | `docs/content/how-to/` | General task-solving guides. |
| `how-to-runbooks.md` | `docs/content/how-to/runbooks/` | Incident response and operations. |
| `explanation.md` | `docs/content/explanation/` | Architectural deep dives and philosophies. |
| `reference-architecture.md` | `docs/content/reference/architecture/` | In-depth technical specifications. |
| `reference-decisions.md` | `docs/content/reference/decisions/` | Architectural Decision Records (ADRs). |
| `reference-operational.md` | `docs/content/reference/operational/` | Risk lists and system monitors. |
| `reference-testing.md` | `docs/content/reference/testing/` | Compliance standards and test plans. |

## Ingest into LLM notebook tools

Upload the generated files to provide a complete, clean mental model of the codebase to an AI assistant.

### Option A: Upload to NotebookLM or Gemini Notebook
1. Open your browser and navigate to **NotebookLM** or **Gemini Notebook**.
2. Create a new notebook for the project.
3. Click **Add Source** and select **Upload Files**.
4. Drag and drop the specific `.md` files from your local `notebook/` directory (e.g. `reference-decisions.md` to feed it ADR history, or `explanation.md` to feed it architectural patterns).

### Option B: Upload to Copilot Notebook
1. Open your enterprise **Copilot** workspace.
2. Create or select a custom Copilot agent/notebook.
3. Upload the target flattened `.md` files as reference documents to lock in your repository's specific design patterns, naming conventions, and diataxis guidelines.
