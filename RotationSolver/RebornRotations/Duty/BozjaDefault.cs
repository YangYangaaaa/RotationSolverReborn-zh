using RotationSolver.Basic.Rotations.Duties;

namespace RotationSolver.RebornRotations.Duty;

[Rotation("Bozja Reborn",  CombatType.PvE)]
internal class BozjaReborn : BozjaRotation
{
	#region Configs
	[RotationConfig(CombatType.PvE, Name = "跳过失力爆发的魔法回避检测，将其作为 AoE 连发使用")]
	public bool BurstAversion { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "跳过失力狂暴的物理回避检测，将其作为 AoE 连发使用")]
	public bool RampageAversion { get; set; } = true;
	#endregion

	public override bool GeneralAbility(IAction nextGCD, out IAction? act)
	{
		if (InCombat)
		{
			if (BannerOfHonedAcuityPvE.CanUse(out act))
			{
				return true;
			}

			if (BannerOfHonoredSacrificePvE.CanUse(out act))
			{
				return true;
			}

			if (LostFontOfPowerPvE.CanUse(out act))
			{
				return true;
			}

			if (LostFontOfMagicPvE.CanUse(out act))
			{
				return true;
			}

			if (LostFocusPvE.CanUse(out act))
			{
				return true;
			}

			if (LostChainspellPvE.CanUse(out act))
			{
				return true;
			}
		}

		return base.GeneralAbility(nextGCD, out act);
	}

	public override bool HealSingleAbility(IAction nextGCD, out IAction? act)
	{
		if (LostCureIiPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealSingleAbility(nextGCD, out act);
	}

	public override bool HealAreaAbility(IAction nextGCD, out IAction? act)
	{
		if (LostFullCurePvE.CanUse(out act))
		{
			return true;
		}

		return base.HealAreaAbility(nextGCD, out act);
	}

	public override bool DefenseSingleGCD(out IAction? act)
    {
		if (LostStoneskinPvE.CanUse(out act))
		{
			return true;
		}

		return base.DefenseSingleGCD(out act);
    }

    public override bool DefenseAreaGCD(out IAction? act)
    {
		if (LostStoneskinIiPvE.CanUse(out act))
		{
			return true;
		}

		return base.DefenseAreaGCD(out act);
    }

	public override bool HealSingleGCD(out IAction? act)
	{
		if (LostCurePvE.CanUse(out act))
		{
			return true;
		}

		return base.HealSingleGCD(out act);
	}

	public override bool HealAreaGCD(out IAction? act)
	{
		if (LostCureIiiPvE.CanUse(out act))
		{
			return true;
		}

		if (LostCureIvPvE.CanUse(out act))
		{
			return true;
		}

		if (LostFullCurePvE.CanUse(out act))
		{
			return true;
		}

		return base.HealAreaGCD(out act);
	}

	public override bool RaiseGCD(out IAction? act)
	{
		if (LostArisePvE.CanUse(out act))
		{
			return true;
		}

		if (LostSacrificePvE.CanUse(out act))
		{
			return true;
		}

		return base.RaiseGCD(out act);
	}

	public override bool GeneralGCD(out IAction? act)
    {
		if (InCombat)
		{
			if (LostSeraphStrikePvE.CanUse(out act))
			{
				return true;
			}

			if (LostSlashPvE.CanUse(out act))
			{
				return true;
			}

			if (LostSpellforgePvE.CanUse(out act))
			{
				return true;
			}

			if (LostSteelstingPvE.CanUse(out act))
			{
				return true;
			}

			if (LostBurstPvE.CanUse(out act, skipTargetStatusNeedCheck: BurstAversion, skipStatusProvideCheck: BurstAversion, skipAoeCheck: !BurstAversion))
			{
				return true;
			}

			if (LostRampagePvE.CanUse(out act, skipTargetStatusNeedCheck: RampageAversion, skipStatusProvideCheck: RampageAversion, skipAoeCheck: !RampageAversion))
			{
				return true;
			}

			if (LostBraveryPvE.CanUse(out act))
			{
				return true;
			}

			if (LostBubblePvE.CanUse(out act))
			{
				return true;
			}

			if (LostShellIiPvE.CanUse(out act))
			{
				return true;
			}

			if (LostShellPvE.CanUse(out act))
			{
				return true;
			}

			if (LostProtectIiPvE.CanUse(out act))
			{
				return true;
			}

			if (LostProtectPvE.CanUse(out act))
			{
				return true;
			}

			if (LostFlareStarPvE.CanUse(out act))
			{
				return true;
			}

			if (BannerOfSolemnClarityPvE.CanUse(out act))
			{
				return true;
			}
		}

		return base.GeneralGCD(out act);
    }
}
