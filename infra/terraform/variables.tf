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

variable "azure_frontdoor_scale" {
  description = "Azure Front Door Scale"
  type        = string
  default     = "Standard"
}

variable "custom_domain" {
  description = "Custom front-door domain"
  type        = string
  default     = ""
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

variable "enable_staging_slot" {
  description = "Enable staging slot for web app"
  type        = bool
  default     = false
}