<template>
  <div class="container">
    <h1>Dashboard</h1>
    <p>Welcome to your dashboard, {{ authStore.user?.firstName }}!</p>
    <p>Role: <strong>{{ authStore.user?.role }}</strong></p>
    
    <div class="dashboard-grid">
      <div class="card">
        <h3>Quick Stats</h3>
        <p>Your personalized learning statistics will appear here.</p>
      </div>
      
      <div class="card">
        <h3>Recent Activity</h3>
        <p>Track your recent interactions and progress.</p>
      </div>
      
      <div class="card">
        <h3>Upcoming</h3>
        <p>View upcoming lessons and assignments.</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore()
</script>

<style scoped>
.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
  margin-top: 2rem;
}
</style>
