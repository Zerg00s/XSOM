# XSOM - Authentication via SharePoint REST API for С# + Helper classes
## Wrapper Library for SharePoint REST operations

### Projects:
- XSOM - Wrapper library for SharePoint REST
- XSOM WinForms Sample - sample for debugging purposes

Tested with SharePoint Online and SharePoint Online + ADFS


![Alt text](sample.png?raw=true "XSOM Sample")

![Alt text](sample2.png?raw=true "XSOM Sample 2")


In SharePoint Online tenants where Legacy authentication is disabled, we won't be able to authenticate by passing login and password in clear text. How to check if legacy authentication is enabled:

```
Connect-SPOService -Url https://contoso-admin.sharepoint.com
$tenant = Get-SPOTenant
$tenant.LegacyAuthProtocolsEnabled
```

How to enable legacy authentication:
```
Connect-SPOService -Url –https://<tenant>-admin.sharepoint.com
Set-SPOTenant –LegacyAuthProtocolsEnabled $true
```

Note: legacy authentication is considered unsafe.
