using Dalamud.Configuration;
using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.Logging;
using System.Collections.Concurrent;
using static RotationSolver.Basic.Configuration.ConfigTypes;

namespace RotationSolver.Basic.Configuration;

internal partial class Configs : IPluginConfiguration
{
	[JsonIgnore]
	public const string
		BasicTimer = "BasicTimer",
		BasicAutoSwitch = "BasicAutoSwitch",
		DutySpecificCrucibleOfTheUnbroken = "DutySpecificCrucibleOfTheUnbroken",
		DutySpecificTheMaskedCarnivale = "DutySpecificTheMaskedCarnivale",
		DutySpecificPvP = "DutySpecificPvP",
		DutySpecificFieldOps = "DutySpecificFieldOps",
		DutySpecificAlliance = "DutySpecificAlliance",
		DutySpecificDeepDungeon = "DutySpecificDeepDungeon",
		DutySpecificVariantDungeon = "DutySpecificVariantDungeon",
		DutySpecificTreasureDungeon = "DutySpecificTreasureDungeon",
		DutySpecificChaoticAlliance = "DutySpecificChaoticAlliance",
		DutySpecificDungeon = "DutySpecificDungeon",
		DutySpecificUltimate = "DutySpecificUltimate",
		DutySpecificExtreme = "DutySpecificExtreme",
		DutySpecificSavage = "DutySpecificSavage",
		BasicParams = "BasicParams",
		UiInformation = "UiInformation",
		UiWindows = "UiWindows",
		PvPSpecificControls = "PvPSpecificControls",
		AutoActionUsage = "AutoActionUsage",
		HealingActionCondition = "HealingActionCondition",
		PhantomDutyRotationConfiguration = "PhantomDutyRotationConfiguration",
		TargetConfig = "TargetConfig",
		Extra = "Extra",
		Rotations = "Rotations",
		List = "List",
		List2 = "List2",
		List3 = "List3",
		Debug = "Debug";

	public const int CurrentVersion = 12;
	public int Version { get; set; } = CurrentVersion;

	public string LastSeenChangelog { get; set; } = "0.0.0.0";
	public bool TutorialDone { get; set; } = false;

	public List<ActionEventInfo> Events { get; private set; } = [];
	public SortedSet<Job> DisabledJobs { get; private set; } = [];

	public string[] RotationLibs { get; set; } = [];
	public List<TargetingType> TargetingTypes { get; set; } = [];

	public MacroInfo DutyStart { get; set; } = new MacroInfo();
	public MacroInfo DutyEnd { get; set; } = new MacroInfo();

	#region Duty Specific
	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定扁平伤害/即死类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockflatdamagedeathimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定减速类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockslowimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定石化类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockpetrificationimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定麻痹类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockparalysisimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定打断类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockinterruptimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定失明类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockblindimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定眩晕类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockstunimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定催眠类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blocksleepimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定束缚类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockbindimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该效果的敌人使用特定迟缓类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockheavyimmuneBLU = false;

	[ConditionBool, UI("阻止对免疫该属性的敌人使用特定属性类技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blockimmuneBLU = true;

	[ConditionBool, UI("若敌人弱某属性，则只使用该属性的技能。", Filter = DutySpecificTheMaskedCarnivale)]
	private static readonly bool _blocknonweakBLU = false;

	[ConditionBool, UI("仍允许使用无属性技能。", Filter = DutySpecificTheMaskedCarnivale, Parent = nameof(BlocknonweakBlu))]
	private static readonly bool _allowunaspectedBLU = true;

	[ConditionBool, UI("O12S - 数据包过滤逻辑。",
		Description = "若你拥有对应的数据包过滤状态，则将 OmegaM/OmegaF 视为免疫（普通版同样适用）。",
		Filter = DutySpecificSavage)]
	private static readonly bool _o12sOmegaMF = true;

	[ConditionBool, UI("M8S - 狼群/石群逻辑。",
		Description = "若你没有对应状态，则将风之狼/石之狼视为免疫。",
		Filter = DutySpecificSavage)]
	private static readonly bool _m8sWindStone = true;

	[ConditionBool, UI("M9S - 仅使用顺劈逻辑。",
		Description = "当钉子、Boss 和链锤同时靠近时，此选项可解决选目标问题。",
		Filter = DutySpecificSavage)]
	private static readonly bool _m9sCleaveOnly = true;

	[ConditionBool, UI("M9S - 牢房选目标逻辑。",
		Description = "若你没有对应状态，则将牢房视为免疫。",
		Filter = DutySpecificSavage)]
	private static readonly bool _m9sCellTargeting = true;

	[ConditionBool, UI("M9S - 小怪选目标逻辑。",
		Description = "根据职业和与目标的距离，优先选择钉门或链锤。",
		Filter = DutySpecificSavage)]
	private static readonly bool _m9sAdsTargeting = true;

	[ConditionBool, UI("M10S - 火蛇/水蛇选目标逻辑。",
		Description = "若你拥有火蛇增益则优先红热，拥有水蛇增益则优先深蓝（普通版同样适用）。",
		Filter = DutySpecificSavage)]
	private static readonly bool _m10sBroTargeting = true;

	[ConditionBool, UI("无尽苍蓝极版 - 鲸背逻辑。",
		Description = "若你没有鲸背状态，则将碧诗玛克甲壳/碧诗玛克光环视为免疫",
		Filter = DutySpecificExtreme)]
	private static readonly bool _limitlessBlueTargeting = true;

	[ConditionBool, UI("漂移烽火极版 - Pall 选目标逻辑。",
		Description = "若你没有对应状态，则将愤怒之 Pall/悲伤之 Pall 视为免疫。",
		Filter = DutySpecificExtreme)]
	private static readonly bool _cinderDriftPallTargeting = true;

	[ConditionBool, UI("终末摧毁极版 - 暗影逻辑。",
		Description = "若你没有对应状态，则将暗影视为免疫",
		Filter = DutySpecificExtreme)]
	private static readonly bool _theUnmakingShadow = true;

	[ConditionBool, UI("亚历山大绝境战 - 狩猎人偶逻辑。",
		Description = "当 HP 低于 25% 时，将狩猎人偶小怪视为免疫。",
		Filter = DutySpecificUltimate)]
	private static readonly bool _teaJagdDoll = true;

	[ConditionBool, UI("亚历山大绝境战 - 真心逻辑。",
		Description = "将真心小怪视为免疫。",
		Filter = DutySpecificUltimate)]
	private static readonly bool _teaTrueHeart = true;

	[ConditionBool, UI("欧米茄绝境战 - 数据包过滤逻辑。",
		Description = "若你拥有对应的数据包过滤状态，则将 OmegaM/OmegaF 视为免疫。",
		Filter = DutySpecificUltimate)]
	private static readonly bool _topOmegaMF = true;

	[ConditionBool, UI("未来重现绝境战 - 暗黑水晶逻辑。",
		Description = "将暗黑水晶小怪视为免疫。",
		Filter = DutySpecificUltimate)]
	private static readonly bool _fruCrystalOfDarkness = true;

	[ConditionBool, UI("狂飙舞绝境战 - 英雄/反派逻辑。",
		Description = "若你没有对应状态，则将 Boss 视为免疫。",
		Filter = DutySpecificUltimate)]
	private static readonly bool _dmuBossImmune = true;

	[ConditionBool, UI("吉姆利特暗区 - 巨像 Rubricatus 小怪。",
		Description = "当巨像 Rubricatus 蓄力会导致其死亡的剧本技能时，将其视为免疫。",
		Filter = DutySpecificDungeon)]
	private static readonly bool _colossusRubricatusImmune = true;

	[ConditionBool, UI("多恩梅格 - 骗子竖琴机制。",
		Description = "若你没有「不被愚弄」状态，则将骗子竖琴视为免疫。",
		Filter = DutySpecificDungeon)]
	private static readonly bool _dohnMhegLyre = true;

	[ConditionBool, UI("美索终端 - 达纳托斯逻辑。",
		Description = "若你没有对应增益，则将第二个 Boss 战中的狱卒视为免疫。",
		Filter = DutySpecificDungeon)]
	private static readonly bool _jailerImmune = true;

	[ConditionBool, UI("叉塔 - 死星逻辑。",
		Description = "若你没有对应状态，则将 Triton/Nereid/Phobos 视为免疫。",
		Filter = DutySpecificFieldOps)]
	private static readonly bool _forkedtowerDeadStar = true;

	[ConditionBool, UI("巡礼之地 - 极悲逻辑。",
		Description = "若你没有光之复仇增益，则将极悲视为免疫；若你没有暗之复仇增益，则将吞噬食者视为免疫。",
		Filter = DutySpecificDeepDungeon)]
	private static readonly bool _eminent = true;

	[ConditionBool, UI("古之迷宫 - 达纳托斯逻辑。",
		Description = "若你没有星界对齐增益，则将达纳托斯视为免疫。",
		Filter = DutySpecificAlliance)]
	private static readonly bool _thanatosImmune = true;

	[ConditionBool, UI("虚无方舟 - 伊尔敏苏与锯齿逻辑。",
		Description = "若你没有对应增益，则将伊尔敏苏与锯齿视为免疫。",
		Filter = DutySpecificAlliance)]
	private static readonly bool _irminsulSawtoothImmune = true;

	[ConditionBool, UI("人偶军事基地 - 高级飞行单元逻辑。",
		Description = "若你没有对应增益，则将每个高级飞行单元视为免疫。",
		Filter = DutySpecificAlliance)]
	private static readonly bool _superiorFlightUnitImmune = true;

	[ConditionBool, UI("希望之炮台："塔" - 汉泽尔与格莱特逻辑。",
		Description = "当你所处的角度会因护盾机制受到反弹伤害时，将汉泽尔/格莱特视为免疫。",
		Filter = DutySpecificAlliance)]
	private static readonly bool _hanselorGretelShieldedImmune = true;

	[ConditionBool, UI("朱诺：第一巡行 - 阿克天使逻辑。",
		Description = "若你没有对应增益，则将每个阿克天使视为免疫。",
		Filter = DutySpecificAlliance)]
	private static readonly bool _jeunoBossImmune = true;

	[ConditionBool, UI("温达斯：第三巡行 - 复活的亚历山大逻辑。",
		Description = "若亚历山大和/或戈尔迪乌斯系统拥有完美防御，则将其视为免疫。",
		Filter = DutySpecificAlliance)]
	private static readonly bool _alexanderImmune = true;

	[ConditionBool, UI("暗黑之云 - 小怪阶段逻辑。",
		Description = "若你没有对应增益，则将暗黑之云/冥渊视为免疫。",
		Filter = DutySpecificChaoticAlliance)]
	private static readonly bool _codImmune = true;

	[ConditionBool, UI("希拉狄哈水道 - 龙族小怪逻辑。",
		Description = "为完成变体路径 12，自定义逻辑将特定龙类视为免疫并按特定顺序击杀。",
		Filter = DutySpecificVariantDungeon)]
	private static readonly bool _drakeImmune = true;

	[ConditionBool, UI("宝物迷宫 - 限时小怪逻辑。",
		Description = "优先攻击宝物迷宫中的限时小怪以完成额外战利品机制。",
		Filter = DutySpecificTreasureDungeon)]
	private static readonly bool _treasuredungeontimed = true;

	[ConditionBool, UI("宝物迷宫 - 编号小怪逻辑。",
		Description = "将带编号牌的小怪设为最高优先级，按编号从低到高顺序攻击以完成额外战利品机制。",
		Filter = DutySpecificTreasureDungeon)]
	private static readonly bool _treasuredungeonnumbered = true;
	#endregion

	#region PvP
	[JobConfig, UI("使用守护的 HP 阈值。",
		Filter = DutySpecificPvP)]
	[Range(0, 1, ConfigUnitType.Percent, 0.02f)]
	public float HealthForGuard { get; set; } = 0.15f;

	[ConditionBool, UI("PvP 中忽略 TTK。", Filter = DutySpecificPvP)]
	private static readonly bool _ignorePvPTTK = true;

	[ConditionBool, UI("碎冰战场中优先 A 级石碑。", Filter = DutySpecificPvP)]
	private static readonly bool _prioAtomelith = false;

	[ConditionBool, UI("碎冰战场中优先 B 级石碑。", Filter = DutySpecificPvP)]
	private static readonly bool _prioBtomelith = false;

	[JobConfig, UI("PvP 中忽略无敌状态。", Filter = DutySpecificPvP)]
	private static readonly bool _ignorePvPInvincibility = false;

	[ConditionBool, UI("PvP 中死亡时自动关闭。",
		 Filter = DutySpecificPvP)]
	private static readonly bool _autoOffWhenDeadPvP = true;

	[ConditionBool, UI("PvP 比赛结束时自动关闭。",
		 Filter = DutySpecificPvP)]
	private static readonly bool _autoOffPvPMatchEnd = true;

	[ConditionBool, UI("PvP 比赛开始时自动开启。",
		 Filter = DutySpecificPvP)]
	private static readonly bool _autoOnPvPMatchStart = true;

	[ConditionBool, UI("在 PvP 区域启用时将 RSR 设为 PvP 专用状态。",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpStateControl = false;

	[ConditionBool, UI("处于守护状态时不使用任何技能。",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpGuardControl = true;

	[ConditionBool, UI("PvP 中若目标进入守护且技能不忽略守护，则取消读条。",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpGuardCancel = false;

	[ConditionBool, UI("使用净化解除眩晕状态",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpPurifyStun = true;

	[ConditionBool, UI("使用净化解除沉默状态",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpPurifySilence = true;

	[ConditionBool, UI("使用净化解除深度冻结状态",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpPurifyDeepFreeze = true;

	[ConditionBool, UI("使用净化解除自然奇迹状态",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpPurifyMiracleOfNature = true;

	[ConditionBool, UI("使用净化解除迟缓状态",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpPurifyHeavy = false;

	[ConditionBool, UI("使用净化解除束缚状态",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpPurifyBind = false;

	[ConditionBool, UI("当 HP 低于 50% 且 MP 超过 2000 用于治疗时，锁定 GCD 循环（实验性）。",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpGCDLockControl = true;

	[ConditionBool, UI("未设置目标时即使处于敌人范围内也允许冲刺（实验性）。",
		 Filter = DutySpecificPvP)]
	private static readonly bool _pvpAllowSprintWithoutTarget = true;

	#endregion

	[ConditionBool, UI("使用 BMR 集成验证移动类技能/导致移动的技能在自动使用时的安全性。（实验性）",
	Filter = AutoActionUsage, Section = 5)]
	private static readonly bool _bmrSafetyCheckAuto = false;

	[ConditionBool, UI("使用 BMR 集成验证移动类技能/导致移动的技能在拦截使用时的安全性。（实验性）",
	Filter = AutoActionUsage, Section = 5)]
	private static readonly bool _bmrSafetyCheckIntercept = false;

	[ConditionBool, UI("拦截玩家输入并排队由 RSR 执行技能。（仅 PvE）",
	Filter = AutoActionUsage, Section = 5)]
	private static readonly bool _interceptAction3 = true;

	[ConditionBool, UI("允许拦截魔法。",
	Filter = AutoActionUsage, Section = 5, Parent = nameof(InterceptAction3))]
	private static readonly bool _interceptSpell3 = true;

	[ConditionBool, UI("允许拦截战技。",
	Filter = AutoActionUsage, Section = 5, Parent = nameof(InterceptAction3))]
	private static readonly bool _interceptWeaponskill3 = true;

	[ConditionBool, UI("允许拦截能力技。",
	Filter = AutoActionUsage, Section = 5, Parent = nameof(InterceptAction3))]
	private static readonly bool _interceptAbility3 = true;

	[ConditionBool, UI("允许拦截宏中的技能。",
	Filter = AutoActionUsage, Section = 5, Parent = nameof(InterceptAction3))]
	private static readonly bool _interceptMacro = false;

	[ConditionBool, UI("允许拦截正在冷却中的技能。",
	Filter = AutoActionUsage, Section = 5, Parent = nameof(InterceptAction3))]
	private static readonly bool _interceptCooldown = false;

	[ConditionBool, UI("将原始玩家输入传递给动作管理器。（启用会导致卡 GCD）",
	Filter = AutoActionUsage, Section = 5, Parent = nameof(InterceptAction3))]
	private static readonly bool _interceptPassing = true;

	[UI("拦截技能执行窗口（RSR 在技能被拦截后允许尝试使用该技能的时间量）",
	Filter = AutoActionUsage, Section = 5)]
	[Range(1, 10, ConfigUnitType.Seconds)]
	public float InterceptActionTime { get; set; } = 5;

	/// <markdown file="Auto" name="使用何种 AoE 技能" section="Action Usage and Control">
	/// - Full：使用所有可用 AoE 技能。
	/// - Cleave：仅使用单体 AoE 技能。
	/// - Off：不使用任何 AoE 技能。
	/// </markdown>
	[UI("使用何种 AoE 技能。",
	Description = "Full：使用所有可用 AoE 技能。\nCleave：仅使用单体 AoE 技能。\nOff：不使用任何 AoE 技能。",
	Filter = AutoActionUsage, Section = 3)]
	public AoEType AoEType { get; set; } = AoEType.Full;

	[ConditionBool, UI("对状态已达上限的敌人忽略状态附加。",
	Filter = AutoActionUsage, Section = 3)]
	private static readonly bool _statuscap2 = true;

	[ConditionBool, UI("不要用 AoE 攻击新怪物。", Description = "当 AoE 可能攻击到非敌对目标时，永不使用任何 AoE 技能。",
		Filter = AutoActionUsage, Section = 3)]
	private static readonly bool _noNewHostiles = false;

	[ConditionBool, UI("区域切换时自动关闭。",
		Description = "在不同区域之间移动时自动关闭战斗状态。",
		Filter = BasicAutoSwitch)]
	private static readonly bool _autoOffBetweenArea = true;

	[ConditionBool, UI("过场动画时自动关闭。",
		Description = "过场动画期间自动关闭战斗状态。",
		Filter = BasicAutoSwitch)]
	private static readonly bool _autoOffCutScene = true;

	[ConditionBool, UI("切换职业时自动关闭",
		Description = "当你切换职业/特职时自动关闭战斗状态。",
		Filter = BasicAutoSwitch)]
	private static readonly bool _autoOffSwitchClass = true;

	[ConditionBool, UI("PvE 中死亡时自动关闭。",
		Description = "角色死亡时自动关闭战斗状态。",
		Filter = BasicAutoSwitch)]
	private static readonly bool _autoOffWhenDead = true;

	[ConditionBool, UI("副本完成后自动关闭。",
	Description = "副本（实例）结束时自动关闭战斗状态。",
	Filter = BasicAutoSwitch)]
	private static readonly bool _autoOffWhenDutyCompleted = true;

	[ConditionBool, UI("更新时启用更新日志弹窗",
	Description = "插件更新时显示一个包含更新日志的弹窗。",
	Filter = UiInformation)]
	private static readonly bool _changelogPopup = true;

	[ConditionBool, UI("在 DTR 栏显示插件状态。",
	Description = "在服务器信息栏中显示插件的当前状态。",
	Filter = UiInformation)]
	private static readonly bool _showInfoOnDtr = true;

	[UI("DTR 行为", Filter = UiInformation, Parent = nameof(ShowInfoOnDtr))]
	public DTRType DTRType { get; set; } = DTRType.DTRNormal;

	[ConditionBool, UI("在 Toast 弹窗中显示插件状态",
	Description = "战斗状态改变时显示 Toast 通知。",
	Filter = UiInformation)]
	private static readonly bool _showInfoOnToast = false;

	[ConditionBool, UI("读条或执行特定动作时锁定移动。",
	Description = "防止在读条或使用特定动作时角色移动。",
	Filter = Extra)]
	private static readonly bool _poslockCasting = false;

	[UI("", Action = ActionID.PassageOfArmsPvE, Parent = nameof(PoslockCasting))]
	public bool PosPassageOfArms { get; set; } = false;

	[UI("", Action = ActionID.FlamethrowerPvE, Parent = nameof(PoslockCasting))]
	public bool PosFlameThrower { get; set; } = false;

	[UI("", Action = ActionID.ImprovisationPvE, Parent = nameof(PoslockCasting))]
	public bool PosImprovisation { get; set; } = false;

	[ConditionBool, UI("AoE 减伤期间施放圣光幕帘时锁定动作。",
	Filter = Extra)]
	private static readonly bool _pldlockCasting = false;

	[ConditionBool, UI("AoE 减伤期间施放命运之轮时锁定动作。",
	Filter = Extra)]
	private static readonly bool _astlockCasting = false;

	[ConditionBool, UI("施放幽灵疾风时锁定动作。",
	Filter = Extra)]
	private static readonly bool _blulockCasting = true;

	/// <markdown file="Auto" name="增伤药水/爆发药/药品使用" section="Action Usage and Control">
	/// 设置是否使用增伤药水以及在哪些副本中使用。你还需要在 `Actions` 标签页的 `Items` 下
	/// 启用特定的药水。
	/// </markdown>
	[JobConfig, UI("仅在轮换中编码支持时自动使用",
	Description = "此设置仅在轮换明确支持自动使用 Tincture 时才会生效。",
	Filter = AutoActionUsage, PvPFilter = JobFilterType.NoJob)]
	private readonly TinctureUseType _TinctureType = TinctureUseType.InHighEndDuty;

	[ConditionBool, UI("自动使用防击退职能技能（亲疏自行、稳定施法）",
	Description = "启用后，会根据列表菜单中的防击退技能列表，在需要时自动使用防击退能力技能。",
	Filter = AutoActionUsage)]
	private static readonly bool _useKnockback = true;

	[ConditionBool, UI("自动使用 HP 药水",
	Description = "启用后允许插件自动使用 HP 药水。",
	Filter = AutoActionUsage)]
	private static readonly bool _useHpPotions = false;

	[UI("HP% 低于此值时使用 HP 药水", Parent = nameof(UseHpPotions))]
	[Range(0, 1, ConfigUnitType.Percent, 0.002f)]
	public float UseHpPotionsPercent { get; set; } = 0.5f;

	[ConditionBool, UI("自动使用 MP 药水",
	Description = "启用后允许插件自动使用 MP 药水。",
	Filter = AutoActionUsage)]
	private static readonly bool _useMpPotions = false;

	[UI("MP% 低于此值时使用 MP 药水", Parent = nameof(UseMpPotions))]
	[Range(0, 1, ConfigUnitType.Percent, 0.002f)]
	public float UseMpPotionsPercent { get; set; } = 0.5f;

	[ConditionBool, UI("自动使用凤凰尾巴",
	Description = "启用后允许插件使用凤凰尾巴道具。（实验性功能）",
	Filter = AutoActionUsage)]
	private static readonly bool _usePhoenixDown = false;

	[ConditionBool, UI("仅当小队中无存活复活者时使用凤凰尾巴",
	Parent = nameof(UsePhoenixDown))]
	private static readonly bool _usePhoenixDownHealerLogic = true;

	[UI("当与目标的距离小于此值时使用伤害型突进技能。",
		Filter = AutoActionUsage)]
	[Range(0, 30, ConfigUnitType.Yalms, 1f)]
	public float DistanceForMoving2 { get; set; } = 3f;

	[ConditionBool, UI("允许对优先标记目标使用 AoE。",
	Description = "启用后允许 AoE 技能命中带有优先标记的目标。",
	Parent = nameof(ChooseAttackMark))]
	private static readonly bool _canAttackMarkAOE = true;

	[ConditionBool, UI("教学模式", Filter = UiInformation)]
	private static readonly bool _teachingMode = false;

	[ConditionBool, UI("战斗中教学模式下自动选目标",
		Description = "教学模式激活时，自动切换你的目标以匹配轮换建议动作的目标。对坦克和治疗很有用，因为最佳目标可能与你当前选择的不同。",
		Parent = nameof(TeachingMode))]
	private static readonly bool _teachingModeAutoTarget = false;

	[ConditionBool, UI("在下一个动作窗口中显示目标提示",
		Description = "教学模式激活时，在下一个动作图标下方显示轮换建议的目标名称。若你未选中该目标则显示为橙色，已选中则显示为绿色。",
		Parent = nameof(TeachingMode))]
	private static readonly bool _teachingModeShowTargetHint = false;

	[ConditionBool, UI("模拟按键效果",
		Filter = UiInformation)]
	private static readonly bool _keyboardNoise = true;

	[ConditionBool, UI("倒计时开始时激活自动模式",
		Filter = BasicAutoSwitch, Section = 1)]
	private static readonly bool _startOnCountdown = true;

	[ConditionBool, UI("倒计时开始时启动手动模式而非自动模式",
			   Parent = nameof(StartOnCountdown))]
	private static readonly bool _countdownStartsManualMode = false;

	[ConditionBool, UI("倒计时期间提前进入战斗时取消自动模式",
		Filter = BasicAutoSwitch, Section = 1)]
	private static readonly bool _cancelStateOnCombatBeforeCountdown = false;

	[ConditionBool, UI("我了解 Auto On 设置会自动开启 RSR 的自动轮换。",
		Filter = BasicAutoSwitch, Section = 1)]
	private static readonly bool _AutoOnYes = false;

	[ConditionBool, UI("受击时自动开启手动模式。",
			   Parent = nameof(AutoOnYes))]
	private static readonly bool _startOnAttackedBySomeone2 = false;

	[ConditionBool, UI("小队进入战斗时自动开启自动模式。",
			   Parent = nameof(AutoOnYes))]
	private static readonly bool _startOnPartyIsInCombat2 = false;

	[ConditionBool, UI("联盟进入战斗时自动开启自动模式。",
			   Parent = nameof(AutoOnYes))]
	private static readonly bool _startOnAllianceIsInCombat2 = false;

	[ConditionBool, UI("在博兹雅/优雷卡/奥秘 Fate/CE 中战斗时自动开启自动模式",
			   Parent = nameof(AutoOnYes))]
	private static readonly bool _startOnFieldOpInCombat2 = false;

	/// <markdown file="Auto" name="非治疗职业时使用治疗能力技" section="Healing Usage and Control">
	/// 当不作为治疗职业时允许使用治疗能力技能（如赤治疗、浴血等）。
	/// </markdown>
	[ConditionBool, UI("非治疗职业时使用治疗能力技能。",
		Filter = HealingActionCondition, Section = 1)]
	private static readonly bool _useHealWhenNotAHealer = true;

	[JobConfig, UI("如有可能则使用打断技能。",
		Filter = AutoActionUsage, Section = 3,
		PvEFilter = JobFilterType.Interrupt,
		PvPFilter = JobFilterType.NoJob)]
	private static readonly bool _interruptibleMoreCheck = true;

	[ConditionBool, UI("挑衅任何不在禁止挑衅列表中的目标。",
		Filter = AutoActionUsage, Section = 3)]
	private static readonly bool _provokeAnything = false;

	[ConditionBool, UI("目标死亡时停止读条。", Filter = Extra)]
	private static readonly bool _useStopCasting = false;

	/// <markdown file="Auto" name="驱散所有可驱散 Debuff" section="Action Usage and Control">
	/// 启用此设置将强制对受到可驱散负面状态影响的所有目标使用 Esuna。
	/// </markdown>
	[ConditionBool, UI("无论治疗如何，驱散所有可驱散的负面状态。",
		Filter = AutoActionUsage, Section = 3,
		PvEFilter = JobFilterType.Dispel, PvPFilter = JobFilterType.NoJob)]
	private static readonly bool _dispelAll = false;

	[ConditionBool, UI("调试模式", Filter = Debug)]
	private static readonly bool _inDebug = false;

	[ConditionBool, UI("启用 Action Tracer（仅在开发者要求时切换）", Filter = Debug)]
	private static readonly bool _enableActionTracer = false;

	[ConditionBool, UI("将追踪器输出镜像到 Dalamud 插件日志",
		Parent = nameof(EnableActionTracer))]
	private static readonly bool _traceMirrorToPluginLog = false;

	[ConditionBool, UI("将 /rotation Manual 设为切换命令。",
		Filter = BasicParams)]
	private static readonly bool _toggleManual = false;

	[ConditionBool, UI("将 /rotation Auto 设为切换命令。（正常行为是在目标选择设置之间循环）",
		Filter = BasicParams)]
	private static readonly bool _toggleAuto = false;

	[ConditionBool, UI("仅在有敌人或副本中时显示这些窗口",
		Filter = UiWindows)]
	private static readonly bool _onlyShowWithHostileOrInDuty = false;

	[ConditionBool, UI("显示控制窗口",
		Filter = UiWindows)]
	private static readonly bool _showControlWindow = false;

	[ConditionBool, UI("锁定控制窗口",
		Filter = UiWindows)]
	private static readonly bool _isControlWindowLock = false;

	[ConditionBool, UI("显示下一个动作窗口", Filter = UiWindows)]
	private static readonly bool _showNextActionWindow = false;

	[ConditionBool, UI("显示拦截动作窗口", Filter = UiWindows)]
	private static readonly bool _showInterceptedActionWindow = false;

	[ConditionBool, UI("无输入", Parent = nameof(ShowNextActionWindow))]
	private static readonly bool _isInfoWindowNoInputs = false;

	[ConditionBool, UI("不移动", Parent = nameof(ShowNextActionWindow))]
	private static readonly bool _isInfoWindowNoMove = false;

	[ConditionBool, UI("显示道具冷却",
		Parent = nameof(ShowCooldownWindow))]
	private static readonly bool _showItemsCooldown = false;

	[ConditionBool, UI("显示 GCD 冷却",
		Parent = nameof(ShowCooldownWindow))]
	private static readonly bool _showGCDCooldown = false;

	[ConditionBool, UI("显示原始冷却",
		Filter = UiInformation)]
	private static readonly bool _useOriginalCooldown = false;

	[ConditionBool, UI("始终显示冷却", Filter = UiInformation)]
	private static readonly bool _showCooldownsAlways = false;

	[ConditionBool, UI("显示工具提示",
		Filter = UiInformation)]
	private static readonly bool _showTooltips = true;

	[ConditionBool, UI("显示动作上下文菜单的启用/禁用切换",
		Filter = UiWindows)]
	private static readonly bool _showContext = true;

	[ConditionBool, UI("在配置窗口顶部显示随机使用提示",
		Description = "在主面板中循环显示提示；每 7 秒更新一次。",
		Filter = UiInformation)]
	private static readonly bool _showHints = true;

	[ConditionBool, UI("在热键栏上为禁用动作着色",
		Description = "启用后，你在 RSR 中禁用的动作将在游戏热键栏上着色。",
		Filter = UiInformation)]
	private static readonly bool _reddenDisabledHotbarActions = false;

	[UI("禁用动作热键栏着色颜色", Parent = nameof(ReddenDisabledHotbarActions), Filter = UiInformation)]
	public Vector4 HotbarDisabledTintColor { get; set; } = new(1f, 0f, 0f, 0.40f);

	[ConditionBool, UI("在 Toast 中显示执行动作反馈",
		Filter = UiInformation)]
	private static readonly bool _showToastsAboutDoAction = false;

	[ConditionBool, UI("允许使用此配置的轮换将轮换中定义为爆发的技能用于爆发", Filter = AutoActionUsage, Section = 4)]
	private static readonly bool _autoBurst = true;

	/// <markdown file="Auto" name="有目标施放凝视/止息列表技能时禁用敌对动作" section="Action Usage and Control">
	/// 此设置与 <see cref="RotationSolver.Basic.Configuration.OtherConfiguration.HostileCastingStop">Gaze/Stop 列表</see> 关联。
	/// </markdown>
	[ConditionBool, UI("若有目标正在施放 Gaze/Stop 列表中的动作则禁用敌对动作（实验性）", Filter = AutoActionUsage, Section = 4)]
	private static readonly bool _castingStop = false;

	[UI("读条结束前可配置的时间，RSR 在此时间内停止执行动作", Filter = AutoActionUsage, Section = 4, Parent = nameof(CastingStop))]
	[Range(0, 15, ConfigUnitType.Seconds)]
	public float CastingStopTime { get; set; } = 2.5f;

	[ConditionBool, UI("在整个持续时间内禁用（启用后将在整个读条期间阻止你的动作。）", Filter = AutoActionUsage, Section = 4, Parent = nameof(CastingStop))]
	private static readonly bool _castingStopCalculate = false;

	/// <markdown file="Auto" name="自动治疗阈值" section="Healing Usage and Control" isSubsection="1">
	/// 启用后，可以自定义在目标上施放治疗的治疗阈值。
	/// </markdown>
	[ConditionBool, UI("自动治疗阈值", Filter = HealingActionCondition, Section = 1, Order = 1)]
	private static readonly bool _autoHeal = true;

	/// <markdown file="Auto" name="达到阈值后停止治疗读条" section="Healing Usage and Control" isSubsection="1">
	/// 启用后，可以自定义在目标上施放治疗的治疗阈值。
	/// </markdown>
	[ConditionBool, UI("达到阈值后停止单体 GCD 治疗。（极其实验性）", Filter = HealingActionCondition, Section = 1, Order = 2, Description = "如果队伍中有另一名治疗，他们的治疗可能会将目标玩家（们）的血量抬到治疗阈值之上，从而浪费 MP。如果发生这种情况，将中断读条。")]
	private static readonly bool _stopHealingAfterThresholdExperimental2 = false;

	/// <markdown file="Auto" name="自动使用 oGCD 能力技" section="Action Usage and Control" isSubsection="1">
	/// 是否使用 oGCD 能力技。
	/// </markdown>
	[ConditionBool, UI("自动使用 oGCD 能力技能", Filter = AutoActionUsage)]
	private static readonly bool _useAbility = true;

	[ConditionBool, UI("使用减伤技能", Filter = AutoActionUsage, Description = "如果你正在打副本，或者你能自己规划治疗和减伤技能的使用，建议勾选此项。")]
	private static readonly bool _useDefenseAbility = true;

	[ConditionBool, UI("自动使用单体减伤技能", Filter = AutoActionUsage, Parent = nameof(UseDefenseAbility))]
	private static readonly bool _useSTDefense = true;

	[ConditionBool, UI("自动使用 AoE 减伤技能", Filter = AutoActionUsage, Parent = nameof(UseDefenseAbility))]
	private static readonly bool _useAOEDefense = true;

	[ConditionBool, UI("使用 BossModReborn 时间轴进行前瞻性减伤",
		Description = "启用后，若加载了 BossModReborn，RSR 将使用其时间轴数据在团伤和坦克死刑命中前触发减伤技能。",
		Filter = AutoActionUsage, Parent = nameof(UseDefenseAbility))]
	private static readonly bool _useBMRTimeline = false;

	[UI("团伤前多少秒使用群体减伤", Parent = nameof(UseBmrTimeline))]
	[Range(1, 15, ConfigUnitType.Seconds, 0.5f)]
	public float BMRRaidwideMitWindow { get; set; } = 5f;

	[UI("坦克死刑前多少秒使用单体减伤", Parent = nameof(UseBmrTimeline),
		PvEFilter = JobFilterType.Tank)]
	[Range(1, 10, ConfigUnitType.Seconds, 0.5f)]
	public float BMRTankbusterMitWindow { get; set; } = 3f;

	[UI("击退前多少秒使用防击退", Parent = nameof(UseBmrTimeline))]
	[Range(1, 10, ConfigUnitType.Seconds, 0.5f)]
	public float BMRKnockbackWindow { get; set; } = 3f;
	[UI("敌人数量", Parent = nameof(UseDefenseAbility),
		PvEFilter = JobFilterType.Tank)]
	[Range(1, 8, ConfigUnitType.None, 0.05f)]
	public int AutoDefenseNumber { get; set; } = 2;

	[JobConfig, Range(0, 1, ConfigUnitType.Percent, 0.02f)]
	[UI("坦克使用单体/自身减伤所需的 HP%%", Parent = nameof(UseDefenseAbility),
		PvEFilter = JobFilterType.Tank)]
	private readonly float _healthForAutoDefense = 1;

	[ConditionBool, UI("自动激活坦克姿态", Parent = nameof(UseAbility),
		PvEFilter = JobFilterType.Tank)]
	private static readonly bool _autoTankStance = true;

	[ConditionBool, UI("队伍中有其他坦克时自动挑衅", Description = "当队伍中有多名坦克时，若敌人正在攻击非坦克队员，则自动使用挑衅。",
		Parent = nameof(UseAbility), PvEFilter = JobFilterType.Tank)]
	private static readonly bool _autoProvokeForTank = true;

	/// <markdown file="Auto" name="自动正北" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._useAbility">
	/// 作为近战输出时，若未命中敌人的正确身位是否自动施放正北。
	/// </markdown>
	[ConditionBool, UI("自动 True North（近战输出）",
		Parent = nameof(UseAbility),
		PvEFilter = JobFilterType.Melee)]
	private static readonly bool _autoUseTrueNorth = true;

	[ConditionBool, UI("非战斗中且在副本内时使用移动速度提升技能。", Parent = nameof(UseAbility))]
	private static readonly bool _autoSpeedOutOfCombat = true;

	[ConditionBool, UI("非战斗中且不在副本内时使用移动速度提升技能。", Parent = nameof(UseAbility))]
	private static readonly bool _autoSpeedOutOfCombatNoDuty = false;

	[ConditionBool, UI("使用地面增益指向技能", Description = "1.    自身目标回退：\r\n如果范围为 0，则始终以玩家为目标，并返回玩家位置上所有受影响的目标。\r\n2.    优先位置 (OnLocations)：\r\n•    尝试为当前区域获取预定义的增益位置。\r\n•    如果未找到且内容为讨伐战或副本，则使用回退点（例如 0,0 或 100,100 点，因为这些大多数时候是竞技场中心）。\r\n•    选择离玩家最近的点，应用一个小的随机偏移，并检查是否在效果范围内。\r\n•    如果是，则返回该点作为目标区域。\r\n3.    Boss 身位回退：\r\n•    如果当前目标是带有身位要求且在范围内的 Boss，则使用 Boss 的位置（或范围内的一个点）作为目标区域。\r\n4.    队伍成员回退：\r\n•    收集范围 + 效果范围内的队伍成员。\r\n•    尝试找到正在被攻击的队伍成员（坦克或焦点目标）。\r\n•    如果找到，则根据距离和效果范围计算是留在玩家位置还是更靠近坦克。\r\n•    如果未找到或不需要，则默认使用玩家位置。", Filter = HealingActionCondition, Section = 3)]
	private static readonly bool _useGroundBeneficialAbility = true;

	/// <markdown file="Auto" name="移动时使用有益地面目标技能" section="Healing Usage and Control">
	/// 启用后允许在移动中使用地面 AoE 技能，如地星（占星）、神圣领域（学者）等。
	/// </markdown>
	[JobConfig, UI("移动时使用地面增益指向技能。", Parent = nameof(UseGroundBeneficialAbility))]
	private static readonly bool _useGroundBeneficialAbilityWhenMoving = false;

	[JobConfig, UI("仅在自身使用地面增益指向技能，跳过其他逻辑。", Parent = nameof(UseGroundBeneficialAbility))]
	private static readonly bool _useGroundBeneficialAbilityOnlySelf = false;

	[JobConfig, UI("若存在则对队伍坦克使用地面增益指向技能，跳过其他逻辑。", Parent = nameof(UseGroundBeneficialAbility))]
	private static readonly bool _useTargetTankForGroundHeal = false;

	[ConditionBool, UI("显示冷却窗口", Filter = UiWindows)]
	private static readonly bool _showCooldownWindow = false;

	[ConditionBool, UI("显示动作时间轴窗口", Filter = UiWindows)]
	private static readonly bool _showActionTimelineWindow = false;

	[ConditionBool, UI("仅在战斗中显示时间轴", Parent = nameof(ShowActionTimelineWindow))]
	private static readonly bool _actionTimelineOnlyInCombat = true;

	[ConditionBool, UI("仅当 RSR 激活时显示时间轴", Parent = nameof(ShowActionTimelineWindow))]
	private static readonly bool _actionTimelineOnlyWhenActive = true;

	[ConditionBool, UI("在时间轴中显示 oGCD 动作", Parent = nameof(ShowActionTimelineWindow))]
	private static readonly bool _actionTimelineShowOGCD = true;

	[ConditionBool, UI("在时间轴中显示自动攻击", Parent = nameof(ShowActionTimelineWindow))]
	private static readonly bool _actionTimelineShowAutoAttack = false;

	[ConditionBool, UI("战斗后将时间轴保存到 JSON 文件", Parent = nameof(ShowActionTimelineWindow))]
	private static readonly bool _actionTimelineSaveToFile = false;

	[ConditionBool, UI("记录 AoE 动作", Filter = List)]
	private static readonly bool _recordCastingArea = true;

	[ConditionBool, UI("战斗结束超过以下时间后自动关闭 RSR：",
		Filter = BasicAutoSwitch)]
	private static readonly bool _autoOffAfterCombat = true;

	[ConditionBool, UI("Enable RSR click counter in main menu",
		Filter = Extra)]
	private static readonly bool _enableClickingCount = true;

	[ConditionBool, UI("隐藏所有警告",
		Filter = UiInformation)]
	private static readonly bool _hideWarning = false;

	/// <markdown file="Auto" name="非治疗职业时仅治疗自己" section="Healing Usage and Control">
	/// 启用后，若不是治疗职业，可以目标他人进行治疗技能将只目标你自己。
	/// </markdown>
	[ConditionBool, UI("非治疗职业时仅治疗自身",
		Filter = HealingActionCondition, Section = 1)]
	private static readonly bool _onlyHealSelfWhenNoHealer = false;

	[ConditionBool, UI("仅当小队中没有存活的治疗时，非治疗职业才使用治疗能力技能。",
	Description = "启用后，非治疗职业（如输出或坦克）只有在小队中没有治疗，或所有治疗均已倒下（0 HP）时才会使用治疗能力技能。\r\n只要有一名治疗存活，非治疗职业就不会使用治疗能力技能。",
	Filter = HealingActionCondition, Section = 1)]
	private static readonly bool _onlyHealAsNonHealIfNoHealers = false;

	[ConditionBool, UI("在聊天中显示已切换的设置和新值。",
		Filter = UiInformation)]
	private static readonly bool _ShowToggledSettingInChat = false;

	[ConditionBool, UI("记录击退动作", Filter = List2)]
	private static readonly bool _recordKnockbackies = false;

	[ConditionBool, UI("自动设置青魔技能", Description = "使用青魔轮换时，RSR 可自动将你的魔法书设置为该轮换所需的法术。", Filter = Extra)]
	private static readonly bool _setBluActions2 = false;

	#region Float
	[UI("战斗结束超过多少时间后自动关闭 RSR...",
		Parent = nameof(AutoOffAfterCombat))]
	[Range(0, 600, ConfigUnitType.Seconds)]
	public float AutoOffAfterCombatTime { get; set; } = 30;

	[UI("视野锥形的角度", Parent = nameof(OnlyAttackInVisionCone))]
	[Range(0, 90, ConfigUnitType.Degree, 0.02f)]
	public float AngleOfVisionCone { get; set; } = 45;

	/// <markdown file="Auto" name="近战范围技能偏移使用" section="Action Usage and Control">
	/// 近战职业使用远程攻击的额外缓冲距离（单位：星码）。例如，
	/// 若你游玩武士且此设置设为 `1`，则从 4 星码开始才会使用飞刃。
	///
	/// 这是因为近战范围的默认"溢出"为 3 星码，意味着你可以在敌人判定圈外 3 星码处施放近战攻击。
	/// 此设置（偏移量）会取最大近战范围并加上你设置的值。
	///
	/// 此设置的存在是为了让你有时间接近并进入敌人的近战范围，而不浪费 GCD。
	/// </markdown>
	[UI("近战远程攻击使用偏移量",
		Filter = AutoActionUsage, Section = 3,
		PvEFilter = JobFilterType.Melee, PvPFilter = JobFilterType.Melee)]
	[Range(0, 5, ConfigUnitType.Yalms, 0.02f)]
	public float MeleeRangedOffset { get; set; } = 1;

	[UI("当其最低 HP 低于此值时。", Parent = nameof(HealWhenNothingTodo))]
	[Range(0, 1, ConfigUnitType.Percent, 0.002f)]
	public float HealWhenNothingTodoBelow { get; set; } = 0.8f;

	[UI("坦克 HP 低于此值时优先治疗坦克。",
		Filter = HealingActionCondition, Section = 1)]
	[Range(0, 1, ConfigUnitType.Percent, 0.02f)]
	public float HealthTankRatio { get; set; } = 0.45f;

	[UI("治疗 HP 低于此值时优先治疗治疗。",
		Filter = HealingActionCondition, Section = 1)]
	[Range(0, 1, ConfigUnitType.Percent, 0.02f)]
	public float HealthHealerRatio { get; set; } = 0.4f;

	[UI("自身 HP 低于此值时优先治疗自身。",
		Filter = HealingActionCondition, Section = 1)]
	[Range(0, 1, ConfigUnitType.Percent, 0.02f)]
	public float HealthSelfRatio { get; set; } = 0.4f;

	#region
	[JobConfig, UI("将复活死亡玩家优先于治疗/减伤。",
		Filter = HealingActionCondition, Section = 2)]
	private static readonly bool _raisePlayerFirst = false;

	[JobConfig, UI("如有可能则使用 Swiftcast/双重施法来复活玩家", Description = "如果禁用，你将永远不会使用 Swiftcast/双重施法来复活玩家。",
		Filter = HealingActionCondition, Section = 2)]
	private static readonly bool _raisePlayerBySwift = true;

	[JobConfig, UI("硬读条复活逻辑",
		Filter = HealingActionCondition, Section = 2)]
	private readonly HardCastRaiseType _HardCastRaiseType = HardCastRaiseType.HardCastNormal;

	[JobConfig, UI("复活风格",
		Filter = HealingActionCondition, Section = 2)]
	private readonly RaiseType _RaiseType = RaiseType.PartyOnly;

	[JobConfig, UI("复活带有濒死负面状态的玩家",
		Filter = HealingActionCondition, Section = 2)]
	private static readonly bool _raiseBrinkOfDeath = true;

	[JobConfig, UI("将非治疗职业从队伍列表底部优先复活（4人本双治疗行为）",
		Filter = HealingActionCondition, Section = 2)]
	private static readonly bool _h2 = false;

	[JobConfig, UI("若无坦克或治疗死亡，则优先复活赤魔和召唤师",
		Filter = HealingActionCondition, Section = 2)]
	private static readonly bool _offRaiserRaise = false;

	#endregion

	/// <markdown file="Auto" name="RSR 应在下一个 GCD 前多久使用 Swiftcast 进行复活" section="Healing Usage and Control">
	/// 如果你施放了一个 GCD 且冷却为 2.5 秒，当冷却开始时有队友死亡，Swiftcast 动作会在你的冷却结束前等待指定的时间
	/// 才施放 Swiftcast。这是为了防止过早使用 Swiftcast 而浪费，避免你的搭档治疗在你的全局冷却期间内成功复活你的目标。
	/// </markdown>
	[JobConfig, UI("RSR 应在下一个 GCD 前多久使用 Swiftcast 进行复活",
		Filter = HealingActionCondition, Section = 2)]
	[Range(0, 1.0f, ConfigUnitType.Seconds, 0.01f)]
	public float SwiftcastBuffer { get; set; } = 0.6f;

	/// <markdown file="Auto" name="复活玩家的随机延迟范围" section="Healing Usage and Control">
	/// 为了不让别人明显看出你在使用 RSR，施放复活动作会延迟两个值之间的随机秒数。
	/// </markdown>
	[UI("复活玩家的随机延迟范围。",
		Filter = HealingActionCondition, Section = 2)]
	[Range(0, 10, ConfigUnitType.Seconds, 0.002f)]
	public Vector2 RaiseDelay2 { get; set; } = new(3f, 3f);

	[UI("驱散状态的随机延迟范围。",
		Filter = HealingActionCondition, Section = 2)]
	[Range(0, 10, ConfigUnitType.Seconds, 0.002f)]
	public Vector2 EsunaDelay { get; set; } = new(0f, 0f);

	[Range(0, 10000, ConfigUnitType.None, 100)]
	[UI("MP 低于此值时永不复活玩家",
		Filter = HealingActionCondition, Section = 2)]
	public int LessMPNoRaise { get; set; } = 2400;

	/// <markdown file="Extra" name="使用 AoE 治疗的 HP 标准差">
	/// 控制队伍成员的 HP 必须相差多少才会使用 AoE 治疗而非单体治疗。
	/// 较低的值要求队伍成员的 HP 更接近才会触发 AoE 治疗（更挑剔）。
	/// 较高的值允许即使 HP 差异较大也使用 AoE 治疗（不那么挑剔）。
	/// 仅在你想要细调 AoE 治疗行为时调整。
	/// </markdown>
	[UI("使用 AoE 治疗的 HP 标准差。", Description = "控制队伍成员的 HP 必须相差多少才会使用 AoE 治疗而非单体治疗。较低的值要求队伍成员的 HP 更接近才会触发 AoE 治疗（更挑剔）。较高的值允许即使 HP 差异较大也使用 AoE 治疗（不那么挑剔）。仅在你想要细调 AoE 治疗行为时调整。",
	Filter = Extra)]
	[Range(0, 0.5f, ConfigUnitType.Percent, 0.02f)]
	public float HealthDifference { get; set; } = 0.25f;

	[ConditionBool, UI("非战斗时治疗队伍成员。",
		Filter = HealingActionCondition, Section = 3)]
	private static readonly bool _healOutOfCombat = false;

	[ConditionBool, UI("治疗单人副本 NPC（仅在需要时启用）", Description = "实验性。",
		Filter = HealingActionCondition, Section = 3)]
	private static readonly bool _friendlyBattleNPCHeal = false;

	[ConditionBool, UI("治疗并复活小队 NPC。",
		Filter = HealingActionCondition, Section = 3)]
	private static readonly bool _friendlyPartyNPCHealRaise3 = true;

	[ConditionBool, UI("将你的陆行鸟视为小队成员",
		Filter = HealingActionCondition, Section = 3)]
	private static readonly bool _chocoboPartyMember = false;

	[ConditionBool, UI("在联盟突袭中将焦点目标玩家视为小队成员", Description = "实验性，包含混沌。",
		Filter = HealingActionCondition, Section = 3)]
	private static readonly bool _focusTargetIsParty = false;

	[ConditionBool, UI("战斗中若无其他事可做，则用 GCD 治疗小队成员。",
		Filter = HealingActionCondition, Section = 3)]
	private static readonly bool _healWhenNothingTodo = true;

	[ConditionBool, UI("坦克死刑时优先选择低 HP 的坦克。",
		Filter = HealingActionCondition, Section = 3)]
	private static readonly bool _priolowtank = false;

	/// <markdown file="Basic" name="/rotation 命令打开的特殊窗口默认持续时间">
	/// 默认情况下通过 /rotation 命令打开的特殊窗口的持续时间。
	/// （位于 主菜单 => 宏）
	/// </markdown>
	[UI("默认情况下通过 /rotation 命令打开的特殊窗口的持续时间。",
		Filter = BasicTimer, Section = 1)]
	[Range(1, 20, ConfigUnitType.Seconds, 1f)]
	public float SpecialDuration { get; set; } = 3;

	[UI("目标死亡或免疫伤害时 RSR 停止攻击的随机延迟范围。",
		Parent = nameof(UseStopCasting))]
	[Range(0, 3, ConfigUnitType.Seconds, 0.002f)]
	public Vector2 StopCastingDelay { get; set; } = new(0.5f, 1);

	[UI("打断敌对目标前的随机延迟范围。",
		Filter = AutoActionUsage, Section = 3,
		PvEFilter = JobFilterType.Interrupt, PvPFilter = JobFilterType.NoJob)]
	[Range(0, 3, ConfigUnitType.Seconds, 0.002f)]
	public Vector2 InterruptDelay { get; set; } = new(0.5f, 1);

	[UI("挑衅随机延迟范围。", Parent = nameof(AutoProvokeForTank))]
	[Range(0, 10, ConfigUnitType.Seconds, 0.05f)]
	public Vector2 ProvokeDelay { get; set; } = new(0.5f, 1);

	[UI("非战斗状态随机延迟范围。",
		Filter = BasicParams)]
	[Range(0, 10, ConfigUnitType.Seconds, 0.002f)]
	public Vector2 NotInCombatDelay { get; set; } = new(2, 3);

	/// <markdown file="Basic" name="点击技能的随机延迟范围">
	/// 在热键栏上显示点击/按键效果之间的延迟。
	/// </markdown>
	[UI("点击动作的随机延迟范围。",
		Filter = BasicTimer)]
	[Range(0.00f, 0.25f, ConfigUnitType.Seconds, 0.002f)]
	public Vector2 ClickingDelay { get; set; } = new(0.1f, 0.2f);

	[UI("闲暇治疗延迟范围。", Parent = nameof(HealWhenNothingTodo))]
	[Range(0, 5, ConfigUnitType.Seconds, 0.05f)]
	public Vector2 HealWhenNothingTodoDelay { get; set; } = new(0.5f, 1);

	/// <markdown file="Basic" name="倒计时结束前多久开始读条或攻击">
	/// 倒计时结束前多久开始读条或攻击。
	/// </markdown>
	[UI("倒计时结束前多久开始读条或攻击。",
		Filter = BasicTimer, Section = 1, PvPFilter = JobFilterType.NoJob)]
	[Range(0, 0.7f, ConfigUnitType.Seconds, 0.002f)]
	public float CountDownAhead { get; set; } = 0.4f;

	[UI("冷却窗口图标大小")]
	[Range(0, 80, ConfigUnitType.Pixels, 0.2f)]
	public float CooldownWindowIconSize { get; set; } = 30;

	[UI("下一个动作尺寸比例", Parent = nameof(ShowControlWindow))]
	[Range(0, 10, ConfigUnitType.Percent, 0.02f)]
	public float ControlWindowNextSizeRatio { get; set; } = 1.5f;

	[UI="GCD 图标大小", Parent = nameof(ShowControlWindow))]
	[Range(0, 80, ConfigUnitType.Pixels, 0.2f)]
	public float ControlWindowGCDSize { get; set; } = 40;

	[UI("oGCD 图标大小", Parent = nameof(ShowControlWindow))]
	[Range(0, 80, ConfigUnitType.Pixels, 0.2f)]
	public float ControlWindow0GCDSize { get; set; } = 30;

	[UI("控制进度高度")]
	[Range(2, 30, ConfigUnitType.Yalms)]
	public float ControlProgressHeight { get; set; } = 8;

	[UI("击杀时间低于此值时停止治疗：", Parent = nameof(UseHealWhenNotAHealer))]
	[Range(0, 30, ConfigUnitType.Seconds, 0.02f)]
	public float AutoHealTimeToKill { get; set; } = 8f;

	[UI("更新 RSR 信息之间的最短时间。（提高此值有助于改善帧率问题，但可能导致轮换性能问题）",
	Filter = BasicTimer)]
	[JobConfig, Range(0, 0.3f, ConfigUnitType.Seconds, 0.002f)]
	public float MinUpdatingTime { get; set; } = 0.00f;

	/// <markdown file="Basic" name="提前量">
	/// 在 GCD 周期中剩余的 GCD 时间百分比，达到此值时 RSR 会尝试排队下一个 GCD。
	///
	/// 此设置控制 RSR 会尝试在单个 GCD 窗口中插入多少个 oGCD。
	/// 数值越小，oGCD 越多，但可能导致更多 GCD 卡顿。
	/// </markdown>
	[JobConfig, Range(0.05f, 0.4f, ConfigUnitType.Percent)]
	[UI("动作提前量（在 GCD 周期中剩余的 GCD 时间百分比，达到此值时 RSR 会尝试排队下一个 GCD）", Filter = BasicTimer,
	Description = "此设置控制 RSR 会尝试在单个 GCD 窗口中插入多少个 oGCD\n数值越小，oGCD 越多，但可能导致更多 GCD 卡顿")]
	private readonly float _action6head = 0.25f;

	/// <summary>
	/// Remove extra lag-induced animation lock delay from instant casts (read tooltip!)
	/// Do NOT use with XivAlexander or NoClippy - this should automatically disable itself if they are detected, but double check first!
	/// </summary>
	[ConditionBool, UI("移除瞬发技能因延迟引起的额外动画锁定延迟（请阅读工具提示！）",
	Description = "请勿与 XivAlexander、BMR 调整或 NoClippy 一起使用 - 检测到它们时应自动禁用此功能，但请先仔细检查！",
	Filter = Extra)]
	private static readonly bool _removeAnimationLockDelay = false;

	/// <summary>
	/// Animation lock max. simulated delay in milliseconds
	/// Configures the maximum simulated delay in milliseconds when using animation lock removal - this is required and cannot be reduced to zero.
	/// Setting this to 20ms will enable triple-weaving when using autorotation. The minimum setting to remove triple-weaving is 26ms.
	/// The minimum of 20ms has been accepted by FFLogs and should not cause issues with your logs.
	/// </summary>
	[UI("动画锁定最大模拟延迟（请阅读工具提示！）",
	Description = "配置使用动画锁定移除时的最大模拟延迟（毫秒） - 这是必需的，不能减小到零。设为 20ms 将在使用自动轮换时启用三段插入。移除三段插入的最小设置为 26ms。20ms 的最小值已被 FFLogs 接受，应该不会导致日志问题。",
	Parent = nameof(RemoveAnimationLockDelay), Filter = Extra)]
	[Range(20, 50, ConfigUnitType.None, 1f)]
	public int AnimationLockDelayMax2 { get; set; } = 26;

	[UI("动画锁定延迟平滑系数", Parent = nameof(RemoveAnimationLockDelay), Filter = Extra)]
	[Range(0.3f, 0.95f, ConfigUnitType.None, 0.01f)]
	public float AnimLockDelaySmoothing { get; set; } = 0.3f;

	/// <summary>
	/// Remove extra framerate-induced cooldown delay
	/// Dynamically adjusts cooldown and animation locks to ensure queued actions resolve immediately regardless of framerate limitations
	/// </summary>
	[ConditionBool, UI("移除因帧率引起的额外冷却延迟",
	Description = "动态调整冷却和动画锁定，确保排队的动作无论帧率限制如何都能立即解决",
	Filter = Extra)]
	private static readonly bool _removeCooldownDelay = false;

	[UI("最大冷却调整（ms）", Parent = nameof(RemoveCooldownDelay), Filter = Extra)]
	[Range(10, 150, ConfigUnitType.None, 1f)]
	public int CooldownAdjustMaxMs { get; set; } = 100;

	// Cactbot timeline integration
	[ConditionBool, UI("启用 cactbot 时间轴集成（极其实验性）",
	Description = "连接到 OverlayPlugin 的 WebSocket 服务器，并对 cactbot 广播消息（团伤、坦克死刑、击退、停机/不可目标）作出反应。",
		Filter = Extra)]
	private static readonly bool _enableCactbotTimeline = false;

	[ConditionBool, UI("显示 cactbot 事件 Toast（调试）",
	Description = "当收到 cactbot 广播并映射到 RotationSolver 特殊事件时显示 Toast。",
	Filter = Extra)]
	private static readonly bool _showCactbotToasts = false;

	[UI("高亮颜色。", Parent = nameof(TeachingMode))]
	public Vector4 TeachingModeColor { get; set; } = new(0f, 1f, 0f, 1f);

	[UI("目标颜色", Parent = nameof(TargetColor))]
	public Vector4 TargetColor { get; set; } = new(1f, 0.2f, 0f, 0.8f);

	[UI("锁定控制窗口的背景", Parent = nameof(ShowControlWindow))]
	public Vector4 ControlWindowLockBg { get; set; } = new(0, 0, 0, 0.55f);

	[UI("未锁定控制窗口的背景", Parent = nameof(ShowControlWindow))]
	public Vector4 ControlWindowUnlockBg { get; set; } = new(0, 0, 0, 0.75f);

	[UI("信息窗口的背景", Filter = UiWindows)]
	public Vector4 InfoWindowBg { get; set; } = new(0, 0, 0, 0.4f);
	#endregion

	#region Target
	[ConditionBool, UI("向屏幕中心的对象/怪物使用移动技能",
	Description = "启用后，移动技能会以屏幕中心的对象或怪物为目标。禁用时，它们会以你的角色面向的对象或怪物为目标。",
	Filter = TargetConfig, Section = 2)]
	private static readonly bool _moveTowardsScreenCenter = false;

	[ConditionBool, UI("优先选择带有攻击标记的怪物/对象目标",
	Description = "带有攻击标记的目标将优先被选为动作目标。",
	Filter = TargetConfig)]
	private static readonly bool _chooseAttackMark = true;

	[ConditionBool, UI("优先选择敌人部位",
	Description = "敌人部位（如泰坦之心）将优先被选为目标。",
	Filter = TargetConfig)]
	private static readonly bool _prioEnemyParts = true;

	[ConditionBool, UI("永不攻击带有停止标记的目标。",
	Description = "带有停止标记的目标不会被攻击。",
	Filter = TargetConfig)]
	private static readonly bool _filterStopMark2 = false;

	[ConditionBool, UI("将 1 HP 目标视为无敌。",
	Description = "只有 1 HP 的目标将被视为无敌并被忽略；适用于目标是无敌但未给予相应状态的罕见情况。",
	Filter = TargetConfig)]
	private static readonly bool _filterOneHPInvincible = true;

	[ConditionBool, UI("在 Fate 中时忽略非 Fate 目标，不在 Fate 中时忽略 Fate 目标。",
	Description = "在 Fate 中时，只考虑 Fate 目标。不在 Fate 中时，Fate 目标会被忽略。",
	Filter = TargetConfig)]
	private static readonly bool _ignoreNonFateInFate = true;

	[ConditionBool, UI("移动到最远位置以进行目标范围移动动作。",
		Filter = TargetConfig, Section = 2)]
	private static readonly bool _moveAreaActionFarthest = false;

	[ConditionBool, UI("所有动作均使用硬目标", Description = "如果禁用，RSR 将仅对盟友使用游戏内置的软目标来进行治疗、护盾等。",
		Filter = TargetConfig, Section = 3)]
	private static readonly bool _switchTargetFriendly2 = false;

	[ConditionBool, UI("战斗中如果附近没有有效的动作目标且未设置目标时，将目标设为最近的可攻击敌人（手动模式下同样有效）",
		Filter = TargetConfig, Section = 3)]
	private static readonly bool _targetFreely = false;

	[ConditionBool, UI("只攻击视野内的目标。",
		Filter = TargetConfig, Section = 1)]
	private static readonly bool _onlyAttackInView = false;

	[ConditionBool, UI("只攻击视野锥形内的目标",
				Filter = TargetConfig, Section = 1)]
	private static readonly bool _onlyAttackInVisionCone = false;

	[ConditionBool, UI("优先目标为狩猎/神器/理符。（神器行为有 bug）",
		Filter = TargetConfig, Section = 1)]
	private static readonly bool _targetHuntingRelicLevePriority = true;

	[ConditionBool, UI("优先目标为任务怪物（覆盖交战设置）。",
		Filter = TargetConfig, Section = 1)]
	private static readonly bool _targetQuestPriority = true;

	[ConditionBool, UI("阻止目标属于其他玩家的任务怪物。",
		Filter = TargetConfig, Section = 1)]
	private static readonly bool targetQuestThings3 = true;

	[ConditionBool, UI("当绝望之怪可用时，忽略所有其他 Fate 目标。",
		Filter = TargetConfig, Section = 1)]
	private static readonly bool forlornPriority = true;

	[ConditionBool, UI("忽略木桩目标",
			   Filter = TargetConfig, Section = 1)]
	private static readonly bool _disableTargetDummys = false;

	[ConditionBool, UI("优先目标为 Fate",
		Filter = TargetConfig, Section = 1)]
	private static readonly bool _targetFatePriority = true;

	[ConditionBool, UI("延迟自动选目标。",
		Filter = TargetConfig)]
	private static readonly bool _targetDelayEnable = false;

	[UI("锁定仇恨目标或新目标进行攻击前的时间范围", Description = "（不要设置过低，否则可能从坦克身上抢走新近仇恨的副本小怪）。",
		Filter = TargetConfig, Parent = nameof(TargetDelayEnable))]
	[Range(0, 3, ConfigUnitType.Seconds)]
	public Vector2 TargetDelay { get; set; } = new(0f, 0f);

	[UI("可作为移动目标的扇形角度大小",
		Description = "若选目标模式基于角色朝向，即角色视野范围内的目标为可移动目标。\n若选目标模式为屏幕中心，即从角色位置向上方绘制扇形范围内的目标为可移动目标。",
		Filter = TargetConfig, Section = 2)]
	[Range(0, 90, ConfigUnitType.Degree, 0.02f)]
	public float MoveTargetAngle { get; set; } = 24;

	[ConditionBool, UI("将木桩目标视为 Boss。",
		Filter = TargetConfig, Section = 3)]
	private static readonly bool _dummyBoss = true;

	[UI("若目标的 TTK 高于此值，则视为 Boss。",
		Filter = TargetConfig, Section = 1)]
	[Range(10, 1800, ConfigUnitType.Seconds, 0.02f)]
	public float BossTimeToKill { get; set; } = 90;

	[UI("若目标的 TTK 低于此值，则视为濒死。",
				Filter = TargetConfig, Section = 1)]
	[Range(0, 60, ConfigUnitType.Seconds, 0.02f)]
	public float DyingTimeToKill { get; set; } = 10;

	[UI("若目标的 HP 百分比低于此值，则视为濒死。",
				Filter = TargetConfig, Section = 1)]
	[Range(0, 0.1f, ConfigUnitType.Percent, 0.01f)]
	public float IsDyingConfig { get; set; } = 0.02f;

	[ConditionBool, UI("Prioritize Low HP targets instead of High HP targets when using Small Target and multiple Small targets present.",
		Filter = TargetConfig)]
	private static readonly bool _smallHP = false;

	[ConditionBool, UI("Prioritize Low HP targets instead of High HP targets when using Big Target and multiple Big targets present.",
		Filter = TargetConfig)]
	private static readonly bool _bigHP = false;

	[UI("/rotation 循环行为", Filter = TargetConfig)]
	public CycleType CycleType { get; set; } = CycleType.CycleNormal;

	[JobConfig, UI("Engage settings", Filter = TargetConfig, PvPFilter = JobFilterType.NoJob)]
	private readonly TargetHostileType _hostileType = TargetHostileType.AllTargetsWhenSoloInDuty;
	#endregion

	#region Integer

	public int ActionSequencerIndex { get; set; }

	[UI("临时解锁移动的修饰键", Description = "RB 适用于手柄玩家", Parent = nameof(PoslockCasting))]
	public ConsoleModifiers PoslockModifier { get; set; }

	[Range(0, 5, ConfigUnitType.None, 1)]
	[UI("每次动作模拟按键的随机范围", Parent = nameof(KeyboardNoise))]
	public Vector2Int KeyboardNoisePresses { get; set; } = new(2, 3);

	[Range(0, 10, ConfigUnitType.None)]
	public int TargetingIndex { get; set; }

	#endregion

	#region Jobs

	[JobConfig, UI("使用醒梦的 MP 阈值", Filter = HealingActionCondition)]
	[Range(0, 10000, ConfigUnitType.None)]
	private readonly int _lucidDreamingMpThreshold = 6000;

	/// <markdown file="Auto" name="AoE 治疗 oGCD 的 HP 阈值（持续回复）" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._autoHeal">
	/// 与 **oGCD** AoE 治疗技能相关，**当目标已由你或队友施加了持续回复效果时**。
	/// 
	/// 此值与<see cref="RotationSolver.Basic.Configuration.Configs.HealthDifference">标准 AoE 治疗偏差</see>共同计算。
	///
	/// 使用此设置旁的按钮计算你偏好的值。
	/// </markdown>
	[JobConfig, Range(0, 1, ConfigUnitType.Percent)]
	private readonly float _healthAreaAbilityHot = 0.55f;

	/// <markdown file="Auto" name="AoE 治疗 GCD 的 HP 阈值（持续回复）" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._autoHeal">
	/// 与 **GCD** AoE 治疗技能相关，**当目标已由你或队友施加了持续回复效果时**。
	/// 
	/// 此值与<see cref="RotationSolver.Basic.Configuration.Configs.HealthDifference">标准 AoE 治疗偏差</see>共同计算。
	///
	/// 使用此设置旁的按钮计算你偏好的值。
	/// </markdown>
	[JobConfig, Range(0, 1, ConfigUnitType.Percent)]
	private readonly float _healthAreaSpellHot = 0.55f;

	/// <markdown file="Auto" name="AoE 治疗 GCD 的 HP 阈值（无持续回复）" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._autoHeal">
	/// 与 **oGCD** AoE 治疗技能相关。
	/// 
	/// 此值与<see cref="RotationSolver.Basic.Configuration.Configs.HealthDifference">标准 AoE 治疗偏差</see>共同计算。
	///
	/// 使用此设置旁的按钮计算你偏好的值。
	/// </markdown> 
	[JobConfig, Range(0, 1, ConfigUnitType.Percent)]
	private readonly float _healthAreaAbility = 0.75f;

	/// <markdown file="Auto" name="AoE 治疗 GCD 的 HP 阈值（无持续回复）" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._autoHeal">
	/// 与 **GCD** AoE 治疗技能相关。
	/// 
	/// 此值与<see cref="RotationSolver.Basic.Configuration.Configs.HealthDifference">标准 AoE 治疗偏差</see>共同计算。
	///
	/// 使用此设置旁的按钮计算你偏好的值。
	/// </markdown> 
	[JobConfig, Range(0, 1, ConfigUnitType.Percent)]
	private readonly float _healthAreaSpell = 0.65f;

	/// <markdown file="Auto" name="单体治疗 oGCD 的 HP 阈值（持续回复）" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._autoHeal">
	/// 与 **oGCD** 单体治疗技能相关，**当目标已由你或队友施加了持续回复效果时**。
	/// </markdown>
	[JobConfig, Range(0, 1, ConfigUnitType.Percent)]
	private readonly float _healthSingleAbilityHot = 0.65f;

	/// <markdown file="Auto" name="单体治疗 GCD 的 HP 阈值（持续回复）" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._autoHeal">
	/// 与 **GCD** 单体治疗技能相关，**当目标已由你或队友施加了持续回复效果时**。
	/// </markdown>
	[JobConfig, Range(0, 1, ConfigUnitType.Percent)]
	private readonly float _healthSingleSpellHot = 0.55f;

	/// <markdown file="Auto" name="单体治疗 oGCD 的 HP 阈值（无持续回复）" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._autoHeal">
	/// 与 **oGCD** AoE 治疗技能相关。
	/// </markdown>
	[JobConfig, Range(0, 1, ConfigUnitType.Percent)]
	private readonly float _healthSingleAbility = 0.7f;

	/// <markdown file="Auto" name="单体治疗 GCD 的 HP 阈值（无持续回复）" section="Healing Usage and Control" subsection="RotationSolver.Basic.Configuration.Configs._autoHeal">
	/// 与 **GCD** 单体治疗技能相关。
	/// </markdown>
	[JobConfig, Range(0, 1, ConfigUnitType.Percent)]
	private readonly float _healthSingleSpell = 0.65f;

	/// <markdown file="Auto" name="坦克使用无敌技能的 HP%" section="Action Usage and Control">
	/// 当 HP 低于所设百分比时，自动使用坦克无敌技能的阈值。
	/// </markdown>
	[JobConfig, Range(0, 1, ConfigUnitType.Percent, 0.02f)]
	[UI("坦克使用无敌技能的 HP%%",
		Filter = AutoActionUsage, Section = 3,
		PvEFilter = JobFilterType.Tank, PvPFilter = JobFilterType.NoJob)]
	private readonly float _healthForDyingTanks = 0.15f;

	[JobConfig]
	private readonly string _PvPRotationChoice = string.Empty;

	[JobConfig]
	private readonly string _rotationChoice = string.Empty;
	#endregion

	[JobConfig]
	private readonly ConcurrentDictionary<uint, ActionConfig> _rotationActionConfig = new();

	[JobConfig]
	private readonly ConcurrentDictionary<uint, ItemConfig> _rotationItemConfig = new();

	[JobChoiceConfig]
	private readonly ConcurrentDictionary<string, string> _rotationConfigurations = new();

	public ConcurrentDictionary<uint, string> DutyRotationChoice = new();

	public void Save()
	{
#if DEBUG
		PluginLog.Information("Saved configurations.");
#endif
		File.WriteAllText(Svc.PluginInterface.ConfigFile.FullName,
			JsonConvert.SerializeObject(this, Formatting.Indented));
	}

	public static Configs Migrate(Configs oldConfigs)
	{
		// Implement migration logic if needed
		if (oldConfigs.Version != CurrentVersion)
		{
			// Reset to default if versions do not match
			return new Configs();
		}
		return oldConfigs;
	}

	public void Backup()
	{
		Save();
		File.Copy(Svc.PluginInterface.ConfigFile.FullName, Svc.PluginInterface.ConfigFile.Directory + "\\RotationSolver_Backup.json", true);
		Svc.Toasts.ShowNormal("配置已备份。");
	}

	public void Restore()
	{
		File.Copy(Svc.PluginInterface.ConfigFile.FullName, Svc.PluginInterface.ConfigFile.Directory + "\\RotationSolver_SafetySave.json", true);
		File.Copy(Svc.PluginInterface.ConfigFile.Directory + "\\RotationSolver_Backup.json", Svc.PluginInterface.ConfigFile.FullName, true);

		var restoredConfigs = JsonConvert.DeserializeObject<Configs>(
									  File.ReadAllText(Svc.PluginInterface.ConfigFile.FullName))
								  ?? new Configs();

		if (restoredConfigs.Version != CurrentVersion)
		{
			Svc.Toasts.ShowNormal("备份的配置与当前版本不兼容。");
			return;
		}

		Service.Config = restoredConfigs;
		Save();
		Svc.Toasts.ShowNormal("配置已恢复。将关闭以应用设置。");
		DataCenter.HoldingRestore = true;
	}
}
