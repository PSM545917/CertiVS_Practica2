Write-Host "Starting PatientCodeAPI..." -ForegroundColor Cyan
$patientCodeApi = Start-Process -FilePath "dotnet" -ArgumentList "run --project PatientCodeAPI --urls https://localhost:7200;http://localhost:5200" -PassThru -WindowStyle Normal

Write-Host "Starting CertiVS_Practica2..." -ForegroundColor Cyan
$mainApi = Start-Process -FilePath "dotnet" -ArgumentList "run --project CertiVS_Practica2 --urls https://localhost:7100;http://localhost:5100" -PassThru -WindowStyle Normal

Write-Host "Both APIs are running. Press Ctrl+C to stop..." -ForegroundColor Green
Write-Host "PatientCodeAPI: https://localhost:7200/swagger" -ForegroundColor Yellow
Write-Host "CertiVS_Practica2: https://localhost:7100/swagger" -ForegroundColor Yellow

try {
    Wait-Process -Id $patientCodeApi.Id
}
catch {
    # If one process exits, kill the other
    if (!$patientCodeApi.HasExited) {
        Stop-Process -Id $patientCodeApi.Id
    }
    if (!$mainApi.HasExited) {
        Stop-Process -Id $mainApi.Id
    }
}
finally {
    Write-Host "Shutting down..." -ForegroundColor Red
} 