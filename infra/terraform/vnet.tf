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

resource "azurerm_subnet" "app_subnet" {
  name                 = "${local.service_prefix}-app-subnet"
  resource_group_name  = azurerm_resource_group.web-rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.1.0/24"]

  delegation {
    name = "app-service-delegation"
    service_delegation {
      name    = "Microsoft.Web/serverFarms"
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
    }
  }
}

resource "azurerm_subnet_network_security_group_association" "app_nsg_association" {
  subnet_id                 = azurerm_subnet.app_subnet.id
  network_security_group_id = azurerm_network_security_group.app_nsg.id
}

resource "azurerm_subnet" "pe_subnet" {
  name                 = "${local.service_prefix}-pe-subnet"
  resource_group_name  = azurerm_resource_group.web-rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.2.0/24"]
}

resource "azurerm_subnet_network_security_group_association" "pe_nsg_association" {
  subnet_id                 = azurerm_subnet.pe_subnet.id
  network_security_group_id = azurerm_network_security_group.app_nsg.id
}