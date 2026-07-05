using ECommons.DalamudServices;
using System.ComponentModel;

namespace RotationSolver.UI;

/// <summary>
/// Attribute to mark tabs that should be skipped.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
internal class TabSkipAttribute : Attribute
{
}

/// <summary>
/// Attribute to specify an icon for a tab.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
internal class TabIconAttribute : Attribute
{
	public uint Icon { get; init; }
}

/// <summary>
/// Enum representing different tabs in the rotation config window.
/// </summary>
internal enum RotationConfigWindowTab : byte
{
	[TabSkip] About,
	[TabSkip] Rotation,

	[Description("实用信息与宏列表。")]
	[TabIcon(Icon = 4)] Main,

	[Description("循环专属配置。")]
	[TabIcon(Icon = 4)] Job,

	[Description("配置副本循环。")]
	[TabIcon(Icon = 4)] DutyRotation,

	[Description("配置当前职业的技能与自定义条件。")]
	[TabIcon(Icon = 4)] Actions,

	[Description("配置反应性技能与状态效果列表。")]
	[TabIcon(Icon = 21)] List,

	[Description("配置基础设置。")]
	[TabIcon(Icon = 14)] Basic,

	[Description("配置用户界面设置。")]
	[TabIcon(Icon = 42)] UI,

	[Description("配置通用技能使用与控制设置。")]
	[TabIcon(Icon = 29)] Auto,

	[Description("配置目标选择设置。")]
	[TabIcon(Icon = 16)] Target,

	[Description("副本专属设置。")]
	[TabIcon(Icon = 16)] Duty,

	[Description("配置可选的辅助功能。")]
	[TabIcon(Icon = 51)] Extra,

	[Description("开发者与循环编写者的调试选项（不使用时请禁用）。")]
	[TabIcon(Icon = 5)] Debug,

	[Description("配置 AutoDuty 设置并查看相关信息。")]
	[TabIcon(Icon = 4)] AutoDuty,
}

internal static class RotationConfigWindowTabExtensions
{
	public static string CNString(this RotationConfigWindowTab rotationConfigWindowTab)
	{
		return rotationConfigWindowTab switch
		{
			RotationConfigWindowTab.About => "关于",
			RotationConfigWindowTab.Rotation => "循环",
			RotationConfigWindowTab.Main => "主窗口",
			RotationConfigWindowTab.Job => "职业",
			RotationConfigWindowTab.Duty => "任务",
			RotationConfigWindowTab.Actions => "技能",
			RotationConfigWindowTab.List => "列表",
			RotationConfigWindowTab.Basic => "基础",
			RotationConfigWindowTab.UI => "界面",
			RotationConfigWindowTab.Auto => "自动",
			RotationConfigWindowTab.Target => "目标",
			RotationConfigWindowTab.Extra => "额外",
			RotationConfigWindowTab.Debug => "调试",
			_ => rotationConfigWindowTab.ToString()
		};
	}
}

/// <summary>
/// Struct representing an incompatible plugin.
/// </summary>
public readonly struct AutoDutyPlugin
{
	public string Name { get; init; }
	public string Icon { get; init; }
	public string Url { get; init; }
	public string Features { get; init; }

	/// <summary>
	/// Checks if the plugin is enabled.
	/// </summary>
	[JsonIgnore]
	public readonly bool IsEnabled
	{
		get
		{
			var name = Name;
			var installedPlugins = Svc.PluginInterface.InstalledPlugins;
			foreach (var x in installedPlugins)
			{
				if ((x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) || x.InternalName.Equals(name, StringComparison.OrdinalIgnoreCase)) && x.IsLoaded)
				{
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>
	/// Checks if the plugin is installed.
	/// </summary>
	[JsonIgnore]
	public readonly bool IsInstalled
	{
		get
		{
			var name = Name;
			var installedPlugins = Svc.PluginInterface.InstalledPlugins;
			foreach (var x in installedPlugins)
			{
				if (x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) || x.InternalName.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
	}
}