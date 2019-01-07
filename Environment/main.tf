variable "subscription_id" {}
variable "client_id" {}
variable "client_certificate_path" {}
variable "tenant_id" {}
variable "client_certificate_password" {}

# variable "azure_sql_server_admin_password" {}

provider "azurerm" {
  # Whilst version is optional, we /strongly recommend/ using it to pin the version of the Provider being used
  version = "=1.20.0"

  subscription_id             = "${var.subscription_id}"
  client_id                   = "${var.client_id}"
  client_certificate_path     = "${var.client_certificate_path}"
  client_certificate_password = "${var.client_certificate_password}"
  tenant_id                   = "${var.tenant_id}"
}

# module "AzureAdConfig" {
#   source = "./Modules/azure_ad_config/"
# }

resource "azurerm_resource_group" "MusicSampleManagerResourceGroup" {
  name     = "MusicSampleManagerRG"
  location = "East US"
}

resource "azurerm_storage_account" "WebsiteBackendSA" {
  name                     = "contributorwebsitebacken"
  resource_group_name      = "${azurerm_resource_group.MusicSampleManagerResourceGroup.name}"
  location                 = "centralus"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "XMLSchemasContainer" {
  name                  = "schemas"
  resource_group_name   = "${azurerm_resource_group.MusicSampleManagerResourceGroup.name}"
  storage_account_name  = "${azurerm_storage_account.WebsiteBackendSA.name}"
  container_access_type = "blob"
}
