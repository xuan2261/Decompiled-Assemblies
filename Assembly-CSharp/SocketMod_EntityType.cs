using Facepunch;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SocketMod_EntityType : SocketMod
{
	public float sphereRadius = 1f;

	public LayerMask layerMask;

	public QueryTriggerInteraction queryTriggers;

	public BaseEntity searchType;

	public bool wantsCollide;

	public SocketMod_EntityType()
	{
	}

	public override bool DoCheck(Construction.Placement place)
	{
		bool flag;
		Vector3 vector3 = place.position + (place.rotation * this.worldPosition);
		List<BaseEntity> list = Pool.GetList<BaseEntity>();
		Vis.Entities<BaseEntity>(vector3, this.sphereRadius, list, this.layerMask.@value, this.queryTriggers);
		List<BaseEntity>.Enumerator enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				bool flag1 = enumerator.Current.GetType().IsAssignableFrom(this.searchType.GetType());
				if (!flag1 || !this.wantsCollide)
				{
					if (!flag1 || this.wantsCollide)
					{
						continue;
					}
					Pool.FreeList<BaseEntity>(ref list);
					flag = false;
					return flag;
				}
				else
				{
					Pool.FreeList<BaseEntity>(ref list);
					flag = true;
					return flag;
				}
			}
			Pool.FreeList<BaseEntity>(ref list);
			return !this.wantsCollide;
		}
		finally
		{
			((IDisposable)enumerator).Dispose();
		}
		return flag;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = (this.wantsCollide ? new Color(0f, 1f, 0f, 0.7f) : new Color(1f, 0f, 0f, 0.7f));
		Gizmos.DrawSphere(Vector3.zero, this.sphereRadius);
	}
}