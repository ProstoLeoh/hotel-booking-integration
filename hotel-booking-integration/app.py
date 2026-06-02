from flask import Flask, jsonify
from flask_cors import CORS
import os

app = Flask(__name__)
CORS(app)

@app.route('/rooms', methods=['GET'])
def get_rooms():
    return jsonify([
        {"id": "101", "type": "single", "price": 5000, "available": True},
        {"id": "102", "type": "double", "price": 8000, "available": True}
    ])

@app.route('/health', methods=['GET'])
def health():
    return jsonify({"status": "ok"})

if __name__ == '__main__':
    port = int(os.environ.get('PORT', 5001))
    app.run(host='0.0.0.0', port=port)
