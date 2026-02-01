# Environment Configuration Guide

This document describes the environment variables and configuration options for the GCSE Physics LMS.

## Quick Setup

For development with default settings, no configuration is needed. The system uses sensible defaults.

## Environment Variables

### Common Settings

```bash
# Application Environment
ASPNETCORE_ENVIRONMENT=Development  # or Production
```

### Authentication Service

```bash
# Database Connection
ConnectionStrings__DefaultConnection=Host=localhost;Database=gcse_auth;Username=postgres;Password=postgres

# JWT Settings
Jwt__Key=YourSuperSecretKeyHere_MustBe32CharsMin
Jwt__Issuer=GCSEPhysicsLMS
Jwt__Audience=GCSEPhysicsLMS
```

### CMS Service

```bash
# MongoDB Connection
MongoDB__ConnectionString=mongodb://localhost:27017
MongoDB__DatabaseName=gcse_cms

# For authenticated MongoDB
MongoDB__ConnectionString=mongodb://username:password@localhost:27017
```

### Teacher-Student Service

```bash
# Database Connection
ConnectionStrings__DefaultConnection=Host=localhost;Database=gcse_relationships;Username=postgres;Password=postgres
```

### Frontend

```bash
# API Gateway URL (for production)
VITE_API_URL=http://localhost:5000
```

## Docker Compose Configuration

The `docker-compose.yml` file contains environment variables for containerized deployment:

```yaml
services:
  authservice:
    environment:
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=gcse_auth;Username=postgres;Password=postgres
      - ASPNETCORE_ENVIRONMENT=Development
      
  cmsservice:
    environment:
      - MongoDB__ConnectionString=mongodb://admin:admin@mongodb:27017
      - MongoDB__DatabaseName=gcse_cms
      - ASPNETCORE_ENVIRONMENT=Development
      
  teacherstudentservice:
    environment:
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=gcse_relationships;Username=postgres;Password=postgres
      - ASPNETCORE_ENVIRONMENT=Development
```

## Database Configuration

### PostgreSQL

**Development (Docker)**:
```
Host: postgres  # or localhost for local development
Port: 5432
Username: postgres
Password: postgres
Databases: gcse_auth, gcse_relationships
```

**Production**:
```
Host: your-postgres-server.com
Port: 5432
Username: secure_username
Password: strong_password_here
SSL Mode: Require
```

### MongoDB

**Development (Docker)**:
```
ConnectionString: mongodb://admin:admin@mongodb:27017
Database: gcse_cms
```

**Production**:
```
ConnectionString: mongodb://username:password@your-mongodb-server.com:27017/?authSource=admin&ssl=true
Database: gcse_cms
```

## Production Configuration

### 1. Generate Secure JWT Key

```bash
# Generate a random 32+ character key
openssl rand -base64 32
```

Use this in your JWT configuration.

### 2. Database Credentials

Create strong passwords for database access:
```bash
# Generate secure password
openssl rand -base64 24
```

### 3. Environment File

Create a `.env` file (DO NOT commit to git):

```bash
# .env
ASPNETCORE_ENVIRONMENT=Production
JWT_SECRET_KEY=<your-generated-key>
POSTGRES_PASSWORD=<your-postgres-password>
MONGO_PASSWORD=<your-mongo-password>
```

Load in docker-compose:
```yaml
services:
  authservice:
    env_file:
      - .env
```

### 4. HTTPS/SSL Configuration

#### Option A: Reverse Proxy (Recommended)

Use NGINX or Traefik as reverse proxy with Let's Encrypt SSL:

```nginx
server {
    listen 443 ssl http2;
    server_name lms.yourdomain.com;

    ssl_certificate /etc/letsencrypt/live/yourdomain.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/yourdomain.com/privkey.pem;

    location / {
        proxy_pass http://localhost:3000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }

    location /api {
        proxy_pass http://localhost:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }
}
```

#### Option B: Kestrel SSL

Configure in appsettings.json:
```json
{
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://*:443",
        "Certificate": {
          "Path": "/path/to/certificate.pfx",
          "Password": "certificate_password"
        }
      }
    }
  }
}
```

### 5. CORS Configuration

For production, update CORS settings in each service's Program.cs:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

### 6. Logging Configuration

Update appsettings.Production.json:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    },
    "Console": {
      "IncludeScopes": false
    },
    "File": {
      "Path": "/var/log/gcse-lms/",
      "IncludeScopes": false
    }
  }
}
```

## Configuration by Environment

### Development

Default configuration works out of the box. Services communicate via localhost.

### Staging

Same as production but with:
- Test databases
- Relaxed CORS
- Detailed logging
- No rate limiting

### Production

- Strong passwords
- HTTPS only
- Strict CORS
- Minimal logging
- Rate limiting enabled
- Connection pooling
- Database SSL

## Memory Configuration

Adjust Docker Compose memory limits based on your server:

```yaml
services:
  authservice:
    deploy:
      resources:
        limits:
          memory: 512M  # Increase for higher load
        reservations:
          memory: 256M
```

## Monitoring Configuration

### Health Checks

All services expose `/api/<service>/health` endpoints.

Configure Docker health checks:
```yaml
services:
  authservice:
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080/api/auth/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
```

### Application Insights (Optional)

Add to appsettings.json:
```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-key-here",
    "EnableAdaptiveSampling": true
  }
}
```

## Configuration Best Practices

1. **Never commit secrets** - Use environment variables or secret managers
2. **Use different keys per environment** - Development, staging, production
3. **Rotate credentials regularly** - Especially JWT keys and database passwords
4. **Enable SSL/TLS** - Always in production
5. **Implement rate limiting** - Protect against abuse
6. **Monitor resource usage** - Adjust memory limits as needed
7. **Use connection pooling** - For better database performance
8. **Enable response compression** - Reduce bandwidth
9. **Configure proper timeouts** - Prevent hanging requests
10. **Set up proper logging** - For debugging and monitoring

## Troubleshooting

### Service Can't Connect to Database

Check connection string format and ensure database is running:
```bash
# Test PostgreSQL connection
docker exec -it gcse-postgres psql -U postgres -d gcse_auth -c "SELECT 1;"

# Test MongoDB connection
docker exec -it gcse-mongodb mongosh --eval "db.adminCommand('ping')"
```

### JWT Token Issues

Ensure all services use the same JWT key and settings.

### CORS Errors

Update CORS policy to include your frontend domain.

### Memory Issues

Reduce service limits or upgrade server RAM.

## Security Checklist

- [ ] Strong JWT secret key (32+ characters)
- [ ] Strong database passwords
- [ ] HTTPS/SSL enabled
- [ ] CORS configured for specific origins
- [ ] Environment variables for secrets
- [ ] Database SSL connections
- [ ] Rate limiting enabled
- [ ] Input validation on all endpoints
- [ ] Password complexity requirements
- [ ] Account lockout after failed attempts
- [ ] Security headers configured
- [ ] Regular dependency updates
- [ ] Security audit logs enabled

---

**Last Updated**: February 2026  
**Version**: 1.0.0
