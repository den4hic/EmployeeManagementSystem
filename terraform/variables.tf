variable "project_name" {
  type        = string
  description = "Project name to be used for resource naming"
}

variable "location" {
  type        = string
  description = "Azure region"
  default     = "West Europe"
}