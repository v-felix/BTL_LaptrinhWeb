# Prefer the private stable SDK when present. This changes only this process and its child processes.
$taskSdkRoot = Join-Path (Split-Path -Parent $PSScriptRoot) 'artifacts\dotnet'
if (Test-Path -LiteralPath (Join-Path $taskSdkRoot 'dotnet.exe')) {
    $env:DOTNET_ROOT = $taskSdkRoot
    $env:PATH = "$taskSdkRoot;$env:PATH"
    $env:DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR = $taskSdkRoot
}
