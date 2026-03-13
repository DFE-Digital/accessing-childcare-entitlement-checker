variable "elz_environment" {
  description = "The ELZ environment to match subscription (e.g. Dev)"
  type        = string
}

variable "environment_prefix" {
  description = "Environment prefix (e.g. d01)"
  type        = string
}
/* 
variable "contentful_delivery_api_key" {
  description = "Contentful Delivery API Key"
  type        = string
  sensitive   = true
}

variable "contentful_preview_api_key" {
  description = "Contentful Preview API Key"
  type        = string
  sensitive   = true
}

variable "contentful_management_api_key" {
  description = "Contentful Management API Key"
  type        = string
  sensitive   = true
}


variable "contentful_space_id" {
  description = "Contentful Space ID"
  type        = string
  sensitive   = true
}

variable "contentful_environment" {
  description = "Contentful Environment"
  type        = string
}

variable "contentful_use_preview_api" {
  description = "Use Contentful Preview API?"
  type        = bool
}

variable "pdf_generation_use_sandbox" {
  description = "Generate PDFs in Sandbox Mode?"
  type        = bool
}

variable "pdf_generation_api_key" {
  description = "PDF Generation API Key"
  type        = string
  sensitive   = true
}
 */
variable "caching_type" {
  description = "Caching type"
  type        = string
}

variable "scripts_clarity" {
  description = "Clarity code"
  type        = string
}

variable "custom_domain" {
  description = "Custom front-door domain"
  type        = string
  default     = "abcdefghijklmnopqrstuvwxyz0123456789.azurefd.net"
}

variable "aspnetcore_environment" {
  description = "ASP.NET Core environment"
  type        = string
}

variable "azure_frontdoor_scale" {
  description = "Azure Front Door Scale"
  type        = string
  default     = "Standard_AzureFrontDoor"
}

variable "rebrand" {
  description = "Force DfE Rebrand before 25th June 2025"
  type        = bool
  default     = false
}

variable "gtaa_base_url" {
  description = "The base url for the 'Get-To-An-Answer' questionnaire service"
  type        = string
}

variable "support_alert_email" {
  description = "Where to send alert emails to"
  type        = string
  sensitive   = true
}

variable "alerting" {
  description = "Alerting configuration per environment"
  type = map(object({
    name                 = string
    alerts_enabled       = bool
    email_alerts_enabled = bool
    smart_alerts_enabled = bool
    thresholds = object({
      availability = number
      cpu          = number
      memory       = number
      error        = number
    })
  }))
  default = {
    d01 = {
      name                 = "Test"
      alerts_enabled       = false
      email_alerts_enabled = false
      smart_alerts_enabled = false
      thresholds = {
        availability = 90
        cpu          = 95
        memory       = 95
        error        = 5
      }
    }
    t01 = {
      name                 = "Staging"
      alerts_enabled       = true
      email_alerts_enabled = false
      smart_alerts_enabled = true
      thresholds = {
        availability = 99.9
        cpu          = 85
        memory       = 85
        error        = 1
      }
    }
    p01 = {
      name                 = "Production"
      alerts_enabled       = true
      email_alerts_enabled = true
      smart_alerts_enabled = true
      thresholds = {
        availability = 99.9
        cpu          = 85
        memory       = 85
        error        = 1
      }
    }
  }
}
