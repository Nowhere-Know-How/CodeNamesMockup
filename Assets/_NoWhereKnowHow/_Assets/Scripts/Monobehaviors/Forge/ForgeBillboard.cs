using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using ForgeAndUnity.Forge;
using TMPro;

public class ForgeBillboard : BillboardBehavior
{
    TextMeshPro text;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }

    public void SetText(string t)
    {
        networkObject.SendRpc(RPC_SET_TEXT, Receivers.AllBuffered, new object[] { t });
    }

    #region RPC-Callbacks
    public override void SetText(RpcArgs args)
    {
        string s = args.GetNext<string>();
        text.text = s;
    }

    #endregion
}
