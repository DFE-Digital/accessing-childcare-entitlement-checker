resource "azurerm_cdn_frontdoor_rule_set" "security_redirects" {
  name                     = "${var.environment_prefix}SecurityRedirects"
  cdn_frontdoor_profile_id = azurerm_cdn_frontdoor_profile.frontdoor-web-profile.id
}

resource "azurerm_cdn_frontdoor_rule" "security_txt_rule" {
  depends_on = [azurerm_cdn_frontdoor_origin_group.frontdoor-origin-group, azurerm_cdn_frontdoor_origin.frontdoor-web-origin]

  name                      = "securityTxtRedirect"
  cdn_frontdoor_rule_set_id = azurerm_cdn_frontdoor_rule_set.security_redirects.id
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
  cdn_frontdoor_rule_set_id = azurerm_cdn_frontdoor_rule_set.security_redirects.id
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
