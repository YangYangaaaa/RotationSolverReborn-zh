# 全面汉化 RotationSolverReborn - Phase 6 收尾与最终验证计划（修订版）

## 背景与现状

### 上下文
本计划是「全面汉化RotationSolverReborn」的收尾阶段。用户要求**全面汉化**插件，翻译需符合 FF14 国服术语习惯，约定俗成的英文术语（GCD、oGCD、AoE、PvP、HP、MP、Boss、BMR 等）保留不译。

### 当前进度（2026-07-04 探索确认）

**已提交的 Phase 1-5（5 个 commit）**：
- `ec4e9dda` Phase 1: Basic enums and attributes
- `cef17b7d` Phase 2: Configs.cs markdown names and toast messages
- `872e17d1` Phase 3: CustomRotation_OtherInfo.cs + DutyRotation.cs
- `caca8883` Phase 4: RSCommands chat messages
- `1092306d` Phase 5: UI hints, tutorial, CNLanguageClient branches

**Phase 6 已完成但未提交（RebornRotations 283 个 Name 属性，40 个文件）**：
- Tank: WAR(12)、PLD(15)、GNB(2)、DRK(7) = 36
- Ranged: MCH(7)、DNC(4)、BRD(11) = 22
- Melee: VPR(9)、SAM(4)、RPR(3)、MNK(6)、NIN(5)、DRG(5) = 32
- Magical: SMN(14)、RDM(9)、PCT(8)、BLM_Default(6)、BLM_RP(4) = 41
- Healer: WHM(16)、SGE(22)、SCH(23)、AST(17) = 78
- Limited: BLU(10) = 10
- Duty: Variant(1)、Phantom(24)、MonsterHunter(4)、Emanation(1)、Bozja(2) = 32
- PvP: 12 个文件，32 个 Name
- 验证通过：无 `##` 后缀、无纯英文 "Word Word" 残留、无损坏字符串

### 重大新发现（本次探索新增）

**遗漏 1：ExtraRotations 完全未翻译** ⚠️
- 14 个文件，**186 个 `[RotationConfig(Name = "...")]` 英文属性**
- 分布：
  - Tank/ChurinDRK.cs: 11
  - Ranged/ChurinMCH.cs: 8、ChurinBRD.cs: 13、ChurinDNC.cs: 14
  - Melee/ChurinMNK.cs: 2、BeirutaNIN.cs: 8
  - Magical/Rabbs_BLM_All_Levels.cs: 7、ChurinSMN.cs: 18、BeirutaRDM.cs: 13、BeirutaPCT.cs: 9
  - Healer/BeirutaWHM.cs: 20、BeirutaSGE.cs: 20、BeirutaSCH.cs: 22、BeirutaAST.cs: 21

**遗漏 2：`[Description("...")]` 属性大量未翻译** ⚠️
- 全项目共 **156 处** `[Description("[A-Za-z]...")]`，扣除已译的 UiString.cs（8/10 已中文），剩余约 **148 处**：
  - `RotationSolver/UI/RotationConfigWindowTab.cs`: 13 处（UI 标签页描述提示）
  - `RotationSolver/IPC/IPCSubscriber.cs`: 15 处（IPC 状态/错误消息）
  - `RotationSolver/RebornRotations/`: 30 处（9 文件，枚举选项描述）
  - `RotationSolver/ExtraRotations/`: 约 90 处（11 文件，枚举选项描述）

**遗漏 3：AST_Reborn.cs 翻译不准确** ⚠️
- 第 40 行 `Name = "使用方位Benefic所需的队友最低 HP 阈值"` → 应为"使用**星位福星**所需的队友最低 HP 阈值"
- 国服占星术士技能 Aspected Benefic = 星位福星

**遗漏 4：SGE/SCH 部分技能名保留英文** ⚠️
- SGE: Rhizomata（根茎）、Soteria（救护）、Kerachole（角溃）、Holos（全息）— 应译
- SCH: Concitation（鼓动）、Accession（应允）— 应译

**风险点：ParentValue 与枚举 Description 一致性** ⚠️
- 前次会话已将 `PotionStrategy` 枚举的 `[Description]` 译为中文（如 `Custom` → "自定义爆发药时机"）
- 但 ExtraRotations 中 19 处 `ParentValue = "Use custom potion timings"` 仍是英文
- ParentValue 匹配逻辑（`RotationConfigWindow.cs:2412-2432`）：比较 `parentConfig.Value`（父配置当前值字符串）与 `parentValue.ToString()`
- **需验证**：枚举配置的 Value 存的是枚举名还是 Description 文本。若存 Description，则 ParentValue 必须同步译为中文，否则子配置不显示（功能 Bug）

### 关键约束与翻译约定

1. **属性本地化**：`[RotationConfig(Name = "...")]` 和 `[Description("...")]` 是编译期常量，必须直接替换字符串
2. **不译约定俗成术语**：GCD、oGCD、AoE、PvP、PvE、DoT、HoT、TTK、BMR、MP、HP、Boss、DOT、UI、IPC 等保留英文
3. **FF14 国服技能名**：必须使用官方译名（如 Aspected Benefic=星位福星、Rhizomata=根茎、Concitation=鼓动 等）
4. **ParentValue 一致性**：若父属性是枚举且其 `[Description]` 被翻译，则对应 `ParentValue` 字符串必须同步翻译为相同的中文文本
5. **Edit 持久化**：使用包含相邻行的长上下文作为 old_string/new_string，Edit 后用 Grep 验证
6. **ImGui ID**：`"Visible Text##UniqueId"` 中 `##` 后部分保留；但 `[RotationConfig(Name=...)]` 和 `[Description(...)]` 不是 ImGui ID，不应有 `##`
7. **验证策略**：不依赖 `dotnet build`（有 556 个预先存在的 JsonIgnore 错误）；使用 Grep + Read + git diff 验证

## 执行计划

### Step 1: 提交已完成的 RebornRotations（Phase 6a/6b/6c）

先提交已完成且验证通过的 283 个 Name 翻译，避免与后续修复混在一起。

**1.1 Phase 6a - PvE 提交**：
```powershell
cd d:\project\rsr-zh\RotationSolverReborn-zh
git add RotationSolver/RebornRotations/Tank/ RotationSolver/RebornRotations/Ranged/ RotationSolver/RebornRotations/Melee/ RotationSolver/RebornRotations/Magical/ RotationSolver/RebornRotations/Healer/ "RotationSolver/RebornRotations/Limited Jobs/"
git commit -m "zh: localize PvE rotation configs (Phase 6a)"
```

**1.2 Phase 6b - PvP 提交**：
```powershell
git add RotationSolver/RebornRotations/PVPRotations/
git commit -m "zh: localize PvP rotation configs (Phase 6b)"
```

**1.3 Phase 6c - Duty 提交**：
```powershell
git add RotationSolver/RebornRotations/Duty/
git commit -m "zh: localize Duty rotation configs (Phase 6c)"
```

### Step 2: 修复 RebornRotations 已知翻译问题

**2.1 修复 AST_Reborn.cs 第 40 行**：
- `Name = "使用方位Benefic所需的队友最低 HP 阈值"` → `Name = "使用星位福星所需的队友最低 HP 阈值"`

**2.2 修复 SGE_Reborn.cs 4 处技能名**：
- `Rhizomata` → `根茎`
- `Soteria` → `救护`
- `Kerachole` → `角溃`
- `Holos` → `全息`
- 涉及行：20、37、40、43、47（Name 字符串内的技能名）

**2.3 修复 SCH_Reborn.cs 2 处技能名**：
- `Concitation` → `鼓动`
- `Accession` → `应允`
- 涉及行：74（Name 字符串内的技能名）

**2.4 提交修复**：
```powershell
git add RotationSolver/RebornRotations/Healer/AST_Reborn.cs RotationSolver/RebornRotations/Healer/SGE_Reborn.cs RotationSolver/RebornRotations/Healer/SCH_Reborn.cs
git commit -m "zh: fix AST/SGE/SCH skill name translations"
```

### Step 3: 翻译 RebornRotations 的 [Description] 属性（30 处，9 文件）

**文件清单与翻译要点**：
- `Tank/GNB_Reborn.cs`: 6 处（"Full target usage"→"对所有目标使用"、"Only use on tankbuster targets prioritizing self"→"仅对死刑目标使用，优先自身"、"Only use on self"→"仅对自身使用"）
- `Melee/SAM_Reborn.cs`: 2 处（"Hagakure"→"叶隐"、"Setsugekka"→"雪月花"）
- `Melee/MNK_Reborn.cs`: 4 处（"Brotherhood"→"义结金兰"、"Perfect Balance"→"震脚"、"Use Immediately"→"立即使用"、"With ROF burst logic"→"配合红莲爆发逻辑"）
- `Magical/SMN_Reborn.cs`: 4 处（宝石顺序：Topaz=黄宝石、Emerald=绿宝石、Ruby=红宝石）
- `Healer/WHM_Reborn.cs`: 3 处（薄冰相关：Thin Air=无咒、Raise=复活）
- `Healer/SGE_Reborn.cs`: 2 处（Toxikon=毒素、Pneuma=灵气）
- `Healer/SCH_Reborn.cs`: 2 处（Catalyze=激流、Galvanize=激昂）
- `Healer/AST_Reborn.cs`: 3 处（"Ignore setting"→"忽略设置"、"When capped"→"满层时"、"Any charges"→"任意层数"）
- `Duty/PhantomDefault.cs`: 4 处（"Dark Cannon"→"暗黑加农"、"Shock Cannon"→"震荡加农"）

**提交**：
```powershell
git add RotationSolver/RebornRotations/
git commit -m "zh: localize RebornRotations enum descriptions (Phase 6d)"
```

### Step 4: 翻译 ExtraRotations（14 文件，186 Name + 约 90 Description + 19 ParentValue）

**关键前置：ParentValue 一致性验证**
- 先 Read `RotationConfigWindow.cs` 的 `DrawRotationConfiguration` 枚举渲染逻辑，确认 `parentConfig.Value` 存的是枚举名还是 Description
- 若存 Description：ParentValue 必须译为与对应枚举 Description 相同的中文
- 若存枚举名：ParentValue 应改为枚举名（如 `PotionStrategy.Custom`），而非字符串字面量
- 已知 `PotionStrategy` 枚举在 `RotationSolver.Basic/Rotations/CustomRotation_OtherInfo.cs:894`，Description 已译中文

**4.1 并行代理分工**（4 个后台代理）：

**代理 A - Tank + Magical（ChurinDRK + 3 个 Magical）**：
- `Tank/ChurinDRK.cs`: 11 Name + 12 Description（MpStrategy/BloodStrategy 枚举）+ 3 ParentValue
- `Magical/Rabbs_BLM_All_Levels.cs`: 7 Name + 18 Description
- `Magical/ChurinSMN.cs`: 18 Name + 11 Description + 3 ParentValue
- `Magical/BeirutaRDM.cs`: 13 Name
- `Magical/BeirutaPCT.cs`: 9 Name

**代理 B - Ranged（3 个文件）**：
- `Ranged/ChurinMCH.cs`: 8 Name + 3 ParentValue
- `Ranged/ChurinBRD.cs`: 13 Name + 5 Description + 6 ParentValue（含 SongTiming.Custom 枚举式 ParentValue，保留）
- `Ranged/ChurinDNC.cs`: 14 Name + 8 Description + 6 ParentValue（Tech Opener/Standard Opener）

**代理 C - Healer（4 个 Beiruta 文件）**：
- `Healer/BeirutaWHM.cs`: 20 Name + 3 Description
- `Healer/BeirutaSGE.cs`: 20 Name + 2 Description
- `Healer/BeirutaSCH.cs`: 22 Name + 8 Description
- `Healer/BeirutaAST.cs`: 21 Name + 5 Description

**代理 D - Melee（2 个文件）**：
- `Melee/ChurinMNK.cs`: 2 Name + 13 Description
- `Melee/BeirutaNIN.cs`: 8 Name + 3 Description

**4.2 翻译执行规范**（每个代理遵循）：
1. Read 文件前 120 行获取上下文与枚举定义
2. Grep `RotationConfig.*Name = "` 和 `\[Description\("` 定位所有待译行
3. 逐个 Edit 替换，old_string 包含相邻 1-2 行确保持久化
4. **不译**约定俗成术语：GCD、oGCD、AoE、PvP、Boss、HP、MP、BMR、DOT、UI、IPC
5. **ParentValue 处理**：
   - 字符串字面量（如 `"Use custom potion timings"`）：根据前置验证结果，译为对应枚举 Description 的中文，或改为枚举名
   - 枚举值引用（如 `SongTiming.Custom`）：保留不动
6. Edit 后 Grep 验证该文件无英文残留（除约定俗成术语）

**4.3 翻译术语对照表（补充）**：
- DRK: The Blackest Night=至黑之夜、Oblation=献奉、Shadowstride=暗影步、Blood Gauge=血量、MP=MP（保留）
- MCH: Bioblaster=生化炮、Wildfire=野火
- BRD: Wanderer's Minuet=旅神小步舞曲、Mage's Ballad=魔人歌、Army's Paeon=军神赞歌、Sandbag=保留（DPS 保留机制俗称，可译"沙包模式"）
- DNC: Technical Step=技巧舞步、Standard Step=标准舞步、Tillana=蒂拉娜、Finishing Move=终结动作、Dance Partner=舞伴
- SMN: Crimson Cyclone=深红旋风、Ruby Rite=红宝石仪式、Garuda=迦楼罗、Physick=医术
- RDM: Embolden=鼓励、Vercure=愈疗、Reprise=重击、Fleche=突进、Contre=还击、Prefulgence=满溢、Vice of Thorns=荆棘之厄
- PCT: HolyInWhite=白圣、CometInBlack=黑彗、Hyperphantasia=超幻想、Motif= motifs=底稿、Rainbow Drip=彩虹滴绘
- BLM: Fire 3=火炎三连、Blizzard 3=冰冻三连、Triple Cast=三连咏唱、Transpose=魔泉
- WHM: Thin Air=无咒、Benediction=神祝祷、Tetragrammaton=四连咒、Afflatus Solace=白百合、Afflatus Rapture=百合花、Medica=治疗、Cure III=治疗三型、Presence of Mind=神速咏唱、Divine Caress=神圣抚摸
- SGE: Toxikon=毒素、Pneuma=灵气
- SCH: Catalyze=激流、Galvanize=激昂
- MNK: Brotherhood=义结金兰、Perfect Balance=震脚、Riddle of Fire=红莲极意
- NIN: Suiton=水遁、Huton=风遁、Trick Attack=骗击、Kunai's Bane=苦无之灾、Raiton=雷遁、Katon=火遁

**4.4 提交**：
```powershell
git add RotationSolver/ExtraRotations/
git commit -m "zh: localize ExtraRotations configs and descriptions (Phase 6e)"
```

### Step 5: 翻译 UI 与 IPC 的 [Description] 属性

**5.1 RotationConfigWindowTab.cs（13 处）**：
- 这是 UI 标签页的描述提示文本，显示给用户
- 翻译示例："Useful information and macro list."→"实用信息与宏列表。"、"Rotation specific configs."→"循环专属配置。" 等

**5.2 IPCSubscriber.cs（15 处）**：
- IPC 状态/错误消息，可能通过 Toast 或日志显示给用户
- 翻译示例："The configuration was set successfully."→"配置设置成功。"、"Invalid lease."→"无效的租约。" 等

**5.3 提交**：
```powershell
git add RotationSolver/UI/RotationConfigWindowTab.cs RotationSolver/IPC/IPCSubscriber.cs
git commit -m "zh: localize UI tab descriptions and IPC messages (Phase 6f)"
```

### Step 6: 最终全局验证

**6.1 全局英文残留扫描**：
```
Grep pattern: RotationConfig\(CombatType\.Pv[EP], Name = "[A-Za-z][a-z]+ [A-Z]
path: d:\project\rsr-zh\RotationSolverReborn-zh\RotationSolver
预期：No matches found
```

**6.2 [Description] 英文残留扫描**：
```
Grep pattern: \[Description\("[A-Za-z]
path: d:\project\rsr-zh\RotationSolverReborn-zh\RotationSolver
预期：仅保留约定俗成术语（AoE、PvP 等），或确认为不需要翻译的纯标识符
```

**6.3 损坏字符串检查**：
```
Grep pattern: Name = "[^"]*(nexility|Relateseto|Ghealing)
预期：No matches found
```

**6.4 `##` 后缀检查**：
```
Grep pattern: (RotationConfig|Description\().*"[^"]*##
预期：No matches found
```

**6.5 ParentValue 一致性抽查**：
- Read ChurinDRK.cs 验证 ParentValue 与 PotionStrategy.Custom 的 Description 一致
- Read ChurinDNC.cs 验证 "Tech Opener"/"Standard Opener" 与对应枚举 Description 一致

**6.6 git diff --stat 总览**：
```powershell
git diff --stat HEAD~6..HEAD
```
预期：约 60+ 文件修改

**6.7 提交历史验证**：
```powershell
git log --oneline -10
```
预期看到 Phase 6a-6f 六个新提交。

### Step 7: 任务列表收尾

更新任务列表，标记 Phase 6 与 Phase 7 为 completed，添加摘要说明实际完成的工作。

## 假设与决策

1. **决策**：先提交已完成且验证通过的 RebornRotations 283 个 Name（Step 1），再做修复和补充，便于回滚。
2. **决策**：ExtraRotations 必须翻译——用户要求"全面汉化"，且这部分是可选循环配置，用户切换到 Churin/Beiruta 系列循环时会看到英文。
3. **决策**：`[Description]` 属性必须翻译——这些是枚举选项的显示文本，直接显示在 UI 下拉框中。
4. **决策**：RotationConfigWindowTab.cs 和 IPCSubscriber.cs 的 [Description] 也需翻译——前者是 UI 提示，后者可能显示给用户。
5. **决策**：ParentValue 必须与父枚举 Description 保持一致；前置验证后决定是译为中文还是改为枚举名引用。
6. **决策**：保留约定俗成英文术语（GCD、oGCD、AoE、PvP、Boss、HP、MP、BMR、DOT、UI、IPC）不译。
7. **假设**：ExtraRotations 的 186 个 Name + 90 Description 可由 4 个并行代理高效完成。
8. **决策**：不依赖 `dotnet build` 验证（556 个预先存在的 JsonIgnore 错误与翻译无关）。

## 验证清单

- [ ] Step 1: Phase 6a/6b/6c 三个提交成功
- [ ] Step 2: AST/SGE/SCH 技能名修复并提交
- [ ] Step 3: RebornRotations 30 处 [Description] 翻译并提交
- [ ] Step 4: ParentValue 一致性前置验证完成
- [ ] Step 4: ExtraRotations 14 文件全部翻译（4 个代理完成）
- [ ] Step 4: ExtraRotations 提交成功
- [ ] Step 5: RotationConfigWindowTab + IPCSubscriber 提交成功
- [ ] Step 6.1: 无纯英文 "Word Word" Name 残留
- [ ] Step 6.2: [Description] 英文残留仅限约定俗成术语
- [ ] Step 6.3: 无损坏字符串
- [ ] Step 6.4: 无错误的 `##` 后缀
- [ ] Step 6.5: ParentValue 与枚举 Description 一致
- [ ] Step 6.6: git diff --stat 显示约 60+ 文件
- [ ] Step 6.7: git log 显示 6 个新提交
- [ ] Step 7: 任务列表收尾

## 风险与缓解

1. **风险**：ParentValue 翻译不当导致子配置不显示（功能 Bug）。
   - **缓解**：Step 4 前置验证枚举 Value 存储机制；翻译后 Read 抽查 ParentValue 与 Description 一致性。
2. **风险**：ExtraRotations 代理 Edit 持久化问题。
   - **缓解**：每个代理 Edit 后 Grep 验证；Step 6 全局扫描。
3. **风险**：FF14 技能名误译。
   - **缓解**：参考术语对照表；对不确定的技能名保留英文（宁可保留也不误译）。
4. **风险**：IPCSubscriber.cs 的 [Description] 可能是 IPC 协议标识符，翻译会破坏 IPC 兼容性。
   - **缓解**：Step 5.2 先 Read 确认 [Description] 是否用于协议匹配；若用于匹配则保留英文，仅翻译用户可见的错误消息。
5. **风险**：ExtraRotations 中枚举式 ParentValue（如 `SongTiming.Custom`）被误改为字符串。
   - **缓解**：代理规范明确：枚举值引用保留，仅处理字符串字面量。
