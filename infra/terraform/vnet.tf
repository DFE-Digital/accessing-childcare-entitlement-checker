resource "azurerm_virtual_network" "vnet" {
  name                = "${local.service_prefix}-vnet"
  location            = local.location
  resource_group_name = azurerm_resource_group.web-rg.name
  address_space       = ["10.0.0.0/16"]
  tags                = local.common_tags
}

resource "azurerm_network_security_group" "app_nsg" {
  name                = "${local.service_prefix}-app-nsg"
  location            = local.location
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
    }
  }

  depends_on = [
    azurerm_network_security_group.app_nsg,
    azurerm_virtual_network.vnet
  ]
}

resource "azurerm_network_security_group" "pe_nsg" {
  name                = "${local.service_prefix}-pe-nsg"
  location            = local.location
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
  ]
}