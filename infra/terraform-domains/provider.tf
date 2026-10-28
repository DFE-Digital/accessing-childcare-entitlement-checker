terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "5.10.0"
    }
  }
  backend "azurerm" {
    container_name   = "tfstate"
    key              = "domains.tfstate"
    use_oidc         = true
    use_azuread_auth = true
  }
}

provider "azurerm" {
  features {}
  storage_use_azuread             = true
  resource_provider_registrations = "none"
}
