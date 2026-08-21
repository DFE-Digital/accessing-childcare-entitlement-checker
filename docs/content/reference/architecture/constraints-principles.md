---
title: Constraints and principles
layout: sub-navigation
sectionKey: Reference
order: 2
includeInBreadcrumbs: true
eleventyNavigation:
  parent: Architecture
  key: Constraints and principles
---
Architectural principles and constraints for the project are defined in this reference.

## Architectural principles

|   ID    | Principal                                                                                                                                                                                    |
|:-------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ARCH001 | Appropriate tools and technologies - Tool and technology selections align with cost-effective services and prioritise automation where possible.                                             |
| ARCH002 | Enterprise Architecture Alignment - The proposed solution is aligned with the Enterprise Architecture and stated services and interfaces.                                                    |
| ARCH003 | NFR Alignment - The proposed solution is designed to meet the specified Non-Functional Requirements (NFRs).                                                                                  |
| ARCH004 | Meets user needs - Services are developed based on user research and understanding of user requirements throughout the project lifecycle.                                                   |
| ARCH005 | Environment strategy is clear - The environments (e.g. development, test, staging, and production) and code progression through them, including release approval, are defined and established. |
| ARCH006 | Auto-scale by Design - The system is designed to scale dynamically with demand. Lower environments are scaled down during off-peak hours.                                                    |
| ARCH007 | Make things accessible and inclusive - Technology, infrastructure, and systems are designed to be accessible and inclusive for all users.                                                     |
| ARCH008 | Be open and use open source - Publishing code and utilising open-source software improves transparency, flexibility, and accountability.                                                     |
| ARCH009 | Make use of open standards - Technology is built using open standards to ensure compatibility, communication with external systems, and ease of upgrades or expansion.                      |
| ARCH010 | Use cloud first - The strategy prioritises public cloud solutions in the order of SaaS, PaaS, and IaaS.                                                                                      |
| ARCH011 | Secure by design - The system protects systems and data by utilising appropriate security controls.                                                                                          |
| ARCH012 | Make privacy integral - User rights are protected by incorporating privacy controls as an integral component of the system design.                                                           |
| ARCH013 | Share, reuse and collaborate - Government collaboration prioritises sharing and reusing technology, data, and services. This approach avoids duplicate efforts and unnecessary costs.        |
| ARCH014 | Integrate and adapt technology - System technologies are compatible with existing organizational processes, infrastructure, and tools, and are adaptable to future requirements.             |
| ARCH015 | Meets the GDS Service Standard - Developed services are compliant with the GDS Service Standard.                                                                                             |
| ARCH016 | Infrastructure as Code - Build, deployment, infrastructure, and networking are managed via scripted automation using Infrastructure as Code (IaC) and CI/CD pipelines.                       |
| ARCH017 | IT Health Checked - Externally available services must undergo an IT Health Check (ITHC) from a CHECK Accredited ITHC supplier prior to exposing production interfaces to the internet.       |

## Constraints

|   ID    | Constraint                                                                                                                                                            |
|:-------:|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| CONS001 | Deployed to Azure.                                                                                                                                                    |
| CONS002 | Technology selection is aligned with the availability of required skills in the specified locales.                                                                    |
| CONS003 | Compliance with the [Government Technology Code of practice](https://www.gov.uk/government/publications/technology-code-of-practice/technology-code-of-practice) is maintained. |
| CONS004 | Adherence to defined project budget and cost constraints.                                                                                                             |
| CONS005 | Adherence to defined project delivery schedules and timelines.                                                                                                       |
