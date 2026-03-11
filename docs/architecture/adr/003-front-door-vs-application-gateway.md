# Decision - 0003 - Front Door vs Application Gateway

## Context and Problem Statement

The site needs to ensure it is protected against threats on the internet, but that it is also performant and scalable.

We needed to ensure:

* Infrastructure costs are as low as possible
* Infrastructure is simple and easy to manage
* The site will support custom domains and SSL
* The site can be scaled out and load balanced accordingly
* The site can be geo-locked to the UK
* The site still allows search engines to crawl the site (even if they are based in the USA, for example Google and Bing)

## Considered Options

* Azure Front Door
* Azure Application Gateway

### Evaluation

|  Criteria   | Comment                                                                                                                                                                                                                                                                                                                                       | Front Door | Application Gateway |
|:-----------:|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:----------:|:-------------------:|
|    Cost     | Front Door Premium is approximately 330 USD per month, Front Door Standard is 35 USD per month, and Application Gateway has a wide range of costs, but usually starting at around 330 USD per month                                                                                                                                           |     4      |          3          |
| Performance | Performance is on-par across the two solutions                                                                                                                                                                                                                                                                                                |     4      |          4          |
|  Security   | Front Door Premium supports additional managed rules, allowing for restrictions to be bypassed for friendly search engine bots. It also supports managed rulesets which adhere to WASP security guidelines. Application Gateway can do this, but at additional cost and usually involving a combination of Application Gateway and Front Door |     4      |          2          |
| Maintenance | Front Door allows for custom domains and SSL certificate generation. Application Gateway requires SSL certificates to be deployed and managed separately, increasing maintenance costs and effort, plus the risk of missing a renewal.                                                                                                        |     4      |          1          |
|  **Total**  |                                                                                                                                                                                                                                                                                                                                               |   **16**   |       **10**        |

## Decision Outcome

Based on the analysis above, we have chosen Azure Front Door to protect the site.

### Considerations on selected technology

Pros and cons had been taken to the DfE Architectural Group monthly meetings and, while Application Gateway does have some features that give it a bonus over Front Door, none of those features are being used within this service
