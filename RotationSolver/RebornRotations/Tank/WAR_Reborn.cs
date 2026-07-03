namespace RotationSolver.RebornRotations.Tank;

[Rotation("Reborn", CombatType.PvE, GameVersion = "7.5")]
[SourceCode(Path = "main/RebornRotations/Tank/WAR_Reborn.cs")]

public sealed class WAR_Reborn : WarriorRotation
{
	#region Config Options
	[RotationConfig(CombatType.PvE, Name = "仅在坦克姿态关闭时使用原初嗜血")]
	public bool NeverscentFlash { get; set; } = false;

	[RotationConfig(CombatType.PvE, Name = "对单体敌人使用血誓/原始直觉")]
	public bool SoloIntuition { get; set; } = false;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "血誓/原始直觉治疗阈值")]
	public float HealIntuition { get; set; } = 0.7f;

	[RotationConfig(CombatType.PvE, Name = "静止时在爆发期间使用两层猛攻")]
	public bool YEETBurst { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "静止时在猛攻即将溢出时使用一层")]
	public bool YEETCooldown { get; set; } = false;

	[RotationConfig(CombatType.PvE, Name = "移动时使用原初释放")]
	public bool InnerReleaseMoving { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "移动时使用原始撕裂（危险）")]
	public bool YEET { get; set; } = false;

	[RotationConfig(CombatType.PvE, Name = "静止时在配置的近战范围外使用原始撕裂（危险）")]
	public bool YEETStill { get; set; } = false;

	[Range(1, 20, ConfigUnitType.Yalms)]
	[RotationConfig(CombatType.PvE, Name = "原始撕裂使用时与Boss的最大距离（危险，设置过高会导致死亡）")]
	public float PrimalRendDistance2 { get; set; } = 3.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "原初嗜血治疗阈值")]
	public float FlashHeal { get; set; } = 0.6f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "战斗振奋治疗阈值")]
	public float ThrillOfBattleHeal { get; set; } = 0.6f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "平衡治疗阈值")]
	public float EquilibriumHeal { get; set; } = 0.6f;

	#endregion

	#region Countdown Logic
	protected override IAction? CountDownAction(float remainTime)
	{
		if (remainTime < 0.54f && TomahawkPvE.CanUse(out var act))
		{
			return act;
		}
		return base.CountDownAction(remainTime);
	}
	#endregion

	#region oGCD Logic
	protected override bool AttackAbility(IAction nextGCD, out IAction? act)
	{
		if (InfuriatePvE.CanUse(out act, gcdCountForAbility: 3))
		{
			return true;
		}

		if (!InnerReleasePvE.EnoughLevel && StatusHelper.PlayerHasStatus(true, StatusID.Berserk) && InfuriatePvE.CanUse(out act, usedUp: true))
		{
			return true;
		}

		if (CombatElapsedLessGCD(1))
		{
			return false;
		}

		if (!StatusHelper.PlayerWillStatusEndGCD(2, 0, true, StatusID.SurgingTempest)
			|| !StormsEyePvE.EnoughLevel)
		{
			if ((InnerReleaseMoving || !IsMoving) && InnerReleasePvE.CanUse(out act))
			{
				return true;
			}
			if ((InnerReleaseMoving || !IsMoving) && !InnerReleasePvE.Info.EnoughLevelAndQuest() && BerserkPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (IsBurstStatus && (InnerReleaseStacks == 0 || InnerReleaseStacks == 3))
		{
			if (InfuriatePvE.CanUse(out act, usedUp: true))
			{
				return true;
			}
		}

		if (CombatElapsedLessGCD(4))
		{
			return false;
		}

		if (OrogenyPvE.CanUse(out act))
		{
			return true;
		}

		if (UpheavalPvE.CanUse(out act))
		{
			return true;
		}

		if (StatusHelper.PlayerHasStatus(false, StatusID.Wrathful) && PrimalWrathPvE.CanUse(out act, skipAoeCheck: true))
		{
			return true;
		}

		if (YEETBurst && OnslaughtPvE.CanUse(out act, usedUp: IsBurstStatus) &&
		   !IsMoving &&
		   !IsLastAction(false, OnslaughtPvE) &&
		   !IsLastAction(false, UpheavalPvE) &&
			StatusHelper.PlayerHasStatus(true, StatusID.SurgingTempest))
		{
			return true;
		}

		if (YEETCooldown && OnslaughtPvE.CanUse(out act, usedUp: true) &&
		   !IsMoving &&
		   !IsLastAction(false, OnslaughtPvE) &&
		   OnslaughtPvE.Cooldown.WillHaveXChargesGCD(OnslaughtMax, 1) &&
			StatusHelper.PlayerHasStatus(true, StatusID.SurgingTempest))
		{
			return true;
		}

		if (MergedStatus.HasFlag(AutoStatus.MoveForward) && MoveForwardAbility(nextGCD, out act))
		{
			return true;
		}

		return base.AttackAbility(nextGCD, out act);
	}

	protected override bool GeneralAbility(IAction nextGCD, out IAction? act)
	{
		var _partyCount = 0;
		foreach (var _ in PartyMembers)
		{
			_partyCount++;
		}

		if ((InCombat && Player?.GetHealthRatio() < HealIntuition && NumberOfHostilesInRange > 0) || (InCombat && _partyCount == 1 && NumberOfHostilesInRange > 0))
		{
			if (BloodwhettingPvE.CanUse(out act))
			{
				return true;
			}
			if (!BloodwhettingPvE.Info.EnoughLevelAndQuest() && RawIntuitionPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (Player?.GetHealthRatio() < ThrillOfBattleHeal)
		{
			if (ThrillOfBattlePvE.CanUse(out act))
			{
				return true;
			}
		}

		if (!StatusHelper.PlayerHasStatus(true, StatusID.Holmgang_409))
		{
			if (Player?.GetHealthRatio() < EquilibriumHeal)
			{
				if (EquilibriumPvE.CanUse(out act))
				{
					return true;
				}
			}
		}

		if (StatusHelper.PlayerHasStatus(true, StatusID.PrimalRendReady) && InCombat && UseBurstMedicine(out act))
		{
			return true;
		}
		return base.GeneralAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.ShakeItOffPvE)]
	protected override bool HealSingleAbility(IAction nextGCD, out IAction? act)
	{
		if (ShakeItOffPvE.CanUse(out act, skipAoeCheck: true))
		{
			return true;
		}

		return base.HealSingleAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.RawIntuitionPvE, ActionID.VengeancePvE, ActionID.RampartPvE, ActionID.RawIntuitionPvE, ActionID.ReprisalPvE)]
	protected override bool DefenseSingleAbility(IAction nextGCD, out IAction? act)
	{
		var RawSingleTargets = SoloIntuition;
		act = null;

		if (StatusHelper.PlayerHasStatus(true, StatusID.Holmgang_409) && Player?.GetHealthRatio() < 0.3f)
		{
			return false;
		}

		if (RawIntuitionPvE.CanUse(out act) && (RawSingleTargets || NumberOfHostilesInRange > 2))
		{
			return true;
		}

		if (!StatusHelper.PlayerWillStatusEndGCD(0, 0, true, StatusID.Bloodwhetting, StatusID.RawIntuition))
		{
			return false;
		}

		if ((!RampartPvE.Cooldown.IsCoolingDown || RampartPvE.Cooldown.ElapsedAfter(60)) && DamnationPvE.CanUse(out act) && DamnationPvE.EnoughLevel)
		{
			return true;
		}

		if ((!RampartPvE.Cooldown.IsCoolingDown || RampartPvE.Cooldown.ElapsedAfter(60)) && VengeancePvE.CanUse(out act) && !DamnationPvE.EnoughLevel)
		{
			return true;
		}

		if (!VengeancePvE.EnoughLevel)
		{
			if (RampartPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (VengeancePvE.EnoughLevel && !DamnationPvE.EnoughLevel)
		{
			if (VengeancePvE.Cooldown.IsCoolingDown && VengeancePvE.Cooldown.ElapsedAfter(30) && RampartPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (DamnationPvE.EnoughLevel)
		{
			if (DamnationPvE.Cooldown.IsCoolingDown && DamnationPvE.Cooldown.ElapsedAfter(30) && RampartPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (ReprisalPvE.CanUse(out act, skipAoeCheck: true))
		{
			return true;
		}

		return base.DefenseSingleAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.ShakeItOffPvE, ActionID.ReprisalPvE)]
	protected override bool DefenseAreaAbility(IAction nextGCD, out IAction? act)
	{
		if (ShakeItOffPvE.CanUse(out act, skipAoeCheck: true))
		{
			return true;
		}

		return base.DefenseAreaAbility(nextGCD, out act);
	}
	#endregion

	#region GCD Logic
	protected override bool GeneralGCD(out IAction? act)
	{
		if (!StatusHelper.PlayerWillStatusEndGCD(3, 0, true, StatusID.SurgingTempest))
		{
			if (ChaoticCyclonePvE.CanUse(out act))
			{
				return true;
			}

			if (InnerChaosPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (!StatusHelper.PlayerWillStatusEndGCD(3, 0, true, StatusID.SurgingTempest) && !StatusHelper.PlayerHasStatus(true, StatusID.NascentChaos) && InnerReleaseStacks > 0)
		{
			if (DecimatePvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}
			if (!DecimatePvE.Info.EnoughLevelAndQuest() && SteelCyclonePvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}

			if (FellCleavePvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}
			if (!FellCleavePvE.Info.EnoughLevelAndQuest() && InnerBeastPvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}
		}

		if (!StatusHelper.PlayerWillStatusEndGCD(3, 0, true, StatusID.SurgingTempest) && InnerReleaseStacks == 0)
		{
			if (PrimalRendPvE.CanUse(out act, skipAoeCheck: true))
			{
				if (PrimalRendPvE.Target.Target != null && PrimalRendPvE.Target.Target.DistanceToPlayer() <= PrimalRendDistance2)
				{
					return true;
				}
				if (YEET || (YEETStill && !IsMoving))
				{
					return true;
				}
			}
			if (PrimalRuinationPvE.CanUse(out act))
			{
				return true;
			}
		}

		// AOE
		if (!StatusHelper.PlayerWillStatusEndGCD(3, 0, true, StatusID.SurgingTempest) || !StormsEyePvE.EnoughLevel)
		{
			if (DecimatePvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}
			if (!DecimatePvE.Info.EnoughLevelAndQuest() && SteelCyclonePvE.CanUse(out act))
			{
				return true;
			}
		}

		if (MythrilTempestPvE.CanUse(out act))
		{
			return true;
		}

		if (OverpowerPvE.CanUse(out act))
		{
			return true;
		}

		// Single Target
		if (!StatusHelper.PlayerWillStatusEndGCD(3, 0, true, StatusID.SurgingTempest) || !StormsEyePvE.EnoughLevel)
		{
			if (FellCleavePvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}
			if (!FellCleavePvE.Info.EnoughLevelAndQuest() && InnerBeastPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (StormsEyePvE.CanUse(out act))
		{
			return true;
		}

		if (StormsPathPvE.CanUse(out act))
		{
			return true;
		}

		if (MaimPvE.CanUse(out act))
		{
			return true;
		}

		if (HeavySwingPvE.CanUse(out act))
		{
			return true;
		}

		// Ranged
		if (TomahawkPvE.CanUse(out act))
		{
			return true;
		}

		return base.GeneralGCD(out act);
	}

	[RotationDesc(ActionID.NascentFlashPvE)]
	protected override bool HealSingleGCD(out IAction? act)
	{
		if (!NeverscentFlash && NascentFlashPvE.CanUse(out act)
			&& (InCombat && NascentFlashPvE.Target.Target?.GetHealthRatio() < FlashHeal))
		{
			return true;
		}

		if (NeverscentFlash && NascentFlashPvE.CanUse(out act)
			&& (InCombat && !StatusHelper.PlayerHasStatus(true, StatusID.Defiance) && NascentFlashPvE.Target.Target?.GetHealthRatio() < FlashHeal))
		{
			return true;
		}

		return base.HealSingleGCD(out act);
	}
	#endregion

	#region Extra Methods
	private static bool IsBurstStatus => !StatusHelper.PlayerWillStatusEndGCD(0, 0, false, StatusID.InnerStrength);
	#endregion
}