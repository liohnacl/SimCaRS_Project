"""
Example of running an RLlib Trainer against a locally running Unity3D editor
instance (available as Unity3DEnv inside RLlib).
For a distributed cloud setup example with Unity,
see `examples/serving/unity3d_[server|client].py`

To run this script against a local Unity3D engine:
1) Install Unity3D and `pip install mlagents`.

2) Open the Unity3D Editor and load an example scene from the following
   ml-agents pip package location:
   `.../ml-agents/Project/Assets/ML-Agents/Examples/`
   This script supports the `3DBall`, `3DBallHard`, `SoccerStrikersVsGoalie`,
    `Tennis`, and `Walker` examples.
   Specify the game you chose on your command line via e.g. `--env 3DBall`.
   Feel free to add more supported examples here.

3) Then run this script (you will have to press Play in your Unity editor
   at some point to start the game and the learning process):
$ python unity3d_env_local.py --env 3DBall --stop-reward [..]
  [--framework=torch]?
"""

import argparse
import os

import ray
from ray import air, tune
from ray.rllib.algorithms.a3c import A3CConfig, A3C
from ray.rllib.env.wrappers.unity3d_env import Unity3DEnv
from ray.rllib.utils.test_utils import check_learning_achieved


parser = argparse.ArgumentParser()
parser.add_argument(
    "--env",
    type=str,
    default="game",
    choices=[
        "game"
    ],
    help="The name of the Env to run in the Unity3D editor: `3DBall(Hard)?|"
    "Pyramids|GridFoodCollector|SoccerStrikersVsGoalie|Sorter|Tennis|"
    "VisualHallway|Walker` (feel free to add more and PR!)",
)
parser.add_argument(
    "--file-name",
    type=str,
    default="D:/Documents/Projects/SimCaRS_Project/build/game.exe",
    # default=None,
    help="The Unity3d binary (compiled) game, e.g. "
    "'/home/ubuntu/soccer_strikers_vs_goalie_linux.x86_64'. Use `None` for "
    "a currently running Unity3D editor.",
)
parser.add_argument(
    "--from-checkpoint",
    type=str,
    default=None,
    help="Full path to a checkpoint file for restoring a previously saved "
    "Trainer state.",
)
parser.add_argument("--num-workers", type=int, default=3)
parser.add_argument(
    "--as-test",
    action="store_true",
    help="Whether this script should be run as a test: --stop-reward must "
    "be achieved within --stop-timesteps AND --stop-iters.",
)
parser.add_argument(
    "--stop-iters", type=int, default=9999999, help="Number of iterations to train."
)
parser.add_argument(
    "--stop-timesteps", type=int, default=999999999, help="Number of timesteps to train."
)
parser.add_argument(
    "--stop-reward",
    type=float,
    default=9999.0,
    help="Reward at which we stop training.",
)
parser.add_argument(
    "--horizon",
    type=int,
    default=5000,
    help="The max. number of `step()`s for any episode (per agent) before "
    "it'll be reset again automatically.",
)
parser.add_argument(
    "--framework",
    choices=["tf", "tf2", "torch"],
    default="tf",
    help="The DL framework specifier.",
)

if __name__ == "__main__":
    ray.init()

    args = parser.parse_args()

    tune.register_env(
        "unity3d",
        lambda c: Unity3DEnv(
            file_name=c["file_name"],
            no_graphics=False,
            episode_horizon=c["episode_horizon"],
        ),
    )

    # Get policies (different agent types; "behaviors" in MLAgents) and
    # the mappings from individual agents to Policies.
    policies, policy_mapping_fn = Unity3DEnv.get_policy_configs_for_game(args.env)

    config = (
        A3CConfig()
        .environment(
            "unity3d",
            env_config={
                "file_name": args.file_name,
                "episode_horizon": args.horizon,
                
            },
            disable_env_checking=True,
            render_env=True
        )
        .framework(framework="tf", eager_tracing=True)
        # For running in editor, force to use just one Worker (we only have
        # one Unity running)!
        .rollouts(
            num_rollout_workers=args.num_workers if args.file_name else 0,
            no_done_at_end=True,
            rollout_fragment_length="auto",
            enable_tf1_exec_eagerly=True
        )
        .training(
            lr=0.0001,
            lambda_=0.99,
            gamma=0.95,
            model={
                "fcnet_hiddens": [256, 256],
                
            },
            train_batch_size=2000,
        )
        .multi_agent(policies=policies, policy_mapping_fn=policy_mapping_fn)
        # Use GPUs iff `RLLIB_NUM_GPUS` env var set to > 0.
        .resources(num_gpus=int(os.environ.get("RLLIB_NUM_GPUS", "1")))
        
    )

    # Switch on Curiosity based exploration for Pyramids env
    # (not solvable otherwise).

    stop = {
        "training_iteration": args.stop_iters,
        "timesteps_total": args.stop_timesteps,
        "episode_reward_mean": args.stop_reward,
    }

    

    # Run the experiment.
    # results = tune.Tuner(
    #     "A3C",
    #     param_space=config.to_dict(),
    #     run_config=air.RunConfig(
    #         stop=stop,
    #         verbose=2,
    #         checkpoint_config=air.CheckpointConfig(
    #             checkpoint_frequency=5,
    #             checkpoint_at_end=True,
    #         ),
    #     ),
    # ).fit()

    # results = tune.Tuner.restore(path=r"~\ray_results\A3C").fit()
    config.build()
    my_new_a3c = A3C.from_checkpoint(r"C:\Users\MJ\ray_results\A3C\A3C_unity3d_576c6_00000_0_2023-01-11_22-50-51\checkpoint_000835")
    my_new_a3c.train()
    a3c_policy = my_new_a3c.get_policy()
    a3c_policy.export_model(r"C:\Users\MJ\ray_results\Model", onnx=True)

    
    # # And check the results.
    # if args.as_test:
    #     check_learning_achieved(results, args.stop_reward)

    ray.shutdown()
