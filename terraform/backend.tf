terraform {
  backend "azurerm" {
    resource_group_name  = "employee-system-rg"
    storage_account_name = "employeesystemstorage"
    container_name       = "prod-tfstate"
    key                  = "prod.terraform.tfstate"
  }
}
