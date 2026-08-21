---
title: Documentation publication strategy
layout: sub-navigation
order: 10
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
---
## Context and problem statement

Technical documentation is stored as Markdown files in the `/docs` directory. While readable directly on GitHub, an accessible, branded, and searchable documentation site published via GitHub Pages is required for stakeholders and developers.

## Decision drivers

* **Branding:** Compliance with GOV.UK/DfE styling for public-facing or stakeholder documentation.
* **Maintainability:** Low-friction processes for updating and adding documentation.
* **Discovery:** Built-in navigation and search functionality.
* **Consistency:** Alignment with other DfE Digital projects.

## Considered options

### Option 1: Retain Markdown in the repository (No action)

* **Positive:** No additional tooling or build steps required; zero financial cost.
* **Negative:** Lacks branding, provides poor navigation for non-technical users, and relies on GitHub's native search functionality.

### Option 2: MkDocs with a custom theme

* **Positive:** Fast build times and standard configuration.
* **Negative:** No mature, officially supported GOV.UK theme exists for MkDocs; introduces a Python environment dependency.

### Option 3: Eleventy (11ty) with the [GOV.UK Eleventy plugin](https://x-govuk.github.io/govuk-eleventy-plugin/)

* **Positive:** Provides a comprehensive GOV.UK-branded documentation site, aligns with DfE Digital standards, and includes built-in navigation and search.
* **Negative:** Requires a Node.js environment for the build process.

## Decision outcome

Chosen option: **Option 3: Eleventy (11ty) with the GOV.UK Eleventy Plugin**.

This option satisfies the requirement for GOV.UK styling using a maintained plugin. It aligns with established DfE Digital standards and provides search, navigation, and branding capabilities.

## Consequences

* **Positive:** Technical documentation matches the styling of other DfE services.
* **Positive:** Publishing to GitHub Pages is automated via GitHub Actions.
* **Negative:** Adds a Node.js dependency to the project's documentation build pipeline.
