using System.ComponentModel;

namespace RotationSolver.Data
{
	internal enum UiString
	{
		[Description("你选择的条件值。点击修改。")]
		ConfigWindow_ConditionSetDesc,

		[Description("条件值")]
		ConfigWindow_ConditionSet,

		[Description("技能条件")]
		ConfigWindow_ActionSet,

		[Description("特性条件")]
		ConfigWindow_TraitSet,

		[Description("目标条件")]
		ConfigWindow_TargetSet,

		[Description("循环条件")]
		ConfigWindow_RotationSet,

		[Description("命名条件")]
		ConfigWindow_NamedSet,

		[Description("区域条件")]
		ConfigWindow_Territoryset,

		[Description("未加载任何循环！请检查循环选项卡！")]
		ConfigWindow_NoRotation,

		[Description("当前副本逻辑。")]
		ConfigWindow_DutyRotationDesc,

		[Description("移除")]
		ConfigWindow_List_Remove,

		[Description("从文件夹加载")]
		ActionSequencer_Load,

		[Description("分析每一帧的 PvE 战斗信息，找到最优技能。")]
		ConfigWindow_About_Punchline,

		[Description("循环无效！\n请更新到最新版本，或联系 {0}！")]
		ConfigWindow_Rotation_InvalidRotation,

		[Description("点击切换循环")]
		ConfigWindow_Helper_SwitchRotation,

		[Description("搜索结果")]
		ConfigWindow_Search_Result,

		[Description("这包含一帧战斗中几乎所有可用信息，包括全队成员的状态、敌对目标的状态、技能冷却、角色的 MP 和 HP、角色位置、敌对目标读条状态、连击状态、战斗时长、玩家等级等。\n\n随后它会在热栏上高亮最优技能，或帮你点击它。")]
		ConfigWindow_About_Description,

		[Description("本工具为一般战斗设计，不针对零式或绝境战内容。\n\n请谨慎使用！虽然 RSR 并非为零式或绝境战设计，但它在这些内容中也能正常工作，只是不会替你处理机制。请注意机制并使用宏。")]
		ConfigWindow_About_Warning,

		[Description("RSR 已帮你点击了 {0:N0} 次技能。")]
		ConfigWindow_About_ClickingCount,

		[Description("状态宏")]
		ConfigWindow_About_Macros,

		[Description("技能与设置宏")]
		ConfigWindow_About_SettingMacros,

		[Description("兼容性")]
		ConfigWindow_About_Compatibility,

		[Description("支持者")]
		ConfigWindow_About_Supporters,

		[Description("链接")]
		ConfigWindow_About_Links,

		[Description("系统警告")]
		ConfigWindow_About_Warnings,

		[Description("警告信息")]
		ConfigWindow_About_Warnings_Warning,

		[Description("警告时间")]
		ConfigWindow_About_Warnings_Time,

		[Description("Rotation Solver 帮你选择目标并点击技能。任何改变这两者的插件都会影响它的决策。\n\n以下是历史上（但不总是）曾导致兼容问题的插件列表：")]
		ConfigWindow_About_Compatibility_Description,

		[Description("无法正确执行 RSR 想要执行的行为。")]
		ConfigWindow_About_Compatibility_Mistake,

		[Description("与 RSR 决策冲突")]
		ConfigWindow_About_Compatibility_Mislead,

		[Description("导致游戏崩溃")]
		ConfigWindow_About_Compatibility_Crash,

		[Description("非常感谢 Ko-fi 的赞助者。")]
		ConfigWindow_About_ThanksToSupporters,

		[Description("打开配置文件夹")]
		ConfigWindow_About_OpenConfigFolder,

		[Description("描述")]
		ConfigWindow_Rotation_Description,

		[Description("配置")]
		ConfigWindow_Rotation_Configuration,

		[Description("副本配置")]
		ConfigWindow_DutyRotation_Configuration,

		[Description("状态")]
		ConfigWindow_Rotation_Status,

		[Description("副本循环状态")]
		ConfigWindow_DutyRotation_Status,

		[Description("用于自定义 RSR 自动使用特定技能的时机。点击左侧列表中的技能图标。在下方你可以设置该技能的使用条件，每个技能可以有不同的条件以覆盖默认循环行为。")]
		ConfigWindow_Actions_Description,

		[Description("显示在冷却窗口")]
		ConfigWindow_Actions_ShowOnCDWindow,

		[Description("允许该技能被拦截系统拦截")]
		ConfigWindow_Actions_IsIntercepted,

		[Description("针对精选怪物列表（如 猎人偶）阻止此技能")]
		ConfigWindow_Actions_IsRestrictedDOT,

		[Description("允许该技能受最低 HP 功能限制")]
		ConfigWindow_Actions_MinHPFeature,

		[Description("目标低于此百分比时，不使用此技能")]
		ConfigWindow_Actions_MinHPPercent,

		[Description("为此移动技能跳过 BossModReborn 的位置安全检查")]
		ConfigWindow_Actions_SkipPositionSafetyCheck,

		[Description("使用此技能所需的击杀时长（TTK）阈值")]
		ConfigWindow_Actions_TTK,

		[Description("使用此技能所需的目标数量")]
		ConfigWindow_Actions_AoeCount,

		[Description("此技能是否检查所需的状态效果")]
		ConfigWindow_Actions_CheckStatus,

		[Description("此技能是否检查目标所需的状态效果")]
		ConfigWindow_Actions_CheckTargetStatus,

		[Description("重新施加 DoT/状态效果前的 GCD 数")]
		ConfigWindow_Actions_GcdCount,

		[Description("自动治疗的 HP 比例（仅适用于治疗技能）")]
		ConfigWindow_Actions_HealRatio,

		[Description("强制条件拥有更高优先级。若强制条件满足，禁用条件将被忽略。")]
		ConfigWindow_Actions_ConditionDescription,

		[Description("强制条件（未支持）")]
		ConfigWindow_Actions_ForcedConditionSet,

		[Description("强制自动使用技能的条件")]
		ConfigWindow_Actions_ForcedConditionSet_Description,

		[Description("禁用条件（未支持）")]
		ConfigWindow_Actions_DisabledConditionSet,

		[Description("禁用技能自动使用的条件")]
		ConfigWindow_Actions_DisabledConditionSet_Description,

		[Description("在此窗口中，你可以使用列表设置可自定义的参数。")]
		ConfigWindow_List_Description,

		[Description("状态")]
		ConfigWindow_List_Statuses,

		[Description("技能")]
		ConfigWindow_List_Actions,

		[Description("地图专属设置")]
		ConfigWindow_List_Territories,

		[Description("状态名称或 ID")]
		ConfigWindow_List_StatusNameOrId,

		[Description("无敌")]
		ConfigWindow_List_Invincibility,

		[Description("优先")]
		ConfigWindow_List_Priority,

		[Description("可驱散的负面状态")]
		ConfigWindow_List_DangerousStatus,

		[Description("止息类负面状态")]
		ConfigWindow_List_NoCastingStatus,

		[Description("若目标拥有其中任一状态，则忽略该目标")]
		ConfigWindow_List_InvincibilityDesc,

		[Description("若目标拥有其中任一状态，则优先攻击该目标")]
		ConfigWindow_List_PriorityDesc,

		[Description("可驱散的负面状态列表")]
		ConfigWindow_List_DangerousStatusDesc,

		[Description("若你拥有其中任一负面状态，则不采取行动")]
		ConfigWindow_List_NoCastingStatusDesc,

		[Description("复制到剪贴板")]
		ConfigWindow_Actions_Copy,

		[Description("从剪贴板粘贴")]
		ActionSequencer_FromClipboard,

		[Description("添加状态")]
		ConfigWindow_List_AddStatus,

		[Description("技能名称或 ID")]
		ConfigWindow_List_ActionNameOrId,

		[Description("坦克死刑")]
		ConfigWindow_List_HostileCastingTank,

		[Description("AoE")]
		ConfigWindow_List_HostileCastingArea,

		[Description("击退")]
		ConfigWindow_List_HostileCastingKnockback,

		[Description("凝视/止息")]
		ConfigWindow_List_HostileCastingStop,

		[Description("若目标正在施放其中任一技能，则使用坦克个人减伤技能")]
		ConfigWindow_List_HostileCastingTankDesc,

		[Description("若目标正在施放其中任一技能，则使用 AoE 减伤技能")]
		ConfigWindow_List_HostileCastingAreaDesc,

		[Description("若目标正在施放其中任一技能，则使用防击退技能")]
		ConfigWindow_List_HostileCastingKnockbackDesc,

		[Description("若敌人正在施放此技能，则停止读条或不采取行动")]
		ConfigWindow_List_HostileCastingStopDesc,

		[Description("添加技能")]
		ConfigWindow_List_AddAction,

		[Description("不作为目标")]
		ConfigWindow_List_NoHostile,

		[Description("不挑衅")]
		ConfigWindow_List_NoProvoke,

		[Description("有益 AoE 位置")]
		ConfigWindow_List_BeneficialPositions,

		[Description("永远不会作为目标的敌人")]
		ConfigWindow_List_NoHostileDesc,

		[Description("你不想作为目标的敌人名称")]
		ConfigWindow_List_NoHostilesName,

		[Description("永远不会被挑衅的敌人")]
		ConfigWindow_List_NoProvokeDesc,

		[Description("你不想挑衅的敌人名称")]
		ConfigWindow_List_NoProvokeName,

		[Description("添加有益 AoE 位置")]
		ConfigWindow_List_AddPosition,

		[Description("能力技")]
		ActionAbility,

		[Description("友好")]
		ActionFriendly,

		[Description("攻击")]
		ActionAttack,

		[Description("普通目标")]
		NormalTargets,

		[Description("拥有持续恢复（HoT）的目标")]
		HotTargets,

		[Description("AoE 治疗 oGCD 的 HP 阈值")]
		HpAoe0Gcd,

		[Description("AoE 治疗 GCD 的 HP 阈值")]
		HpAoeGcd,

		[Description("单体治疗 oGCD 的 HP 阈值")]
		HpSingle0Gcd,

		[Description("单体治疗 GCD 的 HP 阈值")]
		HpSingleGcd,

		[Description("不移动")]
		InfoWindowNoMove,

		[Description("移动")]
		InfoWindowMove,

		[Description("设置搜索")]
		ConfigWindow_Searching,

		[Description("计时器")]
		ConfigWindow_Basic_Timer,

		[Description("自动切换")]
		ConfigWindow_Basic_AutoSwitch,

		[Description("命名条件")]
		ConfigWindow_Basic_NamedConditions,

		[Description("其他")]
		ConfigWindow_Basic_Others,

		[Description("单个技能的动画锁定时间。例如 0.6 秒。")]
		ConfigWindow_Basic_AnimationLockTime,

		[Description("点击持续时长——RSR 会尝试在此刻点击。")]
		ConfigWindow_Basic_ClickingDuration,

		[Description("理想点击时间")]
		ConfigWindow_Basic_IdealClickingTime,

		[Description("实际点击时间")]
		ConfigWindow_Basic_RealClickingTime,

		[Description("自动关闭条件")]
		ConfigWindow_Basic_SwitchCancelConditionSet,

		[Description("自动手动模式条件")]
		ConfigWindow_Basic_SwitchManualConditionSet,

		[Description("自动自动模式条件")]
		ConfigWindow_Basic_SwitchAutoConditionSet,

		[Description("条件名称")]
		ConfigWindow_Condition_ConditionName,

		[Description("信息")]
		ConfigWindow_UI_Information,

		[Description("覆盖层")]
		ConfigWindow_UI_Overlay,

		[Description("窗口")]
		ConfigWindow_UI_Windows,

		[Description("更改 RSR 自动使用技能的方式")]
		ConfigWindow_Auto_Description,

		[Description("技能使用与控制")]
		ConfigWindow_Auto_ActionUsage,

		[Description("RSR 可以使用哪些技能")]
		ConfigWindow_Auto_ActionUsage_Description,

		[Description("治疗使用与控制")]
		ConfigWindow_Auto_HealingCondition,

		[Description("RSR 应如何使用治疗技能")]
		ConfigWindow_Auto_HealingCondition_Description,

		[Description("自定义状态条件（未支持）")]
		ConfigWindow_Auto_StateCondition,

		[Description("群体治疗强制条件")]
		ConfigWindow_Auto_HealAreaConditionSet,

		[Description("单体治疗强制条件")]
		ConfigWindow_Auto_HealSingleConditionSet,

		[Description("群体减伤强制条件")]
		ConfigWindow_Auto_DefenseAreaConditionSet,

		[Description("单体减伤强制条件")]
		ConfigWindow_Auto_DefenseSingleConditionSet,

		[Description("驱散/姿态/身位强制条件")]
		ConfigWindow_Auto_DispelStancePositionalConditionSet,

		[Description("复活/转嫁仇恨强制条件")]
		ConfigWindow_Auto_RaiseShirkConditionSet,

		[Description("前进强制条件")]
		ConfigWindow_Auto_MoveForwardConditionSet,

		[Description("后退强制条件")]
		ConfigWindow_Auto_MoveBackConditionSet,

		[Description("防击退强制条件")]
		ConfigWindow_Auto_AntiKnockbackConditionSet,

		[Description("加速强制条件")]
		ConfigWindow_Auto_SpeedConditionSet,

		[Description("止息条件集")]
		ConfigWindow_Auto_NoCastingConditionSet,

		[Description("这将改变 RSR 使用技能的方式")]
		ConfigWindow_Auto_ActionCondition_Description,

		[Description("配置")]
		ConfigWindow_Target_Config,

		[Description("敌对")]
		ConfigWindow_List_Hostile,

		[Description("敌对目标选择逻辑。使用 /rotation Auto 时会循环切换添加的选项。\n使用 /rotation Settings TargetingTypes add <选项> 添加，\n/rotation Settings TargetingTypes remove <选项> 移除，\n/rotation Settings TargetingTypes removeall 移除所有选项。")]
		ConfigWindow_Param_HostileDesc,

		[Description("上移")]
		ConfigWindow_Actions_MoveUp,

		[Description("下移")]
		ConfigWindow_Actions_MoveDown,

		[Description("敌对目标选择条件")]
		ConfigWindow_Param_HostileCondition,

		[Description("RSR 专注于循环本身。这些是附加功能，随时可能被移除。")]
		ConfigWindow_Extra_Description,

		[Description("事件")]
		ConfigWindow_EventItem,

		[Description("内部")]
		ConfigWindow_Internal,

		[Description("其他")]
		ConfigWindow_Extra_Others,

		[Description("添加事件")]
		ConfigWindow_Events_AddEvent,

		[Description("在此窗口中，你可以设置使用某个技能后触发的宏。")]
		ConfigWindow_Events_Description,

		[Description("副本开始：")]
		ConfigWindow_Events_DutyStart,

		[Description("副本结束：")]
		ConfigWindow_Events_DutyEnd,

		[Description("删除事件")]
		ConfigWindow_Events_RemoveEvent,

		[Description("点击使其反转。\n已反转：{0}")]
		ActionSequencer_NotDescription,

		[Description("成员名称")]
		ConfigWindow_Actions_MemberName,

		[Description("循环为空。请登录或切换职业！")]
		ConfigWindow_Condition_RotationNullWarning,

		[Description("绝境战")]
		ConfigWindow_Duty_Ultimate,

		[Description("零式")]
		ConfigWindow_Duty_Savage,

		[Description("混沌联盟突袭")]
		ConfigWindow_Duty_ChaoticAlliance,

		[Description("极蛮神")]
		ConfigWindow_Duty_Extreme,

		[Description("地下城")]
		ConfigWindow_Duty_Dungeon,

		[Description("深层迷宫")]
		ConfigWindow_Duty_DeepDungeon,

		[Description("异闻迷宫")]
		ConfigWindow_Duty_VariantDungeon,

		[Description("宝物迷宫")]
		ConfigWindow_Duty_TreasureDungeon,

		[Description("联盟突袭")]
		ConfigWindow_Duty_Alliance,

		[Description("野外作战")]
		ConfigWindow_Duty_FieldOps,

		[Description("PvP")]
		ConfigWindow_Duty_PvP,

		[Description("假面狂欢")]
		ConfigWindow_Duty_TheMaskedCarnivale,

		[Description("未破之熔炉")]
		ConfigWindow_Duty_CrucibleOfTheUnbroken,

		[Description("延迟其切换为 true 的时机")]
		ActionSequencer_Delay_Description,

		[Description("延迟其切换")]
		ActionSequencer_Offset_Description,

		[Description("等级足够")]
		ActionConditionType_EnoughLevel,

		[Description("时间偏移")]
		ActionSequencer_TimeOffset,

		[Description("充能层数")]
		ActionSequencer_Charges,

		[Description("原始")]
		ActionSequencer_Original,

		[Description("调整后")]
		ActionSequencer_Adjusted,

		[Description("{0} 的目标")]
		ActionSequencer_ActionTarget,

		[Description("来自所有人")]
		ActionSequencer_StatusAll,

		[Description("来自自己")]
		ActionSequencer_StatusSelf,

		[Description("你不应该使用此项，因为此目标不是该技能的目标。请尝试从技能中选择。")]
		ConfigWindow_Condition_TargetWarning,

		[Description("区域名称")]
		ConfigWindow_Condition_TerritoryName,

		[Description("副本名称")]
		ConfigWindow_Condition_DutyName,

		[Description("请在 {0} 中单独绑定减伤/护盾冷却，以防 RSR 在关键时刻失效！")]
		HighEndWarning,

		[Description("点击执行命令")]
		ConfigWindow_Helper_RunCommand,

		[Description("右键复制命令")]
		ConfigWindow_Helper_CopyCommand,

		[Description("宏编号")]
		ConfigWindow_Events_MacroIndex,

		[Description("共享")]
		ConfigWindow_Events_ShareMacro,

		[Description("技能名称")]
		ConfigWindow_Events_ActionName,

		[Description("将 {0} 修改为 {1}")]
		CommandsChangeSettingsValue,

		[Description("在此循环中找不到该配置。请检查。")]
		CommandsCannotFindConfig,

		[Description("将在 {0} 秒内使用")]
		CommandsInsertAction,

		[Description("找不到该技能。请检查技能名称。")]
		CommandsInsertActionFailure,

		[Description("无法从字符串同时获取值和配置。请确保你同时提供了配置选项和值。")]
		CommandsMissingArgument,

		[Description("开始")]
		SpecialCommandType_Start,

		[Description("取消")]
		SpecialCommandType_Cancel,

		[Description("群体治疗")]
		SpecialCommandType_HealArea,

		[Description("单体治疗")]
		SpecialCommandType_HealSingle,

		[Description("群体减伤")]
		SpecialCommandType_DefenseArea,

		[Description("单体减伤")]
		SpecialCommandType_DefenseSingle,

		[Description("坦克姿态")]
		SpecialCommandType_TankStance,

		[Description("驱散")]
		SpecialCommandType_Dispel,

		[Description("身位")]
		SpecialCommandType_Positional,

		[Description("转嫁仇恨")]
		SpecialCommandType_Shirk,

		[Description("复活")]
		SpecialCommandType_Raise,

		[Description("前进")]
		SpecialCommandType_MoveForward,

		[Description("后退")]
		SpecialCommandType_MoveBack,

		[Description("防击退")]
		SpecialCommandType_AntiKnockback,

		[Description("爆发")]
		SpecialCommandType_Burst,

		[Description("结束特殊")]
		SpecialCommandType_EndSpecial,

		[Description("加速")]
		SpecialCommandType_Speed,

		[Description("极限技")]
		SpecialCommandType_LimitBreak,

		[Description("止息")]
		SpecialCommandType_NoCasting,

		[Description("自动目标")]
		SpecialCommandType_Smart,

		[Description("手动目标")]
		SpecialCommandType_Manual,

		[Description("关闭")]
		SpecialCommandType_Off,

		[Description("打开配置窗口")]
		Commands_Rotation,

		[Description("启动 RSR 战斗循环状态")]
		Commands_Start,

		[Description("禁用 RSR 战斗循环状态")]
		Commands_Off,

		[Description("Rotation Solver Reborn 设置 v")]
		ConfigWindowHeader,

		[Description("此配置为职业专属")]
		JobConfigTip,

		[Description("此选项在你当前职业下不可用\n\n所需角色或职业：\n{0}")]
		NotInJob,

		[Description("欢迎使用 Rotation Solver Reborn！")]
		WelcomeWindow_Header,

		[Description("这是你上次离开后错过的内容")]
		WelcomeWindow_WelcomeBack,

		[Description("看起来你可能是新来的！让我们带你上手！")]
		WelcomeWindow_Welcome,

		[Description("最近的变更：")]
		WelcomeWindow_Changelog,
	}

	public static class EnumExtensions
	{
		private static readonly Dictionary<Enum, string> _enumDescriptions = [];

		public static string GetDescription(this Enum value)
		{
			if (_enumDescriptions.TryGetValue(value, out var description))
			{
				return description;
			}

			var field = value.GetType().GetField(value.ToString());
			if (field == null)
			{
				_enumDescriptions.Add(value, value.ToString());
				return value.ToString();
			}

			var attribute = field.GetCustomAttribute<DescriptionAttribute>();

			var descString = attribute == null ? value.ToString() : attribute.Description;
			_enumDescriptions.Add(value, descString);
			return descString;
		}
	}
}
