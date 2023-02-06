from ray.rllib.policy.policy import Policy

# ckpt =r"C:\Users\MJ\ray_results\A3C\A3C_unity3d_576c6_00000_0_2023-01-11_22-50-51\checkpoint_000830\policies\game"
my_new_a3c = Policy.from_state(state=r"C:\Users\MJ\ray_results\A3C\A3C_unity3d_576c6_00000_0_2023-01-11_22-50-51\checkpoint_000835\algorithm_state.pkl")
my_new_a3c.export_model("C:/Users/MJ/ray_results/Model", onnx=True)