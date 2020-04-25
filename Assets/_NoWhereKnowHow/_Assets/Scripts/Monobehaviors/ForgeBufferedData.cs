using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForgeBufferedData : MonoBehaviour
{
    ForgePlayer forgePlayer;
    TextMeshPro textMeshPro;
    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        forgePlayer = GetComponent<ForgePlayer>();

        forgePlayer.OnPlayerNameChange += ChangeName;
    }

    void ChangeName(string newName)
    {
        textMeshPro.text = newName;
    }

    void Update()
    {
        if (!forgePlayer.networkObject.IsOwner)
        {
            return;
        }
    }
}
