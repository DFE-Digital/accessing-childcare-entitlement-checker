#!/usr/bin/env bash
set -euo pipefail

STORAGE_ACCOUNT_NAME="$1"
RESOURCE_GROUP_NAME="$2"
ENABLE_ACCESS="$3"

if [ "$ENABLE_ACCESS" = "true" ]; then
  echo "Enabling access..."

  az storage account update \
    --name "$STORAGE_ACCOUNT_NAME" \
    --resource-group "$RESOURCE_GROUP_NAME" \
    --allow-shared-key-access true \
    --public-network-access Enabled \
    --default-action Allow > /dev/null

elif [ "$ENABLE_ACCESS" = "false" ]; then
  echo "Disabling access..."

  az storage account update \
    --name "$STORAGE_ACCOUNT_NAME" \
    --resource-group "$RESOURCE_GROUP_NAME" \
    --allow-shared-key-access false \
    --public-network-access Disabled > /dev/null

else
  echo "Invalid value for enable_access: $ENABLE_ACCESS"
  exit 1
fi

sleep 30