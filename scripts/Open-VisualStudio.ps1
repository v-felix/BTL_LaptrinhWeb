param([string]$VisualStudioPath)
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'Use-LocalSdk.ps1')
$projectRoot = Split-Path -Parent $PSScriptRoot
if ($env:DOTNET_ROOT -and (Test-Path -LiteralPath (Join-Path $env:DOTNET_ROOT 'sdk'))) {
    $taskSdk = Get-ChildItem -LiteralPath (Join-Path $env:DOTNET_ROOT 'sdk') -Directory |
        Where-Object Name -NotLike '*-*' | Sort-Object { [version]$_.Name } -Descending | Select-Object -First 1
    if ($taskSdk) { $env:MSBuildSDKsPath = Join-Path $taskSdk.FullName 'Sdks' }
}
if (-not $VisualStudioPath) {
    $vsWhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (Test-Path -LiteralPath $vsWhere) {
        $VisualStudioPath = & $vsWhere -latest -products '*' -find 'Common7\IDE\devenv.exe' | Select-Object -First 1
    }
}
if (-not $VisualStudioPath) {
    $knownVsPath = Join-Path $env:ProgramFiles 'Microsoft Visual Studio\18\Insiders\Common7\IDE\devenv.exe'
    if (Test-Path -LiteralPath $knownVsPath) { $VisualStudioPath = $knownVsPath }
}
if (-not $VisualStudioPath -or -not (Test-Path -LiteralPath $VisualStudioPath)) {
    throw 'Khong tim thay Visual Studio. Cai workload ASP.NET and web development hoac truyen -VisualStudioPath.'
}
$solutionPath = Join-Path $projectRoot 'HospitalManagement.sln'
Start-Process -FilePath $VisualStudioPath -ArgumentList ('"' + $solutionPath + '"') -WindowStyle Normal
