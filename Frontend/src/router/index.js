import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/clients',
      name: 'clients',
      component: () => import('../views/Clients/ClientsView.vue'),
    },
    {
      path: '/clients/:id',
      name: 'detail-client',
      component: () => import('../views/Clients/ClientView.vue'),
    },
    {
      path: '/clients/new',
      name: 'new-client',
      component: () => import('../views/Clients/NewClientView.vue'),
    },
    {
      path: '/clients/:id/edit',
      name: 'edit-client',
      component: () => import('../views/Clients/EditClientView.vue'),
    },
    {
      path: '/movements-history',
      name: 'movements-history',
      component: () => import('../views/MovementHistory/MovenmentsView.vue'),
    },
    {
      path: '/movement/:id',
      name: 'detail-movement',
      component: () => import('../views/MovementHistory/MovementView.vue'),
    },
    {
      path: '/movement/new',
      name: 'new-movement',
      component: () => import('../views/MovementHistory/NewMovementView.vue'),
    },
    {
      path: '/movement/:id/edit',
      name: 'edit-movement',
      component: () => import('../views/MovementHistory/EditMovementView.vue'),
    },
    {
      path: '/current-status-analysis',
      name: 'current-status-analysis',
      component: () => import('../views/CurrentStatusAnalysis/index.vue'),
    }
  ],
})

export default router
