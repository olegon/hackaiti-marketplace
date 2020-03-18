const express = require('express');

const app = express();

app.get('/currencies', (_, res) => {
    res.json([
        {
            "currencyCode": "USD_TO_BRL",
            "currencyValue": 430,
            "scale": 2
        },
        {
            "currencyCode": "USD_TO_EUR",
            "currencyValue": 340,
            "scale": 2
        }
    ]);
})

const serverPort = process.env.SERVER_PORT || 80;

app.listen(serverPort || 8280, '0.0.0.0', () => {
    console.log(`Zup Mock API listening at ${serverPort}`);
});
