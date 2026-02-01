<template>
  <div class="container">
    <h1>Welcome to GCSE Physics For All</h1>
    <p class="intro">A comprehensive Learning Management System for GCSE Physics students, teachers, and parents.</p>
    
    <div class="features">
      <div class="feature-card">
        <h3>📚 For Students</h3>
        <p>Access interactive lessons, quizzes, and track your progress</p>
      </div>
      <div class="feature-card">
        <h3>👨‍🏫 For Teachers</h3>
        <p>Create and manage content, monitor student progress</p>
      </div>
      <div class="feature-card">
        <h3>👪 For Parents</h3>
        <p>Stay informed about your child's learning journey</p>
      </div>
    </div>
    
    <div class="cta">
      <router-link to="/register" class="btn btn-primary">Get Started</router-link>
      <router-link to="/login" class="btn btn-success">Login</router-link>
    </div>
  </div>
</template>

<script setup>
</script>

<style scoped>
.intro {
  font-size: 1.2rem;
  margin: 1.5rem 0;
  color: #666;
}

.features {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
  margin: 3rem 0;
}

.feature-card {
  background: #f8f9fa;
  padding: 2rem;
  border-radius: 8px;
  text-align: center;
}

.feature-card h3 {
  margin-bottom: 1rem;
  font-size: 1.5rem;
}

.cta {
  display: flex;
  gap: 1rem;
  justify-content: center;
  margin-top: 3rem;
}
</style>
