from flask import Flask, request, jsonify
from flask_cors import CORS
import uuid
from datetime import datetime

app = Flask(__name__)
CORS(app)

bookings = {}

@app.route('/bookings', methods=['POST'])
def create_booking():
    data = request.json
    required = ['room_id', 'check_in', 'check_out', 'guest_name', 'guest_email']
    if not all(field in data for field in required):
        return jsonify({"error": "Missing fields"}), 400
    
    booking_id = str(uuid.uuid4())[:8]
    booking = {
        "booking_id": booking_id,
        "room_id": data['room_id'],
        "check_in": data['check_in'],
        "check_out": data['check_out'],
        "guest_name": data['guest_name'],
        "guest_email": data['guest_email'],
        "status": "PENDING",
        "total_amount": data.get('total_amount', 0),
        "created_at": datetime.now().isoformat()
    }
    bookings[booking_id] = booking
    return jsonify(booking), 201

@app.route('/bookings/<booking_id>', methods=['GET'])
def get_booking(booking_id):
    booking = bookings.get(booking_id)
    if not booking:
        return jsonify({"error": "Booking not found"}), 404
    return jsonify(booking)

@app.route('/bookings/<booking_id>/status', methods=['PATCH'])
def update_status(booking_id):
    data = request.json
    new_status = data.get('status')
    if new_status not in ['PENDING', 'CONFIRMED', 'CANCELLED']:
        return jsonify({"error": "Invalid status"}), 400
    if booking_id in bookings:
        bookings[booking_id]['status'] = new_status
        return jsonify(bookings[booking_id])
    return jsonify({"error": "Booking not found"}), 404

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5002, debug=True)
