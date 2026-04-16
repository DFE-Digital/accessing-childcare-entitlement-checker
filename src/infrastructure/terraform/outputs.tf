output "resource_group_name" {
  value = azurerm_resource_group.web-rg.name
}

output "web_app_name" {
  value = azurerm_linux_web_app.web-app-service.name
}

output "frontdoor_hostname" {
  value = var.custom_domain != "" ? var.custom_domain : azurerm_cdn_frontdoor_endpoint.frontdoor-web-endpoint.host_name
}
