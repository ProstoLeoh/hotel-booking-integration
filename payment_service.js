const express = require('express');
const cors = require('cors');
const { v4: uuidv4 } = require('uuid');

const app = express();
app.use(cors());
app.use(express.json());

const payments = {};

app.post('/payments', (req, res) => {
    const { booking_id, amount, payment_method } = req.body;
    
    if (!booking_id || !amount || !payment_method) {
        return res.status(400).json({ error: "Missing fields" });
    }
    
    const success = Math.random() < 0.95;
    const payment_id = uuidv4().slice(0, 8);
    
    const payment = {
        payment_id,
        booking_id,
        amount,
        status: success ? "SUCCESS" : "FAILED",
        payment_method
    };
    payments[payment_id] = payment;
    
    res.status(success ? 200 : 402).json(payment);
});

app.listen(5003, () => {
    console.log('Payment service running on port 5003');
});
