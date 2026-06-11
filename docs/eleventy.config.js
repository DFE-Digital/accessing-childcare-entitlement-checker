import { govukEleventyPlugin } from '@x-govuk/govuk-eleventy-plugin'
import { pathPrefix } from "./path-prefix.js";

export default function(eleventyConfig) {
  eleventyConfig.addPassthroughCopy("content/assets/images");
  eleventyConfig.addPassthroughCopy("content/assets/js")

  eleventyConfig.addPlugin(govukEleventyPlugin, {
    stylesheets: ['/assets/styles.css'],
    titleSuffix: 'Accessing Childcare',
    templates: {
      sitemap: true,
      searchIndex: true
    },
    header: {
      logotype: {
        html: '<img src="/assets/images/department-for-education_white.png" alt="Department for Education">'
      },
      search: {
        indexPath: `${pathPrefix}search-index.json`,
        sitemapPath: '/sitemap'
      }
    },
    serviceNavigation: {
      serviceName: 'Accessing Childcare',
      navigation: [
        {
          text: 'Developers',
          href: '/developers/'
        },
        {
          text: 'Architecture',
          href: '/architecture/'
        },
        {
          text: 'Testing',
          href: '/testing/'
        },
        {
          text: 'Decisions',
          href: '/decisions/'
        },
        {
          text: 'Operational',
          href: '/operational/'
        },
        {
          text: 'Runbooks',
          href: '/runbooks/'
        },
      ]
    },
    footer: {
      logo: false,
      meta: {
        items: [
          {
            href: 'https://github.com/DFE-Digital/accessing-childcare-entitlement-checker',
            text: 'GitHub repository'
          }
        ],
        html: '<script src="/assets/js/mermaid.js" type="module"></script>'
      }
    }
  })

  return {
    pathPrefix: pathPrefix,
    dataTemplateEngine: 'njk',
    htmlTemplateEngine: 'njk',
    markdownTemplateEngine: 'njk',
    dir: {
      input: 'content',
    }
  }
};