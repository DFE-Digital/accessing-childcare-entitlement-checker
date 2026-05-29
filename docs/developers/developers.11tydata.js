export default {
  layout: 'sub-navigation',
  sectionKey: 'Developers',
  eleventyComputed: {
    eleventyNavigation: {
      parent: (data) => {
        if (data.page.filePathStem.endsWith('/index')) {
          return 'Home'
        }
        return 'Developers'
      }
    }
  }
}
