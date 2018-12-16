variable "subscription_id" {}
variable "client_id" {}
variable "client_certificate_path" {}
variable "tenant_id" {}
variable "client_certificate_password" {}

provider "azurerm" {
  # Whilst version is optional, we /strongly recommend/ using it to pin the version of the Provider being used
  version = "=1.20.0"

  subscription_id             = "${var.subscription_id}"
  client_id                   = "${var.client_id}"
  client_certificate_path     = "${var.client_certificate_path}"
  client_certificate_password = "${var.client_certificate_password}"
  tenant_id                   = "${var.tenant_id}"
}

resource "azurerm_resource_group" "MusicSampleManagerResourceGroup" {
  name     = "MusicSampleManagerRG"
  location = "East US"
}

# resource "azurerm_sql_server" "MusicSampleManagerDBServer" {
#   name                         = "msmds"
#   resource_group_name          = "${azurerm_resource_group.test.name}"
#   location                     = "${azurerm_resource_group.test.location}"
#   version                      = "12.0"
#   administrator_login          = ""
#   administrator_login_password = ""


#   tags {
#     environment = "production"
#   }
# }

