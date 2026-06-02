const express = require('express');
const cors = require('cors');
const { v4: uuidv4 } = require('uuid');

const app = express();
const PORT = process.env.PORT || 5002;

app.use(cors());
app.use(express.json());

const bookings = {};

app.post('/bookings', (req, res) => {
    const id = uuidv4().slice(0, 8);
    bookings[id] = { id, ...req.body, status: 'PENDING' };
    res.status(201).json(bookings[id]);
});

app.get('/bookings', (req, res) => {
    res.json(Object.values(bookings));
});

app.get('/health', (req, res) => {
    res.json({ status: 'ok' });
});

app.listen(PORT, () => {
    console.log(`Booking service on port ${PORT}`);
});
