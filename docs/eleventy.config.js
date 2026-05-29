import { govukEleventyPlugin } from '@x-govuk/govuk-eleventy-plugin'
import eleventyNavigationPlugin from '@11ty/eleventy-navigation'

export default function (eleventyConfig) {
  eleventyConfig.addPlugin(eleventyNavigationPlugin)
  eleventyConfig.addPlugin(govukEleventyPlugin, {
    templates: {
      sitemap: false,
      searchIndex: true
    },
    header: {
      logotype: {
        html: '<img src="/assets/department-for-education_white.png" alt="Department for Education" height="30">'
      },
      search: {
        indexPath: '/search-index.json'
      }
    },
    serviceNavigation: {
      navigation: [
        {
          text: 'Home',
          href: '/'
        },
        {
          text: 'Developers',
          href: '/developers/'
        },
        {
          text: 'Architecture',
          href: '/architecture/'
        },
        {
          text: 'Decisions',
          href: '/decisions/'
        },
        {
          text: 'Testing',
          href: '/testing/'
        },
      ]
    },
    footer: {
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

  // Global data
  eleventyConfig.addGlobalData('layout', 'page')

  // Passthrough
  eleventyConfig.addPassthroughCopy({ 'department-for-education_white.png': 'assets/department-for-education_white.png' })
  eleventyConfig.addPassthroughCopy({ 'mermaid.js': 'assets/mermaid.js' })
  eleventyConfig.addPassthroughCopy('**/*.{png,jpg,jpeg,gif,svg}')

  return {
    dataTemplateEngine: 'njk',
    htmlTemplateEngine: 'njk',
    markdownTemplateEngine: 'njk',
    dir: {
      // Input directory (relative to the config file)
      input: '.',
      // Output directory
      output: '_site'
    }
  }
}
