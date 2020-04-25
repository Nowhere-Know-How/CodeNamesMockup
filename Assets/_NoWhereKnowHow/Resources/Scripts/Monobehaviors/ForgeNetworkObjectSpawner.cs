using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForgeAndUnity.Forge;

namespace CodeNames
{
    public class ForgeNetworkObjectSpawner : MonoBehaviour
    {
        NetworkSceneManager _manager;
        void Start()
        {
            if (!NodeManager.IsInitialized || !NodeManager.Instance.IsServer)
            {
                return;
            }
            _manager = NodeManager.Instance.FindNetworkSceneManager(gameObject.scene.name);

            if (_manager == null || !_manager.HasNetworker)
            {
                return;
            }

            GameControllers playerBehavior = _manager.InstantiateNetworkBehavior("GameControllers", null, transform.position, transform.rotation) as GameControllers;
        }

    }
}
