<script setup>
import { ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router';
import { Api } from '@/helpers/axios';
import Alert from 'sweetalert2';
import moment from 'moment';

const router = useRouter()
const movements = ref([])

const getMovements = async () => {
  try {
    const { data } = await Api.get('/Movement')
    console.log(data)
    movements.value = data
    movements.value.forEach(movement => {
      movement.date = moment(movement.date).format('YYYY/MM/DD HH:mm');
    })
  } catch (error) {
    console.log(error)
    movements.value = []
  }
}

const viewMovement = async (id) => {
  router.push(`/movement/${id}`)
}

const editMovement = async (id) => {
  router.push(`/movement/${id}/edit`)
}

const deleteMovement = async (id) => {
  try {
    await Api.delete(`/Movement/${id}`)
    getMovements()
    Alert.fire({
      title: 'Movimiento eliminado',
      text: 'El movimiento ha sido eliminado con exito',
      icon: 'success',
      showConfirmButton: false,
      timerProgressBar: true,
      timer: 3000,
    });
  } catch (error) {
    console.log(error)
    Alert.fire({
      title: 'Error al eliminar movimiento',
      text: 'El movimiento no ha sido eliminado',
      icon: 'error',
      showConfirmButton: false,
      timerProgressBar: true,
      timer: 3000,
    });
  }
}

getMovements()
</script>

<template>
  <div class="text-center flex flex-col h-100 justify-center items-center bg-[url('https://images7.alphacoders.com/888/thumb-1920-888098.jpg')] bg-no-repeat bg-cover bg-center object-cover bg-blend-overlay">
    <h1>Movimientos</h1>
    <p>Gestiona los movimientos registrados en la aplicación</p>

    <div v-if="movements.length > 0" class="my-5 flex gap-5">
      <RouterLink to="/movement/new" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-4 py-2 rounded-lg transition cursor-pointer">
        Crear Compra/Venta
      </RouterLink>
    </div>

    <table :class="movements.length > 0 ? 'bg-white shadow-xl rounded-lg overflow-hidden' : 'w-full max-w-7xl mx-auto bg-white shadow-xl rounded-lg overflow-hidden mt-5'">
      <thead class="bg-gradient-to-r from-amber-500 to-yellow-400 text-white">
        <tr>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Tipo de Movimiento</th>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Cliente</th>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Criptomoneda</th>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Cantidad</th>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Precio</th>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Fecha</th>
          <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Acciones</th>
        </tr>
      </thead>
      <tbody class="divide-y divide-gray-200">
        <tr v-for="movement in movements" :key="movement.id" class="hover:bg-gray-50 transition">
          <td class="px-6 py-4 whitespace-nowrap">{{ movement.action.toUpperCase() }}</td>
          <td class="px-6 py-4 whitespace-nowrap">{{ movement?.client?.name }}</td>
          <td class="px-6 py-4 whitespace-nowrap">{{ movement.cryptoCode?.toUpperCase() }}</td>
          <td class="px-6 py-4 whitespace-nowrap">{{ movement.cryptoAmount }}</td>
          <td class="px-6 py-4 whitespace-nowrap">{{ movement.total }}</td>
          <td class="px-6 py-4 whitespace-nowrap">{{ movement.date }}</td>
          <td class="px-6 py-4 text-center space-x-2">
            <button @click="viewMovement(movement.id)" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-2 py-2 rounded-lg transition">Ver Detalles</button>
            <button @click="editMovement(movement.id)" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-2 py-2 rounded-lg transition">Editar</button>
            <button @click="deleteMovement(movement.id)" class="bg-red-500 hover:bg-red-600 text-white text-sm px-2 py-2 rounded-lg transition">Eliminar</button>
          </td>
        </tr>
        <tr v-if="movements.length === 0" class="hover:bg-gray-50 transition">
          <td class="px-6 py-4 whitespace-nowrap">No hay movimientos registrados</td>
          <td class="px-6 py-4 whitespace-nowrap"></td>
          <td class="px-6 py-4 whitespace-nowrap"></td>
          <td class="px-6 py-4 whitespace-nowrap"></td>
          <td class="px-6 py-4 whitespace-nowrap"></td>
          <td class="px-6 py-4 whitespace-nowrap"></td>
          <td class="px-6 py-4 text-center space-x-2">
            <RouterLink to="/movement/new" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-4 py-2 rounded-lg transition cursor-pointer">
              Crear Compra/Venta
            </RouterLink>
          </td>
        </tr>
      </tbody>
    </table>

  </div>
</template>

<style scoped></style>