using System.ComponentModel;

namespace RotationSolver.ExtraRotations.Healer;

[Rotation("BeirutaWHM", CombatType.PvE, GameVersion = "7.45", Description = "半自动零式/绝境战循环，需配合 CD 计划器或手动输入使用")]
[SourceCode(Path = "main/ExtraRotations/Healer/BeirutaWHM.cs")]
[ExtraRotation]

public sealed class WHM_Reborn : WhiteMageRotation
{
	#region Config Options

	[RotationConfig(CombatType.PvE, Name =
		"请注意，本循环针对高端战斗进行优化。\n" +
		"• 只有描述中列出的技能会被自动使用，其他所有技能都应手动使用或通过 CD 计划器使用\n" +
		"• 请将拦截设置为仅 GCD 使用\n" +
		"• 如果需要延迟本循环的爆发时机，禁用 AutoBurst 即可\n" +
		"• 在爆发阶段、移动期间以及神速咏唱后的 20 秒内，灼烧会稍微提前刷新\n" +
		"• 苦百合仅在爆发阶段使用，蓝百合溢出不会损失伤害\n" +
		"• 进入战斗 6 秒后，本循环会冷却即用法令，如果想用 CD 计划器管理请在技能设置中禁用\n" +
		"• 如果在爆发前 15 秒没有 3 层苦难之心，会开始倾泻蓝百合\n" +
		"• 本循环中单体治疗的使用故意更保守\n" +
		"• 如果关闭 AutoBurst，法令将不会被自动使用\n" +
		"• 使用神速咏唱后的 20 秒内，愈疗、治疗二型、愈疗、安慰之心、狂喜之心和再生会被锁定\n" +
		"• 你可以使用 <tt> 或 <me> 宏来使用庇护所，并在技能设置中禁用它\n")]
	public bool RotationNotes { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "启用即刻咏唱限制逻辑：拥有即刻咏唱时尝试阻止除复活外的其他行为")]
	public bool SwiftLogic { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "移动时使用即刻咏唱")]
	public bool UseSwiftcastForMovement { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "神圣抚摸可用时立即使用")]
	public bool UseDivine { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "仅对坦克使用天赐祝福")]
	public bool BenedictionTankOnly { get; set; } = true;

	[Range(0, 20, ConfigUnitType.Seconds, 0.5f)]
	[RotationConfig(CombatType.PvE, Name = "神速咏唱开始后这么多秒内，仅在实际移动时使用苦百合和耀眼四型")]
	public float PoMMovementOnlyLockSeconds { get; set; } = 8f;

	[Range(0, 5, ConfigUnitType.Seconds, 0.1f)]
	[RotationConfig(CombatType.PvE, Name = "允许移动类操作前的最小移动时间")]
	public float MovementTimeThreshold { get; set; } = 0.8f;

	[Range(0, 10000, ConfigUnitType.None, 100)]
	[RotationConfig(CombatType.PvE, Name = "使用无中生有所需的施法消耗阈值")]
	public float ThinAirNeed { get; set; } = 1000;

	[RotationConfig(CombatType.PvE, Name = "如何管理最后一个无中生有充能")]
	public ThinAirUsageStrategy ThinAirLastChargeUsage { get; set; } = ThinAirUsageStrategy.ReserveLastChargeForRaise;

	public enum ThinAirUsageStrategy : byte
	{
		[Description("在昂贵的法术上使用所有无中生有充能")]
		UseAllCharges,

		[Description("保留最后一层充能用于复活")]
		ReserveLastChargeForRaise,

		[Description("保留最后一层充能用于手动使用")]
		ReserveLastCharge,
	}

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用天赐祝福所需的队友最低 HP 阈值")]
	public float BenedictionHeal { get; set; } = 0.1f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用神名第 2 层充能所需的队友最低 HP 阈值")]
	public float TetragrammatonSecond { get; set; } = 0.7f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用神名最后一层充能所需的队友最低 HP 阈值")]
	public float TetragrammatonLast { get; set; } = 0.6f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "0 层苦难之心时使用安慰之心所需的队友最低 HP 阈值")]
	public float SolaceHeal0 { get; set; } = 0.7f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "1 层苦难之心时使用安慰之心所需的队友最低 HP 阈值")]
	public float SolaceHeal1 { get; set; } = 0.6f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "2 层苦难之心时使用安慰之心所需的队友最低 HP 阈值")]
	public float SolaceHeal2 { get; set; } = 0.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "0 层苦难之心时使用狂喜之心所需的队友平均最低 HP 阈值")]
	public float RaptureHeal0 { get; set; } = 0.8f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "1 层苦难之心时使用狂喜之心所需的队友平均最低 HP 阈值")]
	public float RaptureHeal1 { get; set; } = 0.7f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "2 层苦难之心时使用狂喜之心所需的队友平均最低 HP 阈值")]
	public float RaptureHeal2 { get; set; } = 0.6f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用愈疗/治疗二型所需的队友平均最低 HP 阈值")]
	public float MedicaHeal { get; set; } = 0.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用愈疗所需的队友平均最低 HP 阈值")]
	public float CureIIIHeal { get; set; } = 0.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用庇护所所需的队友平均最低 HP 阈值")]
	public float AsylumHeal { get; set; } = 0.6f;

	#endregion

	#region Helpers

	private const float PresenceOfMindDiaRefreshSeconds = 11f;
	private const float MovingDiaRefreshSeconds = 11f;
	private const float PresenceOfMindLockWindowSeconds = 20f;
	private const float SwiftcastPostActionLockSeconds = 2f;

	private float _lastPresenceOfMindUseCombatTime = float.MinValue;
	private float _lastSwiftcastLockingActionCombatTime = float.MinValue;


	private float PresenceOfMindRemainingTime =>
		HasPresenceOfMind
			? StatusHelper.PlayerStatusTime(true, StatusID.PresenceOfMind)
			: 0f;

	private bool InLast5sOfPresenceOfMind =>
		HasPresenceOfMind &&
		PresenceOfMindRemainingTime <= 5f;

	private bool InConfiguredMovementOnlyWindowOfPresenceOfMind =>
		HasPresenceOfMind &&
		PresenceOfMindRemainingTime > (15f - PoMMovementOnlyLockSeconds);

	private bool HasSufficientMovement =>
		IsMoving &&
		MovingTime > MovementTimeThreshold;

	private bool AllowPoMRestrictedMovementGCDs =>
		PoMMovementOnlyLockSeconds <= 0f ||
		!InConfiguredMovementOnlyWindowOfPresenceOfMind ||
		HasSufficientMovement;

	private bool HasAsylum => StatusHelper.PlayerHasStatus(true, StatusID.Asylum);

	private bool HasLiturgyOfTheBell => StatusHelper.PlayerHasStatus(true, StatusID.LiturgyOfTheBell);

	private bool HasMedicaIii => StatusHelper.PlayerHasStatus(true, StatusID.MedicaIii);

	private bool HasSacredSight => StatusHelper.PlayerHasStatus(true, StatusID.SacredSight);

	private bool HasHealingLockout => HasAsylum || HasLiturgyOfTheBell;

	private bool ShouldHoldRaiseSwift =>
		(HasSwift || IsLastAction(ActionID.SwiftcastPvE)) &&
		SwiftLogic &&
		MergedStatus.HasFlag(AutoStatus.Raise);

	private bool InFirst20sAfterPresenceOfMind =>
		InCombat &&
		_lastPresenceOfMindUseCombatTime > float.MinValue / 2 &&
		CombatTime - _lastPresenceOfMindUseCombatTime <= PresenceOfMindLockWindowSeconds;

	private bool IsPresenceOfMindHealLockActive => InFirst20sAfterPresenceOfMind;

	private bool IsSwiftcastPostActionLockActive =>
		InCombat &&
		_lastSwiftcastLockingActionCombatTime > float.MinValue / 2 &&
		CombatTime - _lastSwiftcastLockingActionCombatTime <= SwiftcastPostActionLockSeconds;

	private void UpdateActionTracking()
	{
		if (!InCombat)
		{
			_lastPresenceOfMindUseCombatTime = float.MinValue;
			_lastSwiftcastLockingActionCombatTime = float.MinValue;
			return;
		}

		if (IsLastAction(ActionID.PresenceOfMindPvE))
		{
			_lastPresenceOfMindUseCombatTime = CombatTime;
		}

		if (IsLastAction(ActionID.DiaPvE) ||
			IsLastAction(ActionID.AeroIiPvE) ||
			IsLastAction(ActionID.AeroPvE) ||
			IsLastAction(ActionID.AfflatusMiseryPvE) ||
			IsLastAction(ActionID.GlareIvPvE) ||
			IsLastAction(ActionID.EsunaPvE))
		{
			_lastSwiftcastLockingActionCombatTime = CombatTime;
		}
	}

	private bool IsTank(IBattleChara? target)
	{
		if (target == null)
			return false;

		IEnumerable<IBattleChara> tanks = PartyMembers.GetJobCategory(JobRole.Tank);
		foreach (IBattleChara tank in tanks)
		{
			if (tank == target)
				return true;
		}

		return false;
	}

	private bool BenedictionTargetAllowed(IBattleChara? target)
	{
		if (target == null)
			return false;

		if (!BenedictionTankOnly)
			return true;

		return IsTank(target);
	}

	private static bool HasSingleHealLockoutStatus(IBattleChara? target)
	{
		if (target == null)
			return true;

		try
		{
			return target.HasStatus(false, StatusID.LivingDead) ||
				   target.HasStatus(false, StatusID.Holmgang) ||
				   target.HasStatus(false, StatusID.WalkingDead);
		}
		catch
		{
			return true;
		}
	}

	private bool CanUseRaptureByBloodLily(out float threshold)
	{
		threshold = 0f;

		switch (BloodLily)
		{
			case 0:
				threshold = RaptureHeal0;
				return true;
			case 1:
				threshold = RaptureHeal1;
				return true;
			case 2:
				threshold = RaptureHeal2;
				return true;
			default:
				return false;
		}
	}

	private bool CanUseSolaceByBloodLily(out float threshold)
	{
		threshold = 0f;

		switch (BloodLily)
		{
			case 0:
				threshold = SolaceHeal0;
				return true;
			case 1:
				threshold = SolaceHeal1;
				return true;
			case 2:
				threshold = SolaceHeal2;
				return true;
			default:
				return false;
		}
	}

	private float CurrentTetragrammatonThreshold => TetragrammatonPvE.Cooldown.CurrentCharges switch
	{
		2 => TetragrammatonSecond,
		1 => TetragrammatonLast,
		_ => TetragrammatonLast,
	};

	private bool CurrentTargetDiaMissingOrEnding(float remainingSeconds)
	{
		if (CurrentTarget == null)
			return false;

		return
			(DiaPvE.EnoughLevel &&
			 (!CurrentTarget.HasStatus(true, StatusID.Dia) ||
			  CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.Dia))) ||
			(!DiaPvE.EnoughLevel && AeroIiPvE.EnoughLevel &&
			 (!CurrentTarget.HasStatus(true, StatusID.AeroIi) ||
			  CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.AeroIi))) ||
			(!DiaPvE.EnoughLevel && !AeroIiPvE.EnoughLevel && AeroPvE.EnoughLevel &&
			 (!CurrentTarget.HasStatus(true, StatusID.Aero) ||
			  CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.Aero)));
	}

	private bool CanUseCurrentDia(out IAction? act, bool skipStatusProvideCheck = false)
	{
		if (DiaPvE.EnoughLevel &&
			DiaPvE.CanUse(out act, skipStatusProvideCheck: skipStatusProvideCheck))
		{
			return true;
		}

		if (!DiaPvE.EnoughLevel && AeroIiPvE.EnoughLevel &&
			AeroIiPvE.CanUse(out act, skipStatusProvideCheck: skipStatusProvideCheck))
		{
			return true;
		}

		if (!DiaPvE.EnoughLevel && !AeroIiPvE.EnoughLevel && AeroPvE.EnoughLevel &&
			AeroPvE.CanUse(out act, skipStatusProvideCheck: skipStatusProvideCheck))
		{
			return true;
		}

		act = null;
		return false;
	}

	private bool IsMovementPreferredNextGCD(IAction nextGCD)
	{
		if (nextGCD == AfflatusMiseryPvE ||
			nextGCD == GlareIvPvE ||
			nextGCD == AfflatusRapturePvE ||
			nextGCD == AfflatusSolacePvE)
		{
			return true;
		}

		if (CanUseCurrentDia(out IAction? diaAct) &&
			nextGCD == diaAct)
		{
			return true;
		}

		return false;
	}

	private bool ShouldSwiftcastForMovement(IAction nextGCD)
	{
		if (!UseSwiftcastForMovement ||
			!InCombat ||
			!HasSufficientMovement ||
			HasSwift ||
			HasSacredSight ||
			IsLastAction(ActionID.SwiftcastPvE) ||
			ShouldHoldRaiseSwift ||
			IsSwiftcastPostActionLockActive)
		{
			return false;
		}

		return !IsMovementPreferredNextGCD(nextGCD);
	}

	#endregion

	#region Countdown Logic

	protected override IAction? CountDownAction(float remainTime)
	{
		IAction? act;

		if (remainTime < 3 && UseBurstMedicine(out act))
			return act;

		if (remainTime < StonePvE.Info.CastTime + CountDownAhead &&
			StonePvE.CanUse(out act))
		{
			return act;
		}

		return base.CountDownAction(remainTime);
	}

	#endregion

	#region oGCD Logic

	protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		bool useLastThinAirCharge =
			ThinAirLastChargeUsage == ThinAirUsageStrategy.UseAllCharges ||
			(ThinAirLastChargeUsage == ThinAirUsageStrategy.ReserveLastChargeForRaise && nextGCD == RaisePvE);

		if (((nextGCD is IBaseAction action && action.Info.MPNeed >= ThinAirNeed && IsLastAction() == IsLastGCD()) ||
			 ((MergedStatus.HasFlag(AutoStatus.Raise) || nextGCD == RaisePvE) && IsLastAction() == IsLastGCD())) &&
			ThinAirPvE.CanUse(out act, usedUp: useLastThinAirCharge))
		{
			return true;
		}

		if (IsBurst &&
			PresenceOfMindPvE.Cooldown.WillHaveOneCharge(5) &&
			UseBurstMedicine(out act))
		{
			return true;
		}

		if (IsBurst && CombatTime > 6f && PresenceOfMindPvE.CanUse(out act, skipTTKCheck: IsInHighEndDuty))
		{
			return true;
		}

		if (StatusHelper.PlayerWillStatusEndGCD(2, 0, true, StatusID.DivineGrace) &&
			DivineCaressPvE.CanUse(out act))
		{
			return true;
		}

		if (HasHealingLockout)
		{
			return base.EmergencyAbility(nextGCD, out act);
		}

		return base.EmergencyAbility(nextGCD, out act);
	}

	protected override bool GeneralAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		if (UseDivine && DivineCaressPvE.CanUse(out act))
		{
			return true;
		}

		return base.GeneralAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.DivineCaressPvE)]
	protected override bool DefenseAreaAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		if (DivineCaressPvE.CanUse(out act))
		{
			return true;
		}

		return base.DefenseAreaAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.DivineBenisonPvE, ActionID.AquaveilPvE)]
	protected override bool DefenseSingleAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		if ((DivineBenisonPvE.Cooldown.IsCoolingDown && !DivineBenisonPvE.Cooldown.WillHaveOneCharge(15)) ||
			(AquaveilPvE.Cooldown.IsCoolingDown && !AquaveilPvE.Cooldown.WillHaveOneCharge(52)))
		{
			return base.DefenseSingleAbility(nextGCD, out act);
		}

		if (DivineBenisonPvE.CanUse(out act))
		{
			return true;
		}

		if (AquaveilPvE.CanUse(out act))
		{
			return true;
		}

		return base.DefenseSingleAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.AsylumPvE)]
	protected override bool HealAreaAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (PartyMembersAverHP < AsylumHeal &&
			AsylumPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealAreaAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.BenedictionPvE, ActionID.TetragrammatonPvE)]
	protected override bool HealSingleAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (HasHealingLockout)
			return false;

		IBattleChara? benisonTarget = DivineBenisonPvE.Target.Target;

		if (benisonTarget != null &&
			IsTank(benisonTarget) &&
			benisonTarget.GetHealthRatio() < 0.9f &&
			DivineBenisonPvE.Cooldown.CurrentCharges == 2 &&
			DivineBenisonPvE.CanUse(out act))
		{
			return true;
		}

		IBattleChara? benedictionTarget = BenedictionPvE.Target.Target;

		if (benedictionTarget != null &&
			!HasSingleHealLockoutStatus(benedictionTarget) &&
			BenedictionTargetAllowed(benedictionTarget) &&
			PartyMembersAverHP > 0.8f &&
			BenedictionPvE.CanUse(out act) &&
			benedictionTarget.GetHealthRatio() < BenedictionHeal)
		{
			return true;
		}

		if (IsLastAction(ActionID.BenedictionPvE))
		{
			return base.HealSingleAbility(nextGCD, out act);
		}

		IBattleChara? tetraTarget = TetragrammatonPvE.Target.Target;

		if (tetraTarget != null &&
			!HasSingleHealLockoutStatus(tetraTarget) &&
			PartyMembersAverHP > 0.8f &&
			TetragrammatonPvE.CanUse(out act, usedUp: true) &&
			tetraTarget.GetHealthRatio() < CurrentTetragrammatonThreshold)
		{
			return true;
		}

		return base.HealSingleAbility(nextGCD, out act);
	}

	protected override bool AttackAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		if (InCombat)
		{
			if (IsBurst && CombatTime > 6f && PresenceOfMindPvE.CanUse(out act, skipTTKCheck: IsInHighEndDuty))
			{
				return true;
			}

			if (!HasHealingLockout &&
				CombatTime > 6f &&
				IsBurst &&
				AssizePvE.CanUse(out act, skipAoeCheck: true))
			{
				return true;
			}

			if (CombatTime > 6f && AssizePvE.CanUse(out act, skipAoeCheck: true))
			{
				return true;
			}

			if (ShouldSwiftcastForMovement(nextGCD) &&
				MovingTime > MovementTimeThreshold + 0.5f &&
				SwiftcastPvE.CanUse(out act))
			{
				return true;
			}
		}

		return base.AttackAbility(nextGCD, out act);
	}

	#endregion

	#region GCD Logic

	[RotationDesc(ActionID.AfflatusRapturePvE, ActionID.MedicaIiPvE, ActionID.CureIiiPvE)]
	protected override bool HealAreaGCD(out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (HasHealingLockout)
			return false;

		if (ShouldHoldRaiseSwift)
			return base.HealAreaGCD(out act);

		if (!IsPresenceOfMindHealLockActive &&
			BloodLily != 3 &&
			HasSufficientMovement &&
			PartyMembersAverHP < 0.8f &&
			AfflatusRapturePvE.CanUse(out act))
		{
			return true;
		}

		if (!IsPresenceOfMindHealLockActive &&
			CanUseRaptureByBloodLily(out float raptureThreshold) &&
			PartyMembersAverHP < raptureThreshold &&
			AfflatusRapturePvE.CanUse(out act))
		{
			return true;
		}

		int hasMedica2 = 0;
		foreach (IBattleChara n in PartyMembers)
		{
			if (n.HasStatus(true, StatusID.MedicaIi))
			{
				hasMedica2++;
			}
		}

		int partyCount = 0;
		foreach (IBattleChara _ in PartyMembers)
		{
			partyCount++;
		}

		if (!IsPresenceOfMindHealLockActive && MedicaIiPvE.EnoughLevel)
		{
			if (MedicaIiiPvE.EnoughLevel &&
				PartyMembersAverHP < MedicaHeal &&
				MedicaIiiPvE.CanUse(out act) &&
				hasMedica2 < partyCount / 2 &&
				!IsLastAction(true, MedicaIiPvE))
			{
				return true;
			}

			if (!MedicaIiiPvE.EnoughLevel &&
				PartyMembersAverHP < MedicaHeal &&
				MedicaIiPvE.CanUse(out act) &&
				hasMedica2 < partyCount / 2 &&
				!IsLastAction(true, MedicaIiPvE))
			{
				return true;
			}
		}

		if (!IsPresenceOfMindHealLockActive &&
			HasMedicaIii &&
			PartyMembersAverHP < CureIIIHeal &&
			CureIiiPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealAreaGCD(out act);
	}

	[RotationDesc(ActionID.AfflatusSolacePvE, ActionID.RegenPvE)]
	protected override bool HealSingleGCD(out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (HasHealingLockout)
			return false;

		if (ShouldHoldRaiseSwift)
			return base.HealSingleGCD(out act);

		IBattleChara? solaceTarget = AfflatusSolacePvE.Target.Target;

		if (!IsPresenceOfMindHealLockActive &&
			!HasSingleHealLockoutStatus(solaceTarget) &&
			solaceTarget != null &&
			PartyMembersAverHP > 0.8f &&
			HasSufficientMovement &&
			BloodLily <= 1 &&
			solaceTarget.GetHealthRatio() < 0.75f &&
			AfflatusSolacePvE.CanUse(out act))
		{
			return true;
		}

		if (!IsPresenceOfMindHealLockActive &&
			CanUseSolaceByBloodLily(out float solaceThreshold) &&
			!HasSingleHealLockoutStatus(solaceTarget) &&
			PartyMembersAverHP > 0.8f &&
			solaceTarget != null &&
			solaceTarget.GetHealthRatio() < solaceThreshold &&
			AfflatusSolacePvE.CanUse(out act))
		{
			return true;
		}

		IBattleChara? regenTarget = RegenPvE.Target.Target;

		if (!IsPresenceOfMindHealLockActive &&
			!HasSingleHealLockoutStatus(regenTarget) &&
			regenTarget != null &&
			IsTank(regenTarget) &&
			!regenTarget.HasStatus(true, StatusID.Regen) &&
			HasSufficientMovement &&
			BloodLily == 3 &&
			regenTarget.GetHealthRatio() < 0.8f &&
			RegenPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealSingleGCD(out act);
	}

	[RotationDesc(ActionID.RaisePvE)]
	protected override bool RaiseGCD(out IAction? act)
	{
		UpdateActionTracking();

		if (RaisePvE.CanUse(out act))
		{
			return true;
		}

		return base.RaiseGCD(out act);
	}

	protected override bool GeneralGCD(out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (HasThinAir && MergedStatus.HasFlag(AutoStatus.Raise))
		{
			return RaiseGCD(out act);
		}

		if (ShouldHoldRaiseSwift)
			return base.GeneralGCD(out act);

		if (!HasHealingLockout &&
			!IsPresenceOfMindHealLockActive &&
			IsBurst &&
			BloodLily < 3 &&
			PresenceOfMindPvE.Cooldown.WillHaveOneCharge(15) &&
			AfflatusRapturePvE.CanUse(out act))
		{
			return true;
		}

		if (HasPresenceOfMind &&
			AllowPoMRestrictedMovementGCDs &&
			AfflatusMiseryPvE.CanUse(out act, skipAoeCheck: true))
		{
			return true;
		}

		if (AllowPoMRestrictedMovementGCDs &&
			GlareIvPvE.CanUse(out act))
		{
			return true;
		}

		if (!HasHealingLockout &&
			!IsPresenceOfMindHealLockActive &&
			BloodLily != 3 &&
			StatusHelper.PlayerHasStatus(true, StatusID.Confession) &&
			StatusHelper.PlayerWillStatusEndGCD(1, 0, true, StatusID.Confession) &&
			CanUseRaptureByBloodLily(out float raptureThreshold) &&
			PartyMembersAverHP < raptureThreshold &&
			AfflatusRapturePvE.CanUse(out act))
		{
			return true;
		}

		if (HolyPvE.EnoughLevel)
		{
			if (HolyIiiPvE.EnoughLevel && HolyIiiPvE.CanUse(out act))
			{
				return true;
			}

			if (!HolyIiiPvE.EnoughLevel && HolyPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (InCombat &&
			InFirst20sAfterPresenceOfMind &&
			CurrentTargetDiaMissingOrEnding(PresenceOfMindDiaRefreshSeconds) &&
			CanUseCurrentDia(out act, skipStatusProvideCheck: true))
		{
			return true;
		}

		if (InCombat &&
			InLast5sOfPresenceOfMind &&
			CurrentTargetDiaMissingOrEnding(PresenceOfMindDiaRefreshSeconds) &&
			CanUseCurrentDia(out act, skipStatusProvideCheck: true))
		{
			return true;
		}

		if (InCombat &&
			HasSufficientMovement &&
			CurrentTargetDiaMissingOrEnding(MovingDiaRefreshSeconds) &&
			CanUseCurrentDia(out act, skipStatusProvideCheck: true))
		{
			return true;
		}

		if (CanUseCurrentDia(out act))
		{
			return true;
		}

		if (GlareIiiPvE.EnoughLevel && GlareIiiPvE.CanUse(out act))
		{
			return true;
		}

		if (GlarePvE.EnoughLevel && !GlareIiiPvE.EnoughLevel && GlarePvE.CanUse(out act))
		{
			return true;
		}

		if (StoneIvPvE.EnoughLevel && !GlarePvE.EnoughLevel && StoneIvPvE.CanUse(out act))
		{
			return true;
		}

		if (StoneIiiPvE.EnoughLevel && !StoneIvPvE.EnoughLevel && StoneIiiPvE.CanUse(out act))
		{
			return true;
		}

		if (StoneIiPvE.EnoughLevel && !StoneIiiPvE.Info.EnoughLevelAndQuest() && StoneIiPvE.CanUse(out act))
		{
			return true;
		}

		if (!StoneIiPvE.EnoughLevel && StonePvE.CanUse(out act))
		{
			return true;
		}

		return base.GeneralGCD(out act);
	}

	#endregion
}