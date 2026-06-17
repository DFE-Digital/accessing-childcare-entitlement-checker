---
title: Remove secrets from Git history
layout: page
showPagination: true
order: 3
sectionKey: Runbooks
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Runbooks
---
This runbook outlines the steps to permanently remove a sensitive secret (e.g., password, personal access token) from the Git history of the repository if it was accidentally committed.

## Step 1: Revoke the compromised secret
CRITICAL: Before rewriting git history, assume the secret has been fully compromised. You must immediately invalidate and rotate the secret on the host provider (e.g., Azure Portal, GitHub, NuGet) to prevent unauthorized access.

## Step 2: Rewrite the repository history (Using Git-filter-repo)
The recommended tool for erasing files and strings from git history is `git-filter-repo` (do not use `git filter-branch` as it is slow and deprecated).

### Option a: Remove a file containing the secret
If the secret is contained within a specific configuration file that should not have been committed:
```bash
git filter-repo --path path/to/secret-file.config --invert-paths
```

### Option b: Replace a specific secret string
If you need to replace a specific string (e.g., `MySuperSecretPassword`) with a placeholder like `REMOVED` across all commits:
1. Create a text file called `expressions.txt` containing the replacement rule:
   ```text
   MySuperSecretPassword==>REMOVED
   ```
2. Execute the replace filter:
   ```bash
   git filter-repo --replace-text expressions.txt
   ```

## Step 3: Force-push changes to GitHub
Since rewriting history changes all subsequent commit hashes, you must force-push the updated history to the remote repository.

*Ensure that branch protection rules are temporarily disabled on main/release branches before running:*
```bash
git push origin --force --all
git push origin --force --tags
```

## Step 4: Notify the team
1. Instruct all developers to delete their local clones of the repository and re-clone a fresh copy. Do not attempt to merge or pull, as doing so will re-introduce the bad history.
2. Re-enable all branch protection rules in GitHub.
