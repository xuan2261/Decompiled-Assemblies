using Rust.Ai.HTN;
using Rust.Ai.HTN.Reasoning;
using Rust.Ai.HTN.Scientist;
using System;
using System.Runtime.CompilerServices;

namespace Rust.Ai.HTN.Scientist.Reasoners
{
	public class PreferredFightingRangeReasoner : INpcReasoner
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

		public PreferredFightingRangeReasoner()
		{
		}

		public static float GetPreferredRange(ScientistContext context, ref NpcPlayerInfo target, AttackEntity firearm)
		{
			if (firearm == null)
			{
				return context.Body.AiDefinition.Engagement.CenterOfMediumRangeFirearm(firearm);
			}
			switch (firearm.effectiveRangeType)
			{
				case NPCPlayerApex.WeaponTypeEnum.CloseRange:
				{
					return context.Body.AiDefinition.Engagement.CloseRangeFirearm(firearm);
				}
				case NPCPlayerApex.WeaponTypeEnum.MediumRange:
				{
					return context.Body.AiDefinition.Engagement.CenterOfMediumRangeFirearm(firearm);
				}
				case NPCPlayerApex.WeaponTypeEnum.LongRange:
				{
					return context.Body.AiDefinition.Engagement.LongRangeFirearm(firearm);
				}
			}
			return context.Body.AiDefinition.Engagement.CenterOfMediumRangeFirearm(firearm);
		}

		public static bool IsAtPreferredRange(ScientistContext context, ref NpcPlayerInfo target, AttackEntity firearm)
		{
			if (firearm == null)
			{
				return false;
			}
			switch (firearm.effectiveRangeType)
			{
				case NPCPlayerApex.WeaponTypeEnum.CloseRange:
				{
					return target.SqrDistance <= context.Body.AiDefinition.Engagement.SqrCloseRangeFirearm(firearm);
				}
				case NPCPlayerApex.WeaponTypeEnum.MediumRange:
				{
					if (target.SqrDistance > context.Body.AiDefinition.Engagement.SqrMediumRangeFirearm(firearm))
					{
						return false;
					}
					return target.SqrDistance > context.Body.AiDefinition.Engagement.SqrCloseRangeFirearm(firearm);
				}
				case NPCPlayerApex.WeaponTypeEnum.LongRange:
				{
					if (target.SqrDistance >= context.Body.AiDefinition.Engagement.SqrLongRangeFirearm(firearm))
					{
						return false;
					}
					return target.SqrDistance > context.Body.AiDefinition.Engagement.SqrMediumRangeFirearm(firearm);
				}
			}
			return false;
		}

		public void Tick(IHTNAgent npc, float deltaTime, float time)
		{
			ScientistContext npcContext = npc.AiDomain.NpcContext as ScientistContext;
			if (npcContext == null)
			{
				return;
			}
			NpcPlayerInfo primaryEnemyPlayerTarget = npcContext.GetPrimaryEnemyPlayerTarget();
			if (primaryEnemyPlayerTarget.Player != null)
			{
				if (PreferredFightingRangeReasoner.IsAtPreferredRange(npcContext, ref primaryEnemyPlayerTarget, npcContext.Domain.GetFirearm()))
				{
					npcContext.SetFact(Facts.AtLocationPreferredFightingRange, 1, true, true, true);
					return;
				}
				npcContext.SetFact(Facts.AtLocationPreferredFightingRange, 0, true, true, true);
			}
		}
	}
}