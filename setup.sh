#!/bin/bash

# GCSE Physics LMS Setup Script

echo "========================================="
echo "GCSE Physics LMS - Setup Script"
echo "========================================="
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null
then
    echo "❌ Docker is not installed. Please install Docker first."
    exit 1
fi

# Check if Docker Compose is installed
if ! command -v docker-compose &> /dev/null
then
    echo "❌ Docker Compose is not installed. Please install Docker Compose first."
    exit 1
fi

echo "✅ Docker and Docker Compose are installed"
echo ""

# Check system resources
total_mem=$(free -m | awk '/^Mem:/{print $2}')
echo "💾 Total System Memory: ${total_mem}MB"

if [ "$total_mem" -lt 2048 ]; then
    echo "⚠️  Warning: System has less than 2GB RAM. Application may run slowly."
else
    echo "✅ Sufficient memory available"
fi

echo ""
echo "Starting GCSE Physics LMS..."
echo ""

# Start Docker Compose
docker-compose up -d

echo ""
echo "========================================="
echo "✅ GCSE Physics LMS is starting!"
echo "========================================="
echo ""
echo "Services will be available at:"
echo "  🌐 Frontend:               http://localhost:3000"
echo "  🔌 API Gateway:            http://localhost:5000"
echo "  🔐 Auth Service:           http://localhost:5001"
echo "  📚 CMS Service:            http://localhost:5002"
echo "  👥 Teacher-Student Service: http://localhost:5003"
echo "  🗄️  PostgreSQL:             localhost:5432"
echo "  🍃 MongoDB:                localhost:27017"
echo ""
echo "To view logs: docker-compose logs -f"
echo "To stop:      docker-compose down"
echo ""
echo "Please wait a moment for all services to start..."
