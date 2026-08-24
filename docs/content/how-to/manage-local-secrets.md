---
title: Manage local secrets
layout: sub-navigation
sectionKey: How-to guides
order: 6
includeInBreadcrumbs: true
eleventyNavigation:
  parent: How-to guides
  key: Manage local secrets
---
Use the .NET Secret Manager to securely configure sensitive variables (such as basic authentication passwords) on your local machine without exposing them in source control.

## Initialise user secrets

To set up secret storage for the web application, execute the following commands from the repository root:

1. **Navigate to the web project directory:**
   ```bash
   cd src/AccessingChildcareEntitlementChecker.Web
   ```
2. **Initialise user secrets:**
   ```bash
   dotnet user-secrets init
   ```

*(Note: This creates a unique secrets identifier in your `AccessingChildcareEntitlementChecker.Web.csproj` file.)*

## Set local secret values

Store sensitive configuration keys using the `set` command.

### Set the local development basic auth password
To set the password required to access the application locally in non-production environments:

```bash
dotnet user-secrets set "DevelopmentBasicAuthPassword" "your_secure_password_here"
```

### Set connection strings or credentials
For other secret configuration keys (e.g., Redis connection strings):

```bash
dotnet user-secrets set "RedisConnection" "your_redis_connection_string"
```

## Verify configured secrets

### List active secrets
To output a list of all secrets stored for the current project:

```bash
dotnet user-secrets list
```

### Remove a secret
To delete a specific secret from your local store:

```bash
dotnet user-secrets remove "DevelopmentBasicAuthPassword"
```

### Clear all secrets
To purge all secrets for the current project:

```bash
dotnet user-secrets clear
```
