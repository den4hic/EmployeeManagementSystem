variable "project_name" {
  type        = string
  description = "Project name to be used for resource naming"
}

variable "location" {
  type        = string
  description = "Azure basic region"
  default     = "East US 2"
}

variable "location_app" {
  type        = string
  description = "Azure region for app service plan"
  default     = "West Europe"
}