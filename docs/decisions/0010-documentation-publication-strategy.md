---
title: Documentation publication strategy
eleventyNavigation:
  key: Documentation
order: 11
---

## Context and Problem Statement

Technical documentation is currently stored as Markdown files in the `/docs` directory. While these are readable in GitHub, we want to provide a more accessible, branded, and searchable documentation site for stakeholders and developers, published via GitHub Pages.

## Decision Drivers

* **Branding**: Must follow GOV.UK/DfE styling for public-facing or stakeholder documentation.
* **Maintainability**: Low friction for developers to add or update documentation.
* **Discovery**: Must support navigation and search.
* **Consistency**: Align with other DfE Digital projects.

## Considered Options

### Option 1: Keep as Markdown in the repository (Do nothing)
* **Pros**: No additional tools or build steps; zero cost.
* **Cons**: No branding; poor navigation for non-technical users; no search functionality beyond GitHub's own search.

### Option 2: MkDocs with a custom theme
* **Pros**: Very popular for technical docs; fast; easy to set up.
* **Cons**: No mature, officially supported GOV.UK theme exists for MkDocs; requires Python environment.

### Option 3: Eleventy (11ty) with the [GOV.UK Eleventy Plugin](https://x-govuk.github.io/govuk-eleventy-plugin/)
* **Pros**: Provides a comprehensive GOV.UK branded documentation site out of the box; widely used within DfE Digital; flexible and extensible; high-quality navigation and search.
* **Cons**: Requires a Node.js environment for the build process.

## Decision Outcome

Chosen option: **Option 3: Eleventy (11ty) with the GOV.UK Eleventy Plugin**.

This option was chosen because it directly addresses the requirement for GOV.UK styling with minimal effort, leveraging a well-maintained plugin specifically designed for this purpose. It aligns with standard practices in DfE Digital and provides the best balance of features (search, navigation, branding) and developer experience.

## Consequences

* **Good**: Documentation will be professional and consistent with other DfE services.
* **Good**: Automated publishing to GitHub Pages via GitHub Actions.
* **Bad**: Adds a Node.js dependency to the project's documentation build pipeline.
