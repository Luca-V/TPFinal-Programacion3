<script setup>
import { ref } from 'vue';
import { Api } from '@/helpers/axios';
import moment from 'moment';

const clients = ref([]);
const selectedClient = ref(null);
const movementsByClient = ref([]);
const salesByClient = ref([]);
const purchasesByClient = ref([]);
const cryptoBalances = ref({});

const getClientes = async () => {
    try {
        const { data } = await Api.get('/Client');
        clients.value = data;
    } catch (error) {
        console.log(error);
        clients.value = [];
    }
};

getClientes();

const calcularCriptoPorCliente = () => {
  const balances = {};

  movementsByClient.value.forEach((movement) => {
    const code = movement.cryptoCode?.toUpperCase();
    const amount = parseFloat(movement.cryptoAmount) || 0;
    const action = movement.action;

    if (!code) return;

    if (!balances[code]) {
      balances[code] = 0;
    }

    if (action === 'purchase') {
      balances[code] += amount;
    } else if (action === 'sale') {
      balances[code] -= amount;
    }
  });

  cryptoBalances.value = balances;
};

const getMovementsByClientSelected = async () => {
    try {
        const { data } = await Api.get(`/Movement`)
        const filtered = data.filter((movement) => movement.clientID === Number(selectedClient.value));
        movementsByClient.value = filtered;
        movementsByClient.value.forEach(movement => {
            movement.date = moment(movement.date).format('YYYY/MM/DD HH:mm');
        });

        const sales = data.filter((movement) => movement.action === 'sale');
        salesByClient.value = sales;

        const purchases = data.filter((movement) => movement.action === 'purchase');
        purchasesByClient.value = purchases;

        calcularCriptoPorCliente();
    } catch (error) {
        console.log(error);
        movementsByClient.value = [];
    }
};
</script>
<template>
    <div class="text-center flex flex-col h-100 justify-center items-center bg-[url('https://images7.alphacoders.com/888/thumb-1920-888098.jpg')] bg-no-repeat bg-cover bg-center object-cover bg-blend-overlay">
        <h1>Análisis de Estado Actual</h1>
        <p>Gestiona el análisis de estado actual de los clientes registrados en la aplicación</p>

        <div class="flex flex-col items-center justify-center">
            <select id="clientID" name="clientID" v-model="selectedClient" @change="getMovementsByClientSelected" placeholder="Seleccione un cliente" class="mt-5 w-80 shadow-sm focus:shadow-lg focus:transition transition focus:duration-500 duration-300 border border-gray-300 bg-white rounded-lg focus:outline-none p-2">
                <option value="">Seleccione un cliente</option>
                <option v-for="client in clients" :key="client.id" :value="client.id">
                    {{ client.name }}
                </option>
            </select>

            <div v-if="selectedClient && movementsByClient.length === 0" class="flex flex-col justify-center items-center gap-5 min-w-7xl mt-5">
                <div class="px-4 py-2 rounded-lg flex flex-col gap-3 transition duration-500">
                    <div class="bg-red-500 text-white px-4 py-2 rounded-lg mt-5 shadow-md">
                        <h3 class="font-bold text-lg text-center">No hay movimientos registrados para el análisis de estado actual de este cliente</h3>
                        <ul class="decoration-none">
                            <li>Crea un movimiento para ver el análisis de estado actual</li>
                        </ul>
                    </div>
                </div>
            </div>

            <div v-if="selectedClient && movementsByClient.length > 0" class="flex flex-col justify-center items-center gap-5 min-w-7xl mt-5">
                <div class="bg-amber-300 px-4 py-2 rounded-lg flex flex-col gap-3 transition duration-500 hover:bg-amber-200">
                    <div v-if="Object.keys(cryptoBalances).length" class="bg-green-300 px-4 py-2 rounded-lg mt-5 shadow-md">
                        <h3 class="font-bold text-lg text-center">Saldo actual por Criptomoneda</h3>
                        <ul class="decoration-none">
                            <li v-for="(amount, code) in cryptoBalances" :key="code">
                                {{ code }}: {{ amount }}
                            </li>
                        </ul>
                    </div>

                    <div>Cantidad de Ventas: {{ salesByClient.length }}</div>
                    <div>Cantidad de Compras: {{ purchasesByClient.length }}</div>
                </div>
                <table class="w-full bg-neutral-100 shadow-2xl rounded-lg">
                    <thead class="bg-gradient-to-r from-amber-500 to-yellow-400 text-white">
                        <tr>
                            <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Tipo de Movimiento</th>
                            <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Criptomoneda</th>
                            <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Cantidad</th>
                            <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Precio</th>
                            <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Fecha</th>
                            <th class="text-center px-6 py-4 text-sm font-semibold uppercase tracking-wider">Acciones</th>
                        </tr>
                    </thead>
                    <tbody class="divide-y divide-gray-200">
                        <tr v-for="movement in movementsByClient" :key="movement.id" class="hover:bg-gray-50 transition">
                            <td class="px-6 py-4 whitespace-nowrap">{{ movement.action.toUpperCase() }}</td>
                            <td class="px-6 py-4 whitespace-nowrap">{{ movement.cryptoCode?.toUpperCase() }}</td>
                            <td class="px-6 py-4 whitespace-nowrap">{{ movement.cryptoAmount }}</td>
                            <td class="px-6 py-4 whitespace-nowrap">{{ movement.total }}</td>
                            <td class="px-6 py-4 whitespace-nowrap">{{ movement.date }}</td>
                            <td class="px-6 py-4 text-center space-x-2">
                                <button @click="viewMovement(movement.id)" class="bg-blue-500 hover:bg-blue-600 text-white text-sm px-2 py-2 rounded-lg transition">
                                    Ver Movimiento
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</template>