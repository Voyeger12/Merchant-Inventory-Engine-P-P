@echo off
setlocal
pushd "%~dp0"
cls
color 0A
echo =======================================================
echo        MERCHANT INVENTORY ENGINE - LAUNCHER
echo =======================================================
echo.
echo [~] Running System Validation (Unit Tests)...
dotnet test MerchantInventoryEngine.Tests/MerchantInventoryEngine.Tests.csproj -v m
IF %ERRORLEVEL% NEQ 0 (
    color 0C
    echo.
    echo [X] ==================================================
    echo [X] CRITICAL FAILURE: Unit tests did not pass!
    echo [X] Application launch aborted to prevent corruption.
    echo [X] ==================================================
    pause
    exit /b %ERRORLEVEL%
)
color 0E
echo.
echo [v] ==================================================
echo [v] Systems Nominal. All Tests Passed!
echo [v] Initiating Engine...
echo [v] ==================================================
dotnet run --project "MerchantInventoryEngine/MerchantInventoryEngine.csproj"
pause
popd
