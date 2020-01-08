$NSwagCmd = "C:\bin\nswag\NetCore30\dotnet-nswag.exe"

& $NSwagCmd run angular-proxy.nswag


# $NSwagCmd = "C:\bin\nswag\NetCore30\dotnet-nswag.exe"
# $SwaggerUri = "http://localhost:8000/swagger/v1/swagger.json"
# $ProxyClass = "ConverterClient"
# $ProxyFileName = "converter.client.ts"
# $ModulePath = "src/app"

# $ProjectFolder = $PSScriptRoot
# $SwaggerFile = [IO.Path]::Combine($ProjectFolder, "swagger.json")
# $OutputFile  = [IO.Path]::Combine($ProjectFolder, $ModulePath, $ProxyFileName)

# Invoke-WebRequest -Uri $SwaggerUri -OutFile "$SwaggerFile"
# & $NSwagCmd openapi2tsclient `
#     /input:"$SwaggerFile" `
# 		/classname:$ProxyClass `
# 		/output:$OutputFile `
# 		/template:Angular `
# 		/httpClass:HttpClient `
# 		/baseUrlTokenName:'CURRENCY_SERVICE_BASE_URL' `
#     /injectionTokenType:InjectionToken
