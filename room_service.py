from flask import Flask, request, jsonify
from flask_cors import CORS

app = Flask(__name__)
CORS(app)

rooms = [
    {"id": "101", "type": "single", "price": 5000, "available": True},
    {"id": "102", "type": "double", "price": 8000, "available": True},
    {"id": "103", "type": "suite", "price": 15000, "available": False},
]

@app.route('/rooms', methods=['GET'])
def get_rooms():
    available = request.args.get('available')
    if available == 'true':
        result = [r for r in rooms if r['available']]
    else:
        result = rooms
    return jsonify(result)

@app.route('/rooms/<room_id>', methods=['GET'])
def get_room(room_id):
    room = next((r for r in rooms if r['id'] == room_id), None)
    if not room:
        return jsonify({"error": "Room not found"}), 404
    return jsonify(room)

@app.route('/rooms/<room_id>/tariffs', methods=['GET'])
def get_tariffs(room_id):
    tariffs = [
        {"name": "standard", "cancellation": "free_until_3days", "price_multiplier": 1.0},
        {"name": "non_refundable", "cancellation": "no_refund", "price_multiplier": 0.85},
        {"name": "flexible", "cancellation": "free_until_1day", "price_multiplier": 1.2}
    ]
    return jsonify(tariffs)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001, debug=True)
