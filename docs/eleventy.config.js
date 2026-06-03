import { govukEleventyPlugin } from '@x-govuk/govuk-eleventy-plugin'
import eleventyNavigationPlugin from '@11ty/eleventy-navigation'

export default function (eleventyConfig) {
  eleventyConfig.addPlugin(eleventyNavigationPlugin)
  eleventyConfig.addPlugin(govukEleventyPlugin, {
    titleSuffix: 'Accessing Childcare Entitlement Checker',
    templates: {
      sitemap: false,
      searchIndex: true
    },
    header: {
      logotype: {
        html: '<img src="/assets/department-for-education_white.png" alt="Department for Education" height="30">'
      },
      search: {
        indexPath: 'search-index.json'
      }
    },
    stylesheets: [ '/assets/styles.css' ],
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

  // Global data
  eleventyConfig.addGlobalData('layout', 'page')

  // Passthrough
  eleventyConfig.addPassthroughCopy({ 'department-for-education_white.png': 'assets/department-for-education_white.png' })
  eleventyConfig.addPassthroughCopy({ 'mermaid.js': 'assets/mermaid.js' })
  eleventyConfig.addPassthroughCopy('**/!(node_modules)/**/*.{png,jpg,jpeg,gif,svg}')

  eleventyConfig.addTransform('fix-relative-paths', function (content) {
    if (this.page.outputPath && this.page.outputPath.endsWith('.html')) {
      const depth = this.page.url.split('/').filter(Boolean).length
      const relativeRoot = '../'.repeat(depth) || './'
      return content
        .replaceAll('src="/', `src="${relativeRoot}`)
        .replaceAll('href="/', `href="${relativeRoot}`)
        .replace('index="search-index.json"', `index="${relativeRoot}search-index.json"`)
    }
    return content
  })

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
