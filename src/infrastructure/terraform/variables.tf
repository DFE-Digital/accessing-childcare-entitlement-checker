variable "elz_environment" {
  description = "The ELZ environment to match subscription (e.g. Dev)"
  type        = string
}

variable "environment_prefix" {
  description = "Environment prefix (e.g. d01)"
  type        = string
}


# variable "aspnetcore_environment" {
#   description = "ASP.NET Core environment"
#   type        = string
# }

variable "azure_frontdoor_scale" {
  description = "Azure Front Door Scale"
  type        = string
  default     = "Standard_AzureFrontDoor"
}

variable "custom_domain" {
  description = "Custom front-door domain"
  type        = string
  default     = ""
}