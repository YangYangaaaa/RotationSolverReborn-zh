namespace RotationSolver.RebornRotations.PVPRotations.Melee;

[Rotation("Default", CombatType.PvP, GameVersion = "7.5")]
[SourceCode(Path = "main/RebornRotations/PVPRotations/Melee/MNK_Default.PVP.cs")]

public sealed class MNK_DefaultPvP : MonkRotation
{
	#region Configurations
	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvP, Name = "使用鲜血浴所需玩家 HP 阈值")]
	public float BloodBathPvPPercent { get; set; } = 0.75f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvP, Name = "使用惩击所需敌方 HP 阈值")]
	public float SmitePvPPercent { get; set; } = 0.25f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvP, Name = "无附近敌人时使用大地回应所需自身 HP 阈值")]
	public float EarthsReplyPercent { get; set; } = 0.5f;

	[RotationConfig(CombatType.PvP, Name = "若范围内有敌人则使用大地回应")]
	public bool EarthsReplyAttack { get; set; } = true;

	[RotationConfig(CombatType.PvP, Name = "若状态将于下个 GCD 内结束则使用大地回应")]
	public bool EarthsReplyStatusEnd { get; set; } = true;
	#endregion

	#region oGCDs
	protected override bool EmergencyAbility(IAction nextGCD, out IAction? action)
	{
		if (EarthsReplyStatusEnd && StatusHelper.PlayerHasStatus(true, StatusID.EarthResonance) && StatusHelper.PlayerWillStatusEndGCD(1, 0, true, StatusID.EarthResonance))
		{
			if (EarthsReplyPvP.CanUse(out action, usedUp: true, skipAoeCheck: true, skipStatusNeed: true))
			{
				return true;
			}
		}

		if (Player?.GetHealthRatio() <= EarthsReplyPercent && StatusHelper.PlayerHasStatus(true, StatusID.EarthResonance))
		{
			if (EarthsReplyPvP.CanUse(out action, usedUp: true, skipAoeCheck: true, skipStatusNeed: true))
			{
				return true;
			}
		}

		if (RiddleOfEarthPvP.CanUse(out action) && InCombat && Player?.GetHealthRatio() < 0.8)
		{
			return true;
		}

		if (BloodbathPvP.CanUse(out action) && Player?.GetHealthRatio() < BloodBathPvPPercent)
		{
			return true;
		}

		if (SwiftPvP.CanUse(out action))
		{
			return true;
		}

		if (SmitePvP.CanUse(out action, usedUp: true) && SmitePvP.Target.Target.GetHealthRatio() <= SmitePvPPercent)
		{
			return true;
		}

		return base.EmergencyAbility(nextGCD, out action);
	}

	protected override bool AttackAbility(IAction nextGCD, out IAction? action)
	{
		if (NumberOfHostilesInRangeOf(6) > 0 && RisingPhoenixPvP.CanUse(out action, usedUp: true) && InCombat)
		{
			return true;
		}

		if (EarthsReplyAttack && EarthsReplyPvP.CanUse(out action, usedUp: true) && HasHostilesInRange)
		{
			return true;
		}

		return base.AttackAbility(nextGCD, out action);
	}
	#endregion

	#region GCDs
	protected override bool GeneralGCD(out IAction? action)
	{
		if (PhantomRushPvP.CanUse(out action))
		{
			return true;
		}

		if (FiresReplyPvP.CanUse(out action, usedUp: true, skipAoeCheck: true))
		{
			return true;
		}

		if (WindsReplyPvP.CanUse(out action))
		{
			return true;
		}

		if (PouncingCoeurlPvP.CanUse(out action))
		{
			return true;
		}

		if (RisingRaptorPvP.CanUse(out action))
		{
			return true;
		}

		if (LeapingOpoPvP.CanUse(out action))
		{
			return true;
		}

		if (DemolishPvP.CanUse(out action))
		{
			return true;
		}

		if (TwinSnakesPvP.CanUse(out action))
		{
			return true;
		}

		if (DragonKickPvP.CanUse(out action))
		{
			return true;
		}

		return base.GeneralGCD(out action);
	}
	#endregion
}