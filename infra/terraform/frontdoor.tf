resource "azurerm_cdn_frontdoor_profile" "frontdoor-web-profile" {
  name                = "${local.service_prefix}-web-fd-profile"
  resource_group_name = azurerm_resource_group.web-rg.name
  sku_name            = "${var.azure_frontdoor_scale}_AzureFrontDoor"
  tags                = local.common_tags
}

resource "azurerm_cdn_frontdoor_origin_group" "frontdoor-origin-group" {
  cdn_frontdoor_profile_id = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id
  name                     = "${local.service_prefix}-web-fd-origin-group"
  session_affinity_enabled = false

  load_balancing {
    sample_size                 = 4
    successful_samples_required = 2
  }
}

resource "azurerm_cdn_frontdoor_origin" "frontdoor-web-origin" {
  cdn_frontdoor_origin_group_id  = azurerm_cdn_frontdoor_origin_group.frontdoor-origin-group.id
  certificate_name_check_enabled = false
  host_name                      = azurerm_linux_web_app.web-app-service.default_hostname
  http_port                      = 80
  https_port                     = 443
  origin_host_header             = azurerm_linux_web_app.web-app-service.default_hostname
  priority                       = 1
  weight                         = 1
  name                           = "${local.service_prefix}-web-fd-origin"
  enabled                        = true
}

resource "azurerm_cdn_frontdoor_endpoint" "frontdoor-web-endpoint" {
  cdn_frontdoor_profile_id = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id
  name                     = "${local.service_prefix}-web-fd-endpoint"
  tags                     = local.common_tags
}

resource "azurerm_cdn_frontdoor_route" "frontdoor-web-route" {
  name                          = "${local.service_prefix}-web-fd-route"
  cdn_frontdoor_endpoint_id     = azurerm_cdn_frontdoor_endpoint.frontdoor-web-endpoint.id
  cdn_frontdoor_origin_group_id = azurerm_cdn_frontdoor_origin_group.frontdoor-origin-group.id
  cdn_frontdoor_origin_ids      = [azurerm_cdn_frontdoor_origin.frontdoor-web-origin.id]
  enabled                       = true

  forwarding_protocol    = "MatchRequest"
  https_redirect_enabled = true
  patterns_to_match      = ["/*"]
  supported_protocols    = ["Http", "Https"]

  cdn_frontdoor_custom_domain_ids = local.has_custom_domain ? [azurerm_cdn_frontdoor_custom_domain.fd-custom-domain[0].id] : []

  link_to_default_domain = !local.has_custom_domain
}

resource "azurerm_cdn_frontdoor_security_policy" "frontdoor-web-security-policy" {
  name                     = "${local.service_prefix}-web-fd-security-policy"
  cdn_frontdoor_profile_id = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id

  security_policies {
    firewall {
      cdn_frontdoor_firewall_policy_id = azurerm_cdn_frontdoor_firewall_policy.web_firewall_policy.id

      association {
        domain {
          cdn_frontdoor_domain_id = azurerm_cdn_frontdoor_endpoint.frontdoor-web-endpoint.id
        }

        dynamic "domain" {
          for_each = local.has_custom_domain ? ["apply"] : []
          content {
            cdn_frontdoor_domain_id = azurerm_cdn_frontdoor_custom_domain.fd-custom-domain[0].id
          }
        }

        patterns_to_match = ["/*"]
      }
    }
  }
}

resource "azurerm_cdn_frontdoor_custom_domain" "fd-custom-domain" {
  count                    = local.has_custom_domain ? 1 : 0
  name                     = "${local.service_prefix}-fd-custom-domain"
  cdn_frontdoor_profile_id = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id
  host_name                = var.custom_domain

  tls {
    certificate_type = "ManagedCertificate"
  }
}

resource "azurerm_cdn_frontdoor_custom_domain_association" "web-app-custom-domain" {
  count                          = local.has_custom_domain ? 1 : 0
  cdn_frontdoor_custom_domain_id = azurerm_cdn_frontdoor_custom_domain.fd-custom-domain[0].id
  cdn_frontdoor_route_ids        = [azurerm_cdn_frontdoor_route.frontdoor-web-route.id]
}

resource "azurerm_monitor_diagnostic_setting" "frontdoor_logging" {
  name                       = "${var.environment_prefix}-frontdoor-to-log-analytics"
  target_resource_id         = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log-analytics-workspace.id

  enabled_log {
    category = "FrontDoorAccessLog"
  }

  enabled_log {
    category = "FrontDoorWebApplicationFirewallLog"
  }

  enabled_metric {
    category = "AllMetrics"
  }
}
