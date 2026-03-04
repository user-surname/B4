param(
  [string]$BackendProject = 'B4.Api',
  [string]$FrontendProject = 'B4.WebActuals'
)

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$backendPath = Join-Path $root $BackendProject
$frontendPath = Join-Path $root $FrontendProject

if (-not (Test-Path $backendPath)) {
  Write-Error "No existe la carpeta del backend: $backendPath"
  exit 1
}

if (-not (Test-Path $frontendPath)) {
  Write-Error "No existe la carpeta del frontend: $frontendPath"
  exit 1
}

Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$backendPath'; dotnet run"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$frontendPath'; npm start"

Write-Host 'Backend y frontend lanzados en ventanas separadas.'
