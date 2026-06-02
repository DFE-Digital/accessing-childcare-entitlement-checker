export default {
  layout: 'sub-navigation',
  sectionKey: 'Testing',
  eleventyComputed: {
    eleventyNavigation: {
      parent: (data) => {
        if (data.page.filePathStem.endsWith('/index')) {
          return 'Home'
        }
        return 'Testing'
      }
    }
  }
}
