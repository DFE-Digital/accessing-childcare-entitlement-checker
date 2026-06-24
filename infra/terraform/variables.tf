variable "elz_environment" {
  description = "The ELZ environment to match subscription (e.g. Dev)"
  type        = string
}

variable "environment_prefix" {
  description = "Environment prefix (e.g. d01)"
  type        = string
}

variable "aspnetcore_environment" {
  description = "ASP.NET Core environment"
  type        = string
}

variable "development_basic_auth_password" {
  description = "Shared password for development-only basic auth"
  type        = string
  sensitive   = true
  default     = ""
}

variable "azure_frontdoor_sku" {
  description = "Azure Front Door SKU"
  type        = string
  default     = "Standard"
}

variable "fd_use_private_link" {
  description = "Should use private link to connect to app service"
  type        = bool
  default     = false
}

variable "custom_domain" {
  description = "Custom front-door domain"
  type        = string
  default     = ""
}

variable "waf_enable_managed_rules" {
  description = "Enable managed rule sets in WAF"
  type        = bool
  default     = false
}

variable "webapp_sku" {
  description = "Web App SKU (e.g. B1)"
  type        = string
  default     = "B1"
}

variable "webapp_zone_balancing" {
  description = "Enable zone balancing on web app"
  type        = bool
  default     = false
}

variable "webapp_instance_count" {
  description = "The number of instances for the web app"
  type        = number
  default     = 1
}

variable "webapp_enable_staging_slot" {
  description = "Enable staging slot for web app"
  type        = bool
  default     = false
}

variable "webapp_enable_public_access" {
  description = "Enable public access for web app"
  type        = bool
  default     = true
}

variable "webapp_always_on" {
  description = "Set always for web app"
  type        = bool
  default     = false
}

variable "enable_web_test" {
  description = "Enable application insights web test"
  type        = bool
  default     = false
}

variable "location" {
  description = "The Azure region to deploy resources into"
  type        = string
  default     = "uksouth"
}

variable "location_short_code" {
  description = "The short code for the Azure region (e.g. uks)"
  type        = string
  default     = "uks"
}