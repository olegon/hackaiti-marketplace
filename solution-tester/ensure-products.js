const axios = require('axios');
const products = require('./products.json');

(async () => {
    for (let product of products) {
        try {
            await axios.post('http://localhost:5000/products', product);
        } catch (error) {
            console.error(error.message);
        }
    }
})();
