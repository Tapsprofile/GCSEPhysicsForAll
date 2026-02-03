# Registration and Learning Path Specification

This document describes the full registration options for Parent, Teacher, and
Student roles plus the course structure and learning path model used in the
portal. It is designed to be extended for future courses such as DVLA or
Nursing.

## Goals
- Keep role-specific registration flows explicit and auditable.
- Support parent/guardian consent for under-16 learners.
- Keep learning paths data-driven with evidence and assessment checkpoints.
- Make it easy to add new course domains without code changes.

## Registration Roles

### Student
**Purpose:** Learners completing courses and submitting evidence.

**Required fields**
- First name, last name
- Date of birth (age validation)
- Email + password
- Year group (or adult learner)
- Preferred subjects

**Optional fields**
- Phone
- Learning goals
- Accessibility needs

**Verification steps**
- Email verification
- Guardian consent (if under 16)

**Consent requirements**
- Terms of service
- Privacy policy
- Guardian consent (under 16)

**Linking**
- Students can request a parent/guardian invite code.

---

### Parent / Guardian
**Purpose:** Oversight, safeguarding consent, and learner support.

**Required fields**
- First name, last name
- Email + phone
- Relationship to student

**Optional fields**
- Address
- Preferred contact method

**Verification steps**
- Email verification
- Phone verification

**Consent requirements**
- Terms of service
- Privacy policy
- Safeguarding acknowledgement

**Linking**
- Parents can link to an existing student using a code.
- Parents can create a new student profile.

---

### Teacher / Mentor
**Purpose:** Delivers reviews, feedback, and guidance.

**Required fields**
- First name, last name
- Work email + phone
- Qualifications
- Years of experience
- Subject expertise

**Optional fields**
- Bio
- Weekly availability
- References

**Verification steps**
- Email verification
- Identity verification
- DBS safeguarding check
- Subject approval by academic lead

**Consent requirements**
- Mentor terms
- Privacy policy
- Code of conduct

**Capacity defaults**
- Max students: 20
- Max weekly reviews: 60

## Registration State Flow

```
draft -> submitted -> pending_verification -> active
           |                    |
           v                    v
        rejected             suspended -> closed
```

States are defined in `backend/Data/registration-options.json` so policy
changes do not require code updates.

## Course Structure Model

```
Subject
  -> Chapters (sequenced by order)
     -> Lessons (granular learning units)
     -> Widgets (quizzes, simulations, labs)
  -> Learning Paths (track-based roadmap)
     -> Steps (grouped chapters + assessment)
     -> Evidence + Assessment requirements
```

**Subjects** are content containers (e.g., Physics, Chemistry, Mathematics).
**Chapters** provide the canonical syllabus order. **Learning paths** group
chapters into steps with milestones, evidence requirements, and assessments.

## Learning Path Model (Key Fields)

- `id`: Unique path ID.
- `subjectId`: Links to a subject.
- `title`: Display name.
- `track`: Core, Higher, Professional, etc.
- `description`: Summary of the roadmap.
- `outcomes`: End goals for the learner.
- `steps`: Ordered steps with evidence and assessments.

### Step Fields
- `title` and `description`
- `milestone`: Checkpoint label
- `recommendedWeeks`
- `chapterIds`: Links to existing chapters
- `prerequisiteStepIds`: Gating rules
- `evidence`: Number of examples and evidence types
- `assessment`: Type and pass criteria

## Adding New Courses (DVLA, Nursing)

1. Add a new subject entry to `backend/Data/subjects.json`.
2. Add a learning path entry to `backend/Data/learning-paths.json`:
   - Use track names like `Professional`, `Licensing`, or `CPD`.
   - Include evidence requirements aligned with the domain.
3. Update the supported subjects list in the registration options if teachers
   should be onboarded for the new course.

Example learning path track names:
- DVLA: `Driver Theory`, `Hazard Perception`
- Nursing: `Fundamentals`, `Clinical Practice`, `Patient Safety`

The platform remains data-driven, so new course domains require no code
changes unless new widgets or assessments are introduced.
