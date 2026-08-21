---
title: Writing documentation
layout: sub-navigation
sectionKey: Tutorials
order: 4
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Tutorials
  key: Writing documentation
---
Welcome to our documentation site! Let's get you set up to contribute to these guides. 

In this tutorial, we'll walk you through running the documentation site on your local machine and creating your very first documentation page.

## 1. Start the local server

Our documentation is built using a static site generator called Eleventy. It takes our simple Markdown files and turns them into the beautifully styled website you're reading right now!

First, open your terminal and navigate to the `docs` folder:

```bash
cd docs
```

Next, let's install the required Node.js dependencies (you only need to do this the first time):

```bash
npm install
```

Finally, start up the local development server:

```bash
npm start
```

You should see a message in your terminal telling you the site is running. Open your web browser and navigate to `http://localhost:8080/`. You should see the documentation site running locally!

## 2. Create a new markdown file

Let's add a new page to the site. Our documentation follows the Diátaxis framework, which means every page belongs in one of four specific folders depending on its purpose:

- `tutorials/`: For friendly, step-by-step learning (like this page!).
- `how-to/`: For direct, action-driven problem solving.
- `explanation/`: For conceptual deep dives into our architecture.
- `reference/`: For austere, factual specifications and lists.

Let's create a new tutorial. Create a file named `my-first-doc.md` inside `docs/content/tutorials/`.

## 3. Add the frontmatter

For Eleventy to understand where to put your page in the navigation menu, you need to add a small block of YAML metadata (called "frontmatter") to the very top of your new file.

Add this to the top of `my-first-doc.md`:

```markdown
---
title: My first doc
layout: sub-navigation
sectionKey: Tutorials
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Tutorials
  key: My first doc
---
Hello world! I am writing documentation!
```

## 4. See it live!

Save your file and jump back to your web browser (still open at `http://localhost:8080/`).

Because you left the `npm start` server running, Eleventy automatically detected your new file and rebuilt the site in the background. Refresh your browser, look under the "Tutorials" section, and you will see your brand new page!

*Friendly tip: Want to understand the technical architecture behind how this documentation site is published? Check out the [Documentation explanation guide](/explanation/documentation-guide/).*
