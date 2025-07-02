<script setup>
import { ref } from 'vue';
import { RouterLink, useRouter } from 'vue-router';
import { Api } from '@/helpers/axios';
import Alert from 'sweetalert2';

const router = useRouter();
const clients = ref([]);

const getClients = async () => {
  try {
    const { data } = await Api.get('/Client');
    clients.value = data;
  } catch (error) {
    console.log(error);
    clients.value = [];
  }
};

getClients();

const detailClient = async (id) => {
  try {
    router.push(`/clients/${id}`);
  } catch (error) {
    console.log(error);
  }
};

const editClient = async (id) => {
  try {
    router.push(`/clients/${id}/edit`);
  } catch (error) {
    console.log(error);
  }
};

const deleteClient = async (id) => {
  try {
    await Api.delete(`/Client/${id}`);
    await getClients();
    Alert.fire({
      title: 'Cliente eliminado',
      text: 'El cliente ha sido eliminado con exito',
      icon: 'success',
      showConfirmButton: false,
      timerProgressBar: true,
      timer: 3000,
    });
  } catch (error) {
    console.log(error);
    Alert.fire({
      title: 'Error al eliminar cliente',
      text: 'El cliente no ha sido eliminado',
      icon: 'error',
      showConfirmButton: false,
      timerProgressBar: true,
      timer: 3000,
    });
  }
};
</script>

<template>
  <div class="text-center flex flex-col h-100 justify-center items-center bg-[url('https://images7.alphacoders.com/888/thumb-1920-888098.jpg')] bg-no-repeat bg-cover bg-center object-cover bg-blend-overlay">
    <h1>Clientes</h1>
    <p>Gestiona los clientes registrados en la aplicación</p>

    <RouterLink v-if="clients.length > 0" to="/clients/new" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-4 py-2 rounded-lg transition cursor-pointer my-5">
      Crear Cliente
    </RouterLink>

    <table :class="clients.length > 0 ? 'bg-white shadow-xl rounded-lg overflow-hidden' : 'w-full max-w-5xl mx-auto bg-white shadow-xl rounded-lg overflow-hidden mt-5'">
      <thead class="bg-gradient-to-r from-amber-500 to-yellow-400 text-white">
        <tr>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Nombre</th>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Email</th>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Acciones</th>
        </tr>
      </thead>
      <tbody class="divide-y divide-gray-200">
        <tr v-for="client in clients" :key="client.id" class="hover:bg-gray-50 transition">
          <td class="px-6 py-4 whitespace-nowrap">{{ client.name }}</td>
          <td class="px-6 py-4 whitespace-nowrap">{{ client.email }}</td>
          <td class="px-6 py-4 text-center space-x-2">
            <button @click="detailClient(client.id)" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-4 py-2 rounded-lg transition cursor-pointer">Ver Detalles</button>
            <button @click="editClient(client.id)" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-4 py-2 rounded-lg transition cursor-pointer">Editar</button>
            <button @click="deleteClient(client.id)" class="bg-red-500 hover:bg-red-600 text-white text-sm px-4 py-2 rounded-lg transition cursor-pointer">Eliminar</button>
          </td>
        </tr>
        <tr v-if="clients.length === 0" class="hover:bg-gray-50 transition">
          <td class="px-6 py-4 whitespace-nowrap">No hay clientes registrados</td>
          <td class="px-6 py-4 whitespace-nowrap"></td>
          <td class="px-6 py-4 text-center space-x-2">
            <RouterLink to="/clients/new" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-4 py-2 rounded-lg transition cursor-pointer">Crear Cliente</RouterLink>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>