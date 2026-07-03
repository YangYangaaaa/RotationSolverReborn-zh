namespace RotationSolver.Basic.Data;

/// <summary>
/// The type of targeting.
/// </summary>
public enum TargetingType
{
	/// <summary>
	/// Find the target whose hit box is biggest.
	/// </summary>
	[Description("大体型")]
	Big,

	/// <summary>
	/// Find the target whose hit box is smallest.
	/// </summary>
	[Description("小体型")]
	Small,

	/// <summary>
	/// Find the target whose HP is highest.
	/// </summary>
	[Description("高血量")]
	HighHP,

	/// <summary>
	/// Find the target whose HP is lowest.
	/// </summary>
	[Description("低血量")]
	LowHP,

	/// <summary>
	/// Find the target whose HP percentage is highest.
	/// </summary>
	[Description("高血量百分比")]
	HighHPPercent,

	/// <summary>
	/// Find the target whose HP percentage is lowest.
	/// </summary>
	[Description("低血量百分比")]
	LowHPPercent,

	/// <summary>
	/// Find the target whose max HP is highest.
	/// </summary>
	[Description("最大血量最高")]
	HighMaxHP,

	/// <summary>
	/// Find the target whose max HP is lowest.
	/// </summary>
	[Description("最大血量最低")]
	LowMaxHP,

	/// <summary>
	/// Find the target that is nearest.
	/// </summary>
	[Description("最近")]
	Nearest,

	/// <summary>
	/// Find the target that is farthest.
	/// </summary>
	[Description("最远")]
	Farthest,

	/// <summary>
	/// PVP: Find the nearest Healer.
	/// </summary>
	[Description("PvP 中优先锁定治疗")]
	PvPHealers,

	/// <summary>
	/// PVP: Find the nearest Tank.
	/// </summary>
	[Description("PvP 中优先锁定坦克")]
	PvPTanks,

	/// <summary>
	/// PVP: Find the nearest DPS.
	/// </summary>
	[Description("PvP 中优先锁定输出")]
	PvPDPS
}