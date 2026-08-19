---
title: Using Azure Cloud Shell
layout: sub-navigation
sectionKey: Tutorials
order: 7
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Tutorials
  key: Using Azure Cloud Shell
---
Let's learn how to use Azure Cloud Shell! In this tutorial, we will show you how to open, configure, and use Cloud Shell inside the Azure Portal. This is especially useful for team members who do not have the Azure CLI or Git installed locally.

## What is Azure Cloud Shell?

Azure Cloud Shell is a browser-based, interactive terminal that Azure hosts for us. We can use it to manage our Azure resources without installing any tools on our local workstation. It comes pre-installed with:
- **Azure CLI (az):** The command line tool to manage resources.
- **Git:** To clone our repositories and manage files.
- **Bash and PowerShell:** Standard shell environments.
- **Common utilities:** Tools like `curl`, `wget`, `zip`, and `jq`.

## 1. Launch Azure Cloud Shell

Let's open your first Cloud Shell session:

1. **Log in:** Go to the [Azure Portal](https://portal.azure.com) and log in with your DfE credentials.
2. **Click the Cloud Shell icon:** Find the **Cloud Shell** icon (`>_`) in the top-right toolbar next to the search bar. Click it.
3. **Select your environment:** A pane will slide open at the bottom of your screen. If this is your first time, you will see a prompt to choose an environment. Select **Bash**.
4. **Configure storage (First-time only):** 
   - Cloud Shell requires an Azure Storage Account to persist your files.
   - If prompted, select your assigned DfE subscription.
   - Click **Create storage**. Azure will automatically provision a small, low-cost storage share for you.

*Friendly tip: Your Cloud Shell session has a persistent `$HOME` directory. This directory is saved in your Azure Storage share. Any repositories you clone or files you create here will remain available the next time you log in!*

## 2. Clone the repository in Cloud Shell

Let's get our repository code into your Cloud Shell session. This lets us access our operational scripts easily:

1. **Copy the clone URL:** Copy the HTTPS clone URL from our GitHub repository.
2. **Run git clone:** In the Cloud Shell terminal, type `git clone` and paste the URL:
   ```bash
   git clone https://github.com/DFE-Digital/accessing-childcare-entitlement-checker.git
   ```
3. **Navigate to the repository:**
   ```bash
   cd accessing-childcare-entitlement-checker
   ```

## 3. Run your first operational command

Let's use the Azure CLI inside your Cloud Shell session to check the status of our Azure Front Door profile. This is a great way to verify that you are connected and authenticated:

1. **Verify your identity:** Azure Cloud Shell automatically logs you in using the credentials you used to log into the Portal. Run this command to verify your active account:
   ```bash
   az account show
   ```
2. **List Front Door profiles:** Let's see the Front Door profiles in your subscription:
   ```bash
   az afd profile list --query "[].{Name:name, ResourceGroup:resourceGroup}" --output table
   ```

## 4. Edit files using the built-in Cloud Shell Editor

Now that we have the repository cloned, let's learn how to view and edit files directly in your browser. Azure Cloud Shell includes a built-in text editor based on Monaco (the engine behind VS Code):

1. **Launch the editor:** Open the editor in your current directory by running:
   ```bash
   code .
   ```
   A file explorer and editor pane will open in the top half of your Cloud Shell window.
2. **Explore files:** Use the file explorer sidebar on the left to navigate through the repository folders. Click on any file (like a markdown file or script) to open it in the editor.
3. **Edit a file:** Make a minor non-code change if you wish (such as adding a comment to a markdown note).
4. **Save and close:**
   - Save your changes by pressing **Ctrl + S** (or **Cmd + S** on macOS).
   - Close the editor pane by pressing **Ctrl + Q** (or click the **...** menu in the top-right of the editor pane and select **Close Editor**).

This editor is extremely useful for making quick configuration adjustments or inspecting script contents directly in the cloud without needing a local development environment.

## Next steps

Now you know how to use Azure Cloud Shell! 

You can launch Cloud Shell anytime, clone repositories, run Azure CLI commands to inspect resources, and use the built-in editor to manage configuration files.

To learn more about how we utilize Cloud Shell during specific operational events, read our [Service shuttering runbook](/how-to/runbooks/service-shuttering/). 

To learn more about the shutter service architecture, read our [Shutter service explanation guide](/explanation/shutter-service/).
