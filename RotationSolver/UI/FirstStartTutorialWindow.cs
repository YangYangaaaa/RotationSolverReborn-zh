using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ECommons.DalamudServices;

namespace RotationSolver.UI;

internal sealed class FirstStartTutorialWindow : Window
{
	private const ImGuiWindowFlags BaseFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoResize;
	private int _stepIndex;

	private static readonly string[] StarterMacros =
	[
		"/rotation Settings AoEType Full\r\n/rotation Auto",
		"/rotation Settings AoEType Cleave\r\n/rotation Manual",
		"/rotation Off",
	];

	private static readonly TutorialStep[] Steps =
	[
		new(
			"欢迎！",
			"本向导将介绍如何配置 Rotation Solver Reborn，以及各个部分的功能，并附带推荐宏。",
			Bullets:
			[
				"使用 /rotation 或插件 UI 按钮打开配置窗口。",
				"使用「下一步/上一步」在各部分之间切换，并随时应用更改。",
				"大多数设置在登录游戏中更改是安全的，但在战斗中调整前请先熟悉。",
				"右键点击任意设置或技能标签可复制其宏命令。"
			]),
		new(
			"主界面",
			"Main 是你的总览中心：插件信息、兼容性信息、链接和宏列表都在这里。",
			RotationConfigWindowTab.Main,
			[
				"使用此标签页检查不兼容的插件并打开支持链接。",
				"阅读宏命令部分以了解快捷聊天命令。",
				"如果更新后出现问题，请先检查此标签页。"
			]),
		new(
			"职业设置",
			"Job 配置控制循环选择和你当前职业的专属选项。",
			RotationConfigWindowTab.Job,
			[
				"点击循环名称（如 Reborn）选择你想运行的循环预设。",
				"适用时调整职业优先级（如 DNC 舞伴、SGE Kardia）。",
				"如果某个职业感觉不对，先从这里开始检查，再动全局设置。"
			]),
		new(
			"技能",
			"Actions 配置决定 RSR 可以使用哪些技能以及它们的行为方式。",
			RotationConfigWindowTab.Actions,
			[
				"点击分类中的技能图标可查看设置、启用/禁用或更改使用规则。",
				"如果你希望 RSR 触发你手动排队的技能，请启用「拦截」。",
				"切换「在冷却窗口显示」以让覆盖层只显示你想要的技能。"
			]),
		new(
			"Auto",
			"Auto 控制全局技能使用、AoE 逻辑、打断、爆发药和治疗行为。",
			RotationConfigWindowTab.Auto,
			[
				"此处可调整 AoE 逻辑（Off、Cleave 和 Full）。",
				"调整治疗阈值和非治疗职业的辅助选项。",
				"如果想要更保守的循环，请先收紧这些设置。"
			]),
		new(
			"Basic",
			"Basic 包含影响所有职业的核心时序和自动化行为。",
			RotationConfigWindowTab.Basic,
			[
				"提前量影响穿插数量和卡顿——数值越小 oGCD 越多。通常不需要更改。",
				"最小更新时间用性能换取响应速度。",
				"Auto Switch 控制 RSR 何时自动开启/关闭（倒计时、死亡、副本事件等）。"
			]),
		new(
			"UI",
			"UI 控制覆盖层、信息窗口和教学模式高亮。",
			RotationConfigWindowTab.UI,
			[
				"此处启用控制、下一技能、冷却和时间轴窗口。",
				"使用教学模式高亮热键栏按钮，直观学习循环。",
				"如果希望窗口仅在副本中或有敌人时显示，请在此切换该选项。"
			]),
		new(
			"Target",
			"Target 控制 RSR 认为有效的敌人或队友。",
			RotationConfigWindowTab.Target,
			[
				"调整视野锥形和接敌行为以避免意外开怪。",
				"配置目标优先级规则（FATE、任务怪、标记）。",
				"如果目标选择感觉不对，请先调整过滤器再更改循环。"
			]),
		new(
			"List",
			"List 管理精选状态列表：驱散、优先目标、击退等。",
			RotationConfigWindowTab.List,
			[
				"需要时使用「重置并更新」恢复精选列表。",
				"使用 + 按钮按 ID 或名称添加或移除状态。",
				"这些列表驱动所有职业的智能反应。"
			]),
		new(
			"Duty",
			"Duty 存放副本专属的特殊行为开关。",
			RotationConfigWindowTab.Duty,
			[
				"目前大部分选项可以保持启用，未来会有更精细的控制。",
				"这些设置会在特定战斗中覆盖通用目标/循环行为。"
			]),
		new(
			"Extra",
			"Extra 用于高级或实验性调整。",
			RotationConfigWindowTab.Extra,
			[
				"为不使用 BMR 的用户提供动画锁和冷却延迟调整。",
				"仅在了解副作用时才更改这些设置。",
			]),
		new(
			"宏",
			"入门宏让你无需打开 UI 即可快速控制 RSR。",
			RotationConfigWindowTab.Main,
			[
				"使用下方宏可即时切换 Auto/Manual/Off。",
				"右键点击任意设置或技能可复制其宏命令。",
				"建立一个小宏栏以便战斗中快速控制。"
			],
			StarterMacros),
	];

	public FirstStartTutorialWindow()
		: base("RSR First Start Tutorial", BaseFlags)
	{
		Size = new Vector2(720, 530);
		SizeCondition = ImGuiCond.FirstUseEver;
		RespectCloseHotkey = true;
	}

	public override bool DrawConditions()
	{
		return DataCenter.PlayerAvailable();
	}

	public override void Draw()
	{
		var step = Steps[_stepIndex];

		ImGui.PushFont(FontManager.GetFont(ImGui.GetFontSize() + 6));
		ImGui.TextColored(ImGuiColors.ParsedGold, step.Title);
		ImGui.PopFont();

		DrawWrappedText(step.Description);
		ImGui.Spacing();

		if (step.Bullets is { Length: > 0 })
		{
			foreach (var bullet in step.Bullets)
			{
				DrawWrappedBullet(bullet);
			}
			ImGui.Spacing();
		}

		if (step.RecommendedMacros is { Length: > 0 })
		{
			ImGui.TextColored(ImGuiColors.HealerGreen, "推荐宏:");
			for (var i = 0; i < step.RecommendedMacros.Length; i++)
			{
				var macro = step.RecommendedMacros[i];
				DrawWrappedBullet(macro);

				ImGui.SameLine();
				var buttonId = $"复制##TutorialMacro_{i}";
				if (ImGui.SmallButton(buttonId))
				{
					ImGui.SetClipboardText(macro);
				Svc.Toasts.ShowNormal("宏已复制到剪贴板。");
				}
			}

			ImGui.Spacing();
		}

		if (step.Tab != null)
		{
			if (ImGui.Button($"打开 {step.Tab} 标签"))
			{
				RotationSolverPlugin.ShowConfigWindow(step.Tab.Value);
			}
			ImGui.Spacing();
		}

		ImGui.Separator();
		DrawNavigation();
	}

	public override void OnClose()
	{
		MarkTutorialComplete();
		base.OnClose();
	}

	private static void DrawWrappedText(string text)
	{
		var wrapPos = ImGui.GetCursorPosX() + ImGui.GetContentRegionAvail().X;
		ImGui.PushTextWrapPos(wrapPos);
		ImGui.TextWrapped(text);
		ImGui.PopTextWrapPos();
	}

	private static void DrawWrappedBullet(string text)
	{
		ImGui.Bullet();
		ImGui.SameLine();
		DrawWrappedText(text);
	}

	private void DrawNavigation()
	{
		ImGui.BeginDisabled(_stepIndex == 0);
		if (ImGui.Button("上一步"))
		{
			_stepIndex = Math.Max(0, _stepIndex - 1);
		}
		ImGui.EndDisabled();

		ImGui.SameLine();

		if (_stepIndex < Steps.Length - 1)
		{
			if (ImGui.Button("下一步"))
			{
				_stepIndex = Math.Min(Steps.Length - 1, _stepIndex + 1);
			}
		}
		else
		{
			if (ImGui.Button("完成"))
			{
				FinishTutorial();
			}
		}
	}

	private void FinishTutorial()
	{
		MarkTutorialComplete();
		IsOpen = false;
	}

	private static void MarkTutorialComplete()
	{
		Service.Config.TutorialDone = true;
		Service.Config.Save();
	}

	private sealed record TutorialStep(
		string Title,
		string Description,
		RotationConfigWindowTab? Tab = null,
		string[]? Bullets = null,
		string[]? RecommendedMacros = null);
}