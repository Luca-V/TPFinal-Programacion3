<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="flex flex-col min-h-screen">
    <header class="flex justify-center p-4 bg-amber-400">
      <nav class="flex gap-4 font-bold">
        <RouterLink to="/" class="hover:text-slate-500 transition-colors duration-500">Inicio</RouterLink>
        <RouterLink to="/clients" class="hover:text-slate-500 transition-colors duration-500">Clientes</RouterLink>
        <RouterLink to="/movements-history" class="hover:text-slate-500 transition-colors duration-500">Historial de Movimientos</RouterLink>
        <RouterLink to="/current-status-analysis" class="hover:text-slate-500 transition-colors duration-500">Analisis de Estado Actual</RouterLink>
      </nav>
    </header>

    <RouterView class="flex-1 bg-neutral-200" />

    <footer class="flex flex-col items-center justify-center p-4 bg-amber-400 text-black">
      <p>Proyecto Final Programacion 3</p>
      <p>Creado por <span class="font-bold">Roberto Luca Valentini</span></p>
    </footer>
  </div>
</template>