---
title: Decisions
layout: sub-navigation
sectionKey: Decisions
eleventyNavigation:
  key: Decisions
  parent: Home
---

The decision-making process follows the flow shown below:

```mermaid
flowchart BT

    C[Constraints]
    HC[Hard Constraint]
    SC[Soft Constraint]

    %% Hard constraints
    STD[Standards] --> HC
    AV[Availability] --> HC
    SCL[Scalability] --> HC
    HC --> C

    %% Soft constraints
    REC[Recruitment] --> SC
    MAI[Maintainability] --> SC
    SUP[Support] --> SC
    SC --> C

    %% Evaluation flow
    C --> OC{Options Comparison}

    OC -->|Selected| TS[Technology Selection]
    OC -->|Rejected| R[Reject]
    OC -->|Spike Required| RS[Run Spike]

    RS --> SD{Spike Successful?}

    SD -->|Yes| TS
    SD -->|No| R

    %% Styling
    classDef constraint fill:#f6e8b1,stroke:#b59b3a,color:#000;
    classDef process fill:#c8c3f2,stroke:#666,color:#000;
    classDef success fill:#cfe8c9,stroke:#5f8f5f,color:#000;
    classDef reject fill:#f3c7c7,stroke:#b36b6b,color:#000;

    class C,HC,SC,STD,AV,SCL,REC,MAI,SUP constraint;
    class OC,RS,SD process;
    class TS success;
    class R reject;
```

The "standards" hard constraint includes typical implementations with the wider DfE landscape, as well as GDS compliance, for example, the use of Azure is not a standard, but a hard constraint as the preferred cloud platform for the department.

We record all significant decisions made during the development of this service. Use the navigation menu to explore these decision details.