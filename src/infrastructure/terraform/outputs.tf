output "resource_group_name" {
  value = azurerm_resource_group.web-rg.name
}

output "web_app_name" {
  value = azurerm_linux_web_app.web-app-service.name
}

output "web_app_slot_name" {
  value = azurerm_linux_web_app_slot.web-app-service-staging.name
}
