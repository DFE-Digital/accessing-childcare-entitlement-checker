output "resource_group_name" {
  value = azurerm_resource_group.web-rg.name
}

output "web_app_name" {
  value = azurerm_linux_web_app.web-app-service.name
}

output "frontdoor_hostname" {
  value = local.host_name
}

output "deployment_storage_account_name" {
  value       = azurerm_storage_account.deployment_storage.name
  description = "The name of the deployment storage account."
}

output "deployment_storage_container_name" {
  value       = azurerm_storage_container.deployments.name
  description = "The name of the deployment blob container."
}

output "deployment_storage_blob_endpoint" {
  value       = azurerm_storage_account.deployment_storage.primary_blob_endpoint
  description = "The primary blob endpoint URL of the deployment storage account."
}

output "staging_slot_in_use" {
  value       = var.webapp_enable_staging_slot
  description = "Indicates whether the staging slot is enabled for the web app."
}
