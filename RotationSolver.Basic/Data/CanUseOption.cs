namespace RotationSolver.Basic.Data;

/// <summary>
/// Specifies options for determining whether an action can be used.
/// </summary>
[Flags]
public enum CanUseOption : byte
{
	/// <summary>
	/// No options specified.
	/// </summary>
	None,

	/// <summary>
	/// Skip status provide check.
	/// </summary>
	[Description("跳过提供状态检查")]
	SkipStatusProvideCheck = 1 << 0,

	/// <summary>
	/// Skip combo check.
	/// </summary>
	[Description("跳过连击检查")]
	SkipComboCheck = 1 << 1,

	/// <summary>
	/// Skip casting and moving check.
	/// </summary>
	[Description("跳过读条与移动检查")]
	SkipCastingCheck = 1 << 2,

	/// <summary>
	/// Indicates that all stacks should be used up.
	/// </summary>
	[Description("消耗全部充能")]
	UsedUp = 1 << 3,

	/// <summary>
	/// Indicates that the action is the last ability.
	/// </summary>
	[Description("在最后一个 oGCD 上")]
	OnLastAbility = 1 << 4,

	/// <summary>
	/// Skip clipping check.
	/// </summary>
	[Description("跳过卡 GCD 检查")]
	SkipClippingCheck = 1 << 5,

	/// <summary>
	/// Skip AoE check.
	/// </summary>
	[Description("跳过 AoE 检查")]
	SkipAoeCheck = 1 << 6,

	/// <summary>
	/// Overriding targettype.
	/// </summary>
	[Description("覆盖目标类型")]
	targetOverride = 1 << 7,
}