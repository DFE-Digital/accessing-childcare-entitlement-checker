terraform {
  required_version = ">= 1.6.0"

  backend "azurerm" {}

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
  }
}

provider "azurerm" {
  features {}
  resource_provider_registrations = "none"
}

variable "resource_group_name" {
  type        = string
  description = "Resource group name for the web app resources."
}

variable "app_service_plan_name" {
  type        = string
  description = "App Service Plan name."
}

variable "web_app_name" {
  type        = string
  description = "Globally unique Linux Web App name."
}

variable "plan_sku" {
  type        = string
  description = "The SKU of the App Service Plan. E.g. F1, D1, B1"
  default     = "F1"
}

data "azurerm_resource_group" "this" {
  name = var.resource_group_name
}

resource "azurerm_service_plan" "this" {
  name                = var.app_service_plan_name
  location            = data.azurerm_resource_group.this.location
  resource_group_name = data.azurerm_resource_group.this.name
  os_type             = "Linux"
  sku_name            = var.plan_sku
}

resource "azurerm_linux_web_app" "this" {
  name                = var.web_app_name
  location            = data.azurerm_resource_group.this.location
  resource_group_name = data.azurerm_resource_group.this.name
  service_plan_id     = azurerm_service_plan.this.id

  site_config {
    always_on = var.plan_sku == "F1" || var.plan_sku == "D1" ? false : true
    application_stack {
      dotnet_version = "8.0"
    }
  }

  https_only = true
}

output "resource_group_name" {
  value = data.azurerm_resource_group.this.name
}

output "web_app_name" {
  value = azurerm_linux_web_app.this.name
}

output "web_app_default_hostname" {
  value = azurerm_linux_web_app.this.default_hostname
}

output "web_app_url" {
  value = "https://${azurerm_linux_web_app.this.default_hostname}"
}
