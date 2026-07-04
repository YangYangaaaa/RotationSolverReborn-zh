namespace RotationSolver.RebornRotations.Ranged;

[Rotation("Reborn", CombatType.PvE, GameVersion = "7.5",
	Description = "请确保三首歌的时间总长为 120 秒，放浪神预设为第一首。")]
[SourceCode(Path = "main/RebornRotations/Ranged/BRD_Reborn.cs")]

public sealed class BRD_Reborn : BardRotation
{
	#region Config Options

	[Range(1, 5, ConfigUnitType.Seconds, 0.1f)]
	[RotationConfig(CombatType.PvE, Name = "增益对齐计时器（实验性，不理解请勿修改）")]
	public float BuffAlignment { get; set; } = 1;

	[RotationConfig(CombatType.PvE, Name = "尝试将九天连箭、战斗之声和光明神的最终乐章分配到特定oGCD槽位（实验性）")]
	public bool OGCDTimers { get; set; } = false;

	[RotationConfig(CombatType.PvE, Name = "仅在带有Boss标识的目标上使用DoT")]
	public bool DOTBoss { get; set; } = false;

	[RotationConfig(CombatType.PvE, Name = "使用弹幕时跳过DoT检查")]
	public bool DOTBarrage { get; set; } = false;

	[Range(80, 100, ConfigUnitType.None, 5)]
	[RotationConfig(CombatType.PvE, Name = "绝峰箭的灵魂之声阈值")]
	public float SoulVoiceConfig { get; set; } = 100;

	[Range(1, 45, ConfigUnitType.Seconds, 1)]
	[RotationConfig(CombatType.PvE, Name = "放浪神的小步舞曲持续时间")]
	public float WANDTime { get; set; } = 43;

	[Range(0, 45, ConfigUnitType.Seconds, 1)]
	[RotationConfig(CombatType.PvE, Name = "贤者的叙事谣持续时间")]
	public float MAGETime { get; set; } = 43;

	[Range(0, 45, ConfigUnitType.Seconds, 1)]
	[RotationConfig(CombatType.PvE, Name = "军神的赞歌持续时间")]
	public float ARMYTime { get; set; } = 34;

	[RotationConfig(CombatType.PvE, Name = "第一首诗人歌")]
	private Song FirstSong { get; set; } = Song.WanderersMinuet;

	[RotationConfig(CombatType.PvE, Name = "对其他玩家使用光阴神的礼赞凯歌")]
	public bool BRDEsuna { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "爆发期间阻止使用防御技能")]
	private bool BurstDefense { get; set; } = true;

	private float WANDRemainTime => 45 - WANDTime;
	private float MAGERemainTime => 45 - MAGETime;
	private float ARMYRemainTime => 45 - ARMYTime;

	private bool InBurstStatus => (!BattleVoicePvE.EnoughLevel && HasRagingStrikes)
		|| (BattleVoicePvE.EnoughLevel && !RadiantFinalePvE.EnoughLevel && HasRagingStrikes && HasBattleVoice)
		|| (MinstrelsCodaTrait.EnoughLevel && HasRagingStrikes && HasRadiantFinale && HasBattleVoice);

	#endregion

	#region Countdown logic
	// Defines logic for actions to take during the countdown before combat starts.
	protected override IAction? CountDownAction(float remainTime)
	{
		// tincture needs to be used on -0.7s exactly
		if (remainTime <= 0.7f && UseBurstMedicine(out var act))
		{
			return act;
		}
		return base.CountDownAction(remainTime);
	}
	#endregion

	#region oGCD Logic
	protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
	{
		if (StatusHelper.PlayerHasStatus(false, StatusID.Doom))
		{
			if (TheWardensPaeanPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (nextGCD.IsTheSameTo(true, StraightShotPvE, VenomousBitePvE, WindbitePvE, IronJawsPvE))
		{
			return base.EmergencyAbility(nextGCD, out act);
		}
		else if (!RagingStrikesPvE.EnoughLevel || HasRagingStrikes)
		{
			if (((EmpyrealArrowPvE.Cooldown.IsCoolingDown && !EmpyrealArrowPvE.Cooldown.WillHaveOneChargeGCD(1)) || !EmpyrealArrowPvE.EnoughLevel) && Repertoire != 3)
			{
				if (!StatusHelper.PlayerHasStatus(true, StatusID.HawksEye_3861) && BarragePvE.CanUse(out act))
				{
					return true;
				}
			}
		}

		return base.EmergencyAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.TheWardensPaeanPvE)]
	protected override bool DispelAbility(IAction nextGCD, out IAction? action)
	{
		if (BRDEsuna && TheWardensPaeanPvE.CanUse(out action))
		{
			return true;
		}
		return base.DispelAbility(nextGCD, out action);
	}

	[RotationDesc(ActionID.NaturesMinnePvE)]
	protected override bool HealSingleAbility(IAction nextGCD, out IAction? act)
	{
		if (NaturesMinnePvE.CanUse(out act))
		{
			return true;
		}
		return base.HealSingleAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.TroubadourPvE)]
	protected override bool DefenseAreaAbility(IAction nextGCD, out IAction? act)
	{
		if ((!BurstDefense || (BurstDefense && !InBurstStatus)) && TroubadourPvE.CanUse(out act))
		{
			return true;
		}
		return base.DefenseAreaAbility(nextGCD, out act);
	}

	protected override bool GeneralAbility(IAction nextGCD, out IAction? act)
	{
		if (TheWanderersMinuetPvE.CanUse(out act) && InCombat && !IsLastAbility(ActionID.ArmysPaeonPvE) && !IsLastAbility(ActionID.MagesBalladPvE))
		{
			if (SongEndAfter(ARMYRemainTime) && (Song != Song.None || StatusHelper.PlayerHasStatus(true, StatusID.ArmysEthos)))
			{
				return true;
			}
		}

		if (MagesBalladPvE.CanUse(out act) && InCombat && !IsLastAbility(ActionID.ArmysPaeonPvE) && !IsLastAbility(ActionID.TheWanderersMinuetPvE))
		{
			if (Song == Song.WanderersMinuet && SongEndAfter(WANDRemainTime) && (Repertoire == 0 || !HasHostilesInMaxRange))
			{
				return true;
			}

			if (Song == Song.ArmysPaeon && SongEndAfterGCD(2) && TheWanderersMinuetPvE.Cooldown.IsCoolingDown)
			{
				return true;
			}
		}

		if (ArmysPaeonPvE.CanUse(out act) && InCombat && !IsLastAbility(ActionID.MagesBalladPvE) && !IsLastAbility(ActionID.TheWanderersMinuetPvE))
		{
			if (TheWanderersMinuetPvE.EnoughLevel && SongEndAfter(MAGERemainTime) && Song == Song.MagesBallad)
			{
				return true;
			}

			if (TheWanderersMinuetPvE.EnoughLevel && SongEndAfter(2) && MagesBalladPvE.Cooldown.IsCoolingDown && Song == Song.WanderersMinuet)
			{
				return true;
			}

			if (!TheWanderersMinuetPvE.EnoughLevel && SongEndAfter(2))
			{
				return true;
			}
		}

		if (Song == Song.None && InCombat)
		{
			switch (FirstSong)
			{
				case Song.WanderersMinuet:
					if (TheWanderersMinuetPvE.CanUse(out act))
					{
						return true;
					}

					break;

				case Song.ArmysPaeon:
					if (ArmysPaeonPvE.CanUse(out act))
					{
						return true;
					}

					break;

				case Song.MagesBallad:
					if (MagesBalladPvE.CanUse(out act))
					{
						return true;
					}

					break;
			}
			if (TheWanderersMinuetPvE.CanUse(out act))
			{
				return true;
			}

			if (MagesBalladPvE.CanUse(out act))
			{
				return true;
			}

			if (ArmysPaeonPvE.CanUse(out act))
			{
				return true;
			}
		}

		return base.GeneralAbility(nextGCD, out act);
	}

	protected override bool AttackAbility(IAction nextGCD, out IAction? act)
	{
		if (IsBurst && Song != Song.None && MagesBalladPvE.EnoughLevel)
		{
			if (((!RadiantFinalePvE.EnoughLevel && !RagingStrikesPvE.Cooldown.IsCoolingDown)
					|| (RadiantFinalePvE.EnoughLevel && !RadiantFinalePvE.Cooldown.IsCoolingDown && RagingStrikesPvE.EnoughLevel && (!RagingStrikesPvE.Cooldown.IsCoolingDown || RagingStrikesPvE.Cooldown.WillHaveOneCharge(BuffAlignment))))
					&& ((CurrentTarget?.HasStatus(true, StatusID.Windbite, StatusID.Stormbite) == true && CurrentTarget?.HasStatus(true, StatusID.VenomousBite, StatusID.CausticBite) == true) || DOTBarrage) && BattleVoicePvE.CanUse(out act))
			{
				return true;
			}

			if (StatusHelper.PlayerHasStatus(true, StatusID.BattleVoice) && RadiantFinalePvE.CanUse(out act))
			{
				return true;
			}

			if (((RadiantFinalePvE.EnoughLevel && HasRadiantFinale && HasBattleVoice)
				|| (!RadiantFinalePvE.EnoughLevel && BattleVoicePvE.EnoughLevel && HasBattleVoice)
				|| (!RadiantFinalePvE.EnoughLevel && !BattleVoicePvE.EnoughLevel))
				&& RagingStrikesPvE.CanUse(out act))
			{
				return true;
			}
		}
		else if (!MagesBalladPvE.EnoughLevel)
		{
			if (!StraightShotPvE.EnoughLevel && RagingStrikesPvE.CanUse(out act))
			{
				return true;
			}

			if (nextGCD.IsTheSameTo(true, StraightShotPvE) && RagingStrikesPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (RadiantFinalePvE.EnoughLevel && RadiantFinalePvE.Cooldown.IsCoolingDown && BattleVoicePvE.EnoughLevel && !BattleVoicePvE.Cooldown.IsCoolingDown)
		{
			return base.AttackAbility(nextGCD, out act);
		}

		if ((RagingStrikesPvE.Cooldown.IsCoolingDown || !RagingStrikesPvE.Cooldown.WillHaveOneCharge(15)) && Song != Song.None && EmpyrealArrowPvE.CanUse(out act))
		{
			return true;
		}

		if (PitchPerfectPvE.CanUse(out act, skipAoeCheck: true, skipComboCheck: true))
		{
			if (SongEndAfter(3) && Repertoire > 0)
			{
				return true;
			}

			if (Repertoire == 3)
			{
				return true;
			}

			if (Repertoire == 2 && EmpyrealArrowPvE.Cooldown.WillHaveOneChargeGCD() && RadiantFinalePvE.Cooldown.IsCoolingDown)
			{
				return true;
			}
		}

		if (SidewinderPvE.EnoughLevel)
		{
			if ((BattleVoicePvE.Cooldown.IsCoolingDown && !BattleVoicePvE.Cooldown.WillHaveOneCharge(10))
			&& (!RadiantFinalePvE.EnoughLevel || (RadiantFinalePvE.EnoughLevel && RadiantFinalePvE.Cooldown.IsCoolingDown && !RadiantFinalePvE.Cooldown.WillHaveOneCharge(10)))
			&& RagingStrikesPvE.Cooldown.IsCoolingDown)
			{
				if (SidewinderPvE.CanUse(out act))
				{
					return true;
				}
			}
		}

		// Bloodletter Overcap protection
		if (BloodletterPvE.Cooldown.WillHaveXCharges(BloodletterMax, 3f))
		{
			if (RainOfDeathPvE.CanUse(out act, usedUp: true))
			{
				return true;
			}

			if (HeartbreakShotPvE.CanUse(out act, usedUp: true))
			{
				return true;
			}

			if (BloodletterPvE.CanUse(out act, usedUp: true))
			{
				return true;
			}
		}

		// Prevents Bloodletter bumpcapping when MAGE is the song due to Repetoire procs
		if (BloodletterPvE.Cooldown.WillHaveXCharges(BloodletterMax, 7.5f) && Song == Song.MagesBallad)
		{
			if (RainOfDeathPvE.CanUse(out act, usedUp: true))
			{
				return true;
			}

			if (HeartbreakShotPvE.CanUse(out act, usedUp: true))
			{
				return true;
			}

			if (BloodletterPvE.CanUse(out act, usedUp: true))
			{
				return true;
			}
		}

		if (BetterBloodletterLogic(out act))
		{
			return true;
		}

		return base.AttackAbility(nextGCD, out act);
	}
	#endregion

	#region GCD Logic
	protected override bool GeneralGCD(out IAction? act)
	{
		if (IronJawsPvE.CanUse(out act))
		{
			return true;
		}

		if (IronJawsPvE.CanUse(out act, skipStatusProvideCheck: true) && (IronJawsPvE.Target.Target?.WillStatusEnd(30, true, IronJawsPvE.Setting.TargetStatusProvide ?? []) ?? false))
		{
			if (StatusHelper.PlayerHasStatus(true, StatusID.BattleVoice, StatusID.RadiantFinale, StatusID.RagingStrikes) && StatusHelper.PlayerWillStatusEndGCD(1, 1, true, StatusID.BattleVoice, StatusID.RadiantFinale, StatusID.RagingStrikes))
			{
				return true;
			}
		}

		if (ResonantArrowPvE.CanUse(out act))
		{
			return true;
		}

		if (CanUseApexArrow(out act))
		{
			return true;
		}

		if (RadiantEncorePvE.CanUse(out act, skipComboCheck: true))
		{
			if (InBurstStatus)
			{
				return true;
			}
		}

		if (BlastArrowPvE.CanUse(out act))
		{
			if (!HasRagingStrikes)
			{
				return true;
			}

			if (HasRagingStrikes && BarragePvE.Cooldown.IsCoolingDown)
			{
				return true;
			}

			if (BlastArrowPvE.Target.Target?.WillStatusEndGCD(1, 0.5f, true, StatusID.Windbite, StatusID.Stormbite, StatusID.VenomousBite, StatusID.CausticBite) ?? false)
			{
				return false;
			}
		}

		//aoe
		if (ShadowbitePvE.CanUse(out act))
		{
			return true;
		}

		if (WideVolleyPvE.CanUse(out act))
		{
			return true;
		}

		if (LadonsbitePvE.CanUse(out act) && !HasHawksEye && !HasBarrage)
		{
			return true;
		}

		if (QuickNockPvE.CanUse(out act) && !HasHawksEye && !HasBarrage)
		{
			return true;
		}

		if (StormbitePvE.EnoughLevel)
		{
			if (StormbitePvE.CanUse(out act))
			{
				if ((DOTBoss && StormbitePvE.Target.Target.IsBossFromIcon()) || !DOTBoss)
				{
					if (StormbitePvE.Target.Target.HasStatus(true, StatusID.Stormbite) == false)
					{
						return true;
					}
				}
			}
		}
		if (CausticBitePvE.EnoughLevel)
		{
			if (CausticBitePvE.CanUse(out act))
			{
				if ((DOTBoss && CausticBitePvE.Target.Target.IsBossFromIcon()) || !DOTBoss)
				{
					if (CausticBitePvE.Target.Target.HasStatus(true, StatusID.VenomousBite) == false)
					{
						return true;
					}
				}
			}
		}

		if (!StormbitePvE.EnoughLevel)
		{
			if (WindbitePvE.CanUse(out act))
			{
				if ((DOTBoss && WindbitePvE.Target.Target.IsBossFromIcon()) || !DOTBoss)
				{
					if (IronJawsPvE.EnoughLevel && WindbitePvE.Target.Target.HasStatus(true, StatusID.Windbite) == false)
					{
						return true;
					}
					if (!IronJawsPvE.EnoughLevel)
					{
						return true;
					}
				}
			}
		}
		if (!CausticBitePvE.EnoughLevel)
		{
			if (VenomousBitePvE.CanUse(out act))
			{
				if ((DOTBoss && VenomousBitePvE.Target.Target.IsBossFromIcon()) || !DOTBoss)
				{
					if (IronJawsPvE.EnoughLevel && VenomousBitePvE.Target.Target.HasStatus(true, StatusID.CausticBite) == false)
					{
						return true;
					}
					if (!IronJawsPvE.EnoughLevel)
					{
						return true;
					}
				}
			}
		}

		if (RefulgentArrowPvE.CanUse(out act, skipComboCheck: true))
		{
			return true;
		}

		if (!RefulgentArrowPvE.Info.EnoughLevelAndQuest() && StraightShotPvE.CanUse(out act))
		{
			return true;
		}

		if (BurstShotPvE.CanUse(out act) && !HasHawksEye && !HasBarrage)
		{
			return true;
		}

		if (HeavyShotPvE.CanUse(out act) && !HasHawksEye && !HasBarrage)
		{
			return true;
		}

		return base.GeneralGCD(out act);
	}
	#endregion

	#region Extra Methods
	private bool CanUseApexArrow(out IAction act)
	{
		if (!ApexArrowPvE.CanUse(out act))
		{
			return false;
		}

		if (QuickNockPvE.CanUse(out _) && SoulVoice == SoulVoiceConfig)
		{
			return true;
		}

		if (LadonsbitePvE.CanUse(out _) && SoulVoice == SoulVoiceConfig)
		{
			return true;
		}

		if (CurrentTarget?.WillStatusEndGCD(1, 1, true, StatusID.Windbite, StatusID.Stormbite, StatusID.VenomousBite, StatusID.CausticBite) ?? false)
		{
			return false;
		}

		if (Song == Song.WanderersMinuet && SoulVoice >= 80 && !HasRagingStrikes)
		{
			return false;
		}

		if (SoulVoice == SoulVoiceConfig && BattleVoicePvE.Cooldown.WillHaveOneCharge(25))
		{
			return false;
		}

		if (SoulVoice >= 80 && HasRagingStrikes && StatusHelper.PlayerWillStatusEnd(10, false, StatusID.RagingStrikes))
		{
			return true;
		}

		if (SoulVoice == SoulVoiceConfig && HasRagingStrikes && HasBattleVoice)
		{
			return true;
		}

		if (Song == Song.MagesBallad && SoulVoice >= 80 && SongEndAfter(22) && SongEndAfter(18))
		{
			return true;
		}

		if (!HasRagingStrikes && SoulVoice == SoulVoiceConfig)
		{
			return true;
		}

		return false;
	}

	private bool BetterBloodletterLogic(out IAction? act)
	{

		if (HeartbreakShotPvE.CanUse(out act, usedUp: true))
		{
			if (InBurstStatus)
			{
				return true;
			}
		}

		if (RainOfDeathPvE.CanUse(out act, usedUp: true))
		{
			if (InBurstStatus)
			{
				return true;
			}
		}

		if (BloodletterPvE.CanUse(out act, usedUp: true))
		{
			if (InBurstStatus)
			{
				return true;
			}
		}
		return false;
	}
	#endregion
}