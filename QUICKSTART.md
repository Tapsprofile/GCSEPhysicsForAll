# Quick Start Guide

## Option 1: Docker Compose (Recommended)

### Linux/Mac
```bash
./setup.sh
```

### Windows
```cmd
setup.bat
```

This will start all services automatically.

## Option 2: Manual Setup

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- PostgreSQL 14+
- MongoDB 6+

### Backend Setup

1. Start PostgreSQL and MongoDB
2. Update connection strings in appsettings.json files
3. Run services:

```bash
# Terminal 1 - Auth Service
cd backend/Services/AuthService
dotnet run

# Terminal 2 - CMS Service
cd backend/Services/CMSService
dotnet run

# Terminal 3 - Teacher-Student Service
cd backend/Services/TeacherStudentService
dotnet run

# Terminal 4 - API Gateway
cd backend/ApiGateway
dotnet run
```

### Frontend Setup

```bash
cd frontend
npm install
npm run dev
```

## Default Credentials

### PostgreSQL
- Host: localhost
- Port: 5432
- Username: postgres
- Password: postgres
- Databases: gcse_auth, gcse_relationships

### MongoDB
- Host: localhost
- Port: 27017
- Username: admin (for Docker)
- Password: admin (for Docker)
- Database: gcse_cms

## First Steps

1. Open http://localhost:3000
2. Click "Register" to create an account
3. Select your role (Student, Teacher, or Parent)
4. Login with your credentials
5. Explore the dashboard and content areas

## Testing the API

### Register a Student
```bash
curl -X POST http://localhost:5000/auth/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "student@test.com",
    "password": "Test123!",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Student"
  }'
```

### Login
```bash
curl -X POST http://localhost:5000/auth/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "student@test.com",
    "password": "Test123!"
  }'
```

### Create Content (as Teacher)
```bash
curl -X POST http://localhost:5000/content/content \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Introduction to Forces",
    "description": "Basic physics concepts",
    "moduleType": "Physics",
    "contentType": "Lesson",
    "data": {
      "content": "Forces are pushes or pulls acting on objects..."
    },
    "tags": ["forces", "mechanics"],
    "order": 1
  }'
```

## Troubleshooting

### Services not starting
- Check Docker is running: `docker ps`
- Check logs: `docker-compose logs -f`
- Restart services: `docker-compose restart`

### Database connection errors
- Ensure PostgreSQL and MongoDB are running
- Check connection strings in appsettings.json
- Verify database credentials

### Frontend not loading
- Check if API Gateway is running on port 5000
- Verify proxy configuration in vite.config.js
- Check browser console for errors

## Memory Issues

If running on limited memory:
- Reduce Docker memory limits in docker-compose.yml
- Run only essential services
- Use swap space if available

## Development Mode

For development without Docker:

1. Start PostgreSQL: `docker run -p 5432:5432 -e POSTGRES_PASSWORD=postgres postgres:14-alpine`
2. Start MongoDB: `docker run -p 27017:27017 mongo:6.0`
3. Run backend services individually (see Manual Setup)
4. Run frontend: `cd frontend && npm run dev`

## Production Deployment

See README.md for production deployment guidelines including:
- Environment variable configuration
- SSL/TLS setup
- Security hardening
- Performance optimization
