#!/bin/bash
set -e

if [ "$#" -ne 2 ]; then
    echo "Usage: $0 <environment_prefix> <shutter|restore>"
    echo "Example: $0 d01 shutter"
    exit 1
fi

ENV_PREFIX=$1
ACTION=$2

# Prefix logic matches Terraform locals.tf and variables.tf defaults
PROJECT_ID="s279"
PREFIX="${PROJECT_ID}${ENV_PREFIX}"
RG="${PREFIX}rg-uks-cec-web"
PROFILE="${PREFIX}-web-fd-profile"
ENDPOINT="${PREFIX}-web-fd-endpoint"
ROUTE="${PREFIX}-web-fd-route"

if [ "$ACTION" = "shutter" ]; then
    echo "Enabling shutter mode for environment $ENV_PREFIX..."
    ORIGIN_GROUP="${PREFIX}-shutter-fd-origin-group"
    RULE_SET_1="${ENV_PREFIX}SecurityRules"
    RULE_SET_2="${ENV_PREFIX}ShutterRules"
    
    az afd route update \
        --resource-group "$RG" \
        --profile-name "$PROFILE" \
        --endpoint-name "$ENDPOINT" \
        --route-name "$ROUTE" \
        --origin-group "$ORIGIN_GROUP" \
        --rule-sets "$RULE_SET_1" "$RULE_SET_2"
    echo "Shutter mode successfully enabled."

elif [ "$ACTION" = "restore" ]; then
    echo "Restoring normal operation for environment $ENV_PREFIX..."
    ORIGIN_GROUP="${PREFIX}-web-fd-origin-group"
    RULE_SET_1="${ENV_PREFIX}SecurityRules"
    
    az afd route update \
        --resource-group "$RG" \
        --profile-name "$PROFILE" \
        --endpoint-name "$ENDPOINT" \
        --route-name "$ROUTE" \
        --origin-group "$ORIGIN_GROUP" \
        --rule-sets "$RULE_SET_1"
    echo "Normal operation successfully restored."

else
    echo "Invalid action: $ACTION. Must be 'shutter' or 'restore'."
    exit 1
fi
