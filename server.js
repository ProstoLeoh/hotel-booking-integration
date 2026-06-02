const express = require('express');
const cors = require('cors');
const { v4: uuidv4 } = require('uuid');

const app = express();
app.use(cors());
app.use(express.json());

const bookings = {};

app.post('/bookings', (req, res) => {
    const id = uuidv4().slice(0, 8);
    bookings[id] = { id, ...req.body, status: 'PENDING' };
    res.status(201).json(bookings[id]);
});

app.listen(5002, () => console.log('Bookings on 5002'));
