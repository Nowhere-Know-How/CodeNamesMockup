using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0,0,0,0.15,0.15,0.15,0.15]")]
	public partial class ForgePlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 5;

		private byte[] _dirtyFields = new byte[2];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		[ForgeGeneratedField]
		private bool _IsGrounded;
		public event FieldEvent<bool> IsGroundedChanged;
		public Interpolated<bool> IsGroundedInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool IsGrounded
		{
			get { return _IsGrounded; }
			set
			{
				// Don't do anything if the value is the same
				if (_IsGrounded == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_IsGrounded = value;
				hasDirtyFields = true;
			}
		}

		public void SetIsGroundedDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_IsGrounded(ulong timestep)
		{
			if (IsGroundedChanged != null) IsGroundedChanged(_IsGrounded, timestep);
			if (fieldAltered != null) fieldAltered("IsGrounded", _IsGrounded, timestep);
		}
		[ForgeGeneratedField]
		private bool _IsStrafing;
		public event FieldEvent<bool> IsStrafingChanged;
		public Interpolated<bool> IsStrafingInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool IsStrafing
		{
			get { return _IsStrafing; }
			set
			{
				// Don't do anything if the value is the same
				if (_IsStrafing == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_IsStrafing = value;
				hasDirtyFields = true;
			}
		}

		public void SetIsStrafingDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_IsStrafing(ulong timestep)
		{
			if (IsStrafingChanged != null) IsStrafingChanged(_IsStrafing, timestep);
			if (fieldAltered != null) fieldAltered("IsStrafing", _IsStrafing, timestep);
		}
		[ForgeGeneratedField]
		private bool _IsSprinting;
		public event FieldEvent<bool> IsSprintingChanged;
		public Interpolated<bool> IsSprintingInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool IsSprinting
		{
			get { return _IsSprinting; }
			set
			{
				// Don't do anything if the value is the same
				if (_IsSprinting == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_IsSprinting = value;
				hasDirtyFields = true;
			}
		}

		public void SetIsSprintingDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_IsSprinting(ulong timestep)
		{
			if (IsSprintingChanged != null) IsSprintingChanged(_IsSprinting, timestep);
			if (fieldAltered != null) fieldAltered("IsSprinting", _IsSprinting, timestep);
		}
		[ForgeGeneratedField]
		private float _InputHorizontal;
		public event FieldEvent<float> InputHorizontalChanged;
		public InterpolateFloat InputHorizontalInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float InputHorizontal
		{
			get { return _InputHorizontal; }
			set
			{
				// Don't do anything if the value is the same
				if (_InputHorizontal == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x20;
				_InputHorizontal = value;
				hasDirtyFields = true;
			}
		}

		public void SetInputHorizontalDirty()
		{
			_dirtyFields[0] |= 0x20;
			hasDirtyFields = true;
		}

		private void RunChange_InputHorizontal(ulong timestep)
		{
			if (InputHorizontalChanged != null) InputHorizontalChanged(_InputHorizontal, timestep);
			if (fieldAltered != null) fieldAltered("InputHorizontal", _InputHorizontal, timestep);
		}
		[ForgeGeneratedField]
		private float _InputVertical;
		public event FieldEvent<float> InputVerticalChanged;
		public InterpolateFloat InputVerticalInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float InputVertical
		{
			get { return _InputVertical; }
			set
			{
				// Don't do anything if the value is the same
				if (_InputVertical == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x40;
				_InputVertical = value;
				hasDirtyFields = true;
			}
		}

		public void SetInputVerticalDirty()
		{
			_dirtyFields[0] |= 0x40;
			hasDirtyFields = true;
		}

		private void RunChange_InputVertical(ulong timestep)
		{
			if (InputVerticalChanged != null) InputVerticalChanged(_InputVertical, timestep);
			if (fieldAltered != null) fieldAltered("InputVertical", _InputVertical, timestep);
		}
		[ForgeGeneratedField]
		private float _InputMagnitude;
		public event FieldEvent<float> InputMagnitudeChanged;
		public InterpolateFloat InputMagnitudeInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float InputMagnitude
		{
			get { return _InputMagnitude; }
			set
			{
				// Don't do anything if the value is the same
				if (_InputMagnitude == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x80;
				_InputMagnitude = value;
				hasDirtyFields = true;
			}
		}

		public void SetInputMagnitudeDirty()
		{
			_dirtyFields[0] |= 0x80;
			hasDirtyFields = true;
		}

		private void RunChange_InputMagnitude(ulong timestep)
		{
			if (InputMagnitudeChanged != null) InputMagnitudeChanged(_InputMagnitude, timestep);
			if (fieldAltered != null) fieldAltered("InputMagnitude", _InputMagnitude, timestep);
		}
		[ForgeGeneratedField]
		private float _GroundDistance;
		public event FieldEvent<float> GroundDistanceChanged;
		public InterpolateFloat GroundDistanceInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float GroundDistance
		{
			get { return _GroundDistance; }
			set
			{
				// Don't do anything if the value is the same
				if (_GroundDistance == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[1] |= 0x1;
				_GroundDistance = value;
				hasDirtyFields = true;
			}
		}

		public void SetGroundDistanceDirty()
		{
			_dirtyFields[1] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_GroundDistance(ulong timestep)
		{
			if (GroundDistanceChanged != null) GroundDistanceChanged(_GroundDistance, timestep);
			if (fieldAltered != null) fieldAltered("GroundDistance", _GroundDistance, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			IsGroundedInterpolation.current = IsGroundedInterpolation.target;
			IsStrafingInterpolation.current = IsStrafingInterpolation.target;
			IsSprintingInterpolation.current = IsSprintingInterpolation.target;
			InputHorizontalInterpolation.current = InputHorizontalInterpolation.target;
			InputVerticalInterpolation.current = InputVerticalInterpolation.target;
			InputMagnitudeInterpolation.current = InputMagnitudeInterpolation.target;
			GroundDistanceInterpolation.current = GroundDistanceInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _IsGrounded);
			UnityObjectMapper.Instance.MapBytes(data, _IsStrafing);
			UnityObjectMapper.Instance.MapBytes(data, _IsSprinting);
			UnityObjectMapper.Instance.MapBytes(data, _InputHorizontal);
			UnityObjectMapper.Instance.MapBytes(data, _InputVertical);
			UnityObjectMapper.Instance.MapBytes(data, _InputMagnitude);
			UnityObjectMapper.Instance.MapBytes(data, _GroundDistance);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_IsGrounded = UnityObjectMapper.Instance.Map<bool>(payload);
			IsGroundedInterpolation.current = _IsGrounded;
			IsGroundedInterpolation.target = _IsGrounded;
			RunChange_IsGrounded(timestep);
			_IsStrafing = UnityObjectMapper.Instance.Map<bool>(payload);
			IsStrafingInterpolation.current = _IsStrafing;
			IsStrafingInterpolation.target = _IsStrafing;
			RunChange_IsStrafing(timestep);
			_IsSprinting = UnityObjectMapper.Instance.Map<bool>(payload);
			IsSprintingInterpolation.current = _IsSprinting;
			IsSprintingInterpolation.target = _IsSprinting;
			RunChange_IsSprinting(timestep);
			_InputHorizontal = UnityObjectMapper.Instance.Map<float>(payload);
			InputHorizontalInterpolation.current = _InputHorizontal;
			InputHorizontalInterpolation.target = _InputHorizontal;
			RunChange_InputHorizontal(timestep);
			_InputVertical = UnityObjectMapper.Instance.Map<float>(payload);
			InputVerticalInterpolation.current = _InputVertical;
			InputVerticalInterpolation.target = _InputVertical;
			RunChange_InputVertical(timestep);
			_InputMagnitude = UnityObjectMapper.Instance.Map<float>(payload);
			InputMagnitudeInterpolation.current = _InputMagnitude;
			InputMagnitudeInterpolation.target = _InputMagnitude;
			RunChange_InputMagnitude(timestep);
			_GroundDistance = UnityObjectMapper.Instance.Map<float>(payload);
			GroundDistanceInterpolation.current = _GroundDistance;
			GroundDistanceInterpolation.target = _GroundDistance;
			RunChange_GroundDistance(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _IsGrounded);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _IsStrafing);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _IsSprinting);
			if ((0x20 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _InputHorizontal);
			if ((0x40 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _InputVertical);
			if ((0x80 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _InputMagnitude);
			if ((0x1 & _dirtyFields[1]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _GroundDistance);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (IsGroundedInterpolation.Enabled)
				{
					IsGroundedInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					IsGroundedInterpolation.Timestep = timestep;
				}
				else
				{
					_IsGrounded = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_IsGrounded(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (IsStrafingInterpolation.Enabled)
				{
					IsStrafingInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					IsStrafingInterpolation.Timestep = timestep;
				}
				else
				{
					_IsStrafing = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_IsStrafing(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (IsSprintingInterpolation.Enabled)
				{
					IsSprintingInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					IsSprintingInterpolation.Timestep = timestep;
				}
				else
				{
					_IsSprinting = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_IsSprinting(timestep);
				}
			}
			if ((0x20 & readDirtyFlags[0]) != 0)
			{
				if (InputHorizontalInterpolation.Enabled)
				{
					InputHorizontalInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					InputHorizontalInterpolation.Timestep = timestep;
				}
				else
				{
					_InputHorizontal = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_InputHorizontal(timestep);
				}
			}
			if ((0x40 & readDirtyFlags[0]) != 0)
			{
				if (InputVerticalInterpolation.Enabled)
				{
					InputVerticalInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					InputVerticalInterpolation.Timestep = timestep;
				}
				else
				{
					_InputVertical = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_InputVertical(timestep);
				}
			}
			if ((0x80 & readDirtyFlags[0]) != 0)
			{
				if (InputMagnitudeInterpolation.Enabled)
				{
					InputMagnitudeInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					InputMagnitudeInterpolation.Timestep = timestep;
				}
				else
				{
					_InputMagnitude = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_InputMagnitude(timestep);
				}
			}
			if ((0x1 & readDirtyFlags[1]) != 0)
			{
				if (GroundDistanceInterpolation.Enabled)
				{
					GroundDistanceInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					GroundDistanceInterpolation.Timestep = timestep;
				}
				else
				{
					_GroundDistance = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_GroundDistance(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (IsGroundedInterpolation.Enabled && !IsGroundedInterpolation.current.UnityNear(IsGroundedInterpolation.target, 0.0015f))
			{
				_IsGrounded = (bool)IsGroundedInterpolation.Interpolate();
				//RunChange_IsGrounded(IsGroundedInterpolation.Timestep);
			}
			if (IsStrafingInterpolation.Enabled && !IsStrafingInterpolation.current.UnityNear(IsStrafingInterpolation.target, 0.0015f))
			{
				_IsStrafing = (bool)IsStrafingInterpolation.Interpolate();
				//RunChange_IsStrafing(IsStrafingInterpolation.Timestep);
			}
			if (IsSprintingInterpolation.Enabled && !IsSprintingInterpolation.current.UnityNear(IsSprintingInterpolation.target, 0.0015f))
			{
				_IsSprinting = (bool)IsSprintingInterpolation.Interpolate();
				//RunChange_IsSprinting(IsSprintingInterpolation.Timestep);
			}
			if (InputHorizontalInterpolation.Enabled && !InputHorizontalInterpolation.current.UnityNear(InputHorizontalInterpolation.target, 0.0015f))
			{
				_InputHorizontal = (float)InputHorizontalInterpolation.Interpolate();
				//RunChange_InputHorizontal(InputHorizontalInterpolation.Timestep);
			}
			if (InputVerticalInterpolation.Enabled && !InputVerticalInterpolation.current.UnityNear(InputVerticalInterpolation.target, 0.0015f))
			{
				_InputVertical = (float)InputVerticalInterpolation.Interpolate();
				//RunChange_InputVertical(InputVerticalInterpolation.Timestep);
			}
			if (InputMagnitudeInterpolation.Enabled && !InputMagnitudeInterpolation.current.UnityNear(InputMagnitudeInterpolation.target, 0.0015f))
			{
				_InputMagnitude = (float)InputMagnitudeInterpolation.Interpolate();
				//RunChange_InputMagnitude(InputMagnitudeInterpolation.Timestep);
			}
			if (GroundDistanceInterpolation.Enabled && !GroundDistanceInterpolation.current.UnityNear(GroundDistanceInterpolation.target, 0.0015f))
			{
				_GroundDistance = (float)GroundDistanceInterpolation.Interpolate();
				//RunChange_GroundDistance(GroundDistanceInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[2];

		}

		public ForgePlayerNetworkObject() : base() { Initialize(); }
		public ForgePlayerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public ForgePlayerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
