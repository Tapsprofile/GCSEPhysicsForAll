# Modular Education Portal (GCSE Physics for All)

Highly modular LMS architecture written for .NET 8 and Vue 3. This README
captures the executive blueprint for a headless, scalable education platform.

## Executive Blueprint (Copilot / Tech Lead Summary)

### Core Strategy
Scale from GCSE Physics to any subject while preserving the 1-on-1
teacher-student relationship using a headless microservices architecture
and a Core-and-Plugin model.

### System Infrastructure
- Front-End: Vue 3 SPA (Vite). Content-agnostic UI that renders modules
  dynamically based on JSON schemas.
- Back-End: .NET 8 microservices behind an API Gateway (YARP).
- Hybrid Data Layer:
  - PostgreSQL for relational data (users, roles, pairing, progress).
  - MongoDB for flexible, JSON-based course content and schemas.

### Modular Component Schema

| Component          | Function                                  | Scalability Benefit                               |
|-------------------|-------------------------------------------|---------------------------------------------------|
| Topic Container    | Subject wrapper (e.g., Physics)           | Duplicate for any subject via config              |
| Widget Library     | Quizzes, LaTeX, 3D models                  | Plug-and-play tools across all chapters           |
| Review Engine      | Assignment submission + feedback workflow | Standardized review loop for any discipline       |

### High-Touch Workflow (1-on-1 Portal)
- Automated Pairing: Availability Matrix service pairs new students with
  teachers (capped at 20:1 ratio) on registration.
- Review Loop: Students submit work via Review Engine. Teachers access a
  Review Queue to provide granular feedback (voice/text) and milestone
  approval.
- Real-Time Sync: SignalR pushes instant notifications for feedback.

### Technical Roadmap
- Content as Data: Headless CMS (Strapi) injects new syllabi without code
  deployments.
- Security: JWT-based auth with RBAC for Teacher vs Student isolation.
- Future-Proofing: Standard JSON Schema for course content makes new
  subjects data-only.

## Feature Set

### 1) Core Learning & Content Management
- Modular Topic Engine: Dynamic subject injection via JSON content models.
- Rich Media Syllabus: LaTeX, embedded video, and interactive 3D models.
- Progressive Disclosure UI: Sequential content unlock (Chapter 1 -> 2).
- Interactive Widget Library: Self-marking quizzes and drag-and-drop
  circuit builders.

### 2) One-on-One Mentorship Portal (High-Touch Layer)
- Automated Mentor Pairing: Load-balancing assignment by subject expertise
  and capacity.
- Submission Review Queue: Teacher dashboard for pending "10 Example"
  submissions.
- Multimodal Feedback: Timestamped video, voice, or annotated text.
- Direct Messaging: Secure real-time chat with assigned mentor.

### 3) Student Experience & Engagement
- Personalized Dashboard: Percent complete, average grade, next milestone.
- Evidence Locker: Portfolio storage for chapter submissions.
- Gamified Achievements: Badges and certificates per milestone.
- Mobile-First UX: Tablet and phone friendly.

### 4) Administrative & Architect Tools
- RBAC: Granular permissions for Students, Teachers, and Super-Admins.
- Headless CMS Integration: AQA / Edexcel content updates without code
  changes.
- Analytics Engine: Pass rates and teacher response time reporting.
- Audit Trail: Safeguarding logs for teacher-student interactions.

## .NET 8 + Vue 3 Implementation Notes
- Services: Identity, Content, Enrollment, Review, Messaging, Notifications,
  Analytics, and Admin.
- Communication: REST for external API, internal gRPC where needed.
- Realtime: SignalR hub for reviews and messaging.
- Deployment: Containerized services with environment-based config and
  scalable horizontal workers.

## Repository Layout
- backend/ - .NET 8 minimal API that serves subject, chapter, and lesson data.
- frontend/ - Vue 3 (Vite) SPA that consumes the API and unlocks chapters
  sequentially.

## Local Development

### Backend (API)
```bash
dotnet run --project backend/ModularEducationPortal.Backend.csproj
```

The API exposes:
- GET /api/subjects
- GET /api/subjects/{subjectId}
- GET /api/subjects/{subjectId}/chapters
- GET /api/subjects/{subjectId}/chapters/{chapterId}

### Frontend (Vue 3)
```bash
cd frontend
npm install
npm run dev
```

Optional environment variable for API base URL:
```bash
VITE_API_BASE=http://localhost:5000
```

## Content Data
Subject coverage and chapter sequencing are defined in:
```
backend/Data/subjects.json
```

This includes GCSE Physics, Chemistry, and Mathematics chapters with lesson
summaries, objectives, and widget references.
