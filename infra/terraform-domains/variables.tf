variable "service_domain_zone_name" {
  description = "Service domain zone hostname"
  type        = string
}

variable "service_domain_resource_group_name" {
  description = "Resource group for the service domain zone"
  type        = string
}

variable "caa_issuers" {
  description = "Certificate authorities that can issue certificates for the service domain"
  type        = list(string)
  default     = []
}
