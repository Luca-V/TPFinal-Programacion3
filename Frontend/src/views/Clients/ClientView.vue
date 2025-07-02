<script setup>
import { ref } from 'vue';
import { RouterLink, useRoute, useRouter } from 'vue-router';
import { Api } from '@/helpers/axios';

const route = useRoute()
const router = useRouter();
const clientData = ref({
    id: route.params.id,
    name: "",
    email: ""
});

const getClient = async () => {
    try {
        const { data } = await Api.get(`/Client/${route.params.id}`);
        clientData.value = data;
    } catch (error) {
        console.log(error);
        Alert.fire({
            title: 'Error al obtener cliente',
            text: 'El cliente no ha sido encontrado',
            icon: 'error',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000,
        });
        router.push('/clients');
    }
}
getClient();

const viewClients = async () => {
    router.push(`/clients`)
}

const editClient = async () => {
    router.push(`/clients/${clientData.value.id}/edit`)
}

const deleteClient = async () => {
    try {
        await Api.delete(`/Client/${clientData.value.id}`);
        Alert.fire({
            title: 'Cliente eliminado',
            text: 'El cliente ha sido eliminado con exito',
            icon: 'success',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000,
        });
        router.push('/clients');
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
}
</script>

<template>
    <div class="text-center flex flex-col h-100 justify-center items-center">
    <h1>Detalles del cliente</h1>
    <p>Detalles del cliente registrado en la aplicación</p>

    <div class="flex flex-col justify-center items-center h-100 gap-5 w-100 mt-10 bg-neutral-100 shadow-2xl rounded-lg">
      <label for="name">Nombre</label>
      <input disabled type="text" id="name" v-model="clientData.name" name="name" placeholder="Nombre del cliente" required class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2" />
      <label for="email">Email</label>
      <input disabled type="email" id="email" v-model="clientData.email" name="email" placeholder="Email del cliente" required class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2" />

      <button @click="viewClients" class="bg-amber-300 px-2 py-2 rounded-lg cursor-pointer w-80 transition duration-500 hover:bg-amber-200">Volver</button>
      <button @click="editClient" class="bg-amber-300 px-2 py-2 rounded-lg cursor-pointer w-80 transition duration-500 hover:bg-amber-200">Editar</button>
      <button @click="deleteClient" class="bg-red-500 px-2 py-2 rounded-lg cursor-pointer w-80 transition duration-500 hover:bg-red-600">Eliminar</button>
    </div>
  </div>
</template>