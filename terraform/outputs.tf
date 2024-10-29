output "api_url" {
  value = "https://${azurerm_windows_web_app.api.default_hostname}"
}

output "ui_url" {
  value = azurerm_static_web_app.ui.default_host_name
}

output "sql_server_name" {
  value = azurerm_mssql_server.sql.fully_qualified_domain_name
}