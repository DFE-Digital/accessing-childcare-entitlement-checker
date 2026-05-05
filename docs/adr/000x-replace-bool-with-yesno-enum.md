# Use a Yes/No enum instead of bools for questions

## Context and Problem Statement

Some questions in the set have yes/no answers. Which type is most appropriate and easiest to use for these questions?

## Decision Drivers

* Ease and speed of development.
* Ease of maintenance.
* Type correctness

## Considered Options

* Use `bool` for yes/no questions. (baseline)
* Add a new type (e.g. a `YesNo` enum) for yes/no questions.

## Decision Outcome

Use a yes/no enum as the typed representation for yes/no questions.

## Pros and cons of the options

### Use `bool` for yes/no questions

#### Pros

* Clarity/Simplicity - probably what you would expect when coming into the project.

#### Cons

* `bool` is a good proxy; but not really the same type as `Yes/No`
* Means that we need to handle yes/no questions differently in the view.

### Using a custom yes/no enum

The pros/cons are the inverse of using `bool`.

### Consequences

* Good, because questions are more consistent.
* Good, because less code to write; when combined with the custom enum tag helper.
* Good, because it's a stricter and more tightly scoped type.
* Bad, because many developers might expect to use a `bool` here.
