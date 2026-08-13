---
title: Remove secrets from Git history
layout: sub-navigation
order: 3
sectionKey: How-to guides
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
  key: Remove secrets from Git history
---
Follow this runbook to permanently remove a sensitive secret (e.g., password, personal access token) from the Git history of the repository if you accidentally committed it.

## Step 1: Revoke and rotate the compromised secret

Assume the secret is fully compromised. Invalidate and rotate the secret immediately on the host provider (e.g., Azure Portal, GitHub, NuGet) to block unauthorised access before rewriting the Git history.

## Step 2: Rewrite the repository history using git-filter-repo

Use `git-filter-repo` to erase files and strings from Git history. Do not use `git filter-branch` (it is slow and deprecated).

### Option A: Remove a file containing the secret
If the secret is contained within a specific configuration file that should not have been committed, run:
```bash
git filter-repo --path path/to/secret-file.config --invert-paths
```

### Option B: Replace a specific secret string
If you need to replace a specific string (e.g., `MySuperSecretPassword`) with a placeholder like `REMOVED` across all commits, follow these steps:
1. Create a text file called `expressions.txt` containing the replacement rule:
   ```text
   MySuperSecretPassword==>REMOVED
   ```
2. Execute the replace filter:
   ```bash
   git filter-repo --replace-text expressions.txt
   ```

## Step 3: Force-push your changes to GitHub

Force-push the updated history to the remote repository. Note that rewriting history changes all subsequent commit hashes.

Temporarily disable branch protection rules on `main` and release branches before running:
```bash
git push origin --force --all
git push origin --force --tags
```

## Step 4: Notify the team and re-enable protection rules

1. Instruct all developers to delete their local clones of the repository and clone a fresh copy. Do not attempt to merge or pull, as doing so re-introduces the compromised history.
2. Re-enable all branch protection rules in GitHub.
