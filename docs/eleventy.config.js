import { govukEleventyPlugin } from '@x-govuk/govuk-eleventy-plugin'

export default function(eleventyConfig) {
  eleventyConfig.addPlugin(govukEleventyPlugin, {
    titleSuffix: 'Accessing Childcare Entitlement Checker',
    templates: {
      sitemap: false,
      searchIndex: false
    },
    header: {
      logotype: {
        html: '<img src="/assets/images/department-for-education_white.png" alt="Department for Education" height="30">'
      }
    },
    serviceNavigation: {
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
        html: '<script src="/assets/mermaid.js" type="module"></script>'
      }
    }
  })

  eleventyConfig.addPassthroughCopy({ 'assets/department-for-education_white.png': 'assets/images/department-for-education_white.png' })
  eleventyConfig.addPassthroughCopy({ 'assets/mermaid.js': 'assets/mermaid.js' })

  const isProduction = process.env.ELEVENTY_ENV === "production";

  return {
    pathPrefix: isProduction ? "/accessing-childcare-entitlement-checker/" : "/",
    dataTemplateEngine: 'njk',
    htmlTemplateEngine: 'njk',
    markdownTemplateEngine: 'njk',
    dir: {
      input: 'content',
    }
  }
};