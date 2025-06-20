# PPAF Samples

Collection of projects and materials for Azure Cosmos DB **Per Partition Automatic Failover (PPAF)**.

---

## Overview

This repository contains scripts and demo applications to manage and test Azure Cosmos DB’s **Per Partition Automatic Failover** feature.

* The provided **PowerShell script** enables, disables, or retrieves the status of chaos faults for Cosmos DB accounts.
* The included **.NET demo app** interacts with Cosmos DB and prints region-level diagnostics to demonstrate PPAF in action.

---

## Folder Structure

* **README.md** – This file, providing an overview of the repository.
* **ppaf-fault-script/** – Contains the PowerShell script for managing chaos fault injection.
* **ppaf-demo-app/** – Contains a .NET demo application that interacts with Cosmos DB and surfaces failover behavior.

---

## How to Test Per Partition Automatic Failover

You can observe the effect of Per Partition Automatic Failover in real time by running the demo app and injecting faults using the provided PowerShell script.

---

### 1 Run the Demo App

* The demo app connects to your Cosmos DB account and writes documents at regular intervals.
* Replace the appsettings.sample.json with appsettings.json and fill in your Cosmos DB account details.
* It displays the **HTTP status code** and **contacted region** for each write.
* Under normal conditions, writes go to the **preferred write region**.

---

### 2 Enable a Fault

Use the script in `ppaf-fault-script/EnableDisableChaosFault.ps1`.

This script allows you to:

* Enable or disable PPAF chaos faults.
* Retrieve the current chaos fault status.

#### Parameters

* `FaultType`: Type of fault (use `"PerPartitionAutomaticFailover"`)
* `ResourceGroup`: Resource group of the Cosmos DB account.
* `AccountName`: Cosmos DB account name.
* `SubscriptionId`: Azure subscription ID.
* `Region`: The preferred write region to inject the fault in.
* `DatabaseName`: Cosmos DB database name.
* `ContainerName`: Cosmos DB container name.
* `Enable`: Flag to enable the fault.
* `Disable`: Flag to disable the fault.
* `GetStatus`: Flag to check current fault status.

#### Example: Enable Fault

```powershell
.\EnableDisableChaosFault.ps1 \
  -FaultType "PerPartitionAutomaticFailover" \
  -ResourceGroup "<ResourceGroup>" \
  -AccountName "<DatabaseAccountName>" \
  -DatabaseName "<DatabaseName>" \
  -ContainerName "<ContainerName>" \
  -SubscriptionId "<SubscriptionId>" \
  -Region "<PreferredWriteRegionName>" \
  -Enable
```

> Fault injection can take up to **15 minutes** to take effect.

---

### 3 Observe PPAF in Action

* After the fault is enabled, the preferred write region will begin rejecting writes.
* The demo app will automatically detect the failure and reroute writes to a **secondary region**.
* This simulates an outage and demonstrates **partition-level failover**.

---

### 4 Disable the Fault

To disable the fault and restore normal operation:

```powershell
.\EnableDisableChaosFault.ps1 \
  -FaultType "PerPartitionAutomaticFailover" \
  -ResourceGroup "<ResourceGroup>" \
  -AccountName "<DatabaseAccountName>" \
  -DatabaseName "<DatabaseName>" \
  -ContainerName "<ContainerName>" \
  -SubscriptionId "<SubscriptionId>" \
  -Region "<PreferredWriteRegionName>" \
  -Disable
```

> Disabling the fault may also take up to **15 minutes** to be reflected.

---

### 5 Verify Recovery

Once the fault is disabled:

* The application will seamlessly resume writing to the **preferred write region**.
* This demonstrates **automatic recovery** from a partition-level failover scenario.

---