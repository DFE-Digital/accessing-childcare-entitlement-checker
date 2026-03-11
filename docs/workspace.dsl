workspace "Childcare Eligibility" "Architecture for the Childcare Eligibility Service" {

    model {
        user = person "User / Citizen" "A parent or guardian checking eligibility for childcare support."
        
        azure = softwareSystem "Azure Online Landing Zone" "The cloud environment hosting the service." {
            
            group "Identity & Security" {
                # Tagged 'Internal' to allow selective hiding in high-level views
                entra = container "Microsoft Entra ID" "Identity Provider" "Acts as the Trust Authority. Issues tokens for secretless access." "Azure Entra, Internal"
                keyVault = container "Azure Key Vault" "Secret Management" "Securely stores sensitive configuration and keys." "Azure Key Vault" {
                    url "https://github.com/dfe-digital/childcare-eligibility/blob/main/docs/architecture/security.md"
                }
            }
            
            group "Application Logic" {
                appService = container "Childcare Web App" ".NET 10 MVC" "Evaluates user input against cached rules and renders the UI." "Docker Container" {
                    url "https://github.com/dfe-digital/childcare-eligibility/tree/main/src"
                }
            }

            group "Data & State" {
                redis = container "Azure Cache for Redis" "Distributed Cache" "Stores user sessions and a 24-hour mirror of journey rules." "Azure Redis"
                blobStorage = container "Azure Blob Storage" "Master Rules Store" "Primary source of truth for journeys.json rulesets." "Azure Storage"
            }
        }

        # RELATIONSHIPS (Logical Flow)
        user -> appService "Accesses service" "HTTPS"
        appService -> redis "Manages session state" "RESP (TLS 1.2)"
        appService -> blobStorage "Hydrates cache (24h expiry)" "HTTPS/Managed Identity"
        appService -> keyVault "Retrieves secrets" "HTTPS/Managed Identity"
        appService -> entra "Requests identity tokens" "OAuth2 (IMDS)"

        # DEPLOYMENT NODES
        deploymentEnvironment "Production" {
            
            deploymentNode "External Connectivity" "Public DNS" "Internet" {
                dnsRouter = infrastructureNode "DNS Router" "Global DNS resolution and traffic routing." "DNS, RedText"
            }

            deploymentNode "Azure Edge Services" "Security & Acceleration" "Global" {
                fdNode = infrastructureNode "Front Door & WAF" "Global Load Balancer and WAF protection." "Azure FD"
            }

            deploymentNode "UK South (Regional Services)" "Managed Infrastructure" "Azure Region" {
                
                deploymentNode "Compute Cluster" "App Service Environment" "Linux (P1v3)" {
                    workerNode = infrastructureNode "App Service Worker" "Scaled instances hosting the Docker container." "PaaS"
                    containerInstance appService
                }

                deploymentNode "Managed Data Services" "Shared PaaS" "Azure" {
                    containerInstance redis
                    containerInstance blobStorage
                    containerInstance keyVault
                    containerInstance entra
                }
            }

            # Physical Routing
            dnsRouter -> fdNode "Forwards traffic"
            fdNode -> workerNode "Routes to origin"
        }
    }

    views {
        # View 1: System Landscape (Aligned with Care Leavers)
        container azure "ContainerView" "The logical architecture of the Childcare Eligibility service." {
            include *
            exclude "element.tag==Internal"
            # Tightened spacing (from 400 to 250) to make elements appear larger
            autoLayout lr 250 250
        }

        # View 2: Infrastructure (Flattened to prevent overlapping)
        deployment azure "Production" "Infrastructure_Deployment" {
            include *
            exclude "element.tag==Internal"
            # Tightened vertical stack (from 500 to 300) to improve legibility without zooming
            autoLayout tb 300 300
        }

        styles {
            element "Element" {
                color #ffffff
            }
            element "Person" {
                shape Person
                background #08427b
            }
            element "Container" {
                background #0072c6
            }
            element "Azure Key Vault" {
                shape Cylinder
            }
            element "Azure Storage" {
                shape Cylinder
            }
            element "Azure Redis" {
                shape Cylinder
            }
            element "Docker Container" {
                shape Cylinder
                background #2496ed
            }
            element "RedText" {
                color #ff0000
            }
        }
    }
}