<script setup>
import { ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { Api, CryptoApi } from '@/helpers/axios';
import Alert from 'sweetalert2';

const route = useRoute();
const router = useRouter();

const clients = ref([]);
const cryptoData = ref({
    id: route.params.id,
    clientID: '',
    action: '',
    cryptoCode: '',
    cryptoAmount: '',
    date: ''
});

async function getClients() {
    const { data } = await Api.get('/Client');
    clients.value = data;
};

getClients();

async function getMovement() {
    try {
        const { data } = await Api.get(`/Movement/${route.params.id}`);

        cryptoData.value = data
    } catch (error) {
        console.log(error)
        Alert.fire({
            title: 'Error al obtener movimiento',
            text: 'El movimiento no ha sido encontrado',
            icon: 'error',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000
        });
    }
}

getMovement();

async function editMovement() {
    if (cryptoData.value.clientID === '' || cryptoData.value.action === '' || cryptoData.value.cryptoCode === '' || cryptoData.value.cryptoAmount === '' || cryptoData.value.date === '') {
        Alert.fire({
            title: 'Error al modificar movimiento',
            text: 'Completa todos los campos obligatorios',
            icon: 'error',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000
        });
        return;
    }

    if (isNaN(cryptoData.value.cryptoAmount)) {
        Alert.fire({
            title: 'Error al modificar movimiento',
            text: 'La cantidad debe ser un número',
            icon: 'error',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000
        });
        return;
    }

    const amount = parseFloat(cryptoData.value.cryptoAmount)

    if (amount <= 0) {
        Alert.fire({
            title: 'Error al modificar movimiento',
            text: 'La cantidad debe ser mayor a 0',
            icon: 'error',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000
        });
        return;
    }

    if (!cryptoData.value.date.match(/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$/)) {
        Alert.fire({
        title: 'Error al crear movimiento',
        text: 'La fecha debe tener el formato MM-DD-YYYY HH:MM',
        icon: 'error',
        showConfirmButton: false,
        timerProgressBar: true,
        timer: 3000,
        });
        return;
    }

    try {
        const response = await CryptoApi.get(`/satoshitango/${cryptoData.value.cryptoCode}/ars/${cryptoData.value.cryptoAmount}`);

        const isPurchase = cryptoData.value.action === 'purchase' ? response.data.totalAsk : response.data.totalBid;

        const total = cryptoData.value.cryptoAmount * isPurchase;

        await Api.put(`/Movement/${route.params.id}`, {
            id: cryptoData.value.id,
            clientID: cryptoData.value.clientID,
            action: cryptoData.value.action,
            cryptoCode: cryptoData.value.cryptoCode,
            cryptoAmount: cryptoData.value.cryptoAmount,
            total: total.toFixed(2).toString(),
            date: cryptoData.value.date
        });

        Alert.fire({
            title: 'Movimiento Modificado',
            text: 'El cliente ha sido modificado con exito',
            icon: 'success',
            showConfirmButton: false,
            timerProgressBar: true,
            timer: 3000
        });

        router.push('/movements-history');
    } catch (error) {
        console.log(error)
        if (error.response.status === 400) {
            Alert.fire({
                title: 'Error al modificar movimiento',
                text: error.response.data,
                icon: 'error',
                showConfirmButton: false,
                timerProgressBar: true,
                timer: 3000,
            });
        } else {
            Alert.fire({
                title: 'Error al modificar movimiento',
                text: 'El movimiento no ha sido modificado',
                icon: 'error',
                showConfirmButton: false,
                timerProgressBar: true,
                timer: 3000
            });
        }

    }
}
</script>

<template>
    <div class="text-center flex flex-col h-100 justify-center items-center">
        <h1>Editar Compra/Venta</h1>
        <p>Edita una compra/venta de criptomonedas registrada en la aplicación</p>

        <form @submit.prevent="editMovement" class="flex flex-col justify-center items-center h-[600px] gap-5 w-100 mt-10 bg-neutral-100 shadow-2xl rounded-lg">
            <label for="clientID">Cliente</label>
            <select id="clientID" name="clientID" v-model="cryptoData.clientID" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2">
                <option value="">Seleccione un cliente</option>
                <option v-for="client in clients" :key="client.id" :value="client.id">
                    {{ client.name }}
                </option>
            </select>

            <label for="action">Tipo de movimiento</label>
            <select id="action" name="action" v-model="cryptoData.action" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2">
                <option value="">Seleccione un tipo de movimiento</option>
                <option value="purchase">Compra</option>
                <option value="sale">Venta</option>
            </select>

            <label for="cryptoCode">Tipo de criptomoneda</label>
            <select id="cryptoCode" name="cryptoCode" v-model="cryptoData.cryptoCode" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2">
                <option value="">Seleccione un tipo de criptomoneda</option>
                <option value="btc">Bitcoin</option>
                <option value="usdc">USDC</option>
                <option value="eth">Ethereum</option>
                <option value="usdt">USDT</option>
                <option value="sol">SOL</option>
            </select>

            <label for="cryptoAmount">Cantidad</label>
            <input type="text" id="cryptoAmount" name="cryptoAmount" v-model="cryptoData.cryptoAmount" placeholder="Cantidad" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2" />

            <label for="date">Fecha</label>
            <input type="datetime-local" id="date" name="date" v-model="cryptoData.date" placeholder="Fecha" class="w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 rounded-lg focus:outline-none p-2" />

            <button type="submit" class="bg-amber-300 px-4 py-2 rounded-lg cursor-pointer w-80 transition duration-500 hover:bg-amber-200">Editar</button>
        </form>
    </div>
</template>