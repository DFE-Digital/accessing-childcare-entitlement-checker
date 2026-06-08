targetScope = 'subscription'

param resourceGroupName string
param location string
param storageAccountName string
param tags object = {}
param storageAccountSku string = 'Standard_ZRS'
param workspaceName string

output resourceGroupName string = resourceGroupName
output storageAccountName string = storageAccountName

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: location
  tags: tags
}

module storage './storage.bicep' = {
  name: 'storageDeployment'
  scope: resourceGroup(resourceGroupName)
  dependsOn: [ rg ]
  params: {
    storageAccountName: storageAccountName
    location: location
    tags: tags
    storageAccountSku: storageAccountSku
    workspaceName: workspaceName
  }
}