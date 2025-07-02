<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { Api } from '@/helpers/axios';
import Alert from 'sweetalert2';

const router = useRouter();

const clientData = ref({
  name: "",
  email: ""
});

const createClient = async () => {
  try {
    await Api.post('/Client', clientData.value);

    Alert.fire({
      title: 'Cliente creado',
      text: 'El cliente ha sido creado con exito',
      icon: 'success',
      showConfirmButton: false,
      timerProgressBar: true,
      timer: 3000,
    });

    router.push('/clients');
  } catch (error) {
    console.log(error);

    Alert.fire({
      title: 'Error al crear cliente',
      text: 'El cliente no ha sido creado',
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
    <h1>Crear Cliente</h1>
    <p>Crea un nuevo cliente en la aplicación</p>

    <form @submit.prevent="createClient" class="flex flex-col justify-center items-center h-100 gap-5 w-100 mt-10 bg-neutral-100 shadow-2xl rounded-lg">
      <label for="name">Nombre</label>
      <input type="text" id="name" v-model="clientData.name" name="name" placeholder="Nombre del cliente" required class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2" />
      <label for="email">Email</label>
      <input type="email" id="email" v-model="clientData.email" name="email" placeholder="Email del cliente" required class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2" />

      <button type="submit" class="bg-amber-300 px-4 py-2 rounded-lg cursor-pointer w-80 transition duration-500 hover:bg-amber-200">Crear</button>
    </form>
  </div>
</template>