const express = require('express');
const cors = require('cors');
const { v4: uuidv4 } = require('uuid');

const app = express();
app.use(cors());
app.use(express.json());

const bookings = {};

app.post('/bookings', (req, res) => {
    const { room_id, check_in, check_out, guest_name, guest_email, total_amount } = req.body;
    
    if (!room_id || !check_in || !check_out || !guest_name || !guest_email) {
        return res.status(400).json({ error: "Missing fields" });
    }
    
    const booking_id = uuidv4().slice(0, 8);
    const booking = {
        booking_id,
        room_id,
        check_in,
        check_out,
        guest_name,
        guest_email,
        status: "PENDING",
        total_amount: total_amount || 0,
        created_at: new Date().toISOString()
    };
    bookings[booking_id] = booking;
    res.status(201).json(booking);
});

app.get('/bookings/:booking_id', (req, res) => {
    const booking = bookings[req.params.booking_id];
    if (!booking) return res.status(404).json({ error: "Booking not found" });
    res.json(booking);
});

app.listen(5002, () => {
    console.log('Booking service running on port 5002');
});
