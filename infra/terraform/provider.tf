terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.52"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "2.11.0"
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
  storage_use_azuread = true
}

provider "azapi" {}