# ppaf-samples

Collection of projects and materials for Azure Cosmos DB Per Partition Automatic Failover.

## Overview

This repository contains scripts and resources to manage and test Azure Cosmos DB's Per Partition Automatic Failover feature. The provided PowerShell script enables, disables, or retrieves the status of chaos faults for Cosmos DB accounts.

## Folder Structure
- **README.md**: This file, providing an overview of the repository.
- **ppaf-fault-script/**: Contains the PowerShell script for managing chaos faults.

## Script: EnableDisableChaosFault.ps1

The script allows you to:
- Enable or disable chaos faults for a Cosmos DB account.
- Retrieve the status of chaos faults.

### Parameters

- `ResourceGroup`: The resource group where the Cosmos DB account is located.
- `AccountName`: The name of the Cosmos DB account.
- `SubscriptionId`: The subscription ID of the Cosmos DB account.
- `Region`: The preferred write region for enabling/disabling the fault.
- `DatabaseName`: The database name where the container is located.
- `ContainerName`: The container name where the fault is applied.
- `Enable`: Switch to enable the chaos fault.
- `Disable`: Switch to disable the chaos fault.
- `GetStatus`: Switch to retrieve the status of the chaos fault.

### Examples

1. **Enable Fault**:
   ```powershell
   .\EnableDisableChaosFault.ps1 -FaultType "PerPartitionAutomaticFailover" -ResourceGroup "{ResourceGroup}" -AccountName "{DatabaseAccountName}" -DatabaseName "{DatabaseName}" -ContainerName "{CollectionName}" -SubscriptionId "{SubscriptionId}" -Region "{PreferredWriteRegionName}" -Enable
    ```
2. **Disable Fault**:
   ```powershell
   .\EnableDisableChaosFault.ps1 -FaultType "PerPartitionAutomaticFailover" -ResourceGroup "{ResourceGroup}" -AccountName "{DatabaseAccountName}" -DatabaseName "{DatabaseName}" -ContainerName "{CollectionName}" -SubscriptionId "{SubscriptionId}" -Region "{PreferredWriteRegionName}" -Disable
   ```