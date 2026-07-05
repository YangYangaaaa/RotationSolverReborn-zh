# RotationSolverReborn-zh 全面汉化实施计划

## Context（背景）

当前仓库已是 RSR 的中文汉化 Fork，但汉化覆盖严重不足：
- **已汉化**：仅 `RotationSolver/Data/UiString.cs`（222 条 `[Description]`）和 `RotationSolver/UI/RotationConfigWindowTab.cs` 的 `CNString()` 扩展（且缺 `DutyRotation`/`AutoDuty` 两个 case）
- **未汉化**：约 700+ 条用户可见英文字符串分布在 60+ 文件中，涵盖 UI 窗口、配置面板、枚举描述、命令聊天消息、循环配置、状态面板等

用户要求"全面汉化"，需在不改任何逻辑代码、不破坏占位符/ImGui ID/上游 rebase 兼容性的前提下，将所有用户可见英文文案翻译为符合 FF14 国服术语的中文。GCD/oGCD/AoE 等约定俗成术语保留原文。

## 核心设计决策

### 1. CNLanguageClient 双语分支策略（关键）

`RotationConfigWindow.cs:61` 已存在 `CNLanguageClient` 运行时检查（11 处使用），用于国服客户端显示中文、其他客户端显示英文。

**采用混合策略**：
- **UI 运行时文本**（`ImGui.Text/Button/TextWrapped/SetTooltip/...`）→ **方案 A：保留英文 + 新增 CN 分支**。不触碰英文行，仅在英文调用旁加 `if (CNLanguageClient) { 中文 } else { 英文 }` 或单行三元。**降低 rebase 冲突**（上游改英文行不与新加 CN 分支冲突）。
- **特性字面量**（`[Description]`/`[UI]`/`[RotationConfig]`/`[Rotation]`）→ **方案 B：全替换为中文**。C# attribute 是编译期常量，无法运行时分支，冲突风险不可避免。
- **静态数组**（`_baseUsageHints`、`FirstStartTutorialWindow.Steps`）→ **方案 B：全替换**。`static readonly` 数组初始化时一次性求值，无法分支；这两处是新手首屏必看，必须中文；上游很少逐条改文案，冲突可接受。
- **聊天消息**（`Svc.Chat.Print/PrintError`、`Svc.Toasts.ShowQuest`）→ **方案 B：全替换**。数量少（~30 条）且孤立，上游很少重写。

### 2. 不译项清单

- 游戏内技能名/状态名/职业名（由 Lumina 数据表提供，如 `Bloodwhetting`、`Nascent Flash`、`Swiftcast`）
- `[RotationDesc(ActionID.XXX)]` 中的 ActionID 枚举值
- `[SourceCode(Path = "...")]` GitHub 路径
- `GameVersion = "7.5"` 版本号
- `Parent = nameof(Xxx)` 属性名引用
- `PluginLog.*` 日志字符串（非用户可见）
- `IncompatiblePlugins.json` 的 `Name` 字段（插件名）
- ImGui `##` / `###` ID 后缀
- 作者署名（`[Rotation("Reborn", ...)]`、`"ChurinDRK"`、`"BeirutaWHM"`、`"Rabbs Blackest Mage"` 等）
- 约定俗成术语：GCD / oGCD / AoE / PvE / PvP / BMR / IPC / DTR / AutoDuty

### 3. 术语对照表（遵循 README 翻译约定）

Rotation=循环、Action=技能、Tank Buster=坦克死刑、Knockback=击退、Gaze/Stop=凝视/止息、Dispel=驱散、Stance=姿态、Positional=身位、Shirk=转嫁仇恨、Raise=复活、Burst=爆发、Anti-Knockback=防击退、Mitigation=减伤、Invulnerability=无敌、HoT/DoT=持续恢复/持续伤害、Ultimate=绝境战、Savage=零式、Extreme=极蛮神、Dungeon=地下城、Deep Dungeon=深层迷宫、Variant Dungeon=异闻迷宫、Alliance Raid=联盟突袭、The Masked Carnivale=假面狂欢、Swiftcast=瞬发、Esuna=医术、True North=真北、yalm=星里、TTK=击杀时间、Forced Condition=强制条件、Disabled Condition=禁用条件。

### 4. 标点规范

中文文本统一使用全角标点（`。，！？：""（）；、`）。中英混排时英文单词与中文之间不加空格（与现有 UiString.cs 一致）。

---

## 执行顺序（7 个阶段，每阶段一次 commit）

### 阶段 0：准备
- 副本名国服译名词典整理（见下方"副本名核实清单"）
- 不改代码

### 阶段 1：Basic 枚举与属性（基础层，~80 条）
全替换下列文件的 `[Description]` 特性：
- `RotationSolver.Basic\Attributes\RangeAttribute.cs`（ConfigUnitType 5 条：Seconds→"时间单位，秒"、Degree→"角度单位，度"、Yalms→"距离单位，星里"、Percent→"比例单位，百分比"、Pixels→"显示单位，像素"）
- `RotationSolver.Basic\Data\DescType.cs`（13 条，GCD/oGCD/AoE 不译）
- `RotationSolver.Basic\Data\TargetType.cs`（13 条）
- `RotationSolver.Basic\Data\TargetHostileType.cs`
- `RotationSolver.Basic\Data\RaiseType.cs`
- `RotationSolver.Basic\Data\HardCastRaiseType.cs`
- `RotationSolver.Basic\Data\CycleType.cs`
- `RotationSolver.Basic\Data\DTRType.cs`
- `RotationSolver.Basic\Data\TinctureUseType.cs`
- `RotationSolver.Basic\Data\CanUseOption.cs`
- `RotationSolver.Basic\Data\RSCommandType.cs`（3 枚：SpecialCommandType 15 条、StateCommandType 7 条、OtherCommandType 7 条；并补 `StateCommandTypeExtensions.CNString()` 缺失的 `TargetOnly→"仅目标"`、`AutoDuty→"AutoDuty"`、`Henched→"Henched"`、`PvP→"PvP"`）
- `RotationSolver.Basic\Actions\ActionBasicInfo.cs`（1 条警告）
- `RotationSolver.Basic\Rotations\CustomRotation_BasicInfo.cs`（4 条）

**验收**：`git diff --stat` 确认仅字符串变化。`git commit -m "zh: localize Basic enums and attributes"`

### 阶段 2：Basic 配置层（~70 条 + 9 个 markdown 块）
- `RotationSolver.Basic\Configuration\Configs.cs`：60+ 条 `[UI("...", Description = "...")]`，按 `#region` 分块替换。注意保留 `Filter = nameof(...)`、`Parent = nameof(...)` 引用。
- `RotationSolver.Basic\Configuration\OtherConfiguration.cs`：9 个 markdown 块（AoE/Tank Buster/Knockback/Gaze Stop/Beneficial Positions/Dispellable Debuffs/Priority/Invulnerability/No-Casting Debuffs）的标题与说明翻译。`file/name/section` 属性值保留英文（内部 key）。

**验收**：`dotnet build RotationSolver.Basic\RotationSolver.Basic.csproj -c Release` 通过。`git commit -m "zh: localize Basic configuration"`

### 阶段 3：状态面板（~100 条）
- `RotationSolver.Basic\Rotations\CustomRotation_OtherInfo.cs`：83 条 `[Description]` 按 `#region Player/Enemy/Party/...` 分块全替换。短标签意译（`"IsCasting"→"正在读条"`、`"Has Swift"→"有瞬发"`、`"Has tank stance"→"有坦克姿态"`、`"Has hostiles in 25 yalms"→"25 星里有敌人"`）。
- `RotationSolver.Basic\Rotations\Duties\DutyRotation.cs`：约 15 条 `[Description]` 属性全替换 + 第 173-265 行调试 `ImGui.Text`（副本名按译名词典、技能 ID 名保留）按方案 A 加 CN 分支。

**验收**：`git commit -m "zh: localize rotation status panel descriptions"`

### 阶段 4：命令系统聊天消息（~30 条，方案 B 全替换）
- `RotationSolver\Commands\RSCommands_BasicInfo.cs`（2 条 targeting 消息）
- `RotationSolver\Commands\RSCommands_OtherCommand.cs`（~18 条设置/targeting/toggle/insert 消息）
- `RotationSolver\Commands\RSCommands_StateSpecialCommand.cs`（14 条 Targeting 状态消息，`UpdateState` 和 `AutodutyUpdateState` 各 7 条重复）
- `RotationSolver\Commands\RSCommands_Actions.cs`（1 条系统警告）

**关键**：保留 `{property.Name}`、`{command}`、`{targetingTypeAdd}`、`{DataCenter.TargetingType}` 等占位符。`AutoDuty`、`Henched`、`PvP` 保留原文。

**验收**：`git commit -m "zh: localize command chat messages"`

### 阶段 5：UI 层（最大阶段，方案 A 为主）

**5.1** `RotationConfigWindowTab.cs`：13 个 `[Description]` 全替换 + `CNString()` 补 `DutyRotation`/`AutoDuty` 两个 case（DutyRotation→"副本循环"、AutoDuty→"AutoDuty"）。

**5.2** `RotationConfigWindow.cs`（5017 行，9 region）分 9 次小 commit：
- 顺序：About → Rotation → Actions → List → AutoDuty → DutyRotation → DutySpecifc → Debug → 顶部通用（Draw/Header/SideBar/HintsBar）
- 每个 region：`grep -n 'ImGui\.\(Text\|Button\|TextWrapped\|SetTooltip\|Checkbox\|TreeNodeEx\|CollapsingHeader\|TabItem\|Selectable\)('` 列出调用，对每条判断：
  - 已有 CNLanguageClient 分支 → 跳过
  - 纯英文 → 加 `if (CNLanguageClient) { 中文 } else { 英文 }` 或 `ImGui.Text(CNLanguageClient ? "中文" : "English")`
  - 来自 `UiString.XXX.GetDescription()` → 跳过
- `_baseUsageHints` 数组（108-149 行，40 条）：方案 B 全替换为中文（静态数组无法分支）

**5.3** `FirstStartTutorialWindow.cs`：11 步教程 + 按钮文案，方案 B 全替换（`Steps` 是 `static readonly` 数组）。

**5.4** 其他 UI 小文件（方案 A 双语分支）：`ControlWindow.cs`、`NextActionWindow.cs`、`InterceptedActionWindow.cs`、`CooldownWindow.cs`、`OverlayWindow.cs`、`CtrlWindow.cs`、`ActionTimelineWindow.cs`、`EasterEggWindow.cs`、`ImGuiHelper.cs`、`ActionContextMenu.cs`、`RotationConfigWindow_Config.cs`、`ImguiTooltips.cs`、`EnumSearch.cs`、`SearchableCollection.cs`。

每文件独立 commit：`git commit -m "zh: localize UI/<file>"`。

### 阶段 6：循环配置（~480 条，方案 B 机械替换）

**6.1** `RotationSolver\RebornRotations\`（28 文件，~283 条 `[RotationConfig] Name` + ~30 条 `[Rotation] Description`）
- 按子目录分批 commit：Tank / Ranged / Melee / Magical / Healer / Duty / Limited Jobs / PVPRotations
- `[Rotation("Reborn", ...)]` 第一个参数保留（作者署名）
- `[Rotation("Phantom Jobs Loaded", ...)]` 描述性名称翻译为"幻影职业已加载"
- `[Rotation(..., Description = "...")]` 文学性描述翻译为中文保留作者风格
- `[RotationConfig(CombatType.PvE, Name = "...")]` 的 Name 翻译，技能名保留英文
- 例：`Name = "Use Bloodwhetting/Raw intuition on single enemies"` → `Name = "对单体敌人使用 Bloodwhetting/Raw intuition"`

**6.2** `RotationSolver\ExtraRotations\`（14 文件，~196 条 `[RotationConfig]` + ~14 条 `[Rotation]`）
- 同 6.1 规则
- 作者署名（`"ChurinDRK"`、`"BeirutaWHM"` 等）保留原文
- 内部 `enum`（如 ChurinDRK 的 `MpStrategy`、`BloodStrategy`）的 `[Description]` 全替换

每子目录一次 commit：`git commit -m "zh: localize RebornRotations/<subdir>"`。

### 阶段 7：收尾与验证
1. `Resources\IncompatiblePlugins.json`：9 条 `Features` 字段翻译（`Name` 字段不动）
2. `RotationSolverPlugin.cs`：第 173 行 `"Warning has been hidden."` → `"警告已隐藏。"`
3. **统一编译验证**：`dotnet clean RotationSolver.sln -c Release && dotnet build RotationSolver.sln -c Release`，0 error
4. **更新 README.md**：在"汉化内容"表格补充本轮新增文件与条目数
5. `git commit -m "zh: complete full localization"`

---

## 副本名核实清单（需查国服译名）

| 英文 | 出现位置 | 建议译名 | 核实方式 |
|---|---|---|---|
| Orbonne Monastery | DutyRotation.cs:181、OrbonneDefault.cs:5 | 瓯博讷修道院 | 代码内已有，确认 |
| Sil'dihn Subterrane | OtherConfiguration.cs:42、DutyRotation.cs:261 | 西尔迪赫水洞 | 灰机wiki |
| Sil'dihn Subterrane (Savage) | OtherConfiguration.cs:42 | 西尔迪赫水洞（零式） | 灰机wiki |
| Mount Rokkon | DutyRotation.cs:262 | 罗克康山 | 灰机wiki |
| Aloalo Island | DutyRotation.cs:263 | 阿罗阿罗岛 | 灰机wiki |
| The Puppets' Bunker | Configs.cs:204 | 人偶军工厂 | 灰机wiki |
| The Tower at Paradigm's Breach | Configs.cs:209 | 待核实 | 灰机wiki |
| Jeuno: The First Walk | Configs.cs:214 | 朱诺：初探 | 灰机wiki |
| Windurst: The Third Walk | Configs.cs:219 | 温达斯特：三探 | 灰机wiki |
| Cloud of Darkness | Configs.cs:224 | 暗之云 | 灰机wiki |
| M4S - Wicked Thunder | OtherConfiguration.cs:63 | M4S - 邪雷 | 灰机wiki |
| Rathalos / Arkveld | DutyRotation.cs:193-202 | 雷狼龙 / 噬龙 | 灰机wiki |
| Occult Crescent | RotationConfigWindow.cs:622 | 灵异新月 | 灰机wiki |
| Beauty's Wicked Wiles | EmanationDefault.cs:5 | 美之邪魅 | 灰机wiki |

核实工作流：先 `grep -rn` 全仓库定位所有出现位置 → WebFetch 灰机wiki（https://ff14.huijiwiki.com/）核实 → 填入译名词典 markdown → 不确定的在 commit message 标注"待国服确认"。

---

## 关键文件（按重要性）

1. `d:\project\rsr-zh\RotationSolverReborn-zh\RotationSolver\UI\RotationConfigWindow.cs`（5017 行，9 region，方案 A 处理核心）
2. `d:\project\rsr-zh\RotationSolverReborn-zh\RotationSolver.Basic\Configuration\Configs.cs`（60+ `[UI]` 全替换）
3. `d:\project\rsr-zh\RotationSolverReborn-zh\RotationSolver.Basic\Rotations\CustomRotation_OtherInfo.cs`（83 条 `[Description]`）
4. `d:\project\rsr-zh\RotationSolverReborn-zh\RotationSolver\Commands\RSCommands_StateSpecialCommand.cs`（14 条 targeting 聊天消息，占位符保护重点）
5. `d:\project\rsr-zh\RotationSolverReborn-zh\RotationSolver.Basic\Rotations\Duties\DutyRotation.cs`（副本调试 + 状态属性）

参考文件：
- `RotationSolver\UI\RotationConfigWindowTab.cs`（CNString 模式参考）
- `RotationSolver\Data\UiString.cs`（已汉化范本，术语一致性参考）
- `sync-upstream.ps1`（rebase 冲突处理流程）
- `README.md`（翻译约定权威来源，验收时同步更新）

---

## 风险点与缓解

1. **rebase 冲突高频文件**：UiString.cs（必冲突，README 已承认）、Configs.cs、CustomRotation_OtherInfo.cs、RebornRotations/*。**缓解**：每文件独立 commit，rebase 时单文件处理；同步前先 `.\sync-upstream.ps1 -DryRun`。
2. **占位符破坏**：`$"...{x}..."` 翻译时可能误删占位符。**缓解**：翻译前后 `grep -n '\$"' <file>` 对比占位符数量；编译器可捕获 `string.Format` 的 `{0}` 不匹配但捕获不到插值字段被删，需人工复核。
3. **ImGui ID `##`/`###` 破坏**：`"Yes##ResetConfirm"` 翻译为 `"是##ResetConfirm"` 时 `##` 后部分必须不动。**缓解**：`grep -n '##' <file>` 翻译前后对比数量。
4. **RotationConfigWindow.cs 文件膨胀**：方案 A 双语分支会让 5017 行涨到 ~7000 行。**缓解**：单行三元 `ImGui.Text(CNLanguageClient ? "中文" : "English")` 优先于 if-else 块。
5. **属性字面量无法分支**：`[UI("...")]` 是编译期常量。**缓解**：接受全替换冲突风险（C# attribute 固有限制）。
6. **字符串中技能名混排**：`Name = "Use Bloodwhetting on single enemies"` 中 `Bloodwhetting` 是技能名应保留。**缓解**：翻译时识别大写开头的专有名词、`/` 分隔的技能对，保留英文片段。

---

## 验收标准

### 必达标准
1. **编译通过**：`dotnet build RotationSolver.sln -c Release` 退出码 0，0 error
2. **零逻辑改动**：所有 diff 均为字符串字面量替换或新增 CNLanguageClient 分支块，无控制流/表达式/方法签名变化
3. **占位符完整**：所有 `$"..."` 的 `{...}` 占位符数量与改动前一致
4. **ImGui ID 完整**：所有 `##` / `###` ID 与改动前一致
5. **术语一致**：README 翻译约定表中的术语在所有文件中译法统一
6. **不译项未译**：游戏内技能名/状态名/职业名、ActionID 枚举、SourceCode 路径、GameVersion、Parent=nameof、PluginLog 字符串、JSON Name 字段、ImGui ID 均保留原文

### 覆盖率标准
1. `RotationSolver.Basic\Data\` 与 `Attributes\` 下所有带 `[Description]` 的枚举 100% 汉化
2. `Configs.cs` 中所有 `[UI("...")]` 第一个参数与 `Description = "..."` 100% 汉化
3. `CustomRotation_OtherInfo.cs` 与 `DutyRotation.cs` 中所有 `[Description]` 100% 汉化
4. `RSCommands_*.cs` 中所有 `Svc.Chat.Print/PrintError` 与 `Svc.Toasts.ShowQuest` 消息 100% 汉化
5. `RotationSolver\UI\` 下所有 .cs 文件中 `ImGui.Text/Button/TextWrapped/SetTooltip/Checkbox/TreeNodeEx/CollapsingHeader/TabItem/Selectable` 用户可见字符串 100% 汉化
6. `RebornRotations\` 与 `ExtraRotations\` 下所有 `[RotationConfig(..., Name = "...")]` 与 `[Rotation(..., Description = "...")]` 100% 汉化（作者署名除外）
7. `Resources\IncompatiblePlugins.json` 的 9 条 Features 100% 汉化
8. `RotationSolverPlugin.cs` 的 1 条警告汉化

### 回归标准
- 汉化完成后 `git rebase main`（模拟下次上游同步）的冲突文件数 ≤ 汉化前 + 5
- `RotationConfigWindow.cs` 在 rebase 时**不产生新冲突**（方案 A 验证）

---

## 验证方法

1. **编译验证**：每个阶段结束后 `dotnet build RotationSolver.sln -c Release`，最终交付前 `dotnet clean` + `dotnet build`
2. **静态检查**：每文件改完后 `git diff <file>` 人工 review；`grep -n '\$"' <file>` 与 `grep -n '##' <file>` 翻译前后对比
3. **游戏内验证**（编译通过后）：
   - `/rotation` 打开配置窗口，13 个 Tab 中文名正确（含 DutyRotation/AutoDuty 补全）
   - About/Rotation/Actions/List/AutoDuty/Debug 各区段文案中文
   - _baseUsageHints 40 条提示轮播中文
   - FirstStartTutorialWindow 11 步教程中文
   - StateCommandType 切换时聊天消息中文（Off/Auto/TargetOnly/Manual/AutoDuty/Henched/PvP 7 种）
   - `/rotation Settings TargetingTypes add Big` 等命令聊天反馈中文
   - 各职业循环配置面板（Job Tab → WAR_Reborn 等）：`[RotationConfig]` Name 中文、技能名保留英文
   - 副本循环（Duty Tab）：进入西尔迪赫水洞/罗克康山/阿罗阿罗岛/瓯博讷修道院时 Tab 名与调试输出副本名中文
   - IncompatiblePlugins.json 在 About - Compatibility 显示中文
