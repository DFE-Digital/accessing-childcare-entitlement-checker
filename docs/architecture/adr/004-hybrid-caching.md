# Decision - 0004 - Hybrid Caching

## Context and Problem Statement

We had some problems using pure distributed caching as discussed in [Decision - 001 - Caching Technology](001-caching-technology.md) where the site started getting timeouts and race conditions as we started to scale-up the performance testing

We found that:

- Some of our serialised data was quite large (over 150kb), which was on the larger side for Redis
- The standard Redis multiplexer was having issues creating new connections
- These connection timeouts were causing 500 errors, or very slow page rendering

As part of further investigations, we noted that:

- Redis itself didn't appear to be the problem - both CPU and memory usage were low

Whilst going through this process, we also found some other issues that we had discovered:

- Clearing the cache was very time consuming and slow (required IAM elevation in the Azure Portal and/or a Powershell script via deployment)
- Clearing related cache entries was cumbersome and required additional development

## Considered Options

- Switching back to in-memory caching
- [Microsoft HybridCache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid?view=aspnetcore-9.0)
- [FusionCache](https://github.com/ZiggyCreatures/FusionCache)

### Evaluation

|     Criteria     | Comment                                                                                                                          | In-memory | HybridCache | FusionCache |
|:----------------:|:---------------------------------------------------------------------------------------------------------------------------------|:---------:|:-----------:|:-----------:|
|     Adoption     | The implementation should we widely used and have excellent documentation                                                        |     5     |      3      |      5      | 
| Development Cost | The implementation should not require any additional development cost, tooling, or understanding of the backing cache            |     2     |      3      |      4      | 
|   Performance    | The caching should work with as little latency as possible across various environments                                           |     5     |      5      |      5      |
|     Scaling      | The caching should survive an application restart, deployment, or scaling automatically                                          |     4     |      1      |      4      |
|  Clearing Cache  | The caching should allow quick and easy clearing, plus support additional things like tagging for easier related content removal |     1     |      3      |      5      |
|    **Total**     |                                                                                                                                  |  **17**   |   **15**    |   **23**    |

## Decision Outcome

Based on the analysis above, we have chose to switch to FusionCache as this had better documentation and was, in fact, ahead of Microsoft in making itself HybridCache compatible (Microsoft hadn't yet released HybridCache themselves).

FusionCache allows us to support the following features:

- L1 cache in-memory
- L2 cache using IDistributedCache (in this case, Redis)
- Ability to use the Redis instance as a backplane to enable communication between nodes
- Tagging support
- Clearing the cache using the `*` tag

### Considerations on selected technology

During development, the cache can be deployed using L1 cache only, which allows application restarts to flush the cache, but still allow for testing of caching.

The performance of this has proven so good that we have enabled caching on the preview environment, and the webhooks invalidate cache entries on save or autosave of content in Contentful
