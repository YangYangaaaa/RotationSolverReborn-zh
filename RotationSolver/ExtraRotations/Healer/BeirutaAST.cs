using System.ComponentModel;

namespace RotationSolver.ExtraRotations.Healer;

[Rotation("BeirutaAST", CombatType.PvE, GameVersion = "7.45", Description = "半自动零式/绝境战循环，需配合 CD 计划器或手动输入使用")]
[SourceCode(Path = "main/ExtraRotations/Healer/BeirutaAST.cs")]
[ExtraRotation]

public sealed class BeirutaAST : AstrologianRotation
{
	#region Config Options

	[RotationConfig(CombatType.PvE, Name =
		"请注意，本循环针对高端战斗进行优化（仅适用于 8 人倒计时战斗）。\n" +
		"• 只有描述中列出的技能会被自动使用，其他所有技能都应手动使用或通过 CD 计划器使用\n" +
		"• 请将拦截设置为仅 GCD 使用\n" +
		"• 如果需要延迟本循环的爆发时机，禁用 AutoBurst 即可\n" +
		"• DoT 效果在爆发阶段或移动期间可能会稍微提前刷新\n" +
		"• 光速由循环自动管理，不应手动使用\n" +
		"• 本循环中地星按冷却使用，如果想用 CD 计划器管理请在技能设置中禁用\n" +
		"• 如果使用了地宫占卜或中间学派，本循环会立即跟随使用太阳星座\n" +
		"• 来自 CD 计划器（或所有 GCD 操作）的大宇宙不可靠，请手动拦截\n" +
		"• 本循环中单体治疗的使用故意更保守\n")]
	public bool RotationNotes { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "开场/爆发开启窗口（GCD 数）")]
	[Range(0, 2, ConfigUnitType.None, 1)]
	public OpenWindowGcd OpenWindow { get; set; } = OpenWindowGcd.ThreeGcd;

	public enum OpenWindowGcd : byte
	{
		[Description("0 GCD（0.0s）")] ZeroGcd,
		[Description("1 GCD（2.2s）")] OneGcd,
		[Description("2 GCD（5.0s）")] TwoGcd,
		[Description("平衡")] ThreeGcd,
	}

	[RotationConfig(CombatType.PvE, Name = "用日耀/方面日耀自动升级地宫占卜")]
	public bool AutoUpgradeHoroscope { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "启用即刻咏唱限制逻辑：拥有即刻咏唱时尝试阻止除复活外的其他行为")]
	public bool SwiftLogic { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "移动时使用光速（仍为爆发保留）")]
	public bool UseLightspeedForMovement { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "移动时使用即刻咏唱")]
	public bool UseSwiftcastForMovement { get; set; } = true;

	[Range(0, 5, ConfigUnitType.Seconds, 0.1f)]
	[RotationConfig(CombatType.PvE, Name = "允许移动类操作前的最小移动时间")]
	public float MovementTimeThreshold { get; set; } = 0.9f;

	[RotationConfig(CombatType.PvE, Name = "使用 GCD 治疗。（如果你是队伍中唯一的治疗则忽略）")]
	public bool GCDHeal { get; set; } = true;

	[RotationConfig(CombatType.PvE, Name = "可用时优先使用小宇宙而非所有其他治疗")]
	public bool MicroPrio { get; set; } = false;

	[Range(4, 20, ConfigUnitType.Seconds)]
	[RotationConfig(CombatType.PvE, Name = "倒计时期间使用地星。")]
	public float UseEarthlyStarTime { get; set; } = 4;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用方面占卜所需的队友最低 HP 阈值")]
	public float AspectedBeneficHeal { get; set; } = 0.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用星位合图所需的队友最低 HP 阈值")]
	public float SynastryHeal { get; set; } = 0.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "触发地宫占卜所需的队友最低 HP 阈值")]
	public float HoroscopeHeal { get; set; } = 0.6f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "触发小宇宙所需的队友最低 HP 阈值")]
	public float MicrocosmosHeal { get; set; } = 0.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "引爆地星所需的队友最低平均 HP 阈值（处于巨星支配时）")]
	public float StellarDetonationHeal { get; set; } = 0.7f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用天星冲日所需的队友最低平均 HP 阈值（仅在未持有巨星支配时）")]
	public float CelestialOppositionHeal { get; set; } = 0.7f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用王冠之贵妇所需的队友最低平均 HP 阈值")]
	public float LadyOfHeals { get; set; } = 0.8f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用先天禀赋第 3 层充能所需的队友最低 HP 阈值")]
	public float EssentialDignityThird { get; set; } = 0.7f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用先天禀赋第 2 层充能所需的队友最低 HP 阈值")]
	public float EssentialDignitySecond { get; set; } = 0.5f;

	[Range(0, 1, ConfigUnitType.Percent)]
	[RotationConfig(CombatType.PvE, Name = "使用先天禀赋最后一层充能所需的队友最低 HP 阈值")]
	public float EssentialDignityLast { get; set; } = 0.3f;

	[RotationConfig(CombatType.PvE, Name = "可用时优先使用先天禀赋而非单体 GCD 治疗")]
	public EssentialPrioStrategy EssentialPrio2 { get; set; } = EssentialPrioStrategy.AnyCharges;

	public enum EssentialPrioStrategy : byte
	{
		[Description("忽略设置")]
		UseGCDs,

		[Description("满层时")]
		CappedCharges,

		[Description("任意层数")]
		AnyCharges,
	}

	[RotationConfig(CombatType.PvE, Name = "提前移动刷新燃烧")]
	public MovingCombustRefreshOption MovingCombustRefresh { get; set; } = MovingCombustRefreshOption.Disable;

	public enum MovingCombustRefreshOption : byte
	{
		[Description("禁用")] Disable,
		[Description("剩余 6 秒")] Six,
		[Description("剩余 9 秒")] Nine,
		[Description("剩余 12 秒")] Twelve,
	}

	#endregion

	#region Constants / Fields

	private const long NeutralSectEarlyMs = 15_000;
	private const long DivinationFirst5sMs = 5_000;
	private const float BallparkPercent = 0.08f;
	private const float DivinationCombustRefreshSeconds = 11f;
	private const float MovementLeadSeconds = 0.5f;
	private const float LightspeedLocksSwiftcastSeconds = 18f;
	private const float SwiftcastLocksLightspeedSeconds = 3f;
	private const float MovementPreferredActionLockSeconds = 2f;

	private long _neutralSectUsedAtMs;
	private bool _neutralSectWasUp;
	private long _divinationUsedAtMs;

	private float _lastLightspeedUseCombatTime = float.MinValue;
	private float _lastSwiftcastUseCombatTime = float.MinValue;
	private float _lastMovementPreferredLockingActionCombatTime = float.MinValue;

	private bool CardsUnderDivinationOnly { get; set; } = true;

	#endregion

	#region Simple Properties

	private float OpenWindowSeconds => OpenWindow switch
	{
		OpenWindowGcd.ZeroGcd => 0f,
		OpenWindowGcd.OneGcd => 2.2f,
		OpenWindowGcd.TwoGcd => 5.1f,
		OpenWindowGcd.ThreeGcd => 7.2f,
		_ => 5.5f,
	};

	private bool IsOpen => InCombat && CombatTime < OpenWindowSeconds;

	private bool InFirst15sAfterNeutralSect =>
		_neutralSectUsedAtMs != 0 &&
		Environment.TickCount64 - _neutralSectUsedAtMs <= NeutralSectEarlyMs;

	private bool InFirst5sAfterDivination =>
		_divinationUsedAtMs != 0 &&
		Environment.TickCount64 - _divinationUsedAtMs < DivinationFirst5sMs;

	private bool OracleGatedByDivination => InFirst5sAfterDivination;

	private bool HasHeliosConjunction => StatusHelper.PlayerHasStatus(true, StatusID.HeliosConjunction);
	private bool HasAspectedHelios => StatusHelper.PlayerHasStatus(true, StatusID.AspectedHelios);
	private bool HasDivining => StatusHelper.PlayerHasStatus(true, StatusID.Divining);
	private bool HasHoroscopeHelios => StatusHelper.PlayerHasStatus(true, StatusID.HoroscopeHelios);
	private bool HasHoroscope => StatusHelper.PlayerHasStatus(true, StatusID.Horoscope);

	private bool HasHealingLockout => HasMacrocosmos || HasGiantDominance || HasEarthlyDominance;

	private bool HasSufficientMovement =>
		IsMoving &&
		MovingTime > MovementTimeThreshold;

	private bool IsLightspeedLockingSwiftcastActive =>
		InCombat &&
		_lastLightspeedUseCombatTime > float.MinValue / 2 &&
		CombatTime - _lastLightspeedUseCombatTime <= LightspeedLocksSwiftcastSeconds;

	private bool IsSwiftcastLockingLightspeedActive =>
		InCombat &&
		_lastSwiftcastUseCombatTime > float.MinValue / 2 &&
		CombatTime - _lastSwiftcastUseCombatTime <= SwiftcastLocksLightspeedSeconds;

	private bool IsMovementPreferredActionLockActive =>
		InCombat &&
		_lastMovementPreferredLockingActionCombatTime > float.MinValue / 2 &&
		CombatTime - _lastMovementPreferredLockingActionCombatTime <= MovementPreferredActionLockSeconds;

	private bool ShouldHoldRaiseSwift =>
		(HasSwift || IsLastAction(ActionID.SwiftcastPvE)) &&
		SwiftLogic &&
		MergedStatus.HasFlag(AutoStatus.Raise);

	private float DivIn =>
		DivinationPvE.Cooldown.CurrentCharges >= 1
			? 0f
			: DivinationPvE.Cooldown.RecastTimeRemainOneCharge;

	private float LightspeedNextChargeIn =>
		LightspeedPvE.Cooldown.CurrentCharges >= LightspeedPvE.Cooldown.MaxCharges
			? 0f
			: LightspeedPvE.Cooldown.RecastTimeRemainOneCharge;

	private bool BurstPrep => DivinationPvE.EnoughLevel && DivIn <= 4f;

	private float MovingCombustRefreshSeconds => MovingCombustRefresh switch
	{
		MovingCombustRefreshOption.Disable => 0f,
		MovingCombustRefreshOption.Six => 6f,
		MovingCombustRefreshOption.Nine => 9f,
		MovingCombustRefreshOption.Twelve => 12f,
		_ => 0f,
	};

	private bool HoldLastLightspeedForDivination
	{
		get
		{
			if (!DivinationPvE.EnoughLevel)
				return false;

			if (DivIn > 60f)
				return false;

			if (BurstPrep)
				return false;

			if (LightspeedPvE.Cooldown.CurrentCharges != 1)
				return false;

			if (HasLightspeed)
				return false;

			float lsMustBeBackBy = MathF.Max(0f, DivIn - 4f);
			bool spendingLastLsIsSafe = LightspeedNextChargeIn <= lsMustBeBackBy;

			return !spendingLastLsIsSafe;
		}
	}

	#endregion

	#region Tracking / Helper Methods

	private void RefreshNeutralSectStamp()
	{
		bool isUpNow = HasNeutralSect;

		if (isUpNow && !_neutralSectWasUp)
			_neutralSectUsedAtMs = Environment.TickCount64;

		_neutralSectWasUp = isUpNow;

		if (!isUpNow)
			_neutralSectUsedAtMs = 0;
	}

	private void StampDivinationUse() => _divinationUsedAtMs = Environment.TickCount64;

	private void UpdateMovementCooldownTracking()
	{
		if (!InCombat)
		{
			_lastLightspeedUseCombatTime = float.MinValue;
			_lastSwiftcastUseCombatTime = float.MinValue;
			_lastMovementPreferredLockingActionCombatTime = float.MinValue;
			return;
		}

		if (IsLastAction(ActionID.LightspeedPvE))
			_lastLightspeedUseCombatTime = CombatTime;

		if (IsLastAction(ActionID.SwiftcastPvE))
			_lastSwiftcastUseCombatTime = CombatTime;

		if (IsLastAction(ActionID.AspectedBeneficPvE) ||
			IsLastAction(ActionID.MacrocosmosPvE) ||
			IsLastAction(ActionID.CombustPvE) ||
			IsLastAction(ActionID.CombustIiPvE) ||
			IsLastAction(ActionID.CombustIiiPvE))
		{
			_lastMovementPreferredLockingActionCombatTime = CombatTime;
		}
	}

	private int AliveHealerCount
	{
		get
		{
			int count = 0;
			IEnumerable<IBattleChara> healers = PartyMembers.GetJobCategory(JobRole.Healer);

			foreach (IBattleChara healer in healers)
			{
				if (!healer.IsDead)
					count++;
			}

			return count;
		}
	}

	private int AlivePartyMemberCount
	{
		get
		{
			int count = 0;
			foreach (IBattleChara member in PartyMembers)
			{
				if (!member.IsDead)
					count++;
			}

			return count;
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

	private static bool HasCelestialIntersection(IBattleChara? target)
	{
		if (target == null)
			return false;

		try
		{
			return target.HasStatus(true, StatusID.Intersection);
		}
		catch
		{
			return false;
		}
	}

	private static bool HasAspectedBeneficFromSelf(IBattleChara? target)
	{
		if (target == null)
			return false;

		try
		{
			return target.HasStatus(true, StatusID.AspectedBenefic);
		}
		catch
		{
			return false;
		}
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

	private bool CanUseCurrentCombust(out IAction? act, bool skipStatusProvideCheck = false)
	{
		if (CombustIiiPvE.EnoughLevel && CombustIiiPvE.CanUse(out act, skipStatusProvideCheck: skipStatusProvideCheck))
			return true;

		if (!CombustIiiPvE.EnoughLevel && CombustIiPvE.EnoughLevel && CombustIiPvE.CanUse(out act, skipStatusProvideCheck: skipStatusProvideCheck))
			return true;

		if (!CombustIiPvE.EnoughLevel && CombustPvE.EnoughLevel && CombustPvE.CanUse(out act, skipStatusProvideCheck: skipStatusProvideCheck))
			return true;

		act = null;
		return false;
	}

	private bool CanUseCurrentGravity(out IAction? act)
	{
		if (GravityIiPvE.EnoughLevel && GravityIiPvE.CanUse(out act))
			return true;

		if (!GravityIiPvE.EnoughLevel && GravityPvE.EnoughLevel && GravityPvE.CanUse(out act))
			return true;

		act = null;
		return false;
	}

	private bool CanUseCurrentMalefic(out IAction? act)
	{
		if (FallMaleficPvE.EnoughLevel && FallMaleficPvE.CanUse(out act))
			return true;

		if (!FallMaleficPvE.EnoughLevel && MaleficIvPvE.EnoughLevel && MaleficIvPvE.CanUse(out act))
			return true;

		if (!MaleficIvPvE.EnoughLevel && MaleficIiiPvE.EnoughLevel && MaleficIiiPvE.CanUse(out act))
			return true;

		if (!MaleficIiiPvE.EnoughLevel && MaleficIiPvE.EnoughLevel && MaleficIiPvE.CanUse(out act))
			return true;

		if (!MaleficIiPvE.Info.EnoughLevelAndQuest() && MaleficPvE.CanUse(out act))
			return true;

		act = null;
		return false;
	}

	private bool CurrentTargetCombustMissingOrEnding(float remainingSeconds)
	{
		if (CurrentTarget == null)
			return false;

		return
			(CombustIiiPvE.EnoughLevel &&
				(!(CurrentTarget.HasStatus(true, StatusID.CombustIii)) ||
				 CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.CombustIii)))
			||
			(!CombustIiiPvE.EnoughLevel && CombustIiPvE.EnoughLevel &&
				(!(CurrentTarget.HasStatus(true, StatusID.CombustIi)) ||
				 CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.CombustIi)))
			||
			(!CombustIiiPvE.EnoughLevel && !CombustIiPvE.EnoughLevel && CombustPvE.EnoughLevel &&
				(!(CurrentTarget.HasStatus(true, StatusID.Combust)) ||
				 CurrentTarget.WillStatusEnd(remainingSeconds, true, StatusID.Combust)));
	}

	private float GetExpectedHpToLive12Seconds()
	{
		if (Player == null)
			return 0f;

		int partyCount = Math.Max(1, AlivePartyMemberCount);
		return BallparkPercent * Player.MaxHp * partyCount * 12f;
	}

	private bool CurrentTargetHasEnoughHpForCombust(float expectedHpToLive12Seconds)
	{
		return CurrentTarget != null &&
			   CurrentTarget.CurrentHp >= expectedHpToLive12Seconds;
	}

	private bool CanCastTankSynastry(IBaseAction actionCheck, IAction next)
	{
		if (!next.IsTheSameTo(false, actionCheck))
			return false;

		IBattleChara? target = actionCheck.Target.Target;
		IBattleChara? synastryTarget = SynastryPvE.Target.Target;

		if (target == null || synastryTarget == null)
			return false;

		if (target != synastryTarget)
			return false;

		if (!IsTank(target))
			return false;

		if (HasSingleHealLockoutStatus(target))
			return false;

		return target.GetHealthRatio() < SynastryHeal;
	}

	private bool IsMovementPreferredNextGCD(IAction nextGCD)
	{
		if (nextGCD.IsTheSameTo(false, AspectedBeneficPvE, MacrocosmosPvE))
			return true;

		if (CanUseCurrentCombust(out IAction? combustAct) &&
			nextGCD == combustAct)
		{
			return true;
		}

		return false;
	}

	private bool ShouldUseSwiftcastForMovement(IAction nextGCD)
	{
		if (!UseSwiftcastForMovement ||
			!InCombat ||
			!HasSufficientMovement ||
			MovingTime <= MovementTimeThreshold + MovementLeadSeconds ||
			HasSwift ||
			HasLightspeed ||
			IsLastAction(ActionID.SwiftcastPvE) ||
			ShouldHoldRaiseSwift ||
			IsLightspeedLockingSwiftcastActive ||
			IsMovementPreferredActionLockActive)
		{
			return false;
		}

		return !IsMovementPreferredNextGCD(nextGCD);
	}

	private bool ShouldUseLightspeedForMovement(IAction nextGCD)
	{
		if (!UseLightspeedForMovement ||
			!InCombat ||
			!HasSufficientMovement ||
			MovingTime <= MovementTimeThreshold + MovementLeadSeconds ||
			HasLightspeed ||
			HasSwift ||
			IsLastAction(ActionID.LightspeedPvE) ||
			HoldLastLightspeedForDivination ||
			IsSwiftcastLockingLightspeedActive ||
			IsMovementPreferredActionLockActive)
		{
			return false;
		}

		return !IsMovementPreferredNextGCD(nextGCD);
	}

	#endregion

	#region Tracking Properties

	public override void DisplayRotationStatus()
	{
		ImGui.Text($"Suntouched 1: {StatusHelper.PlayerWillStatusEndGCD(1, 0, true, StatusID.Suntouched)}");
		ImGui.Text($"Suntouched 2: {StatusHelper.PlayerWillStatusEndGCD(2, 0, true, StatusID.Suntouched)}");
		ImGui.Text($"Suntouched 3: {StatusHelper.PlayerWillStatusEndGCD(3, 0, true, StatusID.Suntouched)}");
		ImGui.Text($"Suntouched 4: {StatusHelper.PlayerWillStatusEndGCD(4, 0, true, StatusID.Suntouched)}");
		ImGui.Text($"Suntouched Time: {StatusHelper.PlayerStatusTime(true, StatusID.Suntouched)}");
	}

	#endregion

	#region Countdown Logic

	protected override IAction? CountDownAction(float remainTime)
	{
		if (remainTime < MaleficPvE.Info.CastTime + CountDownAhead && MaleficPvE.CanUse(out IAction? act))
			return act;

		if (remainTime < 3 && UseBurstMedicine(out act))
			return act;

		if (remainTime < UseEarthlyStarTime && EarthlyStarPvE.CanUse(out act, skipTTKCheck: true))
			return act;

		return remainTime < 30 && AstralDrawPvE.CanUse(out act)
			? act
			: base.CountDownAction(remainTime);
	}

	#endregion

	#region oGCD Logic

	protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
	{
		UpdateMovementCooldownTracking();

		act = null;

		if (!HasLightspeed &&
			InCombat &&
			IsBurst &&
			IsOpen &&
			LightspeedPvE.CanUse(out act, usedUp: true))
		{
			return true;
		}

		if (MicroPrio && HasMacrocosmos)
			return base.EmergencyAbility(nextGCD, out act);

		if (!InCombat)
			return base.EmergencyAbility(nextGCD, out act);

		if (SynastryPvE.CanUse(out act, targetOverride: TargetType.Tank))
		{
			if (CanCastTankSynastry(AspectedBeneficPvE, nextGCD) ||
				CanCastTankSynastry(BeneficIiPvE, nextGCD) ||
				CanCastTankSynastry(BeneficPvE, nextGCD))
			{
				return true;
			}
		}

		if (BurstPrep &&
			LightspeedPvE.Cooldown.CurrentCharges >= 1 &&
			!HasLightspeed &&
			InCombat &&
			IsBurst &&
			LightspeedPvE.CanUse(out act, usedUp: true))
		{
			return true;
		}

		if (!IsOpen &&
			InCombat &&
			IsBurst &&
			BurstPrep &&
			UseBurstMedicine(out act))
		{
			return true;
		}

		if (!IsOpen && IsBurst && InCombat && DivinationPvE.CanUse(out act))
		{
			StampDivinationUse();
			return true;
		}

		if (!IsOpen && DivinationPvE.CanUse(out _) && UseBurstMedicine(out act))
			return true;

		return base.EmergencyAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.ExaltationPvE, ActionID.TheArrowPvE, ActionID.TheSpirePvE, ActionID.TheBolePvE, ActionID.TheEwerPvE, ActionID.CelestialIntersectionPvE)]
	protected override bool DefenseSingleAbility(IAction nextGCD, out IAction? act)
	{
		UpdateMovementCooldownTracking();

		if (!HasDivining && InCombat && TheSpirePvE.CanUse(out act))
			return true;

		if (!HasDivining && InCombat && TheBolePvE.CanUse(out act))
			return true;

		if (ExaltationPvE.CanUse(out act))
			return true;

		if (CelestialIntersectionPvE.Target.Target != null &&
			CelestialIntersectionPvE.Cooldown.CurrentCharges == 1 &&
			CelestialIntersectionPvE.CanUse(out act, usedUp: true, targetOverride: TargetType.Tank) &&
			!HasCelestialIntersection(CelestialIntersectionPvE.Target.Target) &&
			!HasSingleHealLockoutStatus(CelestialIntersectionPvE.Target.Target))
		{
			return true;
		}

		return base.DefenseSingleAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.SunSignPvE)]
	protected override bool DefenseAreaAbility(IAction nextGCD, out IAction? act)
	{
		UpdateMovementCooldownTracking();

		if (SunSignPvE.CanUse(out act))
			return true;

		return base.DefenseAreaAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.CelestialOppositionPvE, ActionID.StellarDetonationPvE, ActionID.HoroscopePvE, ActionID.HoroscopePvE_16558, ActionID.LadyOfCrownsPvE)]
	protected override bool HealAreaAbility(IAction nextGCD, out IAction? act)
	{
		UpdateMovementCooldownTracking();

		act = null;

		if (HasDivining || HasMacrocosmos)
			return false;

		if (HasGiantDominance &&
			PartyMembersAverHP < StellarDetonationHeal &&
			StellarDetonationPvE.CanUse(out act))
		{
			return true;
		}

		if (PartyMembersAverHP < MicrocosmosHeal && MicrocosmosPvE.CanUse(out act))
			return true;

		if (MicroPrio && HasMacrocosmos)
			return base.HealAreaAbility(nextGCD, out act);

		if (!HasGiantDominance &&
			!HasEarthlyDominance &&
			!HasMacrocosmos &&
			!HasHoroscopeHelios &&
			PartyMembersAverHP < CelestialOppositionHeal &&
			CelestialOppositionPvE.CanUse(out act))
		{
			return true;
		}

		if (!HasMacrocosmos &&
			!HasGiantDominance &&
			!HasEarthlyDominance &&
			!HasHoroscope &&
			HasHoroscopeHelios &&
			PartyMembersAverHP < HoroscopeHeal &&
			HoroscopePvE_16558.CanUse(out act))
		{
			return true;
		}

		if (!HasMacrocosmos &&
			!HasGiantDominance &&
			!HasEarthlyDominance &&
			!HasHoroscope &&
			HasHoroscopeHelios &&
			PartyMembersAverHP < HoroscopeHeal &&
			HoroscopePvE.CanUse(out act))
		{
			return true;
		}

		if (LadyOfCrownsPvE.CanUse(out act))
			return true;

		return base.HealAreaAbility(nextGCD, out act);
	}

	[RotationDesc(ActionID.TheArrowPvE, ActionID.TheEwerPvE, ActionID.EssentialDignityPvE, ActionID.CelestialIntersectionPvE)]
	protected override bool HealSingleAbility(IAction nextGCD, out IAction? act)
	{
		UpdateMovementCooldownTracking();

		act = null;

		if (HasDivining || HasHealingLockout)
			return false;

		if (MicroPrio && HasMacrocosmos)
			return base.HealSingleAbility(nextGCD, out act);

		if (!IsOpen && InCombat && TheArrowPvE.CanUse(out act))
			return true;

		if (InCombat &&
			TheEwerPvE.CanUse(out act) &&
			TheEwerPvE.Target.Target != null &&
			!HasSingleHealLockoutStatus(TheEwerPvE.Target.Target) &&
			TheEwerPvE.Target.Target.GetHealthRatio() < 0.8f)
		{
			return true;
		}

		if (!HasGiantDominance &&
			!HasEarthlyDominance &&
			!HasMacrocosmos &&
			EssentialDignityPvE.Cooldown.CurrentCharges == 3 &&
			EssentialDignityPvE.CanUse(out act, usedUp: true) &&
			EssentialDignityPvE.Target.Target != null &&
			!HasSingleHealLockoutStatus(EssentialDignityPvE.Target.Target) &&
			(IsTank(EssentialDignityPvE.Target.Target) || PartyMembersAverHP > 0.8f) &&
			EssentialDignityPvE.Target.Target.GetHealthRatio() < EssentialDignityThird)
		{
			return true;
		}

		if (!HasGiantDominance &&
			!HasEarthlyDominance &&
			!HasMacrocosmos &&
			EssentialDignityPvE.Cooldown.CurrentCharges == 2 &&
			EssentialDignityPvE.CanUse(out act, usedUp: true) &&
			EssentialDignityPvE.Target.Target != null &&
			!HasSingleHealLockoutStatus(EssentialDignityPvE.Target.Target) &&
			(IsTank(EssentialDignityPvE.Target.Target) || PartyMembersAverHP > 0.8f) &&
			EssentialDignityPvE.Target.Target.GetHealthRatio() < EssentialDignitySecond)
		{
			return true;
		}

		if (!HasGiantDominance &&
			!HasEarthlyDominance &&
			!HasMacrocosmos &&
			EssentialDignityPvE.Cooldown.CurrentCharges == 1 &&
			EssentialDignityPvE.CanUse(out act, usedUp: true, targetOverride: TargetType.Tank) &&
			EssentialDignityPvE.Target.Target != null &&
			!HasSingleHealLockoutStatus(EssentialDignityPvE.Target.Target) &&
			EssentialDignityPvE.Target.Target.GetHealthRatio() < EssentialDignityLast)
		{
			return true;
		}

		if (CelestialIntersectionPvE.Target.Target != null &&
			CelestialIntersectionPvE.Cooldown.CurrentCharges == 2 &&
			CelestialIntersectionPvE.CanUse(out act, usedUp: true) &&
			!HasCelestialIntersection(CelestialIntersectionPvE.Target.Target) &&
			!HasSingleHealLockoutStatus(CelestialIntersectionPvE.Target.Target) &&
			PartyMembersAverHP > 0.8f &&
			CelestialIntersectionPvE.Target.Target.GetHealthRatio() < 0.9f)
		{
			return true;
		}

		if (CelestialIntersectionPvE.Target.Target != null &&
			CelestialIntersectionPvE.Cooldown.CurrentCharges == 1 &&
			CelestialIntersectionPvE.CanUse(out act, usedUp: true, targetOverride: TargetType.Tank) &&
			!HasCelestialIntersection(CelestialIntersectionPvE.Target.Target) &&
			!HasSingleHealLockoutStatus(CelestialIntersectionPvE.Target.Target) &&
			CelestialIntersectionPvE.Target.Target.GetHealthRatio() < 0.7f)
		{
			return true;
		}

		return base.HealSingleAbility(nextGCD, out act);
	}

	protected override bool GeneralAbility(IAction nextGCD, out IAction? act)
	{
		UpdateMovementCooldownTracking();

		act = null;

		if (!HasLightspeed &&
			InCombat &&
			IsOpen &&
			LightspeedPvE.CanUse(out act, usedUp: true))
		{
			return true;
		}

		if (StatusHelper.PlayerHasStatus(true, StatusID.Suntouched) &&
			StatusHelper.PlayerWillStatusEndGCD(3, 0, true, StatusID.Suntouched))
		{
			if (SunSignPvE.CanUse(out act, skipAoeCheck: true, skipTTKCheck: true))
				return true;
		}

		if (PartyMembersAverHP < LadyOfHeals && LadyOfCrownsPvE.CanUse(out act))
			return true;

		if (AstralDrawPvE.Cooldown.WillHaveOneCharge(5) && LadyOfCrownsPvE.CanUse(out act))
			return true;

		if (AstralDrawPvE.CanUse(out act))
			return true;

		bool divLearned = DivinationPvE.EnoughLevel;
		bool burstCardsAllowed =
			CardsUnderDivinationOnly
				? (!divLearned || HasDivination)
				: (HasDivination || !DivinationPvE.Cooldown.WillHaveOneCharge(66) || !divLearned);

		if (burstCardsAllowed && InCombat && TheBalancePvE.CanUse(out act))
			return true;

		if (!IsOpen && InCombat && LordOfCrownsPvE.CanUse(out act))
		{
			if (CardsUnderDivinationOnly)
			{
				if (!divLearned || HasDivination)
					return true;
			}
			else
			{
				if ((divLearned && HasDivination) ||
					!divLearned ||
					(divLearned && !DivinationPvE.Cooldown.WillHaveOneCharge(60)) ||
					UmbralDrawPvE.Cooldown.WillHaveOneCharge(3))
				{
					return true;
				}
			}
		}

		bool hasBurstCardToPlay =
			InCombat &&
			burstCardsAllowed &&
			(TheBalancePvE.CanUse(out _) || TheSpearPvE.CanUse(out _));

		bool hasLordToSpend =
			InCombat &&
			LordOfCrownsPvE.CanUse(out _);

		if (UmbralDrawPvE.CanUse(out act) && !(hasBurstCardToPlay && hasLordToSpend))
			return true;

		if (burstCardsAllowed && InCombat && TheSpearPvE.CanUse(out act))
			return true;

		if (InCombat && !OracleGatedByDivination && OraclePvE.CanUse(out act))
			return true;

		if (!HasDivining && AstralDrawPvE.Cooldown.WillHaveOneCharge(10) && InCombat && TheEwerPvE.CanUse(out act))
			return true;

		if (!HasDivining && AstralDrawPvE.Cooldown.WillHaveOneCharge(10) && InCombat && TheBolePvE.CanUse(out act))
			return true;

		if (!HasDivining && UmbralDrawPvE.Cooldown.WillHaveOneCharge(10) && InCombat && TheArrowPvE.CanUse(out act))
			return true;

		if (UmbralDrawPvE.Cooldown.WillHaveOneCharge(10) && InCombat && TheSpirePvE.CanUse(out act))
			return true;

		return base.GeneralAbility(nextGCD, out act);
	}

	protected override bool AttackAbility(IAction nextGCD, out IAction? act)
	{
		UpdateMovementCooldownTracking();

		act = null;

		bool combustSoonForMovement =
			CurrentTarget != null &&
			MovingCombustRefreshSeconds > 0f &&
			CurrentTargetCombustMissingOrEnding(MovingCombustRefreshSeconds);

		if (!HasLightspeed &&
			InCombat &&
			IsBurst &&
			IsOpen &&
			LightspeedPvE.CanUse(out act, usedUp: true))
		{
			return true;
		}

		if (!IsOpen && IsBurst && InCombat && DivinationPvE.CanUse(out act))
		{
			StampDivinationUse();
			return true;
		}

		bool openerLightspeed = IsOpen && !HasLightspeed;
		if (openerLightspeed && LightspeedPvE.CanUse(out act, usedUp: true))
			return true;

		if (AstralDrawPvE.CanUse(out act, usedUp: IsBurst))
			return true;

		if (!HasLightspeed &&
			InCombat &&
			HasDivination &&
			InFirst5sAfterDivination &&
			!HoldLastLightspeedForDivination &&
			LightspeedPvE.CanUse(out act, usedUp: true))
		{
			return true;
		}

		if (InCombat)
		{
			if (ShouldUseSwiftcastForMovement(nextGCD) &&
				!combustSoonForMovement &&
				SwiftcastPvE.CanUse(out act))
			{
				return true;
			}

			if (ShouldUseLightspeedForMovement(nextGCD) &&
				!combustSoonForMovement &&
				LightspeedPvE.CanUse(out act, usedUp: LightspeedPvE.Cooldown.CurrentCharges > 1))
			{
				return true;
			}

			if (!HasGiantDominance && !HasEarthlyDominance && EarthlyStarPvE.CanUse(out act))
				return true;
		}

		return base.AttackAbility(nextGCD, out act);
	}

	#endregion

	#region GCD Logic

	[RotationDesc(ActionID.AspectedHeliosPvE, ActionID.HeliosPvE, ActionID.HeliosConjunctionPvE)]
	protected override bool HealAreaGCD(out IAction? act)
	{
		UpdateMovementCooldownTracking();

		act = null;

		if (HasMacrocosmos ||
			HasGiantDominance ||
			HasEarthlyDominance ||
			HasHoroscopeHelios ||
			(CelestialOppositionPvE.Cooldown.IsCoolingDown && !CelestialOppositionPvE.Cooldown.WillHaveOneCharge(60)))
		{
			return false;
		}

		if (ShouldHoldRaiseSwift)
			return base.HealAreaGCD(out act);

		if (MicroPrio && HasMacrocosmos)
			return base.HealAreaGCD(out act);

		if (CelestialOppositionPvE.Cooldown.IsCoolingDown &&
			!CelestialOppositionPvE.Cooldown.WillHaveOneCharge(60) &&
			!HasDivination &&
			!HasHeliosConjunction &&
			PartyMembersAverHP < 0.6f &&
			HeliosConjunctionPvE.EnoughLevel &&
			HeliosConjunctionPvE.CanUse(out act))
		{
			return true;
		}

		if (CelestialOppositionPvE.Cooldown.IsCoolingDown &&
			!CelestialOppositionPvE.Cooldown.WillHaveOneCharge(60) &&
			!HasMacrocosmos &&
			!HasGiantDominance &&
			!HasDivination &&
			!HasAspectedHelios &&
			PartyMembersAverHP < 0.6f &&
			!HeliosConjunctionPvE.EnoughLevel &&
			AspectedHeliosPvE.CanUse(out act))
		{
			return true;
		}

		if (CelestialOppositionPvE.Cooldown.IsCoolingDown &&
			!CelestialOppositionPvE.Cooldown.WillHaveOneCharge(60) &&
			!HasMacrocosmos &&
			!HasGiantDominance &&
			!HasDivination &&
			(HasHeliosConjunction || HasAspectedHelios) &&
			PartyMembersAverHP < 0.4f &&
			HeliosPvE.CanUse(out act))
		{
			return true;
		}

		return base.HealAreaGCD(out act);
	}

	[RotationDesc(ActionID.AspectedBeneficPvE, ActionID.BeneficIiPvE)]
	protected override bool HealSingleGCD(out IAction? act)
	{
		UpdateMovementCooldownTracking();

		act = null;

		if (HasHealingLockout)
			return false;

		if (ShouldHoldRaiseSwift)
			return base.HealSingleGCD(out act);

		if (MicroPrio && HasMacrocosmos)
			return base.HealSingleGCD(out act);

		bool shouldUseEssentialDignity =
			(EssentialPrio2 == EssentialPrioStrategy.AnyCharges &&
			 EssentialDignityPvE.EnoughLevel &&
			 EssentialDignityPvE.Cooldown.CurrentCharges > 0)
			||
			(EssentialPrio2 == EssentialPrioStrategy.CappedCharges &&
			 EssentialDignityPvE.EnoughLevel &&
			 EssentialDignityPvE.Cooldown.CurrentCharges == EssentialDignityPvE.Cooldown.MaxCharges);

		if (shouldUseEssentialDignity)
			return base.HealSingleGCD(out act);

		bool movingHealWindow =
			InCombat &&
			!HoldLastLightspeedForDivination &&
			!HasLightspeed &&
			!HasSwift &&
			HasSufficientMovement &&
			AspectedBeneficPvE.Target.Target != null &&
			!HasAspectedBeneficFromSelf(AspectedBeneficPvE.Target.Target) &&
			!HasSingleHealLockoutStatus(AspectedBeneficPvE.Target.Target) &&
			(IsTank(AspectedBeneficPvE.Target.Target) || PartyMembersAverHP > 0.8f) &&
			AspectedBeneficPvE.Target.Target.GetHealthRatio() < 0.7f;

		if (AspectedBeneficPvE.CanUse(out act) &&
			AspectedBeneficPvE.Target.Target != null &&
			!HasAspectedBeneficFromSelf(AspectedBeneficPvE.Target.Target) &&
			!HasSingleHealLockoutStatus(AspectedBeneficPvE.Target.Target) &&
			(((AspectedBeneficPvE.Target.Target.GetHealthRatio() < AspectedBeneficHeal) &&
			  (IsTank(AspectedBeneficPvE.Target.Target) || PartyMembersAverHP > 0.8f)) || movingHealWindow) &&
			!HasMacrocosmos &&
			!HasGiantDominance &&
			!HasDivination)
		{
			return true;
		}

		if (BeneficIiPvE.CanUse(out act, targetOverride: TargetType.Tank) &&
			BeneficIiPvE.Target.Target != null &&
			PartyMembersAverHP > 0.9f &&
			!HasSingleHealLockoutStatus(BeneficIiPvE.Target.Target) &&
			!HasMacrocosmos &&
			!HasGiantDominance &&
			!HasDivination)
		{
			return true;
		}

		return base.HealSingleGCD(out act);
	}

	[RotationDesc(ActionID.AscendPvE)]
	protected override bool RaiseGCD(out IAction? act)
	{
		UpdateMovementCooldownTracking();

		if (AscendPvE.CanUse(out act))
			return true;

		return base.RaiseGCD(out act);
	}

	protected override bool GeneralGCD(out IAction? act)
	{
		UpdateMovementCooldownTracking();

		act = null;
		RefreshNeutralSectStamp();

		if (ShouldHoldRaiseSwift)
			return base.GeneralGCD(out act);

		if (AutoUpgradeHoroscope &&
			((HasHoroscope && !HasHoroscopeHelios) ||
			 (InFirst15sAfterNeutralSect && !HasHeliosConjunction && !HasAspectedHelios && !HasDivination)))
		{
			if (HeliosConjunctionPvE.EnoughLevel &&
				HeliosConjunctionPvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}

			if (!HeliosConjunctionPvE.EnoughLevel &&
				AspectedHeliosPvE.CanUse(out act, skipStatusProvideCheck: true))
			{
				return true;
			}
		}

		if (CanUseCurrentGravity(out act))
			return true;

		float expectedHPToLive12Seconds = GetExpectedHpToLive12Seconds();

		if (InCombat &&
			HasSufficientMovement &&
			MovingCombustRefreshSeconds > 0f &&
			CurrentTargetCombustMissingOrEnding(MovingCombustRefreshSeconds) &&
			CurrentTargetHasEnoughHpForCombust(expectedHPToLive12Seconds) &&
			CanUseCurrentCombust(out act, skipStatusProvideCheck: true))
		{
			return true;
		}

		if (HasDivination &&
			InCombat &&
			StatusHelper.PlayerStatusTime(true, StatusID.Divination) <= 5f &&
			CurrentTargetCombustMissingOrEnding(DivinationCombustRefreshSeconds) &&
			CurrentTargetHasEnoughHpForCombust(expectedHPToLive12Seconds) &&
			CanUseCurrentCombust(out act, skipStatusProvideCheck: true))
		{
			return true;
		}

		if (CurrentTargetHasEnoughHpForCombust(expectedHPToLive12Seconds) &&
			CanUseCurrentCombust(out act))
		{
			return true;
		}

		if (CanUseCurrentMalefic(out act))
			return true;

		return base.GeneralGCD(out act);
	}

	#endregion

	#region Extra Methods

	public override bool CanHealSingleSpell =>
		base.CanHealSingleSpell &&
		(GCDHeal || AliveHealerCount == 1);

	public override bool CanHealAreaSpell =>
		base.CanHealAreaSpell &&
		(GCDHeal || AliveHealerCount == 1);

	#endregion
}