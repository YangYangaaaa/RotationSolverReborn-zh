# 全面汉化 RotationSolverReborn —— 续作 2 计划

## 任务摘要

继续执行 RotationSolverReborn 插件的全面中文汉化工作。本计划承接前两次会话:Phase 1 已提交,Phase 2-3 部分完成,需收尾并完成 Phase 4-7。

翻译须符合 FF14 国服术语习惯,约定俗成的英文术语(如 GCD、oGCD、AoE、PvP、PvE、BLU、RSR、BMR、AutoDuty、Reborn)保持不译。

---

## 当前进度分析(基于实际验证)

### ✅ 已完成并提交(Phase 1,commit ec4e9dda)
13 个文件:`RotationSolver.Basic/Attributes/RangeAttribute.cs`、`RotationSolver.Basic/Data/` 下 11 个枚举文件、`ActionBasicInfo.cs`、`CustomRotation_BasicInfo.cs`

### ⚠️ Phase 2 - 部分完成(工作区未提交)

#### OtherConfiguration.cs - ✅ 完成
- 9 个 `/// <markdown>` 块的说明文字已翻译为中文
- `name="..."` 属性保留英文(这些是列表 ID,如 `name="AoE"`、`name="Tank Buster"`,作为程序标识符,且 `name="AoE"` 中 AoE 不译)
- `<see cref="...">` 标签完整保留

#### Configs.cs - ⚠️ 大部分完成,剩余 3 项
子代理 4aabd758 已翻译绝大部分,但以下未完成:

1. **17 个 markdown 块的 `name="..."` 属性仍是英文**(行 449-1044):
   ```
   行 449:  name="Gemdraughts/Tinctures/Pots Usage"
   行 549:  name="Use healing abilities when playing a non-healer role"
   行 569:  name="Cleanse all dispellable debuffs"
   行 662:  name="Disable hostile actions if something is casting an action on the Gaze/Stop list"
   行 675:  name="Automatic Healing Thresholds"
   行 681:  name="Stop Healing Cast After Reaching Threshold"
   行 687:  name="Auto-use oGCD abilities"
   行 737:  name="Auto True North"
   行 755:  name="Use beneficial ground-targeted actions when moving"
   行 803:  name="Only heal self when not a Healer"
   行 835:  name="Melee Range action using offset"
   行 900:  name="How early before next GCD should RSR use swiftcast for raise"
   行 909:  name="Random delay range for resurrecting players"
   行 927:  name="HP standard deviation for using AoE heal"
   行 966:  name="The duration of special windows opened by /rotation commands by default"
   行 995:  name="Clicking actions random delay range"
   行 1007: name="How soon before countdown is finished to start casting or attacking"
   行 1044: name="Action Ahead"
   ```
   注:行 1266-1296 的 4 个 HP 阈值 markdown 块 name 已翻译为中文,需统一这 17 个

2. **3 个 toast 消息未翻译**:
   - 行 1381: `Svc.Toasts.ShowNormal("Configs backed up.");`
   - 行 1395: `Svc.Toasts.ShowNormal("Backed up configs are not compatible with the current version.");`
   - 行 1401: `Svc.Toasts.ShowNormal("Configs restored. Closing to set.");`

3. **损坏行已由子代理修复**(行 1267、1277 已恢复为正确中文)

### ⚠️ Phase 3 - 部分完成(工作区未提交)

#### CustomRotation_OtherInfo.cs - ⚠️ 82/83 完成,1 行损坏
- 第 1430 行损坏:`[Description("Time from next ability to next nexility to next nexility to next GCD")]`
- 应为:`[Description("距下个能力技到下个 GCD 的时间")]`
- 其余 82 个 `[Description]` 已正确翻译(经 Grep 验证,仅 3 个以英文字母开头的预期保留:"RSR 已激活"、"BMR 模块已激活"、"PvP 技能是否忽略无敌状态")

#### DutyRotation.cs - ❌ 未开始
12 个 `[Description]` 全部英文(行 486-567):
- "Has Swift"、"Has tank stance"(×2)、"Is burst"
- "The state of auto. True for on."、"The state of manual. True for manual."
- "Has hostiles in Range"、"Has hostiles in 25 yalms"
- "The number of hostiles in Range"、"The number of hostiles in max Range"
- "The number of all hostiles in Range"、"The number of all hostiles in max Range"

### ❌ Phase 4-7 - 未开始

| Phase | 文件 | 工作量 |
|---|---|---|
| 4 | RSCommands_BasicInfo.cs | 2 处 `Svc.Chat.Print` |
| 4 | RSCommands_StateSpecialCommand.cs | 14 处 `Svc.Chat.Print` |
| 4 | RSCommands_OtherCommand.cs | 23 处 `Svc.Chat.Print` |
| 5 | RotationConfigWindow.cs | 11 处 `CNLanguageClient` + `_baseUsageHints`(40 条,第 108 行) |
| 5 | FirstStartTutorialWindow.cs | ~12 个 `TutorialStep` |
| 6 | RebornRotations/ 下 40 个文件 | 282 个 `Name = "..."` |
| 7 | Resources/IncompatiblePlugins.json | 9 条 `Features` 字段 |
| 7 | README.md | 更新汉化范围说明 |

### 编译验证说明(重要)
项目存在**预先存在的源生成器 Bug**:`Configuration_Configs.g.cs` 报 556 处 `JsonIgnoreAttribute`/`JsonPropertyAttribute` 未找到错误。已通过 `git stash` 测试确认此错误与翻译无关。**不依赖 `dotnet build` 成功作为验证标准**,改用:
- `git diff --stat` 检查改动范围
- 人工抽查翻译质量(占位符、ImGui ID、属性参数是否保留)
- 错误数对比(翻译前后错误数应保持一致)

---

## 翻译约定(沿用 README.md 与前两次会话)

### 保持不译的术语
- **战斗机制**:GCD、oGCD、AoE、DoT、HoT、TTK、MP、TP
- **游戏模式**:PvP、PvE、BLU(青魔)、RSR、BMR、AutoDuty、Reborn
- **职业缩写**:AST、BRD、DNC、DRG、MNK、NIN、PCT、RPR、RDM、SCH、SMN、PLD、WAR、DRK、GNB、BLM、WHM、SGE、SAM、VPR、MCH、BLU、BSM
- **副本编号**:O12S、M8S、M9S、M10S、M4S 等
- **Dalamud/ImGui 技术词**:DTR、Toast、ImGui

### 国服译名规范
- **副本类型**:零式 / 绝境战 / 极蛮神 / 联盟突袭 / 深层迷宫 / 异闻迷宫 / 宝物迷宫 / 假面狂欢 / 变体迷宫
- **副本名**:亚历山大绝境战(TEA)、欧米茄绝境战(TOP)、未来重现绝境战(FRU)、狂飙舞绝境战(DMU)、巴哈姆特绝境战(UCoB)、究极神兵绝境战(UwU)、龙诗绝境战(DSR)
- **战斗术语**:坦克/治疗/近战/远程/魔法、减伤、无敌、复活、读条、瞬发、坦克姿态、死刑、击退、凝视、止息、驱散、净化、守护、冲刺、爆发、连击、身位、仇恨
- **状态**:扁平伤害、即死、减速、石化、麻痹、打断、失明、眩晕、催眠、束缚、迟缓

### 必须保留的代码元素
- **ImGui ID**:`"可见文本##UniqueId"` 中 `##` 后的部分必须原样保留
- **字符串插值占位符**:`{x}`、`{0}`、`{x:F2}` 等必须保留
- **属性参数名**:`Filter =`、`Section =`、`Parent =`、`PvEFilter =`、`PvPFilter =` 等
- **枚举值引用**:`nameof(SomeEnum)`、`JobFilterType.Tank` 等
- **转义字符**:`\r\n`、`\"`、`%%` 等
- **markdown 块的 `file="..."`、`section="..."`、`subsection="..."` 属性**:作为分类标识,保留英文
- **markdown 块的 `name="..."` 属性**:显示给用户的标题,需翻译(但 `name="AoE"` 中 AoE 不译)

---

## 实施步骤

### Step 1:Phase 2 收尾 - Configs.cs 剩余翻译

**文件**:`RotationSolver.Basic/Configuration/Configs.cs`

1. **翻译 17 个 markdown 块的 `name="..."` 属性**(行 449-1044):

| 行号 | 英文原文 | 中文翻译 |
|---|---|---|
| 449 | Gemdraughts/Tinctures/Pots Usage | 增伤药水/爆发药/药品使用 |
| 549 | Use healing abilities when playing a non-healer role | 非治疗职业时使用治疗能力技 |
| 569 | Cleanse all dispellable debuffs | 驱散所有可驱散 Debuff |
| 662 | Disable hostile actions if something is casting an action on the Gaze/Stop list | 有目标施放凝视/止息列表技能时禁用敌对动作 |
| 675 | Automatic Healing Thresholds | 自动治疗阈值 |
| 681 | Stop Healing Cast After Reaching Threshold | 达到阈值后停止治疗读条 |
| 687 | Auto-use oGCD abilities | 自动使用 oGCD 能力技 |
| 737 | Auto True North | 自动正北 |
| 755 | Use beneficial ground-targeted actions when moving | 移动时使用有益地面目标技能 |
| 803 | Only heal self when not a Healer | 非治疗职业时仅治疗自己 |
| 835 | Melee Range action using offset | 近战范围技能偏移使用 |
| 900 | How early before next GCD should RSR use swiftcast for raise | RSR 应在下一个 GCD 前多久使用 Swiftcast 进行复活 |
| 909 | Random delay range for resurrecting players | 复活玩家的随机延迟范围 |
| 927 | HP standard deviation for using AoE heal | 使用 AoE 治疗的 HP 标准差 |
| 966 | The duration of special windows opened by /rotation commands by default | /rotation 命令打开的特殊窗口默认持续时间 |
| 995 | Clicking actions random delay range | 点击技能的随机延迟范围 |
| 1007 | How soon before countdown is finished to start casting or attacking | 倒计时结束前多久开始读条或攻击 |
| 1044 | Action Ahead | 提前量 |

2. **翻译 3 个 toast 消息**:
   - `"Configs backed up."` → `"配置已备份。"`
   - `"Backed up configs are not compatible with the current version."` → `"备份的配置与当前版本不兼容。"`
   - `"Configs restored. Closing to set."` → `"配置已恢复。将关闭以应用设置。"`

**验证**:
- `Grep` 确认 `name="[A-Z]` 在 Configs.cs 中无英文匹配(除 `name="AoE"` 等约定不译的)
- 抽查 3 处,确认 `section="..."`、`subsection="..."` 保留英文

**提交**:`git commit -m "zh: finalize Configs.cs markdown names and toast messages (Phase 2)"`

---

### Step 2:Phase 3 收尾 - 修复损坏行 + DutyRotation.cs

**文件 1**:`RotationSolver.Basic/Rotations/CustomRotation_OtherInfo.cs`

修复第 1430 行损坏:
- 当前:`[Description("Time from next ability to next nexility to next nexility to next GCD")]`
- 改为:`[Description("距下个能力技到下个 GCD 的时间")]`

**文件 2**:`RotationSolver.Basic/Rotations/Duties/DutyRotation.cs`

翻译 12 个 `[Description]`(行 486-567):

| 行号 | 英文原文 | 中文翻译 |
|---|---|---|
| 486 | Has Swift | 有瞬发 |
| 492 | Has tank stance | 有坦克姿态 |
| 498 | Has tank stance | 有坦克无敌 |
| 504 | Is burst | 爆发期 |
| 510 | The state of auto. True for on. | 自动状态(开) |
| 516 | The state of manual. True for manual. | 手动状态 |
| 537 | Has hostiles in Range | 范围内有敌人 |
| 543 | Has hostiles in 25 yalms | 25 米内有敌人 |
| 549 | The number of hostiles in Range | 范围内敌人数 |
| 555 | The number of hostiles in max Range | 最大范围内敌人数 |
| 561 | The number of all hostiles in Range | 范围内全部敌人数 |
| 567 | The number of all hostiles in max Range | 最大范围内全部敌人数 |

**验证**:
- `Grep` 确认 CustomRotation_OtherInfo.cs 第 1430 行已修复
- `Grep` 确认 DutyRotation.cs 无英文 `[Description]` 残留

**提交**:`git commit -m "zh: fix CustomRotation_OtherInfo.cs corruption and localize DutyRotation.cs (Phase 3)"`

---

### Step 3:Phase 4 - 命令系统聊天消息汉化

**文件**:
- `RotationSolver/Commands/RSCommands_BasicInfo.cs`(2 处)
- `RotationSolver/Commands/RSCommands_StateSpecialCommand.cs`(14 处)
- `RotationSolver/Commands/RSCommands_OtherCommand.cs`(23 处)

**执行方式**:
1. 逐文件 `Grep` 定位所有 `Svc.Chat.Print` / `Svc.Chat.PrintError` 调用
2. 翻译字符串字面量,保留 `$"..."` 插值占位符
3. 注意:`Targeting : ...` 类消息翻译为 `目标选择:...`

**关键翻译**:
- `Invalid setting command format.` → `无效的设置命令格式。`
- `Failed to parse the value.` → `解析值失败。`
- `Failed to parse the value as boolean.` → `无法将值解析为布尔类型。`
- `Changed setting {property.Name} to {command}` → `已将设置 {property.Name} 改为 {command}`
- `Targeting :` → `目标选择:`
- `State changed to` → `状态切换为`
- `Rotation set to` → `循环已设置为`
- `Invalid` → `无效`
- `Unknown` → `未知`
- `Added` / `Removed` → `已添加` / `已移除`

**验证**:
- `git diff --stat RotationSolver/Commands/`
- 抽查 3 处,确认 `$"..."` 占位符保留

**提交**:`git commit -m "zh: localize RSCommands chat messages (Phase 4)"`

---

### Step 4:Phase 5 - UI 层汉化

**文件**:
- `RotationSolver/UI/RotationConfigWindow.cs`(11 处 `CNLanguageClient` + `_baseUsageHints` 40 条)
- `RotationSolver/UI/FirstStartTutorialWindow.cs`(~12 个 `TutorialStep`)

#### 5.1 RotationConfigWindow.cs - CNLanguageClient 分支
- 第 61 行 `CNLanguageClient` 定义(保留)
- 10 个使用分支(约第 251、311、320、330、349、592、602、618、757 等行)
- 结构:`if (CNLanguageClient) { 中文 } else { 英文 }`
- 检查每个分支的中文文案,补全/修正为符合国服习惯的翻译

#### 5.2 RotationConfigWindow.cs - _baseUsageHints 数组(第 108 行)
40 条提示字符串,逐条翻译。注意:
- 命令名(`/rotation`、`/rsr`)保留英文
- UI 标签名(Actions、Auto、Basic、UI、List、Target、Extra)保留英文,因为是实际界面标识
- 仅翻译说明文字

#### 5.3 FirstStartTutorialWindow.cs - TutorialStep 数组(第 19 行起)
~12 个 `TutorialStep`,每个含 Title、Description、Bullets:
- `"Welcome!"` → `"欢迎!"`
- `"This walkthrough explains..."` → `"本向导将介绍如何配置 Rotation Solver Reborn..."`
- 各 `Bullets` 数组中的英文说明逐条翻译
- `RotationConfigWindowTab.Main` 等枚举引用保留
- `"Recommended macros:"` → `"推荐宏:"`
- `"Macro copied to clipboard."` → `"宏已复制到剪贴板。"`
- `"Back"` → `"上一步"`、`"Next"` → `"下一步"`、`"Finish"` → `"完成"`
- `"Open {step.Tab} tab"` → `"打开 {step.Tab} 标签"`(注意:`{step.Tab}` 是标签名,保留英文)
- `"Copy##TutorialMacro_{i}"` 中 `##` 后的 ID 保留

**验证**:
- `git diff --stat RotationSolver/UI/`
- 抽查 `CNLanguageClient` 分支结构完整(else 分支未被破坏)
- 确认 `_baseUsageHints` 数组元素数仍为 40
- 确认 `TutorialStep` 构造参数顺序未被改动

**提交**:`git commit -m "zh: localize UI hints, tutorial, and CNLanguageClient branches (Phase 5)"`

---

### Step 5:Phase 6 - 循环配置汉化

**范围**:40 个文件,282 个 `Name = "..."` 属性

**文件分布**(基于 Grep 验证):
- Tank:WAR(12)、PLD(15)、GNB(2)、DRK(7)
- Ranged:MCH(7)、DNC(4)、BRD(11)
- Melee:VPR(9)、SAM(4)、RPR(3)、MNK(6)、NIN(5)、DRG(5)
- Magical:SMN(14)、RDM(9)、PCT(8)、BLM_Default(5)、BLM_RP(4)
- Healer:WHM(16)、SGE(22)、SCH(23)、AST(17)
- Limited Jobs:BLU(10)
- Duty:Variant(1)、Phantom(24)、MonsterHunter(4)、Emanation(1)、Bozja(2)
- PVP:22 个文件,合计约 45 个 Name 属性

**翻译原则**:
- `[RotationConfig(Name = "英文", ...)]` → `[RotationConfig(Name = "中文", ...)]`
- 保留其他参数(`PvEFilter`、`PvPFilter`、`Description` 等)
- 职业专属术语参照国服习惯

**执行方式**:
- 按职业类型分批,每批 1-3 个文件
- 使用 `Read` 读取文件 → 逐个 `Edit` 替换 `Name = "..."` 的字符串
- 可启动子代理并行处理 PvP 文件(独立性强)

**验证**:
- `git diff --stat RotationSolver/RebornRotations/`
- 抽查每个职业 1-2 个文件,确认属性参数完整
- 确认 `PvPFilter`、`Description` 等参数未被误改

**提交**(分 3 次以控制粒度):
- `git commit -m "zh: localize PvE rotation configs (Phase 6a)"`
- `git commit -m "zh: localize PvP rotation configs (Phase 6b)"`
- `git commit -m "zh: localize Duty rotation configs (Phase 6c)"`

---

### Step 6:Phase 7 - 收尾与最终验证

#### 7.1 IncompatiblePlugins.json
9 条 `Features` 字段翻译:
- `"May have issues with Auto rotation and targetting conflicts"` → `"可能与自动循环和目标选择产生冲突"`
- `"Fork of XIV Combo, may have issues with auto rotation and targetting conflicts"` → `"XIV Combo 的 Fork 版本,可能与自动循环和目标选择产生冲突"`
- `"Fork of XIVSlothCombo, may have issues with auto rotation and targetting conflicts"` → `"XIVSlothCombo 的 Fork 版本,可能与自动循环和目标选择产生冲突"`
- `"Combat Reborn fork of Bossmod, may have issues with auto rotation and targetting conflicts, though mitigations exist"` → `"Bossmod 的 Combat Reborn Fork 版本,可能与自动循环和目标选择产生冲突,但已有缓解措施"`
- `"Auto rotation and targetting conflicts"` → `"自动循环与目标选择冲突"`
- `"Skill targetting conflicts"` → `"技能目标选择冲突"`
- `"May have issues with skill queueing settings"` → `"可能与技能排队设置产生冲突"`

**注意**:`Name` 字段(插件名)保持英文不译。

#### 7.2 README.md 更新
更新"汉化内容"表格,新增 Phase 1-7 所有改动文件。

#### 7.3 最终验证
1. **改动范围统计**:`git diff --stat main..HEAD` 确认所有目标文件均已改动
2. **错误数对比**:
   ```powershell
   dotnet build RotationSolver/RotationSolver.csproj -c Release 2>&1 | Select-String "error CS" | Measure-Object | Select-Object Count
   ```
   错误数应保持在 ~556(预先存在的源生成器 Bug),无新增翻译导致的错误
3. **抽查清单**:
   - Configs.cs:确认 `Filter = DutySpecificXxx` 等参数保留
   - CustomRotation_OtherInfo.cs:确认 `JobBuffs` 字典键未改、第 1430 行已修复
   - RotationConfigWindow.cs:确认 `CNLanguageClient` 的 else 分支完整
   - 1 个 Tank / 1 个 Healer / 1 个 PvP 文件:确认 `[RotationConfig]` 参数完整
4. **占位符检查**:随机抽查 5 处 `$"..."` 字符串,确认 `{x}` 占位符保留

#### 7.4 最终提交
- `git commit -m "zh: localize IncompatiblePlugins.json and update README (Phase 7)"`

---

## 假设与决策

1. **OtherConfiguration.cs 的 `name="..."` 保留英文**:这些是列表 ID(如 `name="AoE"`、`name="Tank Buster"`),作为程序内部标识符,且部分含不译术语(AoE)。这与 Configs.cs 的 markdown `name="..."` 不同——Configs.cs 的 name 是显示给用户的设置项标题,需翻译。
2. **Configs.cs 的 markdown `name="..."` 需翻译**:这些是显示给用户的设置组标题(如 `name="Action Ahead"` → `name="提前量"`),与已翻译的 HP 阈值 name 保持一致。
3. **不重构代码**:仅翻译字符串字面量,不改动任何逻辑、不调整属性参数顺序、不修改方法签名。
4. **不翻译的内容**:
   - 代码注释(`//`、`/// <summary>` 中的英文说明)——除非是 `/// <markdown>` 块
   - 调试日志(`PluginLog.Information` / `PluginLog.Warning`)
   - 枚举值名称(如 `DTRType.DTRNormal`)
   - 插件名(`Rotation Solver Reborn`)
5. **编译验证策略**:鉴于预先存在的 556 错误,采用"错误数不增加"作为通过标准。
6. **提交粒度**:每个 Step 一次提交,Phase 6 拆分为 PvE/PvP/Duty 三次提交。

## 风险与缓解

| 风险 | 缓解措施 |
|---|---|
| `Edit` 大块替换失败 | 拆分为小批量 Edit(1-5 条/次) |
| 误改 `nameof(...)` 引用 | 抽查时重点检查 `Parent = nameof(...)` 参数 |
| 误删 `##` 后的 ImGui ID | UI 层翻译时仅替换 `##` 前的可见文本 |
| 占位符 `{x}` 被翻译破坏 | 抽查 5 处 `$"..."` 字符串确认占位符保留 |
| Phase 6 工作量大(282 条) | 可启动子代理并行处理 PvP 文件(独立性强) |
| markdown name 翻译不一致 | Step 1 统一翻译所有 17 个英文 name |

## 验收标准

1. 所有 6 个 Step 的文件均已翻译并提交
2. `git diff --stat main..HEAD` 显示改动文件覆盖计划范围
3. 编译错误数保持在 ~556(无新增翻译导致的错误)
4. 抽查 10 处翻译,占位符、ImGui ID、属性参数均保留完整
5. 翻译术语符合 FF14 国服习惯,约定俗成的英文术语(GCD/AoE/PvP 等)保持不译
6. CustomRotation_OtherInfo.cs 第 1430 行损坏已修复
7. Configs.cs 所有 markdown `name="..."` 翻译一致(英文术语除外)
