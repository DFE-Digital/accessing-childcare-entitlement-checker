resource "azurerm_virtual_network" "vnet" {
  name                = "${local.service_prefix}-vnet"
  location            = var.location
  resource_group_name = azurerm_resource_group.web-rg.name
  address_space       = ["10.0.0.0/16"]
  tags                = local.common_tags
}

resource "azurerm_network_security_group" "app_nsg" {
  name                = "${local.service_prefix}-app-nsg"
  location            = var.location
  resource_group_name = azurerm_resource_group.web-rg.name
  tags                = local.common_tags
}

resource "azapi_resource" "app_subnet" {
  type      = "Microsoft.Network/virtualNetworks/subnets@2024-05-01"
  name      = "${local.service_prefix}-app-subnet"
  parent_id = azurerm_virtual_network.vnet.id

  body = {
    properties = {
      addressPrefixes = ["10.0.1.0/24"]
      delegations = [{
        name = "asp-delegation"
        properties = {
          serviceName = "Microsoft.Web/serverFarms"
        }
      }]
      networkSecurityGroup = {
        id = azurerm_network_security_group.app_nsg.id
      }
      serviceEndpoints = [{
        service = "Microsoft.Storage"
      }]
    }
  }

  depends_on = [
    azurerm_network_security_group.app_nsg,
    azurerm_virtual_network.vnet
  ]
}

resource "azurerm_network_security_group" "pe_nsg" {
  name                = "${local.service_prefix}-pe-nsg"
  location            = var.location
  resource_group_name = azurerm_resource_group.web-rg.name
  tags                = local.common_tags
}

resource "azapi_resource" "pe_subnet" {
  type      = "Microsoft.Network/virtualNetworks/subnets@2024-05-01"
  name      = "${local.service_prefix}-private-endpoint-subnet"
  parent_id = azurerm_virtual_network.vnet.id

  body = {
    properties = {
      addressPrefixes = ["10.0.2.0/24"]
      networkSecurityGroup = {
        id = azurerm_network_security_group.pe_nsg.id
      }
      privateEndpointNetworkPolicies = "NetworkSecurityGroupEnabled"
    }
  }

  depends_on = [
    azurerm_network_security_group.pe_nsg,
    azurerm_virtual_network.vnet,
    azapi_resource.app_subnet
  ]
}

resource "azurerm_monitor_diagnostic_setting" "app_nsg_logs" {
  name                       = "${var.environment_prefix}-app-nsg-diagnostics"
  target_resource_id         = azurerm_network_security_group.app_nsg.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_log {
    category = "NetworkSecurityGroupEvent"
  }

  enabled_log {
    category = "NetworkSecurityGroupRuleCounter"
  }
}

resource "azurerm_monitor_diagnostic_setting" "pe_nsg_logs" {
  name                       = "${var.environment_prefix}-pe-nsg-diagnostics"
  target_resource_id         = azurerm_network_security_group.pe_nsg.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_log {
    category = "NetworkSecurityGroupEvent"
  }

  enabled_log {
    category = "NetworkSecurityGroupRuleCounter"
  }
}

resource "azurerm_monitor_diagnostic_setting" "vnet_logs" {
  name                       = "${var.environment_prefix}-vnet-diagnostics"
  target_resource_id         = azurerm_virtual_network.vnet.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_log {
    category = "VMProtectionAlerts"
  }

  enabled_metric {
    category = "AllMetrics"
  }
}