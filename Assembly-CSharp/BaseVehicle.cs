using Facepunch;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseVehicle : BaseMountable
{
	public GameObjectRef serverGibs;

	[Tooltip("Allow players to mount other mountables/ladders from this vehicle")]
	public bool mountChaining = true;

	public bool shouldShowHudHealth;

	[Header("Mount Points")]
	public BaseVehicle.MountPointInfo[] mountPoints;

	public const BaseEntity.Flags Flag_Headlights = BaseEntity.Flags.Reserved5;

	public bool seatClipCheck;

	public BaseVehicle()
	{
	}

	public bool AnyMounted()
	{
		return this.NumMounted() > 0;
	}

	public override void AttemptMount(BasePlayer player)
	{
		if (this._mounted != null)
		{
			return;
		}
		if (!this.MountEligable())
		{
			return;
		}
		BaseMountable idealMountPoint = this.GetIdealMountPoint(player.eyes.position);
		if (idealMountPoint == null)
		{
			return;
		}
		if (idealMountPoint != this)
		{
			idealMountPoint.AttemptMount(player);
		}
		else
		{
			base.AttemptMount(player);
		}
		if (player.GetMountedVehicle() == this)
		{
			this.PlayerMounted(player, idealMountPoint);
		}
	}

	public virtual void CheckSeatsForClipping()
	{
		BaseVehicle.MountPointInfo[] mountPointInfoArray = this.mountPoints;
		for (int i = 0; i < (int)mountPointInfoArray.Length; i++)
		{
			BaseMountable baseMountable = mountPointInfoArray[i].mountable;
			if (!(baseMountable == null) && baseMountable.IsMounted() && this.IsSeatClipping(baseMountable, 1210122497))
			{
				this.SeatClippedWorld(baseMountable);
			}
		}
	}

	public override bool DirectlyMountable()
	{
		return true;
	}

	public override void DismountAllPlayers()
	{
		BaseVehicle.MountPointInfo[] mountPointInfoArray = this.mountPoints;
		for (int i = 0; i < (int)mountPointInfoArray.Length; i++)
		{
			BaseVehicle.MountPointInfo mountPointInfo = mountPointInfoArray[i];
			if (mountPointInfo.mountable != null)
			{
				mountPointInfo.mountable.DismountAllPlayers();
			}
		}
	}

	public override bool GetDismountPosition(BasePlayer player, out Vector3 res)
	{
		BaseVehicle baseVehicle = base.VehicleParent();
		if (baseVehicle != null)
		{
			return baseVehicle.GetDismountPosition(player, out res);
		}
		List<Vector3> list = Pool.GetList<Vector3>();
		Transform[] transformArrays = this.dismountPositions;
		for (int i = 0; i < (int)transformArrays.Length; i++)
		{
			Transform transforms = transformArrays[i];
			if (this.ValidDismountPosition(transforms.transform.position))
			{
				list.Add(transforms.transform.position);
			}
		}
		if (list.Count != 0)
		{
			Vector3 vector3 = player.transform.position;
			list.Sort((Vector3 a, Vector3 b) => Vector3.Distance(a, vector3).CompareTo(Vector3.Distance(b, vector3)));
			res = list[0];
			Pool.FreeList<Vector3>(ref list);
			return true;
		}
		Debug.LogWarning(string.Concat(new object[] { "Failed to find dismount position for player :", player.displayName, " / ", player.userID, " on obj : ", base.gameObject.name }));
		Pool.FreeList<Vector3>(ref list);
		res = player.transform.position;
		return false;
	}

	public BasePlayer GetDriver()
	{
		if (!this.HasMountPoints())
		{
			return this._mounted;
		}
		BaseVehicle.MountPointInfo mountPointInfo = this.mountPoints[0];
		if (mountPointInfo == null || mountPointInfo.mountable == null)
		{
			return null;
		}
		return mountPointInfo.mountable.GetMounted();
	}

	public BaseMountable GetIdealMountPoint(Vector3 pos)
	{
		if (!this.HasMountPoints())
		{
			return this;
		}
		BaseMountable baseMountable = null;
		float single = Single.PositiveInfinity;
		BaseVehicle.MountPointInfo[] mountPointInfoArray = this.mountPoints;
		for (int i = 0; i < (int)mountPointInfoArray.Length; i++)
		{
			BaseVehicle.MountPointInfo mountPointInfo = mountPointInfoArray[i];
			float single1 = Vector3.Distance(mountPointInfo.mountable.mountAnchor.position, pos);
			if (single1 < single && !mountPointInfo.mountable.IsMounted() && !this.IsSeatClipping(mountPointInfo.mountable, 1218511105) && this.IsSeatVisible(mountPointInfo.mountable, pos, 1218511105))
			{
				baseMountable = mountPointInfo.mountable;
				single = single1;
			}
		}
		return baseMountable;
	}

	public int GetIndexFromSeat(BaseMountable seat)
	{
		for (int i = 0; i < (int)this.mountPoints.Length; i++)
		{
			if (this.mountPoints[i].mountable == seat)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetPlayerSeat(BasePlayer player)
	{
		if (!this.HasMountPoints() && base.GetMounted() == player)
		{
			return 0;
		}
		for (int i = 0; i < (int)this.mountPoints.Length; i++)
		{
			BaseVehicle.MountPointInfo mountPointInfo = this.mountPoints[i];
			if (mountPointInfo.mountable != null && mountPointInfo.mountable.GetMounted() == player)
			{
				return i;
			}
		}
		return -1;
	}

	public bool HasAnyPassengers()
	{
		BaseVehicle.MountPointInfo[] mountPointInfoArray = this.mountPoints;
		for (int i = 0; i < (int)mountPointInfoArray.Length; i++)
		{
			BaseVehicle.MountPointInfo mountPointInfo = mountPointInfoArray[i];
			if (mountPointInfo.mountable != null && mountPointInfo.mountable.GetMounted())
			{
				return true;
			}
		}
		return false;
	}

	public bool HasDriver()
	{
		if (!this.HasMountPoints())
		{
			return this.IsMounted();
		}
		BaseVehicle.MountPointInfo mountPointInfo = this.mountPoints[0];
		if (mountPointInfo == null || mountPointInfo.mountable == null)
		{
			return false;
		}
		return mountPointInfo.mountable.IsMounted();
	}

	public bool HasMountPoints()
	{
		return this.mountPoints.Length != 0;
	}

	public override bool IsMounted()
	{
		return this.HasDriver();
	}

	public virtual bool IsSeatClipping(BaseMountable mountable, int mask = 1218511105)
	{
		if (mountable == null)
		{
			return false;
		}
		Vector3 vector3 = mountable.transform.position;
		Vector3 vector31 = mountable.eyeOverride.transform.position;
		Vector3 vector32 = vector3 + (base.transform.up * 0.15f);
		return GamePhysics.CheckCapsule(vector31, vector32, 0.1f, mask, QueryTriggerInteraction.UseGlobal);
	}

	public virtual bool IsSeatVisible(BaseMountable mountable, Vector3 eyePos, int mask = 1218511105)
	{
		if (mountable == null)
		{
			return false;
		}
		Vector3 vector3 = mountable.transform.position + (base.transform.up * 0.15f);
		return GamePhysics.LineOfSight(eyePos, vector3, mask, 0f);
	}

	public override void MounteeTookDamage(BasePlayer mountee, HitInfo info)
	{
	}

	public virtual bool MountEligable()
	{
		return true;
	}

	public int NumMounted()
	{
		if (!this.HasMountPoints())
		{
			if (!this.IsMounted())
			{
				return 0;
			}
			return 1;
		}
		int num = 0;
		for (int i = 0; i < (int)this.mountPoints.Length; i++)
		{
			BaseVehicle.MountPointInfo mountPointInfo = this.mountPoints[i];
			if (mountPointInfo.mountable != null && mountPointInfo.mountable.GetMounted() != null)
			{
				num++;
			}
		}
		return num;
	}

	public virtual void PlayerDismounted(BasePlayer player, BaseMountable seat)
	{
	}

	public virtual void PlayerMounted(BasePlayer player, BaseMountable seat)
	{
	}

	public virtual void SeatClippedWorld(BaseMountable mountable)
	{
		mountable.DismountPlayer(mountable.GetMounted(), false);
	}

	public override void ServerInit()
	{
		base.ServerInit();
	}

	public override void Spawn()
	{
		base.Spawn();
		this.SpawnSubEntities();
	}

	public virtual void SpawnSubEntities()
	{
		for (int i = 0; i < (int)this.mountPoints.Length; i++)
		{
			BaseVehicle.MountPointInfo component = this.mountPoints[i];
			Vector3 vector3 = Quaternion.Euler(component.rot) * Vector3.forward;
			Vector3 vector31 = component.pos;
			Vector3 vector32 = Vector3.up;
			if (component.bone != "")
			{
				vector31 = this.model.FindBone(component.bone).transform.position + base.transform.TransformDirection(component.pos);
				vector3 = base.transform.TransformDirection(vector3);
				vector32 = base.transform.up;
			}
			BaseEntity baseEntity = GameManager.server.CreateEntity(component.prefab.resourcePath, vector31, Quaternion.LookRotation(vector3, vector32), true);
			baseEntity.Spawn();
			if (component.bone == "")
			{
				baseEntity.SetParent(this, false, false);
			}
			else
			{
				baseEntity.SetParent(this, component.bone, true, true);
			}
			component.mountable = baseEntity.GetComponent<BaseMountable>();
		}
	}

	public override bool SupportsChildDeployables()
	{
		return false;
	}

	public void SwapSeats(BasePlayer player, int targetSeat = 0)
	{
		if (!this.HasMountPoints())
		{
			return;
		}
		int playerSeat = this.GetPlayerSeat(player);
		if (playerSeat == -1)
		{
			return;
		}
		BaseMountable baseMountable = this.mountPoints[playerSeat].mountable;
		int num = playerSeat;
		BaseMountable baseMountable1 = null;
		if (baseMountable1 == null)
		{
			int num1 = 0;
			while (num1 < (int)this.mountPoints.Length)
			{
				num++;
				if (num >= (int)this.mountPoints.Length)
				{
					num = 0;
				}
				BaseVehicle.MountPointInfo mountPointInfo = this.mountPoints[num];
				if (!(mountPointInfo.mountable != null) || mountPointInfo.mountable.IsMounted() || this.IsSeatClipping(mountPointInfo.mountable, 1218511105) || !this.IsSeatVisible(mountPointInfo.mountable, player.eyes.position, 1218511105))
				{
					num1++;
				}
				else
				{
					baseMountable1 = mountPointInfo.mountable;
					break;
				}
			}
		}
		if (baseMountable1 != null && baseMountable1 != baseMountable)
		{
			baseMountable.DismountPlayer(player, true);
			baseMountable1.MountPlayer(player);
			player.MarkSwapSeat();
		}
	}

	public override void VehicleFixedUpdate()
	{
		base.VehicleFixedUpdate();
		if (this.seatClipCheck && this.HasAnyPassengers() && Physics.OverlapBox(base.transform.TransformPoint(this.bounds.center), this.bounds.extents, base.transform.rotation, 1210122497).Length != 0)
		{
			this.CheckSeatsForClipping();
		}
	}

	[Serializable]
	public class MountPointInfo
	{
		public Vector3 pos;

		public Vector3 rot;

		public string bone;

		public GameObjectRef prefab;

		public BaseMountable mountable;

		public MountPointInfo()
		{
		}
	}
}