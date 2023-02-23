import tensorflow as tf
import pickle

with open("gameData.bin", 'rb') as f:
    data = pickle.load(f)
with open("gameData_ref.bin", 'rb') as f:
    data_ref = pickle.load(f)

print(len(data))
print(data_ref.count(0))
print(data_ref.count(1))

# Define your training data
# Binary output (0 or 1) for each input row


def create_model(data, data_ref):
    # Define the neural network model
    model = tf.keras.Sequential([
        tf.keras.layers.InputLayer(input_shape=(4,)),  # Input layer with 3 neurons
        tf.keras.layers.Dense(units=8, activation='relu'),  # Hidden layer with 4 neurons and ReLU activation
        tf.keras.layers.Dense(units=32, activation='tanh'),  # Hidden layer with 4 neurons and ReLU activation
        tf.keras.layers.Dense(units=16, activation='relu'),  # Hidden layer with 4 neurons and ReLU activation
        tf.keras.layers.Dense(units=1, activation='sigmoid')  # Output layer with 1 neuron and sigmoid activation
    ])

    # Compile the model with binary crossentropy loss and Adam optimizer
    model.compile(loss='binary_crossentropy', optimizer='adam')

    # Train the model on some example data

    model.fit(data, data_ref, epochs=100)
    model.save("FlappyGA.h5")

def model_load():
    model = tf.keras.models.load_model('FlappyGA.h5')
    return model



#create_model(data, data_ref)
model = model_load()
tf.saved_model.save(model, "")
# Make predictions on new data
for i in range(1000,1050):
    predictions = model.predict([data[i]])
    print(predictions) 
    print(data_ref[i])
    print()