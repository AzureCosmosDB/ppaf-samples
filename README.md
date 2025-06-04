# ppaf-samples

Collection of projects and materials for Azure Cosmos DB Per Partition Automatic Failover.

## Overview

This repository contains scripts and resources to manage and test Azure Cosmos DB's Per Partition Automatic Failover feature. The provided PowerShell script enables, disables, or retrieves the status of chaos faults for Cosmos DB accounts.

## Folder Structure
- **README.md**: This file, providing an overview of the repository.
- **ppaf-fault-script/**: Contains the PowerShell script for managing chaos faults.
- **ppaf-demo-app/**: Contains the demo application that interacts with the Cosmos DB account to demonstrate the Per Partition Automatic Failover feature.


## How to Test Per Partition Automatic Failover

You can observe the effect of Per Partition Automatic Failover in real time by running the demo app and injecting faults using the provided script.


### 1. Run the PPAF Demo App
The demo app will connect to the Cosmos DB account and creates documents at an interval. 

The app will also display the status of the documents created, including the region it contacted for the write operation. 

In steady state, it will be the preferred write region for you Cosmos DB account.


### 2. Enable Fault

Use the script located at ppaf-fault-script/EnableDisableChaosFault.ps1

The script allows you to:
- Enable or disable chaos faults for a Cosmos DB account.
- Retrieve the status of chaos faults.

#### Parameters

- `ResourceGroup`: The resource group where the Cosmos DB account is located.
- `AccountName`: The name of the Cosmos DB account.
- `SubscriptionId`: The subscription ID of the Cosmos DB account.
- `Region`: The preferred write region for enabling/disabling the fault.
- `DatabaseName`: The database name where the container is located.
- `ContainerName`: The container name where the fault is applied.
- `Enable`: Switch to enable the chaos fault.
- `Disable`: Switch to disable the chaos fault.
- `GetStatus`: Switch to retrieve the status of the chaos fault.

#### Example

1. **Enable Fault**:
   ```powershell
   .\EnableDisableChaosFault.ps1 -FaultType "PerPartitionAutomaticFailover" -ResourceGroup "{ResourceGroup}" -AccountName "{DatabaseAccountName}" -DatabaseName "{DatabaseName}" -ContainerName "{CollectionName}" -SubscriptionId "{SubscriptionId}" -Region "{PreferredWriteRegionName}" -Enable
    ```

The fault can take upto 15 minutes to be applied. 

### 3. PPAF in action
Once enabled, the demo app will start experiencing write operations that fail in the preferred write region, simulating a failover scenario.

The app will now start writing to secondary regions, demonstrating the Per Partition Automatic Failover feature

### 4. Disable the fault
1. **Disable Fault**:
   ```powershell
   .\EnableDisableChaosFault.ps1 -FaultType "PerPartitionAutomaticFailover" -ResourceGroup "{ResourceGroup}" -AccountName "{DatabaseAccountName}" -DatabaseName "{DatabaseName}" -ContainerName "{CollectionName}" -SubscriptionId "{SubscriptionId}" -Region "{PreferredWriteRegionName}" -Disable
   ```
The disable can take upto 15 minutes to be applied. 

### 5. Check Status
Once the fault is disable, the application will seamlessly start writing to the preferred write region again, demonstrating the recovery from the failover scenario.## How to Test Per Partition Automatic Failover

