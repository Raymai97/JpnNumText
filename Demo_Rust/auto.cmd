@echo off
cd %~dp0
copy /y ..\src\JpnNumText.rs JpnNumText.rs >nul && (
rustc Demo.rs && Demo.exe
del /q JpnNumText.rs >nul
)
pause
@echo on