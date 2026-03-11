resource "azurerm_key_vault" "key-vault" {
  location            = local.location
  name                = "${local.prefix}kv-uks-cl"
  resource_group_name = azurerm_resource_group.core-rg.name
  sku_name            = "standard"
  tenant_id           = data.azurerm_client_config.client.tenant_id

  purge_protection_enabled = true

  network_acls {
    default_action = "Allow"
    bypass         = "AzureServices"
  }

  tags = local.common_tags
}

resource "azurerm_key_vault_access_policy" "github-kv-access" {
  key_vault_id       = azurerm_key_vault.key-vault.id
  tenant_id          = data.azurerm_client_config.client.tenant_id
  object_id          = data.azurerm_client_config.client.object_id
  secret_permissions = ["Get", "List", "Set", "Delete", "Purge", "Restore"]
}

resource "azurerm_key_vault_access_policy" "web-app-kv-access" {
  key_vault_id       = azurerm_key_vault.key-vault.id
  tenant_id          = azurerm_linux_web_app.web-app-service.identity[0].tenant_id
  object_id          = azurerm_linux_web_app.web-app-service.identity[0].principal_id
  secret_permissions = ["Get"]
}

resource "azurerm_key_vault_access_policy" "web-app-staging-kv-access" {
  key_vault_id       = azurerm_key_vault.key-vault.id
  tenant_id          = azurerm_linux_web_app_slot.web-app-service-staging.identity[0].tenant_id
  object_id          = azurerm_linux_web_app_slot.web-app-service-staging.identity[0].principal_id
  secret_permissions = ["Get"]
}

# resource "azurerm_key_vault_secret" "contentful-delivery-api-key" {
#   key_vault_id = azurerm_key_vault.key-vault.id
#   name         = "contentful-delivery-api-key"
#   value        = var.contentful_delivery_api_key

#   depends_on = [azurerm_key_vault_access_policy.github-kv-access]
# }

# resource "azurerm_key_vault_secret" "contentful-preview-api-key" {
#   key_vault_id = azurerm_key_vault.key-vault.id
#   name         = "contentful-preview-api-key"
#   value        = var.contentful_preview_api_key

#   depends_on = [azurerm_key_vault_access_policy.github-kv-access]
# }

# resource "azurerm_key_vault_secret" "contentful-management-api-key" {
#   key_vault_id = azurerm_key_vault.key-vault.id
#   name         = "contentful-management-api-key"
#   value        = var.contentful_management_api_key

#   depends_on = [azurerm_key_vault_access_policy.github-kv-access]
# }

# resource "azurerm_key_vault_secret" "contentful-space-id" {
#   key_vault_id = azurerm_key_vault.key-vault.id
#   name         = "contentful-space-id"
#   value        = var.contentful_space_id

#   depends_on = [azurerm_key_vault_access_policy.github-kv-access]
# }

resource "azurerm_key_vault_secret" "application-insights-connection-string" {
  key_vault_id = azurerm_key_vault.key-vault.id
  name         = "application-insights-connection-string"
  value        = azurerm_application_insights.application-insights.connection_string

  depends_on = [azurerm_key_vault_access_policy.github-kv-access]
}

# resource "azurerm_key_vault_secret" "pdf-generation-api-key" {
#   key_vault_id = azurerm_key_vault.key-vault.id
#   name         = "pdf-generation-api-key"
#   value        = var.pdf_generation_api_key

#   depends_on = [azurerm_key_vault_access_policy.github-kv-access]
# }

resource "azurerm_key_vault_secret" "azure-translation-access-key" {
  key_vault_id = azurerm_key_vault.key-vault.id
  name         = "azure-translation-access-key"
  value        = azurerm_cognitive_account.ai-translator.primary_access_key

  depends_on = [azurerm_key_vault_access_policy.github-kv-access]
}