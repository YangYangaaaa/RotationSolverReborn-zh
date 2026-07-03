namespace RotationSolver.Basic.Data;

/// <summary>
/// Hostile target.
/// </summary>
public enum HardCastRaiseType : byte
{
	/// <summary>
	///
	/// </summary>
	[Description("不进行硬读条复活")]
	NoHardCast,

	/// <summary>
	///
	/// </summary>
	[Description("Swiftcast 冷却时进行硬读条复活")]
	HardCastNormal,

	/// <summary>
	///
	/// </summary>
	[Description("Swiftcast 冷却且其他治疗已死亡时硬读条复活")]
	HardCastOnlyHealer,

	/// <summary>
	///
	/// </summary>
	[Description("Swiftcast 冷却且剩余冷却大于复活读条时间时硬读条复活")]
	HardCastSwiftCooldown,

	/// <summary>
	/// 
	/// </summary>
	[Description("Swiftcast 冷却且剩余冷却大于复活读条时间且其他治疗已死亡时硬读条复活")]
	HardCastOnlyHealerSwiftCooldown,
}