---
title: Making infrastructure changes
layout: sub-navigation
sectionKey: Tutorials
order: 5
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Tutorials
  key: Making infrastructure changes
---
Let's learn how to safely make and verify changes to our cloud infrastructure! In this tutorial, we'll navigate to our infrastructure code and make a simple change. Then, we'll run formatting and validation tools to check your work before pushing.

By the end of this guide, you'll know how to run a local health check on your Terraform configurations.

## 1. Navigate to the infrastructure folder

All of our infrastructure as code (IaC) is written using Terraform and is kept in a dedicated folder in our repository. 

Open your terminal and navigate to the Terraform directory:

```bash
cd infra/terraform
```

## 2. Initialise Terraform locally

Before we can ask Terraform to validate our code, we need to run a quick initialisation. To make things easy, we can run this without needing to log in to Azure or fetch cloud state.

Run the following command to initialise Terraform without a backend:

```bash
terraform init -backend=false
```

This will quickly download any required providers and set up a local workspace for verification.

## 3. Make a simple change

Let's make a mock change to see our verification tools in action. 

Open `infra/terraform/locals.tf` in your IDE. Look for the `locals` block and let's add a temporary tag to play with. We'll add some untidy spacing to see if our formatting tool can fix it for us!

```hcl
locals {
  # Add this with wonky spacing on purpose!
  test_tag     =      "onboarding-tutorial"
}
```

Save the file.

## 4. Let Terraform format your code

To keep our codebase clean, readable, and uniform, we enforce formatting rules. Rather than formatting manually, we can let Terraform do all the heavy lifting!

Run this command to automatically clean up the spacing and alignment in all your Terraform files:

```bash
terraform fmt
```

If you open `locals.tf` again, you'll notice that Terraform has magically aligned your spacing!

## 5. Validate your changes

Now that our code looks neat, let's make sure there are no typos, syntax errors, or broken resource references.

Run the validation tool:

```bash
terraform validate
```

If everything is correct, Terraform will reward you with a success message: `Success! The configuration is valid.`

Once you're happy with how the tools work, don't forget to revert your temporary `test_tag` local variable in `locals.tf` before committing.

*Friendly tip: To understand the architectural design, variable blending strategies, and security checks that protect our infrastructure, check out the [Infrastructure as code explanation](/explanation/infrastructure-as-code/).*
