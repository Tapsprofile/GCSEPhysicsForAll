# GCSE Physics For All - LMS

A comprehensive Learning Management System for GCSE Physics supporting Students, Teachers, and Parents.

## Architecture

### Technology Stack
- **Frontend**: Vue 3 with Vite
- **Backend**: .NET Core 8 with microservices architecture
- **Databases**: 
  - PostgreSQL for relational data (users, relationships)
  - MongoDB for flexible content management
- **API Gateway**: Ocelot for routing
- **Deployment**: Docker Compose on Ubuntu 20.04

### System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                        Frontend (Vue 3)                      │
│            Content-Agnostic Dynamic UI                       │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│                   API Gateway (Ocelot)                       │
│                   Port 5000 / 8080                           │
└──────┬──────────────┬──────────────┬────────────────────────┘
       │              │              │
       ▼              ▼              ▼
┌──────────┐   ┌─────────┐   ┌──────────────────┐
│  Auth    │   │   CMS   │   │ Teacher-Student  │
│ Service  │   │ Service │   │    Service       │
│ :5001    │   │ :5002   │   │    :5003         │
└────┬─────┘   └────┬────┘   └────┬─────────────┘
     │              │              │
     ▼              ▼              ▼
┌─────────┐   ┌──────────┐   ┌─────────┐
│PostgreSQL│   │ MongoDB  │   │PostgreSQL│
│ :5432    │   │ :27017   │   │ :5432    │
└──────────┘   └──────────┘   └──────────┘
```

## Services

### 1. Authentication Service (Port 5001)
- User registration and login
- JWT token generation
- User role management (Student, Teacher, Parent, Admin)
- **Database**: PostgreSQL

**Endpoints**:
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `GET /api/auth/health` - Health check

### 2. Content Management Service (Port 5002)
- Dynamic content creation and management
- Flexible schema for different content types
- Support for lessons, quizzes, videos, assignments
- **Database**: MongoDB

**Endpoints**:
- `GET /api/content` - Get all content (with filters)
- `GET /api/content/{id}` - Get specific content
- `POST /api/content` - Create new content
- `PUT /api/content/{id}` - Update content
- `DELETE /api/content/{id}` - Delete content
- `GET /api/content/health` - Health check

### 3. Teacher-Student Service (Port 5003)
- Manage teacher-student relationships
- Manage parent-student relationships
- Query relationships for access control
- **Database**: PostgreSQL

**Endpoints**:
- `POST /api/relationship/teacher-student` - Assign teacher to student
- `GET /api/relationship/teacher/{id}/students` - Get teacher's students
- `GET /api/relationship/student/{id}/teachers` - Get student's teachers
- `POST /api/relationship/parent-student` - Assign parent to student
- `GET /api/relationship/parent/{id}/students` - Get parent's students
- `DELETE /api/relationship/teacher-student` - Remove relationship
- `GET /api/relationship/health` - Health check

### 4. API Gateway (Port 5000)
Routes requests to appropriate microservices:
- `/auth/*` → Authentication Service
- `/content/*` → CMS Service
- `/relationship/*` → Teacher-Student Service

## Features

### For Students
- Access learning materials dynamically
- Track progress on lessons and quizzes
- View assigned content from teachers

### For Teachers
- Create and publish content (lessons, quizzes, videos)
- Manage student assignments
- Track student progress
- Organize content by modules

### For Parents
- Monitor child's learning progress
- View assigned content and completion status
- Access student performance data

## Memory Efficiency

The system is optimized for memory-efficient operation on Ubuntu 20.04:

- **API Gateway**: 128-256 MB
- **Auth Service**: 192-384 MB
- **CMS Service**: 192-384 MB
- **Teacher-Student Service**: 192-384 MB
- **PostgreSQL**: 256-512 MB
- **MongoDB**: 256-512 MB
- **Frontend (NGINX)**: 64-128 MB

**Total Recommended RAM**: 2-4 GB for all services

## Getting Started

### Prerequisites
- Docker and Docker Compose
- Ubuntu 20.04 or compatible
- Minimum 2GB RAM

### Installation

1. Clone the repository:
```bash
git clone https://github.com/Tapsprofile/GCSEPhysicsForAll.git
cd GCSEPhysicsForAll
```

2. Start all services with Docker Compose:
```bash
docker-compose up -d
```

3. Access the application:
- Frontend: http://localhost:3000
- API Gateway: http://localhost:5000
- Auth Service: http://localhost:5001
- CMS Service: http://localhost:5002
- Teacher-Student Service: http://localhost:5003

### Development Mode

#### Backend
```bash
cd backend
dotnet restore
dotnet build
```

Run individual services:
```bash
cd backend/Services/AuthService
dotnet run
```

#### Frontend
```bash
cd frontend
npm install
npm run dev
```

### Database Setup

#### PostgreSQL
Default credentials:
- Host: localhost
- Port: 5432
- Username: postgres
- Password: postgres
- Databases: gcse_auth, gcse_relationships

#### MongoDB
Default credentials:
- Host: localhost
- Port: 27017
- Username: admin
- Password: admin
- Database: gcse_cms

## Content-Agnostic UI

The frontend is designed to dynamically render content based on the active module:

1. **Dynamic Forms**: Content is stored as flexible JSON in MongoDB
2. **Module-Based Rendering**: UI adapts based on `moduleType` and `contentType`
3. **Extensible**: New content types can be added without frontend changes

Example content structure:
```json
{
  "title": "Newton's Laws of Motion",
  "moduleType": "Physics",
  "contentType": "Lesson",
  "data": {
    "sections": [...],
    "diagrams": [...],
    "exercises": [...]
  }
}
```

## API Examples

### Register a new student
```bash
curl -X POST http://localhost:5000/auth/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "student@example.com",
    "password": "password123",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Student"
  }'
```

### Create content
```bash
curl -X POST http://localhost:5000/content/content \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Introduction to Forces",
    "description": "Basic concepts of forces in physics",
    "moduleType": "Physics",
    "contentType": "Lesson",
    "data": {
      "content": "Forces are pushes or pulls..."
    },
    "tags": ["forces", "mechanics"],
    "order": 1
  }'
```

## Security

- JWT-based authentication
- Password hashing with SHA256
- CORS enabled for development (configure for production)
- Environment-based configuration

## Production Deployment

For production deployment:

1. Update environment variables in `docker-compose.yml`
2. Configure proper database credentials
3. Set up SSL/TLS certificates
4. Configure CORS policies
5. Set `ASPNETCORE_ENVIRONMENT=Production`
6. Use stronger JWT keys

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is open source and available for educational purposes.

## Support

For support and questions, please open an issue on GitHub.
