namespace RotationSolver.Basic.Data;

/// <summary>
/// The type of description.
/// </summary>
public enum DescType : byte
{
	/// <summary>
	/// No description.
	/// </summary>
	None,

	/// <summary>
	/// Burst actions.
	/// </summary>
	[Description("爆发技能")]
	BurstActions,

	/// <summary>
	/// Area heal GCDs.
	/// </summary>
	[Description("群体治疗 GCD")]
	HealAreaGCD,

	/// <summary>
	/// Area heal oGCDs.
	/// </summary>
	[Description("群体治疗 oGCD")]
	HealAreaAbility,

	/// <summary>
	/// Single target heal GCDs.
	/// </summary>
	[Description("单体治疗 GCD")]
	HealSingleGCD,

	/// <summary>
	/// Single target heal oGCDs.
	/// </summary>
	[Description("单体治疗 oGCD")]
	HealSingleAbility,

	/// <summary>
	/// Area defensive GCDs (shields, mitigation, etc).
	/// </summary>
	[Description("群体减伤 GCD")]
	DefenseAreaGCD,

	/// <summary>
	/// Area defensive oGCDs (shields, mitigation, etc).
	/// </summary>
	[Description("群体减伤 oGCD")]
	DefenseAreaAbility,

	/// <summary>
	/// Single target defensive GCDs (shields, mitigation, etc).
	/// </summary>
	[Description("单体减伤 GCD")]
	DefenseSingleGCD,

	/// <summary>
	/// Single target defensive oGCDs (shields, mitigation, etc).
	/// </summary>
	[Description("单体减伤 oGCD")]
	DefenseSingleAbility,

	/// <summary>
	/// Move forward GCD.
	/// </summary>
	[Description("前冲 GCD")]
	MoveForwardGCD,

	/// <summary>
	/// Move forward ability.
	/// </summary>
	[Description("前冲 oGCD")]
	MoveForwardAbility,

	/// <summary>
	/// Move back ability.
	/// </summary>
	[Description("后撤 oGCD")]
	MoveBackAbility,

	/// <summary>
	/// Speed ability.
	/// </summary>
	[Description("加速 oGCD")]
	SpeedAbility,
}