<script setup>
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router';
import { Api } from '@/helpers/axios';
import Alert from 'sweetalert2';

const route = useRoute()
const router = useRouter();

const clients = ref([])
const cryptoData = ref({
    id: route.params.id,
    clientID: '',
    action: '',
    cryptoCode: '',
    cryptoAmount: '',
    date: ''
})

async function getClients() {
    const { data } = await Api.get('/Client')
    clients.value = data;
}

getClients()

async function getMovement() {
    try {
        const { data } = await Api.get(`/Movement/${route.params.id}`)
        cryptoData.value = data
    } catch (error) {
        console.log(error)
        Alert.fire({
            title: 'Error al obtener movimiento',
            text: 'El movimiento no ha sido encontrado',
            icon: 'error',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000,
        });
        router.push('/movements-history');
    }
}

getMovement()

const viewMovements = async () => {
    router.push(`/movements-history`)
}

const editMovement = async () => {
    router.push(`/movement/${cryptoData.value.id}/edit`)
}

const deleteMovement = async () => {
    try {
        await Api.delete(`/Movement/${cryptoData.value.id}`);
        Alert.fire({
            title: 'Movimiento eliminado',
            text: 'El movimiento ha sido eliminado con exito',
            icon: 'success',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000
        });
        router.push('/movements-history');
    } catch (error) {
        console.log(error);
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
</script>

<template>
    <div class="text-center flex flex-col h-100 justify-center items-center">
        <h1>Detalles del movimiento</h1>
        <p>Detalles del movimiento registrado en la aplicación</p>

        <div class="flex flex-col justify-center items-center h-[720px] gap-5 w-100 mt-5 bg-neutral-100 shadow-2xl rounded-lg">
            <label for="clientID">Cliente</label>
            <select disabled id="clientID" name="clientID" v-model="cryptoData.clientID" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2">
                <option value="">Seleccione un cliente</option>
                <option v-for="client in clients" :key="client.id" :value="client.id">
                    {{ client.name }}
                </option>
            </select>

            <label for="action">Tipo de movimiento</label>
            <select disabled id="action" name="action" v-model="cryptoData.action" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2">
                <option value="">Seleccione un tipo de movimiento</option>
                <option value="purchase">Compra</option>
                <option value="sale">Venta</option>
            </select>

            <label for="cryptoCode">Tipo de criptomoneda</label>
            <select disabled id="cryptoCode" name="cryptoCode" v-model="cryptoData.cryptoCode" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2">
                <option value="">Seleccione un tipo de criptomoneda</option>
                <option value="btc">Bitcoin</option>
                <option value="usdc">USDC</option>
                <option value="eth">Ethereum</option>
                <option value="usdt">USDT</option>
                <option value="sol">SOL</option>
            </select>

            <label for="cryptoAmount">Cantidad</label>
            <input disabled type="text" id="cryptoAmount" name="cryptoAmount" v-model="cryptoData.cryptoAmount" placeholder="Cantidad" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2" />

            <label for="date">Fecha</label>
            <input disabled type="datetime-local" id="date" name="date" v-model="cryptoData.date" placeholder="Fecha" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2" />

            <button @click="viewMovements" class="bg-amber-300 px-4 py-2 rounded-lg cursor-pointer w-80 transition duration-500 hover:bg-amber-200">Volver</button>
            <button @click="editMovement" class="bg-amber-300 px-4 py-2 rounded-lg cursor-pointer w-80 transition duration-500 hover:bg-amber-200">Editar</button>
            <button  @click="deleteMovement" class="bg-red-500 px-4 py-2 rounded-lg cursor-pointer w-80 transition duration-500 hover:bg-red-600">Eliminar</button>
        </div>
    </div>
</template>