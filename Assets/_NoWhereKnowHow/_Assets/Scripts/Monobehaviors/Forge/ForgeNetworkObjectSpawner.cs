using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForgeAndUnity.Forge;

namespace CodeNames
{
    public class ForgeNetworkObjectSpawner : MonoBehaviour
    {
        public string networkBehaviorName;
        public Transform t;

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

            if (t == null)
            {
                t = transform;
            }
            _manager.InstantiateNetworkBehavior(networkBehaviorName, null, t.position, t.rotation);
        }

    }
}
