using System.ComponentModel;

namespace RotationSolver.ExtraRotations.Healer;

[Rotation("BeirutaSGE", CombatType.PvE, GameVersion = "7.45", Description = "半自动零式/绝境战循环，需配合 CD 计划器或手动输入使用")]
[SourceCode(Path = "main/ExtraRotations/Healer/BeirutaSGE.cs")]
[ExtraRotation]

public sealed class BeirutaSGE : SageRotation
{
	#region Config Options

	[RotationConfig(CombatType.PvE, Name =
		"请注意，本循环针对高端战斗进行优化。\n" +
		"• 治疗行为设计为自动使用，而减伤保持最少，以更好地支持 CD 计划器或手动输入\n" +
		"• 只有描述中列出的技能会被自动使用，其他所有技能都应手动使用或通过 CD 计划器使用\n" +
		"• 如果队伍中没有团辅，请将拦截设置为仅 GCD 使用，并在需要时手动使用最后一层黏膜\n" +
		"• 如果需要延迟本循环的爆发时机，禁用 AutoBurst 即可\n" +
		"• 将活化施加给自己会被视为使用魂灵风息或均衡预后的信号，具体取决于队伍平均 HP 设置\n" +
		"• 使用活化或宏循环 DefenseArea 来手动触发均衡预后\n" +
		"• 本循环中单体 GCD 治疗受到严格限制\n" +
		"• 如果启用倒计时活化/护盾，请注意可能有恶意玩家用短倒计时诱导你（离开时将 StartOnCountdown 设为 False）\n")]
	public bool RotationNotes { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "尝试通过在 GCD 逻辑末尾允许均衡预后来防止卡死（实验性）")]
	public bool AntiBrick { get; set; } = false;

	[RotationConfig(CombatType.PvE, Name = "非战斗时使用均衡")]
	public bool OOCEukrasia { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "启用即刻咏唱限制逻辑：拥有即刻咏唱时尝试阻止除复活外的其他行为")]
	public bool SwiftLogic { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "移动时使用即刻咏唱")]
	public bool UseSwiftcastForMovement { get; set; } = true;

	[Range(0, 5, ConfigUnitType.Seconds, 0.1f)]
	[RotationConfig(CombatType.PvE, Name = "允许移动类操作前的最小移动时间")]
	public float MovementTimeThreshold { get; set; } = 0.8f;

	[RotationConfig(CombatType.PvE, Name = "宏观宇宙生效时锁定治疗行为")]
	public bool LockHealingActionsDuringMacrocosmos { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "倒计时开场时使用活化")]
	public bool UseZoeInOpener { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "倒计时开场时使用均衡预后")]
	public bool EukrasianPrognosisDuringCountdown { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "使用哪种开场")]
	public OpenerStrategy OpenerSelection { get; set; } = OpenerStrategy.PneumaOpener;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用白牛清液所需的目标 HP 阈值")]
	public float TaurocholeHeal { get; set; } = 0.7f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用灵橡清液所需的目标 HP 阈值")]
	public float DruocholeHeal { get; set; } = 0.6f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用拯救所需的心脏目标 HP 阈值")]
	public float SoteriaHeal { get; set; } = 0.8f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用消化所需的队伍平均 HP 阈值")]
	public float PepsisHeal { get; set; } = 0.4f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用寄生清液所需的队伍平均 HP 阈值")]
	public float IxocholeHeal { get; set; } = 0.8f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用自生所需的队伍平均 HP 阈值")]
	public float PhysisHeal { get; set; } = 0.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "单体目标时使用魂灵风息所需的队伍平均 HP 阈值")]
	public float PneumaHeal { get; set; } = 0.40f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "多目标时使用魂灵风息所需的队伍平均 HP 阈值")]
	public float PneumaDyskrasiaHeal { get; set; } = 0.70f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用白牛清液所需的队伍平均 HP 阈值")]
	public float HealSingleTaurocholeHeal { get; set; } = 0.7f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用灵橡清液所需的队伍平均 HP 阈值")]
	public float HealSingleDruocholeHeal { get; set; } = 0.6f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "在魂灵风息（如果较低）或均衡预后（如果较高）上使用活化所需的队伍平均 HP 阈值")]
	public float ZoePneumaHeal { get; set; } = 0.40f;

	public enum OpenerStrategy : byte
	{
		[Description("战前使用箭毒开场")]
		ToxikonOpener,

		[Description("战前使用魂灵风息开场")]
		PneumaOpener,
	}

	#endregion

	#region Constants / Fields

	private const long PsycheDotRefreshMs = 18_000;
	private const float EarlyDotRefreshSeconds = 12f;
	private const float SwiftcastPostActionLockSeconds = 2f;
	private const float MovementLeadSeconds = 0.5f;
	private static bool IsPartyMedicated
	{
		get
		{
			if (PartyMembers == null) return false;
			foreach (var member in PartyMembers)
			{
				if (member?.StatusList == null) continue;
				foreach (var status in member.StatusList)
				{
					if (status.StatusId == (uint)StatusID.Medicated) return true;
				}
			}
			return false;
		}
	}

	private long _psycheUsedAtMs;
	private float _lastSwiftcastLockingActionCombatTime = float.MinValue;

	#endregion

	#region Tracking Properties

	private IBaseAction? _lastEukrasiaActionAim;
	private IBaseAction? _EukrasiaActionAim;

	private bool HasKerachole => StatusHelper.PlayerHasStatus(true, StatusID.Kerachole);
	private bool HasZoe => StatusHelper.PlayerHasStatus(true, StatusID.Zoe);
	private bool HasMacrocosmos => StatusHelper.PlayerHasStatus(true, StatusID.Macrocosmos);
	private bool HasEukrasianPrognosis => StatusHelper.PlayerHasStatus(true, StatusID.EukrasianPrognosis);
	private bool HasMedicated => StatusHelper.PlayerHasStatus(true, StatusID.Medicated);

	private const int PneumaAoeThreshold = 2;

	private int GetEnemiesAroundTarget(float radius)
	{
		if (CurrentTarget == null || AllHostileTargets == null)
			return 0;

		int count = 0;
		foreach (var enemy in AllHostileTargets)
		{
			if (enemy != null &&
				Vector3.Distance(CurrentTarget.Position, enemy.Position) <
				(radius + enemy.HitboxRadius))
				count++;
		}
		return count;
	}

	private bool IsTargetAoeAtLeast(IBaseAction action, int threshold)
		=> GetEnemiesAroundTarget(action.Info.EffectRange) >= threshold;

	private bool IsPhlegmaAoeAtLeast(int threshold)
		=> GetEnemiesAroundTarget(5f) >= threshold;

	private bool InFirst20sAfterPsyche =>
		_psycheUsedAtMs != 0 &&
		Environment.TickCount64 - _psycheUsedAtMs < PsycheDotRefreshMs;

	private bool HasSufficientMovement =>
		IsMoving &&
		MovingTime > MovementTimeThreshold;

	private bool HasHealingLockout =>
		LockHealingActionsDuringMacrocosmos &&
		HasMacrocosmos;

	private bool IsSwiftcastPostActionLockActive =>
		InCombat &&
		_lastSwiftcastLockingActionCombatTime > float.MinValue / 2 &&
		CombatTime - _lastSwiftcastLockingActionCombatTime <= SwiftcastPostActionLockSeconds;

	private bool ShouldDeferToRaise() =>
		(HasSwift || IsLastAction(ActionID.SwiftcastPvE)) &&
		SwiftLogic &&
		MergedStatus.HasFlag(AutoStatus.Raise);

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

	private void UpdateActionTracking()
	{
		if (!InCombat)
		{
			_lastSwiftcastLockingActionCombatTime = float.MinValue;
			return;
		}

		if (IsLastAction(ActionID.EukrasiaPvE) ||
			IsLastAction(ActionID.PhlegmaPvE) ||
			IsLastAction(ActionID.ToxikonPvE) ||
			IsLastAction(ActionID.ToxikonIiPvE) ||
			IsLastAction(ActionID.DyskrasiaPvE) ||
			IsLastAction(ActionID.DyskrasiaIiPvE) ||
			IsLastAction(ActionID.EsunaPvE))
		{
			_lastSwiftcastLockingActionCombatTime = CombatTime;
		}
	}

	private bool IsMovementPreferredNextGCD(IAction nextGCD)
	{
		if (nextGCD == EukrasiaPvE ||
			nextGCD == PhlegmaPvE ||
			nextGCD == ToxikonPvE ||
			nextGCD == ToxikonIiPvE ||
			nextGCD == DyskrasiaPvE ||
			nextGCD == DyskrasiaIiPvE ||
			nextGCD == EsunaPvE)
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
			MovingTime <= MovementTimeThreshold + MovementLeadSeconds ||
			HasSwift ||
			Addersting > 0 ||
			IsLastAction(ActionID.SwiftcastPvE) ||
			ShouldDeferToRaise() ||
			IsSwiftcastPostActionLockActive)
		{
			return false;
		}

		return !IsMovementPreferredNextGCD(nextGCD);
	}

	private bool CanUseCountdownEukrasianPrognosis(out IAction? act)
	{
		act = null;

		if (EukrasianPrognosisIiPvE.EnoughLevel &&
			EukrasianPrognosisIiPvE.IsEnabled &&
			EukrasianPrognosisIiPvE.CanUse(out act))
		{
			return true;
		}

		if (EukrasianPrognosisPvE.EnoughLevel &&
			EukrasianPrognosisPvE.IsEnabled &&
			EukrasianPrognosisPvE.CanUse(out act))
		{
			return true;
		}

		return false;
	}

	private bool ActionTargetBelow(IBaseAction action, float threshold)
		=> action.Target.Target?.GetHealthRatio() < threshold;

	private bool AnyPartyMemberBelow(float threshold)
	{
		foreach (var member in PartyMembers)
		{
			if (member.GetHealthRatio() < threshold) return true;
		}
		return false;
	}

	public override void DisplayRotationStatus()
	{
		ImGui.Text($"Last E.Action Aim Cleared From Queue: {_lastEukrasiaActionAim}");
		ImGui.Text($"Current E.Action Aim: {_EukrasiaActionAim}");
		ImGui.Text($"Swiftcast Movement Lock: {IsSwiftcastPostActionLockActive}");
	}

	#endregion

	#region Countdown Logic

	protected override IAction? CountDownAction(float remainTime)
	{
		IAction? act;

		if (OpenerSelection == OpenerStrategy.PneumaOpener
			&& remainTime < PneumaPvE.Info.CastTime + CountDownAhead
			&& PneumaPvE.CanUse(out act))
		{
			return act;
		}

		if (remainTime <= 2.1f && UseBurstMedicine(out act))
		{
			return act;
		}

		if (OpenerSelection == OpenerStrategy.ToxikonOpener
			&& remainTime < 1.5f + CountDownAhead
			&& ToxikonIiPvE.CanUse(out act))
		{
			return act;
		}

		if (remainTime is < 7f and > 6f
			&& EukrasianPrognosisDuringCountdown
			&& HasEukrasia
			&& CanUseCountdownEukrasianPrognosis(out act))
		{
			return act;
		}

		if (remainTime is < 8f and > 7f
			&& EukrasianPrognosisDuringCountdown
			&& !HasEukrasia
			&& EukrasiaPvE.CanUse(out act))
		{
			return act;
		}

		if (remainTime is < 14f and > 9f
			&& UseZoeInOpener
			&& ZoePvE.CanUse(out act))
		{
			return act;
		}

		if (remainTime < 4f && EukrasiaPvE.CanUse(out act))
		{
			return act;
		}

		if (remainTime is < 14f and > 9f && KardiaPvE.CanUse(out act))
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

		act = null;

		if (IsLastGCD(false,
				EukrasianPrognosisIiPvE,
				EukrasianPrognosisPvE,
				EukrasianDiagnosisPvE,
				EukrasianDyskrasiaPvE,
				EukrasianDosisIiiPvE,
				EukrasianDosisIiPvE,
				EukrasianDosisPvE)
			|| !InCombat)
		{
			ClearEukrasia(nextGCD);
		}

		if (ChoiceEukrasia(out act))
		{
			return true;
		}

		if (ZoePvE.EnoughLevel && !ZoePvE.Cooldown.IsCoolingDown)
		{
			if (PartyMembersAverHP < ZoePneumaHeal
				&& nextGCD.IsTheSameTo(false, PneumaPvE)
				&& ZoePvE.CanUse(out act))
			{
				return true;
			}
		}

		return base.EmergencyAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.PsychePvE, ActionID.SwiftcastPvE)]
	protected override bool AttackAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (ShouldSwiftcastForMovement(nextGCD) &&
			SwiftcastPvE.CanUse(out act))
		{
			return true;
		}

		if (IsBurst && CombatTime > 10f && PsychePvE.CanUse(out act))
		{
			StampPsycheUse();
			return true;
		}

		return base.AttackAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.HaimaPvE, ActionID.TaurocholePvE, ActionID.KrasisPvE)]
	protected override bool DefenseSingleAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (!HasKerachole
			&& TaurocholePvE.CanUse(out act))
		{
			return true;
		}

		if (KrasisPvE.CanUse(out act))
		{
			return true;
		}

		if (HaimaPvE.CanUse(out act))
		{
			return true;
		}


		return base.DefenseSingleAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.PhysisPvE, ActionID.PepsisPvE, ActionID.IxocholePvE)]
	protected override bool HealAreaAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (HasHealingLockout)
			return false;

		if (!MergedStatus.HasFlag(AutoStatus.DefenseArea)
			&& !MergedStatus.HasFlag(AutoStatus.DefenseSingle)
			&& PartyMembersAverHP < PepsisHeal
			&& PepsisPvE.CanUse(out act))
		{
			return true;
		}

		if (PartyMembersAverHP < IxocholeHeal && IxocholePvE.CanUse(out act))
		{
			return true;
		}

		if (PartyMembersAverHP < PhysisHeal && PhysisIiPvE.CanUse(out act))
		{
			return true;
		}

		if (PartyMembersAverHP < PhysisHeal && !PhysisIiPvE.EnoughLevel && PhysisPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealAreaAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.KrasisPvE, ActionID.TaurocholePvE, ActionID.DruocholePvE, ActionID.KardiaPvE, ActionID.SoteriaPvE)]
	protected override bool HealSingleAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (HasHealingLockout)
			return false;

		IBattleChara? taurocholeTarget = TaurocholePvE.Target.Target;
		IBattleChara? druocholeTarget = DruocholePvE.Target.Target;
		IBattleChara? kardiaTarget = KardiaPvE.Target.Target;

		// Krasis always first
		if (KrasisPvE.CanUse(out act))
			return true;

		// Addersgall > 2: spend freely, prefer Taurochole first, but do not use it under Kerachole
		if (Addersgall > 2)
		{
			if (!HasKerachole && TaurocholePvE.CanUse(out act))
				return true;

			if (DruocholePvE.CanUse(out act))
				return true;
		}

		// Prevent immediately chaining another single-target heal after Taurochole
		if (IsLastAction(ActionID.TaurocholePvE))
			return base.HealSingleAbility(nextGCD, out act);

		IBattleChara? soteriaTarget = null;
		foreach (var member in PartyMembers)
		{
			if (member.HasStatus(true, StatusID.Kardion)) { soteriaTarget = member; break; }
		}

		if (PartyMembersAverHP > 0.85f &&
			soteriaTarget != null &&
			soteriaTarget.GetHealthRatio() < SoteriaHeal &&
			SoteriaPvE.CanUse(out act))
		{
			return true;
		}


		// Addersgall == 2: more restrictive
		if (Addersgall == 2 && PartyMembersAverHP > 0.85f)
		{
			if (!HasKerachole &&
				taurocholeTarget != null &&
				taurocholeTarget.GetHealthRatio() < HealSingleTaurocholeHeal &&
				TaurocholePvE.CanUse(out act))
			{
				return true;
			}

			if (druocholeTarget != null &&
				!HasSingleHealLockoutStatus(druocholeTarget) &&
				druocholeTarget.GetHealthRatio() < HealSingleDruocholeHeal &&
				DruocholePvE.CanUse(out act))
			{
				return true;
			}
		}

		if (PartyMembersAverHP > 0.85f &&
			kardiaTarget != null &&
			!HasSingleHealLockoutStatus(kardiaTarget) &&
			kardiaTarget.GetHealthRatio() < 0.8f &&
			KardiaPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealSingleAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.KardiaPvE, ActionID.RhizomataPvE, ActionID.SoteriaPvE)]
	protected override bool GeneralAbility(IAction nextGCD, out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (InCombat || !HasKardia)
		{
			if (KardiaPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (Addersgall <= 1 && RhizomataPvE.CanUse(out act))
		{
			return true;
		}

		if (InCombat && Addersgall <= 1 && RhizomataPvE.CanUse(out act))
		{
			return true;
		}

		IBattleChara? soteriaTarget = null;
		foreach (var member in PartyMembers)
		{
			if (member.HasStatus(true, StatusID.Kardion)) { soteriaTarget = member; break; }
		}

		if (PartyMembersAverHP > 0.8f &&
			soteriaTarget != null &&
			soteriaTarget.GetHealthRatio() < SoteriaHeal &&
			SoteriaPvE.CanUse(out act))
		{
			return true;
		}

		if (IsPartyMedicated && UseBurstMedicine(out act))
		{
			return true;
		}

		return base.GeneralAbility(nextGCD, out act);
	}

	#endregion

	#region Eukrasia Logic

	private void SetEukrasia(IBaseAction act)
	{
		if (act == null || (_EukrasiaActionAim != null && IsLastGCD(true, _EukrasiaActionAim)))
		{
			return;
		}

		_EukrasiaActionAim = act;
	}

	private void ClearEukrasia(IAction nextGCD)
	{
		if (_EukrasiaActionAim != null)
		{
			_lastEukrasiaActionAim = _EukrasiaActionAim;
			_EukrasiaActionAim = null;

			if (HasEukrasia
				&& ((InCombat && HasHostilesInMaxRange && nextGCD == null && SecondsSinceLastNextGCDChange >= 2f)
					|| (!InCombat && !HasHostilesInMaxRange)))
			{
				StatusHelper.StatusOff(StatusID.Eukrasia);
			}
		}
	}

	private bool ChoiceEukrasia(out IAction? act)
	{
		act = null;
		bool selected = false;

		if (EukrasianPrognosisIiPvE.EnoughLevel
			&& EukrasianPrognosisIiPvE.IsEnabled
			&& MergedStatus.HasFlag(AutoStatus.DefenseArea)
			&& EukrasianPrognosisIiPvE.CanUse(out _))
		{
			SetEukrasia(EukrasianPrognosisIiPvE);
			selected = true;
		}
		else if (!EukrasianPrognosisIiPvE.EnoughLevel
			&& EukrasianPrognosisPvE.EnoughLevel
			&& EukrasianPrognosisPvE.IsEnabled
			&& MergedStatus.HasFlag(AutoStatus.DefenseArea)
			&& EukrasianPrognosisPvE.CanUse(out _))
		{
			SetEukrasia(EukrasianPrognosisPvE);
			selected = true;
		}
		else if (EukrasianDiagnosisPvE.EnoughLevel
			&& EukrasianDiagnosisPvE.IsEnabled
			&& Addersting < 3
			&& MovingTime > MovementTimeThreshold
			&& MergedStatus.HasFlag(AutoStatus.DefenseSingle)
			&& EukrasianDiagnosisPvE.CanUse(out _))
		{
			SetEukrasia(EukrasianDiagnosisPvE);
			selected = true;
		}
		else if (EukrasianDyskrasiaPvE.EnoughLevel
			&& EukrasianDyskrasiaPvE.IsEnabled
			&& !MergedStatus.HasFlag(AutoStatus.DefenseSingle)
			&& !MergedStatus.HasFlag(AutoStatus.DefenseArea)
			&& EukrasianDyskrasiaPvE.CanUse(out _))
		{
			SetEukrasia(EukrasianDyskrasiaPvE);
			selected = true;
		}
		else if ((!EukrasianDyskrasiaPvE.CanUse(out _) || !DyskrasiaPvE.CanUse(out _))
			&& EukrasianDosisIiiPvE.CanUse(out _)
			&& EukrasianDosisIiiPvE.EnoughLevel
			&& !MergedStatus.HasFlag(AutoStatus.DefenseSingle)
			&& !MergedStatus.HasFlag(AutoStatus.DefenseArea)
			&& EukrasianDosisIiiPvE.IsEnabled)
		{
			SetEukrasia(EukrasianDosisIiiPvE);
			selected = true;
		}
		else if ((!EukrasianDyskrasiaPvE.CanUse(out _) || !DyskrasiaPvE.CanUse(out _))
			&& EukrasianDosisIiPvE.CanUse(out _)
			&& !EukrasianDosisIiiPvE.EnoughLevel
			&& EukrasianDosisIiPvE.EnoughLevel
			&& !MergedStatus.HasFlag(AutoStatus.DefenseSingle)
			&& !MergedStatus.HasFlag(AutoStatus.DefenseArea)
			&& EukrasianDosisIiPvE.IsEnabled)
		{
			SetEukrasia(EukrasianDosisIiPvE);
			selected = true;
		}
		else if ((!EukrasianDyskrasiaPvE.CanUse(out _) || !DyskrasiaPvE.CanUse(out _))
			&& EukrasianDosisPvE.CanUse(out _)
			&& !EukrasianDosisIiPvE.EnoughLevel
			&& EukrasianDosisPvE.EnoughLevel
			&& !MergedStatus.HasFlag(AutoStatus.DefenseSingle)
			&& !MergedStatus.HasFlag(AutoStatus.DefenseArea)
			&& EukrasianDosisPvE.IsEnabled)
		{
			SetEukrasia(EukrasianDosisPvE);
			selected = true;
		}

		if (!selected)
		{
			return false;
		}

		if (!HasEukrasia && EukrasiaPvE.CanUse(out act))
		{
			return true;
		}

		return false;
	}

	#endregion

	#region Eukrasia Execution

	private bool DoEukrasianPrognosisIi(out IAction? act)
	{
		act = null;

		if (_EukrasiaActionAim != EukrasianPrognosisIiPvE)
		{
			return false;
		}

		if (!HasEukrasia)
		{
			return EukrasiaPvE.CanUse(out act);
		}

		return EukrasianPrognosisIiPvE.CanUse(out act);
	}

	private bool DoEukrasianPrognosis(out IAction? act)
	{
		act = null;

		if (_EukrasiaActionAim != EukrasianPrognosisPvE)
		{
			return false;
		}

		if (!HasEukrasia)
		{
			return EukrasiaPvE.CanUse(out act);
		}

		return EukrasianPrognosisPvE.CanUse(out act);
	}

	private bool DoEukrasianDiagnosis(out IAction? act)
	{
		act = null;

		if (_EukrasiaActionAim != EukrasianDiagnosisPvE)
		{
			return false;
		}

		if (!HasEukrasia)
		{
			return EukrasiaPvE.CanUse(out act);
		}

		return EukrasianDiagnosisPvE.CanUse(out act);
	}

	private bool DoEukrasianDyskrasia(out IAction? act)
	{
		act = null;

		if (_EukrasiaActionAim != EukrasianDyskrasiaPvE)
		{
			return false;
		}

		if (!HasEukrasia)
		{
			return EukrasiaPvE.CanUse(out act);
		}

		return EukrasianDyskrasiaPvE.CanUse(out act);
	}

	private bool DoEukrasianDosisIii(out IAction? act)
	{
		act = null;

		if (_EukrasiaActionAim != EukrasianDosisIiiPvE)
		{
			return false;
		}

		if (!HasEukrasia)
		{
			return EukrasiaPvE.CanUse(out act);
		}

		return EukrasianDosisIiiPvE.CanUse(out act);
	}

	private bool DoEukrasianDosisIi(out IAction? act)
	{
		act = null;

		if (_EukrasiaActionAim != EukrasianDosisIiPvE)
		{
			return false;
		}

		if (!HasEukrasia)
		{
			return EukrasiaPvE.CanUse(out act);
		}

		return EukrasianDosisIiPvE.CanUse(out act);
	}

	private bool DoEukrasianDosis(out IAction? act)
	{
		act = null;

		if (_EukrasiaActionAim != EukrasianDosisPvE)
		{
			return false;
		}

		if (!HasEukrasia)
		{
			return EukrasiaPvE.CanUse(out act);
		}

		return EukrasianDosisPvE.CanUse(out act);
	}

	#endregion

	#region Damage / DoT Helpers

	private void StampPsycheUse() => _psycheUsedAtMs = Environment.TickCount64;

	private bool CurrentTargetEukrasianDosisMissingOrEnding(float remainingSeconds)
	{
		if (CurrentTarget == null)
		{
			return false;
		}

		return
			(EukrasianDosisIiiPvE.EnoughLevel &&
			 (!CurrentTarget.HasStatus(true, StatusID.EukrasianDosisIii) ||
			  CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.EukrasianDosisIii))) ||
			(!EukrasianDosisIiiPvE.EnoughLevel && EukrasianDosisIiPvE.EnoughLevel &&
			 (!CurrentTarget.HasStatus(true, StatusID.EukrasianDosisIi) ||
			  CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.EukrasianDosisIi))) ||
			(!EukrasianDosisIiiPvE.EnoughLevel && !EukrasianDosisIiPvE.EnoughLevel && EukrasianDosisPvE.EnoughLevel &&
			 (!CurrentTarget.HasStatus(true, StatusID.EukrasianDosis) ||
			  CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.EukrasianDosis)));
	}

	private bool ShouldEarlyRefreshEukrasianDosis()
	{
		if (!InCombat || CurrentTarget == null)
		{
			return false;
		}

		if (!CurrentTargetEukrasianDosisMissingOrEnding(EarlyDotRefreshSeconds))
		{
			return false;
		}

		return MovingTime > MovementTimeThreshold || (HasBuffs && InFirst20sAfterPsyche) || HasMedicated;
	}

	private bool PrepareEarlyEukrasianDosisRefresh(out IAction? act)
	{
		act = null;

		if (!ShouldEarlyRefreshEukrasianDosis())
		{
			return false;
		}

		_EukrasiaActionAim = null;

		if (EukrasianDosisIiiPvE.EnoughLevel && EukrasianDosisIiiPvE.IsEnabled)
		{
			SetEukrasia(EukrasianDosisIiiPvE);
			return DoEukrasianDosisIii(out act);
		}

		if (EukrasianDosisIiPvE.EnoughLevel && EukrasianDosisIiPvE.IsEnabled)
		{
			SetEukrasia(EukrasianDosisIiPvE);
			return DoEukrasianDosisIi(out act);
		}

		if (EukrasianDosisPvE.EnoughLevel && EukrasianDosisPvE.IsEnabled)
		{
			SetEukrasia(EukrasianDosisPvE);
			return DoEukrasianDosis(out act);
		}

		return false;
	}

	#endregion

	#region GCD Logic

	[RotationDesc(ActionID.PneumaPvE, ActionID.EukrasianPrognosisIiPvE)]
	protected override bool HealAreaGCD(out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (HasHealingLockout || HasBuffs)
			return false;

		if (ShouldDeferToRaise())
		{
			return base.HealAreaGCD(out act);
		}

		if (PartyMembersAverHP < PneumaHeal ||
	(IsTargetAoeAtLeast(PneumaPvE, PneumaAoeThreshold) &&
	 PartyMembersAverHP < PneumaDyskrasiaHeal))
		{
			if (PneumaPvE.CanUse(out act))
			{
				return true;
			}
		}

		if (PartyMembersAverHP < 0.9f &&
			HasSufficientMovement &&
			Addersting <= 1 &&
			HasEukrasia &&
			EukrasianPrognosisPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealAreaGCD(out act);
	}

	[RotationDesc(ActionID.EukrasianDiagnosisPvE)]
	protected override bool HealSingleGCD(out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (HasHealingLockout)
			return false;

		if (ShouldDeferToRaise())
		{
			return base.HealSingleGCD(out act);
		}

		if (HasSufficientMovement &&
			Addersting <= 1 &&
			!HasBuffs &&
			HasEukrasia &&
			EukrasianDiagnosisPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealSingleGCD(out act);
	}

	[RotationDesc(ActionID.EgeiroPvE)]
	protected override bool RaiseGCD(out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (EgeiroPvE.CanUse(out act))
		{
			return true;
		}

		return base.RaiseGCD(out act);
	}

	protected override bool GeneralGCD(out IAction? act)
	{
		UpdateActionTracking();

		act = null;

		if (ShouldDeferToRaise())
		{
			return base.GeneralGCD(out act);
		}

		if (HasZoe)
		{
			if (PartyMembersAverHP < ZoePneumaHeal)
			{
				if (PneumaPvE.CanUse(out act))
				{
					return true;
				}
			}
			else if (!HasEukrasianPrognosis)
			{
				_EukrasiaActionAim = null;

				if (EukrasianPrognosisIiPvE.EnoughLevel && EukrasianPrognosisIiPvE.IsEnabled)
				{
					SetEukrasia(EukrasianPrognosisIiPvE);

					if (DoEukrasianPrognosisIi(out act))
					{
						return true;
					}
				}
				else if (EukrasianPrognosisPvE.EnoughLevel && EukrasianPrognosisPvE.IsEnabled)
				{
					SetEukrasia(EukrasianPrognosisPvE);

					if (DoEukrasianPrognosis(out act))
					{
						return true;
					}
				}
			}
		}

		if (DoEukrasianPrognosisIi(out act))
		{
			return true;
		}

		if (DoEukrasianPrognosis(out act))
		{
			return true;
		}

		if (DoEukrasianDiagnosis(out act))
		{
			return true;
		}

		if (PrepareEarlyEukrasianDosisRefresh(out act))
		{
			return true;
		}

		if (CombatTime > 10f
	&& IsBurst
	&& PhlegmaPvE.CanUse(out act, usedUp:
		HasBuffs
		|| HasMedicated
		|| IsPhlegmaAoeAtLeast(2)
		|| PhlegmaPvE.Cooldown.WillHaveXChargesGCD(2, 1)
		|| (HasSufficientMovement && PhlegmaPvE.Cooldown.WillHaveXChargesGCD(2, 4))))
		{
			return true;
		}

		if ((HasSufficientMovement || IsPhlegmaAoeAtLeast(2)) &&
	ToxikonPvE.CanUse(out act))
		{
			return true;
		}

		if (DoEukrasianDyskrasia(out act))
		{
			return true;
		}

		if (DyskrasiaPvE.CanUse(out act))
		{
			return true;
		}

		if (DoEukrasianDosisIii(out act))
		{
			return true;
		}

		if (DoEukrasianDosisIi(out act))
		{
			return true;
		}

		if (DoEukrasianDosis(out act))
		{
			return true;
		}

		if (DosisPvE.CanUse(out act))
		{
			return true;
		}

		if (OOCEukrasia && !InCombat && !HasEukrasia && EukrasiaPvE.CanUse(out act))
		{
			return true;
		}

		if (InCombat && !HasHostilesInRange && EukrasiaPvE.CanUse(out act))
		{
			return true;
		}

		if (AntiBrick && InCombat && HasHostilesInRange && HasEukrasia)
		{
			if (EukrasianPrognosisPvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}
		}

		return base.GeneralGCD(out act);
	}

	#endregion
}