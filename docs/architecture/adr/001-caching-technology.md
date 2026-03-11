# Decision - 0001 - Caching Technology

## Context and Problem Statement

Due to complex models we needed to think about how we could implement caching to ensure that we didn't hit any problems with Contentful rate limiting.

Also, because the site was due to use a form of translation, and this translation has the potential to become expensive, we wanted to cache the responses from the translation layer to ensure reduced costs.

Other projects have implemented caching, but this has been in-memory and incurred a penalty whenever the application service was restarted or scaled out.

We needed to ensure:

* Infrastructure costs are as low as possible
* There was an obvious performance improvement
* The cache would survive an application service restart or scaling
* The backing location for the cache could be altered if required
* The cache could be easily disabled for testing and preview purposes
* The caching did not require any additional installation for local development

## Considered Options

* Direct caching to Redis
* In-memory caching
* Distributed caching using IDistributedCache (supporting multiple backplanes such as in-memory, redis, cosmos DB, etc)

### Evaluation

|      Criteria       | Comment                                                                                                                                                                                                     | Redis Direct | In-memory | IDistributedCache |
|:-------------------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:------------:|:---------:|:-----------------:|
|     Suitability     | The data being cached is mostly serialised Contentful Responses and is, for the most part, cached per page. Some additional responses are cached per ID, and we store cached data for URLs and breadcrumbs. |      4       |     4     |         4         |
| Infrastructure Cost | The implementation should cost as little as possible to run and maintain                                                                                                                                    |      3       |     4     |         3         | 
|  Development Cost   | The implementation should not require any additional development cost, tooling, or understanding of the backing cache                                                                                       |      2       |     4     |         4         | 
|     Performance     | The caching should work with as little latency as possible across various environments                                                                                                                      |      3       |     4     |         4         |
|       Scaling       | The caching should survive an application restart, deployment, or scaling automatically                                                                                                                     |      4       |     1     |         4         |
|     Flexibility     | The caching should be configurable per environment, only using extra resource where specifically needed. This includes things like End to End test environments                                             |      2       |     1     |         4         |
|      **Total**      |                                                                                                                                                                                                             |    **18**    |  **18**   |      **23**       |

## Decision Outcome

Based on the analysis above, we have chosen IDistrubutedCache as the caching mechanism for the project. 

### Considerations on selected technology

During development, the cache is deployed using the in-memory backplane, which allows application restarts to flush the cache, but still allow for testing of caching.
The preview environment has caching turned off to allow real-time updates to be seen.