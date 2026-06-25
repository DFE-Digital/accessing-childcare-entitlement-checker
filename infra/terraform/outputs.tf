output "resource_group_name" {
  value = azurerm_resource_group.web-rg.name
}

output "web_app_name" {
  value = azurerm_linux_web_app.web-app-service.name
}

output "frontdoor_hostname" {
  value = local.host_name
}

output "staging_slot_in_use" {
  value       = var.webapp_enable_staging_slot
  description = "Indicates whether the staging slot is enabled for the web app."
}
