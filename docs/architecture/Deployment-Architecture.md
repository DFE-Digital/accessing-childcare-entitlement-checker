# Deployment Architecture

The build and release pipelines are all controlled via GitHub actions which act as our CI/CD process.

The actions are categorised into different types:
- **Validate**: Perform some sort of validation, such as running tests and are typically pass/fail
- **Generate**: Generate some sort of artifact, such as reporting
- **Deploy**: Perform a deployment to a target environment

## Build Pipeline

On creation of a Pull Request or push to main, multiple validation actions run, depending on what areas of the repo
have been modified. Any failures should be addressed before merging. Failures on main should be fixed as priority.

## Release Pipeline

The site is deployed via GitHub Actions.
> Before deploying a release, it is best to check if Contentful Migrations need to be applied first

Once the team is happy to publish a release, this can be done by running the "Deploy - Environment" action.

Most times the site is published, the cache will not need clearing, but if Contentful Migrations have changed the structure of any of the models, you will also need to select "True" when asked whether to clear the cache.

Publishing runs the following steps:

```mermaid
%%{ init: { 'flowchart': { 'curve': 'step' } } }%%
flowchart TD
    accDescr: Deployment process flow
    
    A["Build Docker Image"]-->B["Push Docker Image to Registry"]
    B-->C["Provision Terraform Infrastructure"]
    C-->D["Push Docker Image to Azure Web App deployment slot"]
    D-->E["Request Deployment Slot Swap"]
    E-->F{Is slot warmed up?}
    F--"No"-->F
    F--Yes-->G["Deployment slot swapped to Production"]
    G-->H["Deployment slot Deleted"]
```

## Contentful Models Deployment

Contentful Models are deployed via Contentful Migrations through the Contentful CLI. Because contentful and app service environments
are not one-to-one, the release process is managed seperately.

### App Service to Contentful Environment Mapping
```mermaid
%%{ init: { 'flowchart': { 'curve': 'stepAfter' } } }%%
flowchart TD
    accDescr: "App Service to Contentful Environment Mapping"
    
    A["Local Dev Environment"]-->Dev["Contentful - Dev"]
    B["Local/Remote E2E Tests"]-->E2E["Contentful - E2E"]
    C["App Service - Test"]-->Staging["Contentful - Test"]
    D["App Service - Staging"]--Preview-->Prod["Contentful - Production"]
    E["App Service - Production"]--Published-->Prod
```


### Contentful Migration Process
```mermaid
%%{ init: { 'flowchart': { 'curve': 'stepAfter' } } }%%
flowchart TD
    accDescr: "Contentful Migration Process"
    
    A["Install the CLI"]-->B["Login to Contentful"]
    B-->C["Running Migrations .NET App"]
    C-->D["Foreach migration"]
    D-->E{Is migration already applied?}
    E--"No"-->G{Any migrations left?}
    E--Yes-->F["Run Migration"]
    F-->G
    G--Yes-->D
    G--"No"-->H["Migration Complete"]
```
The Contentful migration process is executed either locally (in development), or using the GitHub Action "Deploy - Contentful Migrations", at which point you are asked which environment you wish to run the deployment against