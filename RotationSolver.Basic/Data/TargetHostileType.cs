namespace RotationSolver.Basic.Data;

/// <summary>
/// Hostile target.
/// </summary>
public enum TargetHostileType : byte
{
	/// <summary>
	/// All targets that are in range for any abilities (Tanks/Autoduty).
	/// </summary>
	[Description("所有技能范围内的目标（坦克/AutoDuty）")]
	AllTargetsCanAttack,

	/// <summary>
	/// Previously engaged targets (Non-Tanks).
	/// </summary>
	[Description("已交战过的目标（非坦克）")]
	TargetsHaveTarget,

	/// <summary>
	/// All targets when solo in duty, or previously engaged.
	/// </summary>
	[Description("单人副本中攻击所有目标（含灵异新月），或已交战过的目标。")]
	AllTargetsWhenSoloInDuty,

	/// <summary>
	/// All targets when solo, or previously engaged.
	/// </summary>
	[Description("单人时攻击所有目标，或已交战过的目标。")]
	AllTargetsWhenSolo,

	/// <summary>
	/// Solo Deep Dungeons: out of combat pull the nearest single enemy; in combat only previously engaged.
	/// </summary>
	[Description("单人深层迷宫：单人非战斗时拉最近的单个敌人；战斗中仅攻击已交战目标。")]
	SoloDeepDungeonSmart,

	//[Description("Only attack targets in your parties enemy list")]
	//TargetIsInEnemiesList,

	//[Description("All targets when solo, or only attack targets in your parties enemy list")]
	//AllTargetsWhenSoloTargetIsInEnemiesList,

	//[Description("All targets when solo in duty, or only attack targets in your parties enemy list")]
	//AllTargetsWhenSoloInDutyTargetIsInEnemiesList,
}
