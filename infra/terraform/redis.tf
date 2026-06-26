resource "azurerm_managed_redis" "redis" {
  name                  = "${local.service_prefix}-redis"
  resource_group_name   = azurerm_resource_group.web-rg.name
  location              = var.location
  sku_name              = var.redis_sku_name
  public_network_access = "Enabled"
  tags                  = local.common_tags

  default_database {
    client_protocol                    = "Encrypted"
    clustering_policy                  = "NoCluster"
    eviction_policy                    = "VolatileLRU"
    access_keys_authentication_enabled = false
  }
}

resource "azurerm_managed_redis_access_policy_assignment" "app_redis_access" {
  managed_redis_id = azurerm_managed_redis.redis.id
  object_id        = azurerm_user_assigned_identity.app_identity.principal_id
}

resource "azurerm_monitor_diagnostic_setting" "redis_diagnostics" {
  name                       = "${local.service_prefix}-redis-diagnostics"
  target_resource_id         = azurerm_managed_redis.redis.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_metric {
    category = "AllMetrics"
  }
}

resource "azurerm_monitor_diagnostic_setting" "redis_database_diagnostics" {
  name                       = "${local.service_prefix}-redis-db-diagnostics"
  target_resource_id         = "${azurerm_managed_redis.redis.id}/databases/default"
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_log {
    category = "ConnectionEvents"
  }
}
