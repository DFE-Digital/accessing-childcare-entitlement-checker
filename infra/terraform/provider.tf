terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.52"
    }
  }
  backend "azurerm" {}
}

provider "azurerm" {
  features {}
}
