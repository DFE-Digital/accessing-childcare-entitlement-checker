terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.52"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "2.9.0"
    }
  }
  backend "azurerm" {
    container_name = "tfstate"
    key            = "accessing-childcare-entitlement-checker.tfstate"
    use_oidc       = true
  }
}

provider "azurerm" {
  features {}
}

provider "azapi" {}