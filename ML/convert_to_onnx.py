import tf2onnx
import onnx
from  logistic_regression import model_load


tf_model = model_load()
onnx_model , _ = tf2onnx.convert.from_keras(tf_model)       # _ is throwaway variable name
onnx.save(onnx_model, "FlappyGA.onnx")



