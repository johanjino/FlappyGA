import tensorflow as tf
from matplotlib import pyplot as plt
import numpy as np
import pickle

with open("gameData.bin", 'rb') as f:
    data = pickle.load(f)
    data = np.array([np.array(x) for x in data])
with open("gameData_ref.bin", 'rb') as f:
    data_ref = pickle.load(f)
    data_ref = np.array(data_ref)

# print(len(data))                #for debugging and verification
# print(data_ref.count(0))        #for debugging and verification
# print(data_ref.count(1))        #for debugging and verification

# Define your training data
# Binary output (0 or 1) for each input row


def create_model(data, data_ref):
    # Define the neural network model
    model = tf.keras.Sequential([
        tf.keras.layers.InputLayer(input_shape=(4,)),  # Input layer with 3 neurons
        tf.keras.layers.Dense(units=8, activation='relu'),  # Hidden layer with 8 neurons and ReLU activation
        tf.keras.layers.Dense(units=32, activation='tanh'),  # Hidden layer with 32 neurons and tanh activation
        tf.keras.layers.Dense(units=16, activation='relu'),  # Hidden layer with 16 neurons and ReLU activation
        tf.keras.layers.Dense(units=1, activation='sigmoid')  # Output layer with 1 neuron and sigmoid activation
    ])

    # Compile the model with binary crossentropy loss and Adam optimizer
    model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])

    # Train the model on some example data
    history = model.fit(data, data_ref, validation_split = 0.1, epochs=100, batch_size=4)
    print(history.history.keys())
    plt.plot(history.history['accuracy'])
    plt.plot(history.history['loss'])
    #plt.plot(history.history['val_accuracy'])
    #plt.plot(history.history['val_loss'])
    plt.title('model accuracy')
    plt.ylabel('accuracy/loss')
    plt.xlabel('datasize (%)')
    plt.legend(['accuracy', 'loss'], loc='upper left')
    plt.show()
    plt.savefig('ML_result.png')
    model.save("FlappyGA.h5")

def model_load():
    model = tf.keras.models.load_model('FlappyGA.h5')
    return model



#create_model(data, data_ref)
model = model_load()

# Make predictions on new data
for i in range(1000,1050):
    predictions = model.predict([data[i]])
    print(predictions) 
    print(data_ref[i])
    print()