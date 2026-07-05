namespace RotationSolver.Basic.Data;

/// <summary>
/// Where to use tinctures.
/// </summary>
public enum TinctureUseType : byte
{
	/// <summary>
	/// Do not use tinctures.
	/// </summary>
	[Description("不使用幻药/烈药/药剂")]
	Nowhere,

	/// <summary>
	/// Only use tinctures in high-end duties.
	/// </summary>
	[Description("在高难副本中使用幻药/烈药/药剂")]
	InHighEndDuty,

	/// <summary>
	/// Use tinctures anywhere.
	/// </summary>
	[Description("任意场合使用幻药/烈药/药剂")]
	Anywhere,
}