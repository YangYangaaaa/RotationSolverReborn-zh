namespace RotationSolver.Basic.Data;

/// <summary>
/// Hostile target.
/// </summary>
public enum DTRType : byte
{
	/// <summary>
	/// Cycle between first Auto, Manual, and Off
	/// </summary>
	[Description("在「首个自动、手动、关闭」之间循环")]
	DTRNormal,

	/// <summary>
	/// Cycle between each Auto, Manual, and Off
	/// </summary>
	[Description("在「每个自动、手动、关闭」之间循环")]
	DTRAllAuto,

	/// <summary>
	/// Cycle between Auto and Off
	/// </summary>
	[Description("在「自动、关闭」之间循环")]
	DTRAuto,

	/// <summary>
	/// Cycle between Manual and Off
	/// </summary>
	[Description("在「手动、关闭」之间循环")]
	DTRManual,

	/// <summary>
	/// Cycle between Manual and Auto
	/// </summary>
	[Description("在「手动、自动」之间循环")]
	DTRManualAuto,
}