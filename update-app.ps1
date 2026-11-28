#нужная Оле штука, чтобы тестировать разметку, а не выполнять много команд по пересборке и обновлению приложения

$ErrorActionPreference = "Stop"

Set-Location $PSScriptRoot

$adb = "G:\Android\Sdk\platform-tools\adb.exe"
$project = ".\cashback-maui\cashback-maui.csproj"
$apk = ".\cashback-maui\bin\Debug\net10.0-android\com.companyname.cashbackmaui-Signed.apk"

if (!(Test-Path $adb)) {
    throw "ADB not found"
}

Write-Host "`n=== 1. Checking phone ===" -ForegroundColor Cyan

& $adb devices

if ($LASTEXITCODE -ne 0) {
    throw "ADB error"
}

Write-Host "`n=== 2. Building application ===" -ForegroundColor Cyan

dotnet build $project `
    -c Debug `
    -f net10.0-android `
    -p:AndroidEnableFastDeployment=false `
    -p:EmbedAssembliesIntoApk=true `
    -p:JavaSdkDirectory="G:\Android\Jdk" `
    -p:AndroidSdkDirectory="G:\Android\Sdk"

if ($LASTEXITCODE -ne 0) {
    throw "Build failed"
}

Write-Host "`n=== 3. Installing APK ===" -ForegroundColor Cyan

if (!(Test-Path $apk)) {
    throw "APK not found: $apk"
}

& $adb install -r $apk

if ($LASTEXITCODE -ne 0) {
    throw "Installation failed"
}

Write-Host "`n=== 4. Launching application ===" -ForegroundColor Cyan

& $adb shell monkey -p com.companyname.cashbackmaui 1

if ($LASTEXITCODE -ne 0) {
    throw "Launch failed"
}

Write-Host "Application updated successfully!" -ForegroundColor Green