@echo off
setlocal
pushd "%~dp0"
start "" dotnet run --project "MerchantInventoryEngine/MerchantInventoryEngine.csproj"
popd
exit /b 0
