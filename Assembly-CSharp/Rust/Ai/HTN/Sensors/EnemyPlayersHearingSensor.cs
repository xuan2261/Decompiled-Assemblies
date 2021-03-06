using Rust.Ai.HTN;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rust.Ai.HTN.Sensors
{
	[Serializable]
	public class EnemyPlayersHearingSensor : INpcSensor
	{
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

		public EnemyPlayersHearingSensor()
		{
		}

		public void Tick(IHTNAgent npc, float deltaTime, float time)
		{
			EnemyPlayersHearingSensor.TickStatic(npc);
		}

		public static void TickFootstepHearingTest(IHTNAgent npc, ref NpcPlayerInfo info)
		{
			if (info.SqrDistance < npc.AiDefinition.Sensory.SqrHearingRange)
			{
				float player = info.Player.estimatedSpeed2D;
				if (player <= 2f)
				{
					return;
				}
				if (player > 5f)
				{
					npc.AiDomain.NpcContext.EnemyPlayersAudible.Add(info);
				}
				else
				{
					HTNPlayer hTNPlayer = npc as HTNPlayer;
					if (hTNPlayer)
					{
						AttackEntity heldEntity = hTNPlayer.GetHeldEntity() as AttackEntity;
						if (info.SqrDistance < npc.AiDefinition.Engagement.SqrCloseRangeFirearm(heldEntity))
						{
							npc.AiDomain.NpcContext.EnemyPlayersAudible.Add(info);
							return;
						}
					}
					else if (info.SqrDistance < npc.AiDefinition.Engagement.SqrCloseRange)
					{
						npc.AiDomain.NpcContext.EnemyPlayersAudible.Add(info);
						return;
					}
				}
			}
		}

		public static void TickStatic(IHTNAgent npc)
		{
			npc.AiDomain.NpcContext.EnemyPlayersAudible.Clear();
			List<NpcPlayerInfo> enemyPlayersInRange = npc.AiDomain.NpcContext.EnemyPlayersInRange;
			for (int i = 0; i < enemyPlayersInRange.Count; i++)
			{
				NpcPlayerInfo item = enemyPlayersInRange[i];
				if (item.Player == null || item.Player.transform == null || item.Player.IsDestroyed || item.Player.IsDead() || item.Player.IsWounded())
				{
					enemyPlayersInRange.RemoveAt(i);
					i--;
				}
				else
				{
					EnemyPlayersHearingSensor.TickFootstepHearingTest(npc, ref item);
				}
			}
		}
	}
}