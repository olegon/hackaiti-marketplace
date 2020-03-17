const express = require('express');

const app = express();

app.get('/currencies', (_, res) => {
    res.json([
        {
            "currencyCode": "USD_TO_BRL",
            "currencyValue": 400,
            "scale": 2
        },
        {
            "currencyCode": "USD_TO_EUR",
            "currencyValue": 300,
            "scale": 2
        }
    ]);
})

app.listen(80, '0.0.0.0', () => {
    console.log('Zup Mock API listening at 80');
});

