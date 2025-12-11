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
`f292716c-b2a3-458b-a683-a64fdea52893`
_Create a Service Principal to access Azure automatically:
```bash
az ad sp create-for-rbac --name "transfer-app" --role="Contributor" --scopes="/subscriptions/f292716c-b2a3-458b-a683-a64fdea52893"
```
_At the console, you will be delivered with the credentials:
```powershell
{
    $appId = "bdc2b628-5163-4687-ac22-9bac526cc77e"  # ARM_CLIENT_ID
    $displayName = "transfer-app"
    $password = "ju18Q~dvAZcOEhlFRnQ3Mo-VFpZZ7Wvv~U8NJc_i" # ARM_CLIENT_SECRET
    tenant = "f8b91cf2-2363-4bd0-8671-3285c809af5d"  # ARM_TENANT_ID
}
```
## Step 5: Save Credentials as Environment Variables
_Use the credentials provided to set environment variables for automatic Azure access.
```powershell
$env:ARM_CLIENT_ID = "bdc2b628-5163-4687-ac22-9bac526cc77e"
$env:ARM_CLIENT_SECRET = "ju18Q~dvAZcOEhlFRnQ3Mo-VFpZZ7Wvv~U8NJc_i"
$env:ARM_SUBSCRIPTION_ID = "f292716c-b2a3-458b-a683-a64fdea52893"
$env:ARM_TENAT_ID = "f8b91cf2-2363-4bd0-8671-3285c809af5d"
```
_Copy these four variables into your console within the project scope and press Enter:
