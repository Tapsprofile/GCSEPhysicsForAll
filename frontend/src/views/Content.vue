<template>
  <div class="container">
    <h1>Content Library</h1>
    
    <div class="filters">
      <select v-model="filter.moduleType" @change="loadContent">
        <option value="">All Modules</option>
        <option value="Physics">Physics</option>
        <option value="Chemistry">Chemistry</option>
        <option value="Biology">Biology</option>
      </select>
      
      <select v-model="filter.contentType" @change="loadContent">
        <option value="">All Types</option>
        <option value="Lesson">Lesson</option>
        <option value="Quiz">Quiz</option>
        <option value="Video">Video</option>
        <option value="Assignment">Assignment</option>
      </select>
    </div>
    
    <div v-if="loading" class="loading">Loading content...</div>
    
    <div v-else class="grid">
      <div v-for="item in content" :key="item.id" class="card">
        <h3 class="card-title">{{ item.title }}</h3>
        <p>{{ item.description }}</p>
        <div class="content-meta">
          <span class="badge">{{ item.moduleType }}</span>
          <span class="badge">{{ item.contentType }}</span>
        </div>
      </div>
    </div>
    
    <div v-if="!loading && content.length === 0" class="empty">
      <p>No content found. Create some content to get started!</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { contentService } from '../services/auth'

const content = ref([])
const loading = ref(false)
const filter = ref({
  moduleType: '',
  contentType: ''
})

const loadContent = async () => {
  loading.value = true
  try {
    const params = {}
    if (filter.value.moduleType) params.moduleType = filter.value.moduleType
    if (filter.value.contentType) params.contentType = filter.value.contentType
    
    content.value = await contentService.getAll(params)
  } catch (error) {
    console.error('Error loading content:', error)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadContent()
})
</script>

<style scoped>
.filters {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
}

.filters select {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: #666;
}

.empty {
  text-align: center;
  padding: 3rem;
  color: #666;
}

.content-meta {
  margin-top: 1rem;
  display: flex;
  gap: 0.5rem;
}

.badge {
  display: inline-block;
  padding: 0.25rem 0.75rem;
  background-color: #3498db;
  color: white;
  border-radius: 12px;
  font-size: 0.875rem;
}
</style>
