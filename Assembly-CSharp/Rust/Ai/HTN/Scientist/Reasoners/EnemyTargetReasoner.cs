using Rust.Ai.HTN;
using Rust.Ai.HTN.Reasoning;
using Rust.Ai.HTN.Scientist;
using System;
using System.Runtime.CompilerServices;

namespace Rust.Ai.HTN.Scientist.Reasoners
{
	public class EnemyTargetReasoner : INpcReasoner
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

		public EnemyTargetReasoner()
		{
		}

		public void Tick(IHTNAgent npc, float deltaTime, float time)
		{
			ScientistContext npcContext = npc.AiDomain.NpcContext as ScientistContext;
			if (npcContext == null)
			{
				return;
			}
			npcContext.SetFact(Facts.HasEnemyTarget, npcContext.Memory.PrimaryKnownEnemyPlayer.PlayerInfo.Player != null, true, true, true);
		}
	}
}