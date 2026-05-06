# Use English literals as localisation keys

## Context and Problem Statement

We implemented localisation in the app as best practice for building web apps; and because we expect a Welsh translation in a future phase.

Traditionally .NET localisation used code generation to build static classes that exposed resource values as members. However, the new default pattern is to use `IStringLocalizer` which takes a "stringly typed" approach to resource keys.

This ADR discusses the pros and cons of two "stringly typed" localisation key schemes. (Outlined in the "Considered Options" section)

## Decision Drivers

* We want to maximise ease and speed of development, but don't want to compromise our ability to translate the app in the future.
* There is no requirement in the current phase to translate. But we know that in the next phase we will almost certainly need to facilitate translating to one additional language. (Welsh)
* We don't expect many translation passes. (changes) The text will only be translated after the feature set has stabilised.

> [!IMPORTANT]
> Both approaches are conventional, widely used in the industry; and can be considered best practice.

## Considered Options

The two options considered can be referred to as "Key-based" and "literal-based" or ("message as key").

### Key based

Key based localisation uses stable identifiers; usually with some semantic structure. For example; in a Razor template we might use `@Localizer["ChildName.Label"]` or `@Localizer["Scheme1"]`.

#### Pros

* Changing wording is consistent across all languages - edit the relevant resource file.
* Altering text for the default language doesn't alter the key - no change required to resource files other than the affected language.
* Sufficiently descriptive or structured keys can give hints to translators even when viewed in isolation from the app.

#### Cons

* Requires resource files even for the default language.
* Requires inventing keys with a structure or scheme, and raises the possibility of key choice as a nit during reviews.
* Code is more difficult to read and write - there's the cognitive burden of manually parsing and cross-referencing keys.
* Using semantic keys with `IStringLocalizer` eliminates its advantage over strongly typed resources.

### Literal based

Literal based localisation uses the default language value (in our case English) as the key. For example; `@Localizer["What is the child's name?"]` or `@Localizer["30 hours for working parents"]`.

#### Pros

* Resource files not needed for the default language.
* Code is easier to read and immediately shows intent and context.
* No need to invent keys.

#### Cons

* Altering the default text also changes the key - which means updating keys for all languages.
* Identical keys can't easily have different translations, although since the keys are scoped to the views or models collisions are unlikely.

## Decision Outcome

Chosen option: "Literal-based localisation".

* `IStringLocalizer` is designed to support this approach
* Microsoft [specifically describes this](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization/make-content-localizable?view=aspnetcore-8.0#istringlocalizer) as intended to improve productivity because default-language resource files are not required.
* If we wanted semantic keys, strongly typed generated resources would probably be preferable to stringly-typed semantic keys.
* It fits the current requirements: no immediate translation, there is only one additional language expected in the next phase and we do not anticipate many translation passes.
* The risk of identical English text needing different translations is reduced because resource files are scoped to small views/models.

> [!IMPORTANT]
> As always this decision can be revisited at a later date.

### Consequences

* Good, because easy and quick to implement.
* Good, because maintains translation capability.
* Bad, because changes to English copy will break localisation keys
* Bad, because identicial keys can't have different translations without reintroducing semantic (or non-literal) keys.
