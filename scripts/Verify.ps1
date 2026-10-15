$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'Use-LocalSdk.ps1')
$projectRoot = Split-Path -Parent $PSScriptRoot
Push-Location -LiteralPath $projectRoot
try {
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw 'Tool restore that bai.' }
    dotnet build HospitalManagement.sln -c Release
    if ($LASTEXITCODE -ne 0) { throw 'Build that bai.' }
    dotnet test HospitalManagement.sln -c Release --no-build --logger 'trx;LogFileName=hospital-tests.trx'
    if ($LASTEXITCODE -ne 0) { throw 'Kiem thu that bai.' }
    dotnet ef migrations has-pending-model-changes --project HospitalManagement.Web --configuration Release --no-build
    if ($LASTEXITCODE -ne 0) { throw 'Model va migration chua dong bo.' }
}
finally { Pop-Location }
