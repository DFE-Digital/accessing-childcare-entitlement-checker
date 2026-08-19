resource "azurerm_cdn_frontdoor_rule_set" "security_rules" {
  name                     = "${var.environment_prefix}SecurityRules"
  cdn_frontdoor_profile_id = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id
}

resource "azurerm_cdn_frontdoor_rule" "security_txt_rule" {
  depends_on = [azurerm_cdn_frontdoor_origin_group.frontdoor-origin-group, azurerm_cdn_frontdoor_origin.frontdoor-web-origin]

  name                      = "securityTxtRedirect"
  cdn_frontdoor_rule_set_id = azurerm_cdn_frontdoor_rule_set.security_rules.id
  order                     = 0
  behaviour_on_match        = "Continue"

  conditions {
    request_path {
      operator   = "BeginsWith"
      values     = [".well-known/security.txt", "security.txt"]
      transforms = ["Lowercase"]
    }
  }

  actions {
    url_redirect {
      redirect_type         = "PermanentRedirect"
      redirect_protocol     = "Https"
      destination_host_name = "vdp.security.education.gov.uk"
      destination_path      = "/security.txt"
    }
  }
}

resource "azurerm_cdn_frontdoor_rule" "thanks_txt_rule" {
  depends_on = [azurerm_cdn_frontdoor_origin_group.frontdoor-origin-group, azurerm_cdn_frontdoor_origin.frontdoor-web-origin]

  name                      = "thanksTxtRedirect"
  cdn_frontdoor_rule_set_id = azurerm_cdn_frontdoor_rule_set.security_rules.id
  order                     = 1
  behaviour_on_match        = "Continue"

  conditions {
    request_path {
      operator   = "BeginsWith"
      values     = [".well-known/thanks.txt", "thanks.txt"]
      transforms = ["Lowercase"]
    }
  }

  actions {
    url_redirect {
      redirect_type         = "PermanentRedirect"
      redirect_protocol     = "Https"
      destination_host_name = "vdp.security.education.gov.uk"
      destination_path      = "/thanks.txt"
    }
  }
}

resource "azurerm_cdn_frontdoor_rule_set" "shutter_rules" {
  name                     = "${var.environment_prefix}ShutterRules"
  cdn_frontdoor_profile_id = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id
}

resource "azurerm_cdn_frontdoor_rule" "shutter_rewrite_rule" {
  name                      = "shutterRewriteRule"
  cdn_frontdoor_rule_set_id = azurerm_cdn_frontdoor_rule_set.shutter_rules.id
  order                     = 1
  behaviour_on_match        = "Continue"

  conditions {
    request_path {
      operator         = "BeginsWith"
      negate_condition = true
      values           = ["/assets/"]
      transforms       = ["Lowercase"]
    }
  }

  actions {
    url_rewrite {
      source_pattern          = "/"
      destination             = "/index.html"
      preserve_unmatched_path = false
    }
  }

  depends_on = [
    azurerm_cdn_frontdoor_origin_group.shutter-origin-group,
    azurerm_cdn_frontdoor_origin.frontdoor-shutter-origin
  ]
}
