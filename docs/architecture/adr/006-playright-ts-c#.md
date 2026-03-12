# Decision - 0002 - Translation Provider

## Context and Problem Statement

As approximately 25% of all young people accessing the site do not have English as a first language, we needed the ability to allow users to translate the site.

We needed to ensure:

* Infrastructure costs are as low as possible
* The content could be cached
* We reduced the need to procure a solution
* As many languages were supported as possible
* The translations took into account context within the page

## Considered Options

* Google Translate
* Microsoft Translate

### Evaluation

|     Criteria     | Comment                                                                                                                                                                                                                                           | Google Translate | Microsoft Translate |
|:----------------:|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:----------------:|:-------------------:|
|       Cost       | The cost of Google translate is approximately 20 USD per million characters and would be passed on to the Care Leavers team. The cost of Microsoft Translate is 10 USD per million characters, but would be cross-billed to the DfE AI department |        2         |          4          |
|   Performance    | The translation should work with as little latency as possible across various environments and support being cached                                                                                                                               |        4         |          4          |
| Language Support | Google Translate supports 249 languages, whereas Microsoft Translate supports 179                                                                                                                                                                 |        4         |          3          |
|    **Total**     |                                                                                                                                                                                                                                                   |      **10**      |       **11**        |

## Decision Outcome

Based on the analysis above, we have chosen Microsoft Translate to enable translation for the project.

### Considerations on selected technology

This has been discussed at length with the AI Group and is being used as proof that the translation and AI features will work within DfE.

Because of this, the AI Team are taking on the cost of this, which reduces the impact on the Care Leavers team during this time of unknown budget contraints.