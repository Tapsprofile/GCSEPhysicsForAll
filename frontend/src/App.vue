<template>
  <div class="app">
    <header class="header">
      <div>
        <p class="kicker">GCSE Modular Education Portal</p>
        <h1>Physics, Chemistry, and Mathematics</h1>
        <p class="subtitle">
          Content is data-driven and unlocks sequentially as chapters are completed.
        </p>
      </div>
      <div class="status" v-if="statusMessage">
        {{ statusMessage }}
      </div>
    </header>

    <main class="layout">
      <section class="panel">
        <h2>Subjects</h2>
        <div v-if="subjects.length === 0" class="empty-state">
          No subjects available yet.
        </div>
        <ul class="list">
          <li v-for="subject in subjects" :key="subject.id">
            <button
              class="list-button"
              :class="{ active: selectedSubject?.id === subject.id }"
              @click="selectSubject(subject)"
            >
              <span class="list-title">{{ subject.title }}</span>
              <span class="list-meta">{{ subject.chapterCount }} chapters</span>
            </button>
          </li>
        </ul>
      </section>

      <section class="panel">
        <h2>Chapters</h2>
        <div v-if="!selectedSubject" class="empty-state">
          Select a subject to view chapters.
        </div>
        <ul v-else class="list">
          <li v-for="chapter in orderedChapters" :key="chapter.id">
            <button
              class="list-button"
              :class="{
                active: selectedChapter?.id === chapter.id,
                locked: !isUnlocked(chapter)
              }"
              :disabled="!isUnlocked(chapter)"
              @click="selectChapter(chapter)"
            >
              <span class="list-title">
                {{ chapter.order }}. {{ chapter.title }}
              </span>
              <span class="list-meta">
                {{ chapter.lessonCount }} lessons
                <span v-if="isCompleted(chapter)" class="chip success">Completed</span>
                <span v-else-if="!isUnlocked(chapter)" class="chip muted">Locked</span>
              </span>
            </button>
          </li>
        </ul>
      </section>

      <section class="panel detail">
        <h2>Chapter Detail</h2>
        <div v-if="!selectedChapter" class="empty-state">
          Select a chapter to see learning objectives and lessons.
        </div>
        <div v-else-if="chapterDetail" class="detail-content">
          <div class="detail-header">
            <div>
              <h3>{{ chapterDetail.title }}</h3>
              <p class="summary">{{ chapterDetail.summary }}</p>
            </div>
            <button class="primary" @click="markComplete" :disabled="isCompleted(chapterDetail)">
              {{ isCompleted(chapterDetail) ? "Completed" : "Mark chapter complete" }}
            </button>
          </div>

          <section>
            <h4>Learning Objectives</h4>
            <ul class="bullet-list">
              <li v-for="objective in chapterDetail.learningObjectives" :key="objective">
                {{ objective }}
              </li>
            </ul>
          </section>

          <section>
            <h4>Lessons</h4>
            <div class="cards">
              <div v-for="lesson in chapterDetail.lessons" :key="lesson.id" class="card">
                <div>
                  <h5>{{ lesson.title }}</h5>
                  <p>{{ lesson.summary }}</p>
                </div>
                <span class="chip">{{ lesson.estimatedMinutes }} min</span>
              </div>
            </div>
          </section>

          <section v-if="chapterDetail.widgets?.length">
            <h4>Widgets</h4>
            <div class="cards">
              <div v-for="widget in chapterDetail.widgets" :key="widget.label" class="card">
                <div>
                  <h5>{{ widget.label }}</h5>
                  <p>{{ widget.purpose }}</p>
                </div>
                <span class="chip">{{ widget.type }}</span>
              </div>
            </div>
          </section>
        </div>
      </section>
    </main>
  </div>
</template>

<script setup>
import { computed, onMounted, ref } from "vue";
import { fetchJson } from "./api";

const subjects = ref([]);
const chapters = ref([]);
const selectedSubject = ref(null);
const selectedChapter = ref(null);
const chapterDetail = ref(null);
const statusMessage = ref("");
const completedChapterIds = ref(new Set());

const orderedChapters = computed(() =>
  [...chapters.value].sort((a, b) => a.order - b.order)
);

const progressKey = computed(() =>
  selectedSubject.value ? `progress:${selectedSubject.value.id}` : null
);

onMounted(async () => {
  await loadSubjects();
});

async function loadSubjects() {
  statusMessage.value = "Loading subjects...";
  try {
    subjects.value = await fetchJson("/api/subjects");
  } catch (error) {
    statusMessage.value = error.message;
    return;
  }
  statusMessage.value = "";
}

async function selectSubject(subject) {
  selectedSubject.value = subject;
  selectedChapter.value = null;
  chapterDetail.value = null;
  loadProgress(subject.id);

  statusMessage.value = "Loading chapters...";
  try {
    chapters.value = await fetchJson(`/api/subjects/${subject.id}/chapters`);
    statusMessage.value = "";
  } catch (error) {
    statusMessage.value = error.message;
  }
}

async function selectChapter(chapter) {
  selectedChapter.value = chapter;
  statusMessage.value = "Loading chapter...";
  try {
    chapterDetail.value = await fetchJson(
      `/api/subjects/${selectedSubject.value.id}/chapters/${chapter.id}`
    );
    statusMessage.value = "";
  } catch (error) {
    statusMessage.value = error.message;
  }
}

function loadProgress(subjectId) {
  const stored = localStorage.getItem(`progress:${subjectId}`);
  if (!stored) {
    completedChapterIds.value = new Set();
    return;
  }

  try {
    const parsed = JSON.parse(stored);
    completedChapterIds.value = new Set(parsed.completedChapterIds || []);
  } catch (error) {
    completedChapterIds.value = new Set();
  }
}

function saveProgress() {
  if (!progressKey.value) {
    return;
  }

  const payload = {
    completedChapterIds: Array.from(completedChapterIds.value)
  };
  localStorage.setItem(progressKey.value, JSON.stringify(payload));
}

function markComplete() {
  if (!chapterDetail.value) {
    return;
  }

  completedChapterIds.value.add(chapterDetail.value.id);
  saveProgress();
}

function isCompleted(chapter) {
  return completedChapterIds.value.has(chapter.id);
}

function isUnlocked(chapter) {
  if (chapter.order === 1) {
    return true;
  }

  const previous = orderedChapters.value.find(
    (item) => item.order === chapter.order - 1
  );

  if (!previous) {
    return true;
  }

  return completedChapterIds.value.has(previous.id);
}
</script>
