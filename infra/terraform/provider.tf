terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.52"
    }
  }
  backend "azurerm" {
    container_name       = "tfstate"
    key                  = "accessing-childcare-entitlement-checker.tfstate"
    use_oidc             = true
  }
}

provider "azurerm" {
  features {}
}
