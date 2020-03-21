const axios = require('axios');
const availableProducts = require('./products.json');
const availablecurrencyCodes = ['USD', 'BRL', 'EUR'];

async function run() {
    while (true) {
        try {
            const createCartResponse = await createCart();
            // console.log(createCartResponse);

            await addItemsToCart(createCartResponse.id);

            const checkoutCartResponse = await checkoutCart(createCartResponse.id, choose(availablecurrencyCodes));
            console.log(checkoutCartResponse.id);
        }
        catch (error) {
            console.error(error)
        }
    }
}

async function createCart() {
    const product = choose(availableProducts);

    const payload = {
        'customerId': random(0, 1_000_000).toString(),
        'item': {
            'sku': product.sku,
            'quantity': random(0, 1_000) + 1
        }
    };

    const { data } = await axios.post('http://localhost:5050/carts', payload);

    return data;
}

async function addItemsToCart(cartId) {
    const howManyItems = random(0, 5);

    for (let i = 0; i < howManyItems; i++) {
        const addItemToCartResponse =  await addItemToCart(cartId);
    
        // console.log(addItemToCartResponse);
    }
}

async function addItemToCart(cartId) {
    const product = choose(availableProducts);

    const payload = {
        'sku': product.sku,
        'quantity': random(0, 1_000) + 1
    };

    const { data } = await axios.patch(`http://localhost:5050/carts/${cartId}/items`, payload);

    return data;
}

async function checkoutCart(cartId, currencyCode) {
    const headers = {
        'x-team-control': `${random(0, 4_000_000_000)}`
    };
    
    const payload = {
        currencyCode
    };

    const { data } = await axios.default.post(`http://localhost:5050/carts/${cartId}/checkout`, payload, { headers });
    
    return data;
}

function choose(array) {
    const index = Math.floor(Math.random() * array.length);

    return array[index];
}

function random(min = 0, max = 1_000_000) {
    return Math.floor(Math.random() * (max - min) + min)
}

run();