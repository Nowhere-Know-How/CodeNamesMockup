using UnityEngine;
using UnityEngine.AI;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using ForgeAndUnity.Forge;

/// <summary>
/// Can be controlled by any <see cref="NetworkingPlayer"/> if he has a matching <see cref="PlayerToken"/>.
/// </summary>
//[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class ForgePlayer : ForgePlayerBehavior, INetworkScenePlayer, INetworkSceneObject, IRPCSerializable {
    //Fields
    public static string                playerTokenClient;

    string                              _playerToken;

    public string                       PlayerToken             { get { return _playerToken; } set { _playerToken = value; } }
    public NetworkSceneManager          Manager                 { get; set; }
    public NetworkingPlayer             Player                  { get; set; }

    //Events
    public delegate void CheckMultiServerPlayerIdEvent (RpcArgs pArgs);
    public event CheckMultiServerPlayerIdEvent OnCheckPlayerToken;


    //Functions
    #region Unity
    void Awake () {

    }

    protected override void NetworkStart () {
        base.NetworkStart();
        RegisterEventsServerClient();
        if (networkObject.IsServer) {
            RegisterEventsServer();
        } else {
            RegisterEventsClient();
            CheckPlayerToken(playerTokenClient);
        }

        networkObject.positionInterpolation.Enabled = false;
        networkObject.positionChanged += WarpToFirstValue;
    }

    void Update () {
        if (networkObject == null) {
            return;
        }

        if (!networkObject.IsOwner) {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
            return;
        }

        ProcessPlayerInput();
        networkObject.position = transform.position;
        networkObject.rotation = transform.rotation;
    }

    #endregion

    #region Register Events
    void RegisterEventsServer () {
        OnCheckPlayerToken += OnCheckPlayerToken_Server;
    }

    void RegisterEventsClient () {
        OnCheckPlayerToken += OnCheckPlayerToken_Client;
    }

    void RegisterEventsServerClient () {
        OnCheckPlayerToken += OnCheckPlayerToken_ServerClient;
    }

    #endregion

    #region CheckPlayerToken
    public void AssignPlayerOwnership (string pPlayerToken, NetworkingPlayer pPlayer) {
        pPlayer.disconnected += (sender) => {
            if (networkObject == null) {
                return;
            }

            networkObject.Destroy();
        };

        networkObject.AssignOwnership(pPlayer);
        networkObject.SendRpc(pPlayer, RPC_CHECK_PLAYER_TOKEN, (pPlayerToken ?? string.Empty));
    }

    void CheckPlayerToken (string pPlayerToken) {
        networkObject.SendRpc(RPC_CHECK_PLAYER_TOKEN, Receivers.Server, (pPlayerToken ?? string.Empty));
    }

    void OnCheckPlayerToken_Server (RpcArgs pArgs) {
        string playerTokenClient = pArgs.GetNext<string>();
        if (_playerToken == playerTokenClient || (string.IsNullOrEmpty(playerTokenClient) && Player == pArgs.Info.SendingPlayer)) {
            Player = pArgs.Info.SendingPlayer;
            AssignPlayerOwnership(_playerToken, pArgs.Info.SendingPlayer);
        }
    }

    void OnCheckPlayerToken_Client (RpcArgs pArgs) {
        playerTokenClient = pArgs.GetNext<string>();
    }

    void OnCheckPlayerToken_ServerClient (RpcArgs pArgs) {
        // your code here...
    }

    #endregion

    #region Helpers
    public void ProcessPlayerInput () {
        if (Input.GetKey(KeyCode.W)) {
            transform.position = new Vector3(1f, 1f, 1f);
        } else if (Input.GetKey(KeyCode.A)) {
            transform.position = new Vector3(1f, 1f, 2f);
        } else if (Input.GetKey(KeyCode.S)) {
            transform.position = new Vector3(1f, 1f, 3f);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.position = new Vector3(1f, 1f, 4f);
        }
    }

    #endregion

    #region Events
    public void WarpToFirstValue (Vector3 field, ulong timestep) {
        networkObject.positionChanged -= WarpToFirstValue;
        networkObject.positionInterpolation.Enabled = true;
        networkObject.positionInterpolation.current = networkObject.position;
        networkObject.positionInterpolation.target = networkObject.position;
    }

    #endregion

    #region RPC-Callbacks
    public override void CheckPlayerToken (RpcArgs pArgs) {
        if (OnCheckPlayerToken != null) {
            OnCheckPlayerToken(pArgs);
        }
    }

    #endregion

    #region INetworkSceneObject-Implementation
    public void SetNetworkObject (NetworkObject pNetworkObject) {
        networkObject = pNetworkObject as ForgePlayerNetworkObject;
    }

    public NetworkObject GetNetworkObject () {
        return networkObject;
    }

    public uint GetNetworkId () {
        return (networkObject != null) ? networkObject.NetworkId : 0;
    }

    #endregion

    #region IRPCSerializable-Implementation
    public byte[] ToByteArray () {
        return _playerToken.ObjectToByteArray();
    }

    public void FromByteArray (byte[] pByteArray) {
        _playerToken = pByteArray.ByteArrayToObject<string>();
    }

    #endregion
}
