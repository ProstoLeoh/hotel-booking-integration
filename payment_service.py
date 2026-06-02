from flask import Flask, request, jsonify
from flask_cors import CORS
import uuid
import random

app = Flask(__name__)
CORS(app)

payments = {}

@app.route('/payments', methods=['POST'])
def process_payment():
    data = request.json
    required = ['booking_id', 'amount', 'payment_method']
    if not all(field in data for field in required):
        return jsonify({"error": "Missing fields"}), 400
    
    success = random.random() < 0.95
    payment_id = str(uuid.uuid4())[:8]
    
    payment = {
        "payment_id": payment_id,
        "booking_id": data['booking_id'],
        "amount": data['amount'],
        "status": "SUCCESS" if success else "FAILED",
        "payment_method": data['payment_method']
    }
    payments[payment_id] = payment
    
    return jsonify(payment), 200 if success else 402

@app.route('/payments/<payment_id>', methods=['GET'])
def get_payment(payment_id):
    payment = payments.get(payment_id)
    if not payment:
        return jsonify({"error": "Payment not found"}), 404
    return jsonify(payment)

@app.route('/notify', methods=['POST'])
def send_notification():
    data = request.json
    print(f"[NOTIFICATION] To: {data.get('email')} | Subject: {data.get('subject')}")
    return jsonify({"status": "sent"}), 200

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5003, debug=True)
