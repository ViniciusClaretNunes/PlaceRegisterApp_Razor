@echo off
echo ===========================================
echo  Iniciando a aplicacao PlaceRegisterApp_Razor...
echo ===========================================
echo.

REM Verifica se o .NET SDK esta instalado
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERRO: O .NET SDK nao foi encontrado.
    echo Baixe e instale em: https://dotnet.microsoft.com/download
    pause
    exit /b
)

REM Entra na pasta do script (caminho atual)
cd /d "%~dp0"

REM Verifica se o banco de dados SQLite existe, senao cria
if not exist "data" mkdir data

REM Compila e executa o projeto
start http://localhost:5000
dotnet run

pause
