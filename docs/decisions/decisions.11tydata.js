export default {
  layout: 'sub-navigation',
  sectionKey: 'Decisions',
  eleventyComputed: {
    eleventyNavigation: {
      parent: (data) => {
        if (data.page.filePathStem.endsWith('/index')) {
          return 'Home'
        }
        return 'Decisions'
      }
    }
  }
}
