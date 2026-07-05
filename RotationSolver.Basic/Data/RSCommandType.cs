namespace RotationSolver.Basic.Data;

/// <summary>
/// Special State.
/// </summary>
public enum SpecialCommandType : byte
{
	/// <summary>
	/// To end this special duration before the set time.
	/// </summary>
	[Description("在设定时间前结束特殊状态窗口。")]
	EndSpecial,

	/// <summary>
	/// Open a window to use AoE heal.
	/// </summary>
	[Description("开启群体治疗窗口。")]
	HealArea,

	/// <summary>
	/// Open a window to use single heal.
	/// </summary>
	[Description("开启单体治疗窗口。")]
	HealSingle,

	/// <summary>
	/// Open a window to use AoE defense.
	/// </summary>
	[Description("开启群体减伤窗口。")]
	DefenseArea,

	/// <summary>
	/// Open a window to use single defense.
	/// </summary>
	[Description("开启单体减伤窗口。")]
	DefenseSingle,

	/// <summary>
	/// Open a window to use Esuna, tank stance actions or True North.
	/// </summary>
	[Description("开启医术/坦克姿态/真北窗口。")]
	DispelStancePositional,

	/// <summary>
	/// Open a window to use Raise or Shirk.
	/// </summary>
	[Description("开启复活/转嫁仇恨窗口。")]
	RaiseShirk,

	/// <summary>
	/// Open a window to move forward.
	/// </summary>
	[Description("开启前冲窗口。")]
	MoveForward,

	/// <summary>
	/// Open a window to move back.
	/// </summary>
	[Description("开启后撤窗口。")]
	MoveBack,

	/// <summary>
	/// Open a window to use knockback immunity actions.
	/// </summary>
	[Description("开启防击退窗口。")]
	AntiKnockback,

	/// <summary>
	/// Open a window to burst.
	/// </summary>
	[Description("开启爆发窗口。")]
	Burst,

	/// <summary>
	/// Open a window to speed up.
	/// </summary>
	[Description("开启加速窗口。")]
	Speed,

	/// <summary>
	/// Open a window to use limit break.
	/// </summary>
	[Description("开启极限技窗口。")]
	LimitBreak,

	/// <summary>
	/// Open a window to do not use the casting action.
	/// </summary>
	[Description("开启停止读条窗口。")]
	NoCasting,

	/// <summary>
	/// Intercepting action.
	/// </summary>
	[Description("RSR 拦截技能时的指示器。")]
	Intercepting,
}

/// <summary>
/// The state of the plugin.
/// </summary>
public enum StateCommandType : byte
{
	/// <summary>
	/// Stop the addon. Always remember to turn it off when it is not in use!
	/// </summary>
	[Description("关闭插件。不使用时请务必关闭！")]
	Off,

	/// <summary>
	/// Start the addon in Auto mode. When out of combat or when combat starts, switches the target according to the set condition.
	/// </summary>
	[Description("以自动模式启动插件。非战斗或战斗开始时，按设定条件切换目标。 " +
		"\r\n 可选：可在命令末尾追加目标类型，例如：/rotation Auto Big")]
	Auto,

	/// <summary>
	/// Start the addon in Target-Only mode. RSR will auto-select targets per normal logic but will not perform any actions.
	/// </summary>
	[Description("以仅目标模式启动。RSR 将按正常逻辑自动选取目标但不执行任何技能。")]
	TargetOnly,

	/// <summary>
	/// Start the addon in Manual mode. You need to choose the target manually. This will bypass any engage settings that you have set up and will start attacking immediately once something is targeted.
	/// </summary>
	[Description("以手动模式启动插件。需手动选择目标，此模式将绕过所有交战设置，锁定目标后立即开始攻击。")]
	Manual,

	/// <summary>
	/// 
	/// </summary>
	[Description("此模式由 AutoDuty 插件管理")]
	AutoDuty,

	/// <summary>
	/// 
	/// </summary>
	[Description("此模式由 Henchman 插件或其他仅需 RSR 执行循环而不需选目标的插件管理。")]
	Henched,

	/// <summary>
	/// 
	/// </summary>
	[Description("PvP 专用活动可选模式。")]
	PvP,
}

/// <summary>
/// CN DTR strings.
/// </summary>
public static class StateCommandTypeExtensions
{
	/// <summary>
	///
	/// </summary>
	public static string CNString(this StateCommandType stateCommandType)
	{
		return stateCommandType switch
		{
			StateCommandType.Off => "关闭",
			StateCommandType.Auto => "自动目标",
			StateCommandType.TargetOnly => "仅目标",
			StateCommandType.Manual => "手动目标",
			StateCommandType.AutoDuty => "AutoDuty",
			StateCommandType.Henched => "Henched",
			StateCommandType.PvP => "PvP",
			_ => stateCommandType.ToString(),
		};
	}
}

/// <summary>
/// Some Other Commands.
/// </summary>
public enum OtherCommandType : byte
{
	/// <summary>
	/// Open the settings.
	/// </summary>
	[Description("打开设置。")]
	Settings,

	/// <summary>
	/// Open the rotations.
	/// </summary>
	[Description("打开循环列表。")]
	Rotations,

	/// <summary>
	/// Open the rotations.
	/// </summary>
	[Description("打开副本循环列表。")]
	DutyRotations,

	/// <summary>
	/// Perform the actions.
	/// </summary>
	[Description("执行技能。")]
	DoActions,

	/// <summary>
	/// Toggle the actions.
	/// </summary>
	[Description("切换技能开关。")]
	ToggleActions,

	/// <summary>
	/// Do the next action.
	/// </summary>
	[Description("执行下一技能。")]
	NextAction,

	/// <summary>
	/// Cycles between states following settings in Target > Configuration.
	/// </summary>
	[Description("按「目标 > 配置」中的设置在状态间循环。")]
	Cycle,
}