terraform {
    required_version = "1.14"
    required_providers {
      azurerm = {
        source = "hashicorp/azurerm"
        version = "~> 4.2"
      }
    }
}

provider "azurerm" {
    features {  
        resource_group {
          prevent_deletion_if_contains_resources = false
        }
    }
}

variable "locationService" {
    type = string
    description = "Server Location"
    default = "West Europe"
}

variable "locationService1" {
    type = string
    description = "Server Location"
    default = "southeastasia"
}

resource "azurerm_resource_group" "rg" {
  name     = "transfer-westus-rg"
  location = "westus"
}

#MS API Gateway
resource "azurerm_service_plan" "plan_apigateway" {
  name                = "transfer-weu-apigateway-plan"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "apigateway-appservice" {
  name                = "transfer-weu-apigateway-appservice"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.plan_apigateway.id

  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#MS Transaction
resource "azurerm_service_plan" "plan_transaction" {
  name                = "transfer-weu-transaction-plan"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "transaction-appservice" {
  name                = "transfer-weu-transaction-appservice"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.plan_transaction.id

  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#MS Balance
resource "azurerm_service_plan" "plan_balance" {
  name                = "transfer-weu-balance-plan"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "balance-appservice" {
  name                = "transfer-weu-balance-appservice"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.plan_balance.id

  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#MS Transfer
resource "azurerm_service_plan" "plan_transfer" {
  name                = "transfer-weu-transfer-plan"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "transfer-appservice" {
  name                = "transfer-weu-transfer-appservice"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.plan_transfer.id

  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#MS Notification
resource "azurerm_service_plan" "plan_notification" {
  name                = "transfer-weu-notification-plan"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "notification-appservice" {
  name                = "transfer-westus-notification-appservice"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.plan_notification.id

  site_config {
    always_on = false
    application_stack {
      docker_image_name = "nginx:latest"
    }
  }
}

#SQL for Transaction MS
resource "azurerm_mssql_server" "transaction_sql_server" {
  name                         = "transfer-westus-transaction-sqlserver"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "sqladminuser"
  administrator_login_password = "Iloveswing77!"
}

resource "azurerm_mssql_database" "transaction_sql_database" {
  name           = "transfer-westus-transaction-sql-database"
  server_id      = azurerm_mssql_server.transaction_sql_server.id
  sku_name       = "Basic"
}

#SQL for Balance MS
resource "azurerm_mssql_server" "balance_sql_server" {
  name                         = "transfer-westus-balance-sqlserver"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "sqladminuser"
  administrator_login_password = "Iloveswing77!"
}

resource "azurerm_mssql_database" "balance_sql_database" {
  name           = "transfer-westus-balance-sql-database"
  server_id      = azurerm_mssql_server.balance_sql_server.id
  sku_name       = "Basic"
}

#SQL for Transfer MS
resource "azurerm_mssql_server" "transfer_sql_server" {
  name                         = "transfer-westus-transfer-sqlserver"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "sqladminuser"
  administrator_login_password = "Iloveswing77!"
}

resource "azurerm_mssql_database" "transfer_sql_database" {
  name           = "transfer-westus-transfer-sql-database"
  server_id      = azurerm_mssql_server.transfer_sql_server.id
  sku_name       = "Basic"
}

#Cosmos DB for Notification MS
resource "azurerm_cosmosdb_account" "notification_cosmos" {
  name                = "transfer-westus-notification-cosmos"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  offer_type          = "Standard"
  consistency_policy {
    consistency_level       = "Session"
  }
  geo_location {
    location          = azurerm_resource_group.rg.location
    failover_priority = 0
  }
}

resource "azurerm_cosmosdb_sql_database" "notification_cosmosdb_database" {
  name                = "transfer-westus-notification-cosmos-db"
  resource_group_name = azurerm_resource_group.rg.name
  account_name = azurerm_cosmosdb_account.notification_cosmos.name
}

#Azure Storage Account Function
resource "azurerm_storage_account" "storage_account_function" {
  name                     = "transferwestusstoracc"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

#Azure Function Service Plan
resource "azurerm_service_plan" "deadletter_service_plan" {
  name                = "transfer-southeastasia-deadletter-service-plan"
  location            = var.locationService1
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "deadletter_function_app" {
  name                       = "transfer-southeastasia-deadletter-function-app"
  location                   = var.locationService1
  resource_group_name        = azurerm_resource_group.rg.name
  service_plan_id            = azurerm_service_plan.deadletter_service_plan.id
  storage_account_name       = azurerm_storage_account.storage_account_function.name
  storage_account_access_key = azurerm_storage_account.storage_account_function.primary_access_key

  site_config {
    always_on = false
  }
}

#Azure Service Bus
resource "azurerm_servicebus_namespace" "transfer-servicebus" {
  name                = "transfer-weu-servicebus"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"
}

#App Insights
resource "azurerm_application_insights" "transfer_app_insights" {
  name                = "transfer-weu-app-insights"
  location            = var.locationService
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
}

#Key Vault
resource "azurerm_key_vault" "transfer_key_vault" {
  name                        = "transfer-weu-key-vault"
  location                    = var.locationService
  resource_group_name         = azurerm_resource_group.rg.name
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  sku_name                    = "standard"
}

data "azurerm_client_config" "current" {}