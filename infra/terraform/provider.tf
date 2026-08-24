terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 5.0"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "2.12.0"
    }
  }
  backend "azurerm" {
    container_name   = "tfstate"
    key              = "accessing-childcare-entitlement-checker.tfstate"
    use_oidc         = true
    use_azuread_auth = true
  }
}

provider "azurerm" {
  features {}
  storage_use_azuread             = true
  resource_provider_registrations = "none"
}

provider "azapi" {}