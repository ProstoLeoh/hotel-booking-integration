from flask import Flask, jsonify
from flask_cors import CORS

app = Flask(__name__)
CORS(app)

@app.route('/rooms', methods=['GET'])
def get_rooms():
    return jsonify([
        {"id": "101", "type": "single", "price": 5000, "available": True},
        {"id": "102", "type": "double", "price": 8000, "available": True}
    ])

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001, debug=True)
