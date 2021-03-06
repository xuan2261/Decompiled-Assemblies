using Rust.Ai.HTN;
using Rust.Ai.HTN.NPCTurret;
using Rust.Ai.HTN.Sensors;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rust.Ai.HTN.NPCTurret.Sensors
{
	[Serializable]
	public class AnimalsInRangeSensor : INpcSensor
	{
		public const int MaxAnimals = 128;

		public static BaseNpc[] QueryResults;

		public static int QueryResultCount;

		public float LastTickTime
		{
			get;
			set;
		}

		public float TickFrequency
		{
			get;
			set;
		}

		static AnimalsInRangeSensor()
		{
			AnimalsInRangeSensor.QueryResults = new BaseNpc[128];
			AnimalsInRangeSensor.QueryResultCount = 0;
		}

		public AnimalsInRangeSensor()
		{
		}

		public void Tick(IHTNAgent npc, float deltaTime, float time)
		{
			if (npc == null || npc.transform == null || npc.IsDestroyed || npc.AiDefinition == null || AnimalsInRangeSensor.QueryResults == null)
			{
				return;
			}
			NPCTurretDomain aiDomain = npc.AiDomain as NPCTurretDomain;
			if (aiDomain == null || aiDomain.NPCTurretContext == null)
			{
				return;
			}
			AttackEntity firearm = aiDomain.GetFirearm();
			AnimalsInRangeSensor.QueryResultCount = BaseEntity.Query.Server.GetInSphere(npc.transform.position, npc.AiDefinition.Engagement.CloseRangeFirearm(firearm) + npc.AiDefinition.Engagement.CloseRange, AnimalsInRangeSensor.QueryResults, (BaseEntity entity) => {
				BaseNpc baseNpc = entity as BaseNpc;
				if (!(baseNpc == null) && baseNpc.isServer && !baseNpc.IsDestroyed && !(baseNpc.transform == null) && !baseNpc.IsDead())
				{
					return true;
				}
				return false;
			});
			List<AnimalInfo> animalsInRange = npc.AiDomain.NpcContext.AnimalsInRange;
			if (AnimalsInRangeSensor.QueryResultCount > 0)
			{
				for (int i = 0; i < AnimalsInRangeSensor.QueryResultCount; i++)
				{
					BaseNpc queryResults = AnimalsInRangeSensor.QueryResults[i];
					Vector3 vector3 = queryResults.transform.position - npc.transform.position;
					float single = vector3.sqrMagnitude;
					if (single <= npc.AiDefinition.Engagement.SqrCloseRangeFirearm(firearm) + npc.AiDefinition.Engagement.SqrCloseRange)
					{
						bool flag = false;
						int num = 0;
						while (num < animalsInRange.Count)
						{
							AnimalInfo item = animalsInRange[num];
							if (item.Animal != queryResults)
							{
								num++;
							}
							else
							{
								item.Time = time;
								item.SqrDistance = single;
								animalsInRange[num] = item;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							AnimalInfo animalInfo = new AnimalInfo()
							{
								Animal = queryResults,
								Time = time,
								SqrDistance = single
							};
							animalsInRange.Add(animalInfo);
						}
					}
				}
			}
			for (int j = 0; j < animalsInRange.Count; j++)
			{
				if (time - animalsInRange[j].Time > npc.AiDefinition.Memory.ForgetAnimalInRangeTime)
				{
					animalsInRange.RemoveAt(j);
					j--;
				}
			}
		}
	}
}