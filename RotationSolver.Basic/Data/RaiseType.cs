namespace RotationSolver.Basic.Data;

/// <summary>
/// Specifies who to raise.
/// </summary>
public enum RaiseType : byte
{
	/// <summary>
	/// Raise only party members.
	/// </summary>
	[Description("仅复活小队成员。")]
	PartyOnly,

	/// <summary>
	/// Raise party members and alliance supports.
	/// </summary>
	[Description("复活小队成员与联盟辅助。")]
	PartyAndAllianceSupports,

	/// <summary>
	/// Raise party members and alliance healers.
	/// </summary>
	[Description("复活小队成员与联盟治疗。")]
	PartyAndAllianceHealers,

	/// <summary>
	/// Raise All In Duty.
	/// </summary>
	[Description("副本内复活所有人。")]
	All,

	/// <summary>
	/// Raise all.
	/// </summary>
	[Description("复活所有人。")]
	AllOutOfDuty,

	/// <summary>
	/// Raise all.
	/// </summary>
	[Description("仅复活小队治疗。")]
	PartyHealersOnly,
}