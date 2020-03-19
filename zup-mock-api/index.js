const express = require('express');
const bodyParser = require('body-parser');
const morgan = require('morgan');

const app = express();

app.use(morgan('combined'));

app.use(bodyParser.json());

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
});

app.post('/invoices', (req, res) => {
    console.log(req.body);
    
    const rnd = Math.random();

    if (rnd < 0.1) {
        res.status(500).send();
    }
    else {
        res.status(200).send();
    }
});

const serverPort = process.env.SERVER_PORT || 80;

app.listen(serverPort || 8280, '0.0.0.0', () => {
    console.log(`Zup Mock API listening at ${serverPort}`);
});
