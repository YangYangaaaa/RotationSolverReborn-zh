using ECommons.DalamudServices;
using ECommons.EzIpcManager;
using ECommons.Reflection;
#pragma warning disable CS0169 // Field is never used
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
#nullable disable

namespace RotationSolver.IPC
{
	using System.ComponentModel;

	//internal static class BossModReborn_IPCSubscriber
	//{
	//	private static readonly EzIPCDisposalToken[] _disposalTokens = EzIPC.Init(typeof(BossModReborn_IPCSubscriber), "BossMod", SafeWrapper.AnyException);

	//	internal static bool IsEnabled => IPCSubscriber_Common.IsReady("BossModReborn");

	//	[EzIPC("AI.GetPreset", true)] internal static readonly Func<string> Presets_GetActive;

	//	[EzIPC("AI.SetPreset", true)] internal static readonly Action<string> Presets_SetActive;

	//	internal static void Dispose() => IPCSubscriber_Common.DisposeAll(_disposalTokens);
	//}

	//internal static class BossMod_IPCSubscriber
	//{
	//	private static readonly EzIPCDisposalToken[] _disposalTokens = EzIPC.Init(typeof(BossMod_IPCSubscriber), "BossMod", SafeWrapper.AnyException);

	//	internal static bool IsEnabled => IPCSubscriber_Common.IsReady("BossMod") || IPCSubscriber_Common.IsReady("BossModReborn");

	//	[EzIPC] internal static readonly Func<uint, bool> HasModuleByDataId;
	//	[EzIPC] internal static readonly Func<IReadOnlyList<string>, bool, List<string>> Configuration;
	//	[EzIPC("Presets.Get", true)] internal static readonly Func<string, string> Presets_Get;
	//	[EzIPC("Presets.Create", true)] internal static readonly Func<string, bool, bool> Presets_Create;
	//	[EzIPC("Presets.Delete", true)] internal static readonly Func<string, bool> Presets_Delete;
	//	[EzIPC("Presets.GetActive", true)] internal static readonly Func<string> Presets_GetActive;
	//	[EzIPC("Presets.SetActive", true)] internal static readonly Func<string, bool> Presets_SetActive;
	//	[EzIPC("Presets.ClearActive", true)] internal static readonly Func<bool> Presets_ClearActive;
	//	[EzIPC("Presets.GetForceDisabled", true)] internal static readonly Func<bool> Presets_GetForceDisabled;
	//	[EzIPC("Presets.SetForceDisabled", true)] internal static readonly Func<bool> Presets_SetForceDisabled;
	//	/** string presetName, string moduleTypeName, string trackName, string value*/
	//	[EzIPC("Presets.AddTransientStrategy")] internal static readonly Func<string, string, string, string, bool> Presets_AddTransientStrategy;

	//	internal static void Dispose() => IPCSubscriber_Common.DisposeAll(_disposalTokens);
	//}

	public static class Wrath_IPCSubscriber
	{
		public enum CancellationReason
		{
			[Description("Wrath 用户手动撤销了你的租约。")]
			WrathUserManuallyCancelled,
			[Description("检测到你的插件已被禁用，不过你不太可能看到此消息。")]
			LeaseePluginDisabled,
			[Description("Wrath 插件正在被禁用。")]
			WrathPluginDisabled,
			[Description("你的租约已通过 IPC 调用释放，理论上这是你自己的操作。")]
			LeaseeReleased,
			[Description("IPC 服务已被远程禁用。请查看 /res/ipc_status.txt 的提交历史。\n https://github.com/PunishXIV/WrathCombo/commits/main/res/ipc_status.txt")]
			AllServicesSuspended,
		}

		public enum AutoRotationConfigOption
		{
			InCombatOnly = 0, // bool
			DPSRotationMode = 1, // enum
			HealerRotationMode = 2, // enum
			FATEPriority = 3, // bool
			QuestPriority = 4, // bool
			SingleTargetHPP = 5, // int
			AoETargetHPP = 6, // int
			SingleTargetRegenHPP = 7, // int
			ManageKardia = 8, // bool
			AutoRez = 9, // bool
			AutoRezDPSJobs = 10, // bool
			AutoCleanse = 11, // bool
			IncludeNPCs = 12, // bool
			OnlyAttackInCombat = 13, //bool
		}

		public enum DPSRotationMode
		{
			Manual = 0,
			Highest_Max = 1,
			Lowest_Max = 2,
			Highest_Current = 3,
			Lowest_Current = 4,
			Tank_Target = 5,
			Nearest = 6,
			Furthest = 7,
		}

		public enum HealerRotationMode
		{
			Manual = 0,
			Highest_Current = 1,
			Lowest_Current = 2
		}

		public enum SetResult
		{
			[Description("不应出现的默认值。")]
			IGNORED = -1,
			[Description("配置设置成功。")]
			Okay = 0,
			[Description("配置将被设置，正在异步处理。")]
			OkayWorking = 1,
			[Description("IPC 服务当前已禁用。")]
			IPCDisabled = 10,
			[Description("无效的租约。")]
			InvalidLease = 11,
			[Description("已拉黑的租约。")]
			BlacklistedLease = 12,
			[Description("尝试设置的配置已被设置。")]
			Duplicate = 13,
			[Description("玩家对象不可用。")]
			PlayerNotAvailable = 14,
			[Description("尝试设置的配置不可用。")]
			InvalidConfiguration = 15,
			[Description("尝试设置的值无效。")]
			InvalidValue = 16,
		}

		private static Guid? _curLease;

		internal static bool IsEnabled => IPCSubscriber_Common.IsReady("WrathCombo");

		private static readonly EzIPCDisposalToken[] _disposalTokens = EzIPC.Init(typeof(Wrath_IPCSubscriber), "WrathCombo", SafeWrapper.IPCException);

		[EzIPC] private static readonly Func<string, string, string, Guid?> RegisterForLeaseWithCallback;
		[EzIPC] internal static readonly Func<bool> GetAutoRotationState;
		[EzIPC] private static readonly Func<Guid, bool, SetResult> SetAutoRotationState;
		[EzIPC] private static readonly Action<Guid> ReleaseControl;

		public static bool DoThing(Func<SetResult> action)
		{
			var result = action();
			var check = result.CheckResult();
			if (!check && result == SetResult.InvalidLease)
			{
				check = action().CheckResult();
			}

			return check;
		}

		private static bool CheckResult(this SetResult result)
		{
			switch (result)
			{
				case SetResult.Okay:
				case SetResult.OkayWorking:
					return true;
				case SetResult.InvalidLease:
					_curLease = null;
					Register();
					return false;
				case SetResult.IPCDisabled:
				case SetResult.Duplicate:
				case SetResult.PlayerNotAvailable:
				case SetResult.InvalidConfiguration:
				case SetResult.InvalidValue:
				case SetResult.IGNORED:
					return false;
				default:
					throw new ArgumentOutOfRangeException(nameof(result), result, null);
			}
		}

		// Minimal API: only disabling Auto-Rotation
		internal static void DisableAutoRotation()
		{
			if (Register())
			{
				DoThing(() => SetAutoRotationState(_curLease!.Value, false));
			}
		}

		internal static void Release()
		{
			if (_curLease.HasValue)
			{
				ReleaseControl(_curLease.Value);
				_curLease = null;
			}
		}

		internal static void Dispose()
		{
			Release();
			IPCSubscriber_Common.DisposeAll(_disposalTokens);
		}

		// Callback name must be resolvable by Wrath; provide a no-op handler.
		// The callback signature is reflected by Wrath; keep it stable.
		public static void LeaseCancelled(CancellationReason reason, string info)
		{
			// Intentionally minimal: just clear our lease so subsequent calls re-register.
			_curLease = null;
		}

		private static bool Register()
		{
			if (_curLease.HasValue)
			{
				return true;
			}

			if (!IsEnabled)
			{
				return false;
			}

			// Use Dalamud plugin info for internal and display names where available.
			var internalName = Svc.PluginInterface.InternalName ?? "RotationSolver";
			var displayName = Svc.PluginInterface.Manifest?.Name ?? "Rotation Solver";
			var callbackName = $"{typeof(Wrath_IPCSubscriber).FullName}.LeaseCancelled";

			_curLease = RegisterForLeaseWithCallback(internalName, displayName, callbackName);
			return _curLease.HasValue;
		}
	}

	internal class IPCSubscriber_Common
	{
		internal static bool IsReady(string pluginName) => DalamudReflector.TryGetDalamudPlugin(pluginName, out _, false, true);

		internal static Version Version(string pluginName) => DalamudReflector.TryGetDalamudPlugin(pluginName, out var dalamudPlugin, false, true) ? dalamudPlugin.GetType().Assembly.GetName().Version : new Version(0, 0, 0, 0);

		internal static void DisposeAll(EzIPCDisposalToken[] _disposalTokens)
		{
			foreach (var token in _disposalTokens)
			{
				try
				{
					token.Dispose();
				}
				catch (Exception ex)
				{
					Svc.Log.Error($"Error while unregistering IPC: {ex}");
				}
			}
		}
	}
}