# src/infrastructure/terraform/backend.tfvars
resource_group_name  = "rg-terrastate"
storage_account_name = "mystorageacct"
container_name       = "tfstate"
key                  = "accessing-childcare-entitlement-checker.tfstate"

# optionally:
subscription_id = ".."
# tenant_id       = "..."