# Guide for creating Infraestructure with Terraform on Azure
_Building a Microservice Infraestructure with .NET 10 in Azure with Terraform

## Step 1: Download and execute Terraform
_Use this link and select your OS: 
```
https://developer.hashicorp.com/terraform/install
```

## Step 2: Clear Previous Login
```bash
az account clear
```

## Step 3: Log in to Azure Using Device Code
_Follow the link provided by Microsoft and enter the code that appears.
```bash
az login --use-device-code
```

## Step 4: Select Subscription and Create Service Principal
_Click on your Subscription, using the Subscription ID:
`Put your Subscription Id from Azure`
_Create a Service Principal to access Azure automatically:
```bash
az ad sp create-for-rbac --name "transfer-app" --role="Contributor" --scopes="/subscriptions/Your-Suscription_id"
```
_At the console, you will be delivered with the credentials:
```powershell
{
    $appId = "xxxxxxxxxxxxxxxx"  # ARM_CLIENT_ID
    $displayName = "transfer-app"
    $password = "xxxxxxxxxxxxxxxxxxxxxxxxxxxx" # ARM_CLIENT_SECRET
    tenant = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx"  # ARM_TENANT_ID
}
```
## Step 5: Save Credentials as Environment Variables
_Use the credentials provided to set environment variables for automatic Azure access.
```powershell
$env:ARM_CLIENT_ID = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
$env:ARM_CLIENT_SECRET = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
$env:ARM_SUBSCRIPTION_ID = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
$env:ARM_TENAT_ID = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
```
_Copy these four variables into your console within the project scope and press Enter:
