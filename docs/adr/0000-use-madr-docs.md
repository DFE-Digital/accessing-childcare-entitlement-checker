# Use Markdown Architectural Decision Records

## Context and Problem Statement

We want to track decisions which can't be represented in C# types or infrastructure-as-code. (IAC)

## Decision Drivers

* Some decisions are implicit in the implementations; are not representable via the code/type system or IAC, and would be unwieldy or difficult to discover if expressed as comments.
* MADR is [encouraged by DfE standards](https://dfe-digital.github.io/architecture/standards/architecture-documentation/#architecture-documentation)

## Considered Options

* other Markdown docs in the repo
* confluence docs or stored teams chats
* code comments

## Decision Outcome

We chose [Markdown Architectural Decision Records (MADR)](https://adr.github.io/madr/) because they're already encouraged by DfE, fit in with other Markdown docs in our repo, and provide a central discoverable place to surface otherwise implicit decision making.

We chose to make the frontmatter and other elements optional.

ADRs will be stored in `/docs/adr` and follow a sequential naming convention:`NNNN-short-title.md`.

ADRs are immutable once accepted. If a decision changes, a new ADR should be created that supersedes the previous one.

### When to Create an ADR

Create an ADR when a decision:

* has multiple viable options or trade-offs
* is not obvious from the code or infrastructure
* is likely to be questioned or revisited in the future

## More Information

See [https://github.com/adr/madr/blob/develop/template/adr-template.md](https://github.com/adr/madr/blob/develop/template/adr-template.md) for a template.
