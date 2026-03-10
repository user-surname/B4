param(
  [string]$BackendProject = 'B4.Api',
  [string]$FrontendProject = 'B4.WebActuals',
  [int]$BackendPort = 5029,
  [int]$FallbackBackendPort = 5030
)

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..')
$backendPath = Join-Path $repoRoot $BackendProject
$frontendPath = Join-Path $repoRoot $FrontendProject

if (-not (Test-Path $backendPath)) {
  Write-Error "No existe la carpeta del backend: $backendPath"
  exit 1
}

if (-not (Test-Path $frontendPath)) {
  Write-Error "No existe la carpeta del frontend: $frontendPath"
  exit 1
}

$selectedBackendPort = $BackendPort
$isPrimaryPortBusy = Get-NetTCPConnection -LocalPort $BackendPort -State Listen -ErrorAction SilentlyContinue
if ($isPrimaryPortBusy) {
  $selectedBackendPort = $FallbackBackendPort
  Write-Warning "El puerto $BackendPort esta ocupado. Se usara $selectedBackendPort para el backend."
}

Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$backendPath'; dotnet run --urls `"http://localhost:$selectedBackendPort`""
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$frontendPath'; npm start"

Write-Host "Backend y frontend lanzados en ventanas separadas. Backend: http://localhost:$selectedBackendPort"
