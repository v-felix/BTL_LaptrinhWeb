$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'Use-LocalSdk.ps1')
$projectRoot = Split-Path -Parent $PSScriptRoot
Push-Location -LiteralPath $projectRoot
try {
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw 'Khong khoi phuc duoc dotnet-ef.' }
    dotnet restore
    if ($LASTEXITCODE -ne 0) { throw 'NuGet restore that bai.' }
    dotnet run --project HospitalManagement.Web --launch-profile HospitalManagement.Web -- --initialize-only
    if ($LASTEXITCODE -ne 0) { throw 'Khoi tao database that bai. Kiem tra SQL Server va connection string.' }
    Write-Host 'Database da san sang. Mo HospitalManagement.sln trong Visual Studio va nhan F5.'
}
finally { Pop-Location }
