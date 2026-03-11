# Decision - 0005 - PDF Generation

## Context and Problem Statement

In order to meet the needs of young people without internet access (such as those young people within the justice system), 
we need to be able to allow the site to be printed and saved as a PDF, thus allowing the PDFs to be uploaded to intranets, or printed for portability.

In order to reduce cost and improve performance, the resulting PDF should be cached.

We looked into the following methods:

- PDF Generation using a commercial library
- PDF Generation using an open-source library
- PDF Generation using a free HTTP service
- PDF Generation using a paid-for HTTP service

We found that:

- Most libraries allowed templates, or direct plotting of elements, but not rendering from HTML to PDF
- Some online services could not properly parse the HTML from the site

## Considered Options

We considered the following options, but with a focus on speed of development and cost, as we had very little time and no budget

- [Iron PDF](https://ironpdf.com/)
- [Gotenberg.Dev](https://https://gotenberg.dev/)
- [Adobe PDF Services API](https://developer.adobe.com/document-services/apis/pdf-services/html-to-pdf/)
- [PuppeteerSharp](https://www.puppeteersharp.com/)
- [PDF Endpoint](https://pdfendpoint.com/)

### Evaluation

|     Criteria     | Comment                                                                                                                          | IronPDF | Gotenberg | Adobe | PuppeteerSharp | PDF Endpoint |
|:----------------:|:---------------------------------------------------------------------------------------------------------------------------------|:-------:|:---------:|:-----:|:--------------:|:------------:|
|       Cost       | The cost to implement should be as low as possible                                                                               |    1    |     5     |   4   |        5       |        4     |
| Development Cost | The implementation should not require any additional development cost, tooling, or changes to the site to render PDFs            |    4    |     1     |   4   |        1       |        4     |
|   Performance    | The generation should work with as little latency as possible, and not require larger machines or more memory to run             |    5    |     1     |   3   |        1       |        3     |
|     Layout       | The PDFs should allow rendering headers and footers, and should preserve the same layout as if the user printed from the browser |    5    |     3     |   1   |        5       |        5     |
|    **Total**     |                                                                                                                                  | **15**  |  **10**   |**12** |     **12**     |     **16**   |

## Decision Outcome

Based on the analysis above, we chose PDF Endpoint, as this worked out of the box with very little extra development.

The Free plan gave us enough credits to be able to render our PDFs and cache them for a month.

We discounted the others for the following reasons:

- IronPDF
  - Initial cost of $999 was too high
- Gotenberg and PuppeteerSharp
  - Additional cost of infrastructure and memory overhead to run this, plus performance hit was deemed to great
  - Extra development cost would have taken too long
- Adobe
  - Content within the Care Leavers site meant their API was unable to render our pages at all
- PDF Endpoint
  - Content worked well
  - Staging/dev API allowed unlimited calls for free
  - Free limit was good enough to meet our needs on a once-per-month cache

### Considerations on selected technology

This was approved in principal in discussions with the lead architect of Vulnerable Children and Families at the time.

Also fully discussed as part of the Tech Assessment within the assessment
