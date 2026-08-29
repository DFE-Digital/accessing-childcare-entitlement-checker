---
title: Creating a new page
layout: sub-navigation
sectionKey: Tutorials
order: 3
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Tutorials
  key: Creating a new page
---
Let's build a new page for our web application! In this tutorial, we'll add a frontend page using GovUK components. We'll also set up the routing and write a quick test to make sure it loads perfectly.

## 1. Create the controller

First, we need an MVC Controller to handle the web request and return our new page.

Navigate to `src/Dfe.Acec.Web/Controllers` and create a new file named `NewJourneyPageController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Controllers;

[Route("new-journey-page")]
public class NewJourneyPageController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        // For this simple page, we are just returning the view!
        return View();
    }
}
```

## 2. Create the view

Now, let's create the actual HTML view that the user will see. We use Razor views combined with special GovUK ASP.NET Core Tag Helpers to make building accessible pages super easy.

Navigate to `src/Dfe.Acec.Web/Views/NewJourneyPage` (you may need to create this folder) and create a file named `Index.cshtml`:

```html
@{
    ViewData["Title"] = "Our new journey page";
}

<govuk-grid-row>
    <govuk-grid-column width-two-thirds="true">
        
        <h1 class="govuk-heading-l">
            Welcome to the new journey page!
        </h1>
        
        <p class="govuk-body">
            This page was built using GovUK frontend components.
        </p>

        <govuk-button href="/next-step">
            Continue
        </govuk-button>

    </govuk-grid-column>
</govuk-grid-row>
```

## 3. Write an E2E test

It's a great habit to write a quick automated test for every new page to ensure it doesn't break in the future! We use Playwright for our End-to-End (E2E) tests.

Open up your E2E test project and add a new scenario for your page (for example, in `tests/Dfe.Acec.Tests.E2e/Features/NewJourneyPage.feature`):

```gherkin
Feature: New Journey Page
  As a user
  I want to visit the new journey page
  So that I can continue my application

  Scenario: The page loads successfully
    Given I navigate to "/new-journey-page"
    Then I should see the heading "Welcome to the new journey page!"
```

That's it! You've successfully built a brand new, accessible, and tested page in the application.
