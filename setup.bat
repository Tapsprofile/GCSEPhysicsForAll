@echo off
REM GCSE Physics LMS Setup Script for Windows

echo =========================================
echo GCSE Physics LMS - Setup Script
echo =========================================
echo.

REM Check if Docker is installed
docker --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ Docker is not installed. Please install Docker Desktop first.
    pause
    exit /b 1
)

REM Check if Docker Compose is installed
docker-compose --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ Docker Compose is not installed. Please install Docker Compose first.
    pause
    exit /b 1
)

echo ✅ Docker and Docker Compose are installed
echo.

echo Starting GCSE Physics LMS...
echo.

REM Start Docker Compose
docker-compose up -d

echo.
echo =========================================
echo ✅ GCSE Physics LMS is starting!
echo =========================================
echo.
echo Services will be available at:
echo   🌐 Frontend:               http://localhost:3000
echo   🔌 API Gateway:            http://localhost:5000
echo   🔐 Auth Service:           http://localhost:5001
echo   📚 CMS Service:            http://localhost:5002
echo   👥 Teacher-Student Service: http://localhost:5003
echo   🗄️  PostgreSQL:             localhost:5432
echo   🍃 MongoDB:                localhost:27017
echo.
echo To view logs: docker-compose logs -f
echo To stop:      docker-compose down
echo.
echo Please wait a moment for all services to start...
pause
