output "zone_name" {
  value       = data.azurerm_dns_zone.service-zone.name
  description = "The name of the service.gov.uk DNS zone."
}

output "name_servers" {
  value       = data.azurerm_dns_zone.service-zone.name_servers
  description = "The name servers to give to GDS for the service.gov.uk delegation."
}
