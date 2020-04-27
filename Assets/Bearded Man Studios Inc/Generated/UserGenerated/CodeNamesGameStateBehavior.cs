using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[][][\"string\"][]]")]
	[GeneratedRPCVariableNames("{\"types\":[[][][\"CardWords\"][]]")]
	public abstract partial class CodeNamesGameStateBehavior : NetworkBehavior
	{
		public const byte RPC_START_CODE_NAMES_ON_SERVER = 0 + 5;
		public const byte RPC_END_CODE_NAMES_ON_SERVER = 1 + 5;
		public const byte RPC_SEND_CARD_WORDS_TO_CLIENT = 2 + 5;
		public const byte RPC_DEACTIVATE_GAME_OBJECTS_ON_CLIENT = 3 + 5;
		
		public CodeNamesGameStateNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (CodeNamesGameStateNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("StartCodeNamesOnServer", StartCodeNamesOnServer);
			networkObject.RegisterRpc("EndCodeNamesOnServer", EndCodeNamesOnServer);
			networkObject.RegisterRpc("SendCardWordsToClient", SendCardWordsToClient, typeof(string));
			networkObject.RegisterRpc("DeactivateGameObjectsOnClient", DeactivateGameObjectsOnClient);

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new CodeNamesGameStateNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new CodeNamesGameStateNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void StartCodeNamesOnServer(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void EndCodeNamesOnServer(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void SendCardWordsToClient(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void DeactivateGameObjectsOnClient(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}