using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.Logging;
using Newtonsoft.Json.Converters;

namespace RotationSolver.Basic.Configuration;

internal class OtherConfiguration
{
	private static readonly HttpClient Http = CreateHttpClient();
	private static HttpClient CreateHttpClient()
	{
		var c = new HttpClient();
		try
		{
			c.DefaultRequestHeaders.UserAgent.ParseAdd("RotationSolver");
			c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
		}
		catch { /* best-effort headers */ }
		return c;
	}
	/// <markdown file="List" name="AoE" section="Actions">
	/// **`建议每个补丁后点击重置按钮。`**
	///
	/// 若仇恨列表中任意敌人正在施放列表中的技能,RSR 将使用群体减伤。
	/// 这些技能通常是范围攻击。
	/// </markdown>
	public static HashSet<uint> HostileCastingArea = [];

	/// <markdown file="List" name="Tank Buster" section="Actions">
	/// **`建议每个补丁后点击重置按钮。`**
	///
	/// 若目标正在被列表中的技能锁定,RSR 将对目标(治疗)或自身(坦克)使用减伤。
	/// </markdown>
	public static HashSet<uint> HostileCastingTank = [];

	/// <markdown file="List" name="Knockback" section="Actions">
	/// **`建议每个补丁后点击重置按钮。`**
	///
	/// **点击"记录击退技能"需自行承担风险。某些副本需要你利用击退到达正确安全点,
	/// 例如西尔狄赫水道(零式)。**
	///
	/// 当你将被列表中的技能击退时,RSR 会使用防击退技能。
	/// </markdown>
	public static HashSet<uint> HostileCastingKnockback = [];

	/// <markdown file="List" name="Gaze/Stop" section="Actions">
	/// **`建议每个补丁后点击重置按钮。`**
	///
	/// 若目标正在施放列表中的技能,RSR 将在技能判定前数秒停止读条
	/// <see cref="RotationSolver.Basic.Configuration.Configs._castingStop">此处</see>。
	/// </markdown>
	public static HashSet<uint> HostileCastingStop = [];

	public static Dictionary<uint, string[]> NoHostileNames = [];
	public static Dictionary<uint, string[]> NoProvokeNames = [];

	/// <markdown file="List" name="Beneficial Positions" section="Map-Specific Settings">
	/// 添加一个偏好位置,用于地面**治疗** AoE 技能(例如:地星)。
	///
	/// 你可以添加多个位置,以防 Boss 战将你转移到另一个平台,例如 M4S - 邪恶之雷。
	/// </markdown>
	public static Dictionary<uint, Vector3[]> BeneficialPositions = [];

	/// <markdown file="List" name="Dispellable Debuffs" section="Statuses">
	/// **`建议每个补丁后点击重置按钮。`**
	///
	/// 列表中的状态将优先于其他可驱散状态被驱散(康复)。
	/// </markdown>
	public static HashSet<uint> DangerousStatus = [];

	/// <markdown file="List" name="Priority" section="Statuses">
	/// **`建议每个补丁后点击重置按钮。`**
	///
	/// 在自动模式下,若仇恨列表中任意敌人带有此状态,将优先将其作为目标。
	/// </markdown>
	public static HashSet<uint> PriorityStatus = [];

	/// <markdown file="List" name="Invulnerability" section="Statuses">
	/// **`建议每个补丁后点击重置按钮。`**
	///
	/// 若目标带有列表中的状态,将被视为无敌并忽略。
	/// </markdown>
	public static HashSet<uint> InvincibleStatus = [];

	/// <markdown file="List" name="No-Casting Debuffs" section="Statuses">
	/// **`建议每个补丁后点击重置按钮。`**
	///
	/// 若你带有列表中的任意状态,RSR 将停止所有行动。
	/// </markdown>
	public static HashSet<uint> NoCastingStatus = [];
	public static List<Job> DancePartnerPriority = [];
	public static List<Job> TheSpearPriority = [];
	public static List<Job> TheBalancePriority = [];
	public static List<Job> KardiaTankPriority = [];

	public static RotationSolverRecord RotationSolverRecord = new();

	public static void Init()
	{
		if (!Directory.Exists(Svc.PluginInterface.ConfigDirectory.FullName))
		{
			_ = Directory.CreateDirectory(Svc.PluginInterface.ConfigDirectory.FullName);
		}

		_ = Task.Run(() => InitOne(ref DangerousStatus, nameof(DangerousStatus)));
		_ = Task.Run(() => InitOne(ref PriorityStatus, nameof(PriorityStatus)));
		_ = Task.Run(() => InitOne(ref InvincibleStatus, nameof(InvincibleStatus)));
		_ = Task.Run(() => InitOne(ref DancePartnerPriority, nameof(DancePartnerPriority)));
		_ = Task.Run(() => InitOne(ref TheSpearPriority, nameof(TheSpearPriority)));
		_ = Task.Run(() => InitOne(ref TheBalancePriority, nameof(TheBalancePriority)));
		_ = Task.Run(() => InitOne(ref KardiaTankPriority, nameof(KardiaTankPriority)));
		_ = Task.Run(() => InitOne(ref NoHostileNames, nameof(NoHostileNames)));
		_ = Task.Run(() => InitOne(ref NoProvokeNames, nameof(NoProvokeNames)));
		_ = Task.Run(() => InitOne(ref HostileCastingArea, nameof(HostileCastingArea)));
		_ = Task.Run(() => InitOne(ref HostileCastingTank, nameof(HostileCastingTank)));
		_ = Task.Run(() => InitOne(ref BeneficialPositions, nameof(BeneficialPositions)));
		_ = Task.Run(() => InitOne(ref RotationSolverRecord, nameof(RotationSolverRecord), false));
		_ = Task.Run(() => InitOne(ref NoCastingStatus, nameof(NoCastingStatus)));
		_ = Task.Run(() => InitOne(ref HostileCastingKnockback, nameof(HostileCastingKnockback)));
		_ = Task.Run(() => InitOne(ref HostileCastingStop, nameof(HostileCastingStop)));
	}

	public static async Task InitAsync(CancellationToken cancellationToken = default)
	{
		if (!Directory.Exists(Svc.PluginInterface.ConfigDirectory.FullName))
		{
			_ = Directory.CreateDirectory(Svc.PluginInterface.ConfigDirectory.FullName);
		}

		await Task.WhenAll(
			Task.Run(() => InitOne(ref DangerousStatus, nameof(DangerousStatus)), cancellationToken),
			Task.Run(() => InitOne(ref PriorityStatus, nameof(PriorityStatus)), cancellationToken),
			Task.Run(() => InitOne(ref InvincibleStatus, nameof(InvincibleStatus)), cancellationToken),
			Task.Run(() => InitOne(ref DancePartnerPriority, nameof(DancePartnerPriority)), cancellationToken),
			Task.Run(() => InitOne(ref TheSpearPriority, nameof(TheSpearPriority)), cancellationToken),
			Task.Run(() => InitOne(ref TheBalancePriority, nameof(TheBalancePriority)), cancellationToken),
			Task.Run(() => InitOne(ref KardiaTankPriority, nameof(KardiaTankPriority)), cancellationToken),
			Task.Run(() => InitOne(ref NoHostileNames, nameof(NoHostileNames)), cancellationToken),
			Task.Run(() => InitOne(ref NoProvokeNames, nameof(NoProvokeNames)), cancellationToken),
			Task.Run(() => InitOne(ref HostileCastingArea, nameof(HostileCastingArea)), cancellationToken),
			Task.Run(() => InitOne(ref HostileCastingTank, nameof(HostileCastingTank)), cancellationToken),
			Task.Run(() => InitOne(ref BeneficialPositions, nameof(BeneficialPositions)), cancellationToken),
			Task.Run(() => InitOne(ref RotationSolverRecord, nameof(RotationSolverRecord), false), cancellationToken),
			Task.Run(() => InitOne(ref NoCastingStatus, nameof(NoCastingStatus)), cancellationToken),
			Task.Run(() => InitOne(ref HostileCastingKnockback, nameof(HostileCastingKnockback)), cancellationToken),
			Task.Run(() => InitOne(ref HostileCastingStop, nameof(HostileCastingStop)), cancellationToken)
		);
	}

	public static Task Save()
	{
		return Task.Run(async () =>
		{
			await SavePriorityStatus();
			await SaveDangerousStatus();
			await SaveInvincibleStatus();
			await SaveDancePartnerPriority();
			await SaveTheSpearPriority();
			await SaveTheBalancePriority();
			await SaveKardiaTankPriority();
			await SaveNoHostileNames();
			await SaveHostileCastingArea();
			await SaveHostileCastingTank();
			await SaveBeneficialPositions();
			await SaveRotationSolverRecord();
			await SaveNoProvokeNames();
			await SaveNoCastingStatus();
			await SaveHostileCastingKnockback();
			await SaveHostileCastingStop();
		});
	}
	#region Action Tab
	public static void ResetHostileCastingArea()
	{
		InitOne(ref HostileCastingArea, nameof(HostileCastingArea), true, true);
		SaveHostileCastingArea().Wait();
	}

	public static void ResetHostileCastingTank()
	{
		InitOne(ref HostileCastingTank, nameof(HostileCastingTank), true, true);
		SaveHostileCastingTank().Wait();
	}

	public static void ResetHostileCastingKnockback()
	{
		InitOne(ref HostileCastingKnockback, nameof(HostileCastingKnockback), true, true);
		SaveHostileCastingKnockback().Wait();
	}

	public static void ResetHostileCastingStop()
	{
		InitOne(ref HostileCastingStop, nameof(HostileCastingStop), true, true);
		SaveHostileCastingStop().Wait();
	}

	public static Task SaveHostileCastingArea()
	{
		return Task.Run(() => Save(HostileCastingArea, nameof(HostileCastingArea)));
	}

	public static Task SaveHostileCastingTank()
	{
		return Task.Run(() => Save(HostileCastingTank, nameof(HostileCastingTank)));
	}

	private static Task SaveHostileCastingKnockback()
	{
		return Task.Run(() => Save(HostileCastingKnockback, nameof(HostileCastingKnockback)));
	}

	private static Task SaveHostileCastingStop()
	{
		return Task.Run(() => Save(HostileCastingStop, nameof(HostileCastingStop)));
	}
	#endregion

	#region Status Tab

	public static void ResetPriorityStatus()
	{
		InitOne(ref PriorityStatus, nameof(PriorityStatus), true, true);
		SavePriorityStatus().Wait();
	}

	public static void ResetInvincibleStatus()
	{
		InitOne(ref InvincibleStatus, nameof(InvincibleStatus), true, true);
		SaveInvincibleStatus().Wait();
	}

	public static void ResetDangerousStatus()
	{
		InitOne(ref DangerousStatus, nameof(DangerousStatus), true, true);
		SaveDangerousStatus().Wait();
	}

	public static void ResetNoCastingStatus()
	{
		InitOne(ref NoCastingStatus, nameof(NoCastingStatus), true, true);
		SaveNoCastingStatus().Wait();
	}

	public static Task SavePriorityStatus()
	{
		return Task.Run(() => Save(PriorityStatus, nameof(PriorityStatus)));
	}

	public static Task SaveInvincibleStatus()
	{
		return Task.Run(() => Save(InvincibleStatus, nameof(InvincibleStatus)));
	}

	public static Task SaveDangerousStatus()
	{
		return Task.Run(() => Save(DangerousStatus, nameof(DangerousStatus)));
	}

	public static Task SaveNoCastingStatus()
	{
		return Task.Run(() => Save(NoCastingStatus, nameof(NoCastingStatus)));
	}

	#endregion
	public static Task SaveRotationSolverRecord()
	{
		return Task.Run(() => Save(RotationSolverRecord, nameof(RotationSolverRecord)));
	}

	public static Task SaveNoProvokeNames()
	{
		return Task.Run(() => Save(NoProvokeNames, nameof(NoProvokeNames)));
	}

	public static Task SaveBeneficialPositions()
	{
		return Task.Run(() => Save(BeneficialPositions, nameof(BeneficialPositions)));
	}

	public static void ResetDancePartnerPriority()
	{
		InitOne(ref DancePartnerPriority, nameof(DancePartnerPriority), true, true);
		SaveDancePartnerPriority().Wait();
	}

	public static void ResetTheSpearPriority()
	{
		InitOne(ref TheSpearPriority, nameof(TheSpearPriority), true, true);
		SaveTheSpearPriority().Wait();
	}

	public static void ResetTheBalancePriority()
	{
		InitOne(ref TheBalancePriority, nameof(TheBalancePriority), true, true);
		SaveTheBalancePriority().Wait();
	}

	public static void ResetKardiaTankPriority()
	{
		InitOne(ref KardiaTankPriority, nameof(KardiaTankPriority), true, true);
		SaveKardiaTankPriority().Wait();
	}

	public static Task SaveDancePartnerPriority()
	{
		return Task.Run(() => Save(DancePartnerPriority, nameof(DancePartnerPriority)));
	}

	public static Task SaveTheSpearPriority()
	{
		return Task.Run(() => Save(TheSpearPriority, nameof(TheSpearPriority)));
	}

	public static Task SaveTheBalancePriority()
	{
		return Task.Run(() => Save(TheBalancePriority, nameof(TheBalancePriority)));
	}

	public static Task SaveKardiaTankPriority()
	{
		return Task.Run(() => Save(KardiaTankPriority, nameof(KardiaTankPriority)));
	}

	public static Task SaveNoHostileNames()
	{
		return Task.Run(() => Save(NoHostileNames, nameof(NoHostileNames)));
	}

	private static string GetFilePath(string name)
	{
		var directory = Svc.PluginInterface.ConfigDirectory.FullName;

		return directory + $"\\{name}.json";
	}

	private static void Save<T>(T value, string name)
	{
		SavePath(value, GetFilePath(name));
	}

	private static void SavePath<T>(T value, string path)
	{
		var retryCount = 3;
		var delay = 1000; // 1 second delay

		for (var i = 0; i < retryCount; i++)
		{
			try
			{
				File.WriteAllText(path,
				JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings()
				{
					TypeNameHandling = TypeNameHandling.None,
				}));
				return; // Exit the method if successful
			}
			catch (IOException ex) when (i < retryCount - 1)
			{
				PluginLog.Warning($"Failed to save the file to {path}. Retrying in {delay}ms...: {ex.Message}");
				Thread.Sleep(delay); // Wait before retrying
			}
			catch (Exception ex)
			{
				PluginLog.Warning($"Failed to save the file to {path}: {ex.Message}");
				return; // Exit the method if an unexpected exception occurs
			}
		}
	}

	private static void InitOne<T>(ref T value, string name, bool download = true, bool forceDownload = false) where T : new()
	{
		var path = GetFilePath(name);
		PluginLog.Information($"Initializing {name} from {path}");

		if (File.Exists(path) && !forceDownload)
		{
			try
			{
				value = JsonConvert.DeserializeObject<T>(File.ReadAllText(path), new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.None,
					Converters = [new StringEnumConverter()] // Add this line
				})! ?? throw new Exception("Deserialized value is null.");
				PluginLog.Information($"Loaded {name} from local file.");
			}
			catch (Exception ex)
			{
				PluginLog.Warning($"Failed to load {name} from local file. Reinitializing to default: {ex.Message}");
				value = new T(); // Reinitialize to default
			}
		}
		else if (download || forceDownload)
		{
			try
			{
				var url = $"https://raw.githubusercontent.com/{Service.USERNAME}/{Service.REPO}/main/Resources/{name}.json";
				var str = Http.GetStringAsync(url).Result;

				File.WriteAllText(path, str);
				value = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.None,
					Converters = [new StringEnumConverter()] // Add this line
				})! ?? throw new Exception("Deserialized value is null.");

				PluginLog.Information($"Downloaded and loaded {name} from GitHub.");
			}
			catch (Exception ex)
			{
				PluginLog.Warning($"Failed to download {name} from GitHub. Reinitializing to default. Exception: {ex.Message}");
				_ = BasicWarningHelper.AddSystemWarning($"Github download failed.");
				value = new T(); // Reinitialize to default
				SavePath(value, path); // Save the default value
			}
		}
		else
		{
			value = new T(); // Reinitialize to default
			SavePath(value, path); // Save the default value
		}
	}
}
