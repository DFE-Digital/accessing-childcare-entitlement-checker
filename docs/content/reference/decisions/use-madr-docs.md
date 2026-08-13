---
title: Use markdown architectural decision records
layout: sub-navigation
order: 0
sectionKey: Reference
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Decisions
  key: Use markdown ADR
---
## Context and problem statement

Decisions that cannot be represented in C# types or infrastructure-as-code (IaC) require tracking.

## Decision drivers

* Some decisions are implicit in the implementations, are not representable via the code/type system or IaC, and would be difficult to discover if expressed solely as comments.
* MADR is [encouraged by DfE standards](https://dfe-digital.github.io/architecture/standards/architecture-documentation/#architecture-documentation).

## Considered options

* Other Markdown documents in the repository
* Confluence documentation or stored Teams chats
* Code comments

## Decision outcome

[Markdown Architectural Decision Records (MADR)](https://adr.github.io/madr/) were selected because they are recommended by DfE standards, integrate with existing Markdown documentation in the repository, and provide a central, discoverable location to surface implicit decision-making.

Frontmatter and other metadata elements are optional.

ADRs are stored in `/docs/adr` and follow a sequential naming convention: `NNNN-short-title.md`.

ADRs are immutable once accepted. If a decision changes, a new ADR is created to supersede the previous decision.

### Criteria for creating an ADR

An ADR is created when a decision:

* Has multiple viable options or trade-offs
* Is not obvious from the code or infrastructure
* Is likely to be questioned or revisited in the future

## More information

See the [MADR template](https://github.com/adr/madr/blob/develop/template/adr-template.md) for structure details.
