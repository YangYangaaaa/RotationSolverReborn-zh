# 全面汉化 RotationSolverReborn —— 续作计划

## 任务摘要

继续执行 RotationSolverReborn 插件的全面中文汉化工作。翻译须符合 FF14 国服术语习惯,约定俗成的英文术语(如 GCD、oGCD、AoE、PvP、PvE、BLU、RSR、BMR、AutoDuty、Reborn)保持不译。

本计划承接上一会话:Phase 1 已提交(commit ec4e9dda),Phase 2 由后台子代理(4aabd758)处理中。本计划覆盖 Phase 2 收尾及 Phase 3-7 的全部剩余工作。

---

## 当前状态分析(基于 Phase 1 探索)

### 已完成
- **Phase 1 已提交**(commit ec4e9dda `zh: localize Basic enums and attributes`):
  - 13 个文件,涵盖 `RotationSolver.Basic/Attributes/` 与 `RotationSolver.Basic/Data/` 下的枚举 `[Description]` 属性
  - 包括 RangeAttribute、DescType、TargetType、TargetHostileType、RaiseType、HardCastRaiseType、CycleType、DTRType、TinctureUseType、CanUseOption、RSCommandType、ActionBasicInfo、CustomRotation_BasicInfo

### 进行中(Phase 2)
- **后台子代理 4aabd758** 正在翻译 `Configs.cs`:
  - 已完成:Duty Specific 区段(42 条)、PvP 区段(17 条)、AutoActionUsage 开头(~10 条)、副本完成后自动关闭/changelogPopup/showInfoOnDtr/DTRType/showInfoOnToast/poslockCasting(约到第 426 行)
  - 待完成:Configs.cs 第 428-1412 行剩余的 `[UI]` / `[Description]` 条目(HealingActionCondition、UiWindows、BasicTimer、BasicParams、Target、Jobs、Float 等区段),以及文件末尾的 toast 提示字符串("Configs backed up."、"Configs restored. Closing to set." 等)
- **OtherConfiguration.cs**:9 个 `/// <markdown>` 块仍未翻译(子代理尚未开始)

### 未开始
| Phase | 文件 | 工作量 |
|---|---|---|
| 3 | CustomRotation_OtherInfo.cs | 83 个 `[Description]` |
| 3 | DutyRotation.cs | 12 个 `[Description]` |
| 4 | RSCommands_BasicInfo.cs | 2 处 `Svc.Chat.Print` |
| 4 | RSCommands_StateSpecialCommand.cs | 14 处 `Svc.Chat.Print` |
| 4 | RSCommands_OtherCommand.cs | 23 处 `Svc.Chat.Print` |
| 5 | RotationConfigWindow.cs | 10 个 `CNLanguageClient` 分支 + `_baseUsageHints` 数组(40 条提示,第 108-149 行) |
| 5 | FirstStartTutorialWindow.cs | ~10 个 `TutorialStep`(Title/Description/Bullets) |
| 6 | RebornRotations/ 下 40 个文件 | 282 个 `[RotationConfig(Name = "...")]` |
| 7 | Resources/IncompatiblePlugins.json | 9 条 `Features` 字段 |
| 7 | README.md | 更新汉化范围说明 |

### 编译验证说明(重要)
项目存在**预先存在的源生成器 Bug**:`Configuration_Configs.g.cs` 报 556 处 `JsonIgnoreAttribute`/`JsonPropertyAttribute` 未找到错误。已通过 `git stash` 测试确认此错误与翻译无关,是 `RotationSolver.SourceGenerators.JobConfigGenerator` 的缺陷。**因此不能依赖 `dotnet build` 成功作为验证标准**,改用以下策略:
- `git diff --stat` 检查改动范围
- 人工抽查翻译质量(占位符、ImGui ID、属性参数是否保留)
- 错误数对比(翻译前后错误数应保持一致或仅因新增中文导致无新增错误)

---

## 翻译约定(沿用 README.md 与 Phase 1 风格)

### 保持不译的术语
- **战斗机制**:GCD、oGCD、AoE、DoT、HoT、TTK、MP、TP
- **游戏模式**:PvP、PvE、BLU(青魔)、RSR、BMR、AutoDuty、Reborn
- **职业缩写**:AST、BRD、DNC、DRG、MNK、NIN、PCT、RPR、RDM、SCH、SMN、PLD、WAR、DRK、GNB、BLM、WHM、SGE、SAM、VPR、MCH、BLU、BSM
- **副本编号**:O12S、M8S、M9S、M10S、M4S 等(国际服缩写已成国服习惯)
- **Dalamud/ImGui 技术词**:DTR、Toast、ImGui

### 国服译名规范
- **副本类型**:零式 / 绝境战 / 极蛮神 / 联盟突袭 / 深层迷宫 / 异闻迷宫 / 宝物迷宫 / 假面狂欢 / 变体迷宫
- **副本名**:亚历山大绝境战(TEA)、欧米茄绝境战(TOP)、未来重现绝境战(FRU)、狂飙舞绝境战(DMU)
- **战斗术语**:坦克/治疗/近战/远程/魔法、减伤、无敌、复活、读条、瞬发、坦克姿态、死刑、击退、凝视、驱散、净化、守护、冲刺
- **状态**:扁平伤害、即死、减速、石化、麻痹、打断、失明、眩晕、催眠、束缚、迟缓

### 必须保留的代码元素
- **ImGui ID**:`"可见文本##UniqueId"` 中 `##` 后的部分必须原样保留
- **字符串插值占位符**:`{x}`、`{0}`、`{x:F2}` 等必须保留
- **属性参数名**:`Filter =`、`Section =`、`Parent =`、`PvEFilter =`、`PvPFilter =` 等
- **枚举值引用**:`nameof(SomeEnum)`、`JobFilterType.Tank` 等
- **转义字符**:`\r\n`、`\"`、`%%` 等

---

## 实施步骤

### Phase 2:收尾 Basic 配置层(等待子代理 + 补充)

**执行者**:主代理协调,子代理 4aabd758 完成

1. **等待子代理完成** `Configs.cs` 剩余翻译(第 428-1412 行):
   - UiInformation 区段:Teaching Mode、Auto Mode、Countdown、Healing non-healer、Interrupt、Provoke、Cleanse、Debug、Action tracer、Toggle commands、Window settings
   - Extra 区段:Colors、Cactbot timeline、Target color、Window backgrounds
   - TargetConfig 区段(第 1128-1249 行):Movement actions、Attack markers、Enemy parts、Stop markers、1hp invincible、FATE、Vision cone、Hunt/Relic/Leve priority、Quest priority、Forlorn、Target dummies、TTK、Engage settings
   - Jobs 区段:Lucid Dreaming MP threshold、Healing thresholds(8 个 HP% 配置)、Tank invulnerability HP、PvP rotation choice
   - 文件末尾:Toast 提示("Configs backed up."、"Configs restored. Closing to set."、"Backed up configs are not compatible..."、"Github download failed." 等)

2. **主代理翻译** `OtherConfiguration.cs` 的 9 个 `/// <markdown>` 块:
   - HostileCastingArea(AoE)
   - HostileCastingTank(Tank Buster)
   - HostileCastingKnockback(Knockback)
   - HostileCastingStop(Gaze/Stop)
   - BeneficialPositions
   - DangerousStatus(Dispellable Debuffs)
   - PriorityStatus(Priority)
   - InvincibleStatus(Invulnerability)
   - NoCastingStatus(No-Casting Debuffs)
   - 注意:`<see cref="...">` 标签内的代码引用保留,只翻译说明文字

3. **验证**:
   - `git diff --stat RotationSolver.Basic/Configuration/`
   - 抽查 5 处翻译,确认占位符与属性参数保留
   - 确认未误改 `nameof(...)` 引用

4. **提交**:`git commit -m "zh: localize Configs.cs and OtherConfiguration.cs (Phase 2)"`

---

### Phase 3:状态面板汉化

**执行者**:主代理

**文件**:
- `RotationSolver.Basic/Rotations/CustomRotation_OtherInfo.cs`(1910 行,83 个 `[Description]`)
- `RotationSolver.Basic/Rotations/Duties/DutyRotation.cs`(12 个 `[Description]`)

**翻译清单**(CustomRotation_OtherInfo.cs 关键条目):
| 英文 | 中文 |
|---|---|
| IsCasting | 正在读条 |
| Is RSR active | RSR 已激活 |
| Has Swift | 有瞬发 |
| Has tank stance | 有坦克姿态 |
| Is Moving or Jumping | 正在移动或跳跃 |
| Is Dead, or inversely, is Alive | 已死亡 |
| In Combat | 战斗中 |
| Not In Combat Delay | 非战斗延迟 |
| Player's MP | 玩家 MP |
| Has companion | 有随行宠 |
| Has Chocobo | 有陆行鸟 |
| Is Full Party | 满编小队 |
| Has Swift (DutyRotation) | 有瞬发 |
| Has tank stance (DutyRotation) | 有坦克姿态 |
| Is burst | 爆发期 |
| The state of auto. True for on. | 自动状态(开) |
| The state of manual. True for manual. | 手动状态 |
| Has hostiles in Range | 范围内有敌人 |
| Has hostiles in 25 yalms | 25 米内有敌人 |
| The number of hostiles in Range | 范围内敌人数 |
| The number of hostiles in max Range | 最大范围内敌人数 |
| The number of all hostiles in Range | 范围内全部敌人数 |
| The number of all hostiles in max Range | 最大范围内全部敌人数 |
| Min HP | 最低 HP |
| Average HP | 平均 HP |
| Has Tank | 有坦克 |
| Has Healer | 有治疗 |
| Has Melee | 有近战 |
| Has Ranged | 有远程 |
| Has Magic | 有魔法 |
| BMR Active | BMR 激活 |
| BMR Raidwide In | BMR 范围攻击倒计时 |
| BMR Tankbuster In | BMR 坦克死刑倒计时 |
| BMR Knockback In | BMR 击退倒计时 |
| Just used GCD | 刚使用 GCD |
| Just used Ability | 刚使用能力技 |
| Just used Action | 刚使用技能 |
| Just used Combo Action | 刚使用连击技能 |
| Combat Time | 战斗时间 |
| Is In High End Duty | 在高难副本中 |
| Is In Alliance Raid | 在联盟突袭中 |
| Is In UCoB | 在巴哈姆特绝境战中 |
| Is In UwU | 在究极神兵绝境战中 |
| Is In TEA | 在亚历山大绝境战中 |
| Is In DSR | 在龙诗绝境战中 |
| Is In TOP | 在欧米茄绝境战中 |
| Is In FRU | 在未来重现绝境战中 |
| Is In DMU | 在狂飙舞绝境战中 |
| Is In COD | 在暗黑之云中 |
| Can heal area ability | 可群体能力治疗 |
| Can heal single ability | 可单体能力治疗 |
| Can heal area spell | 可群体魔法治疗 |
| Can heal single spell | 可单体魔法治疗 |
| Health threshold (余下治疗类条目) | HP 阈值 |
| PotionStrategy 枚举(4 条) | 药品策略相关 |

**执行方式**:
- 由于 `[Description]` 是编译期特性,无法用 `CNLanguageClient` 双分支,必须直接替换
- 按区段(Player / Friends / Combat / BMR / Action History / Territory / Healing)分批 `Edit`
- 每个 `Edit` 调用处理 1-5 个相邻条目,避免大块替换失败

**DutyRotation.cs 翻译**(12 条):参照上表对应条目

**验证**:
- `git diff --stat`
- 抽查:确认 `JobBuffs` 字典中的职业缩写键(AST/BRD/DNC 等)未被误改
- 确认 `StatusID.xxx` 枚举引用未被误改

**提交**:`git commit -m "zh: localize CustomRotation_OtherInfo.cs and DutyRotation.cs (Phase 3)"`

---

### Phase 4:命令系统聊天消息汉化

**执行者**:主代理

**文件**:
- `RotationSolver/Commands/RSCommands_BasicInfo.cs`(2 处)
- `RotationSolver/Commands/RSCommands_StateSpecialCommand.cs`(14 处)
- `RotationSolver/Commands/RSCommands_OtherCommand.cs`(23 处)

**执行方式**:
- 逐文件 `Grep` 定位所有 `Svc.Chat.Print` / `Svc.Chat.PrintError` 调用
- 翻译字符串字面量,保留 `$"..."` 插值占位符
- 注意:`Targeting : ...` 类消息(UpdateState、AutodutyUpdateState)需翻译为 `目标选择:...`
- 警告/错误消息(`PrintError`)翻译为中文,保留 `!` 等标点

**关键翻译**:
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

### Phase 5:UI 层汉化

**执行者**:主代理

**文件**:
- `RotationSolver/UI/RotationConfigWindow.cs`(10 个 `CNLanguageClient` 分支 + `_baseUsageHints` 40 条)
- `RotationSolver/UI/FirstStartTutorialWindow.cs`(~10 个 `TutorialStep`)

#### 5.1 RotationConfigWindow.cs - CNLanguageClient 分支
`CNLanguageClient` 在第 61 行定义为语言检测,第 251、311、320、330、349、592、602、618、757 行有 10 个分支。每个分支结构为:
```csharp
if (CNLanguageClient)
{
    // 中文文案
}
else
{
    // 英文文案
}
```
**执行方式**:逐个分支读取上下文,将中文文案替换/补全为符合国服习惯的翻译。若分支已有中文,检查并修正;若为空或英文,补充中文。

#### 5.2 RotationConfigWindow.cs - _baseUsageHints 数组(第 108-149 行)
40 条提示字符串,全部为英文。逐条翻译:
- `"Right-click any action, setting, or toggle to view/copy its macro chat command."` → `"右键点击任意技能、设置或开关,可查看/复制其宏聊天命令。"`
- `"Use /rsr as a shorter alias for /rotation."` → `"使用 /rsr 作为 /rotation 的简短别名。"`
- ...等 40 条

**注意**:提示中引用的命令名(`/rotation`、`/rsr`)、UI 标签名(Actions、Auto、Basic、UI、List、Target、Extra)保留英文,因为这些是实际界面/命令标识。中文翻译只针对说明文字。

#### 5.3 FirstStartTutorialWindow.cs - TutorialStep 数组(第 19 行起)
~10 个 `TutorialStep`,每个含 Title、Description、Bullets。逐个翻译:
- `"Welcome!"` → `"欢迎!"`
- `"This walkthrough explains..."` → `"本向导将介绍如何配置 Rotation Solver Reborn..."`
- 各 `Bullets` 数组中的英文说明逐条翻译
- `RotationConfigWindowTab.Main` 等枚举引用保留

**验证**:
- `git diff --stat RotationSolver/UI/`
- 抽查 `CNLanguageClient` 分支结构完整(else 分支未被破坏)
- 确认 `_baseUsageHints` 数组元素数仍为 40
- 确认 `TutorialStep` 构造参数顺序未被改动

**提交**:`git commit -m "zh: localize UI hints, tutorial, and CNLanguageClient branches (Phase 5)"`

---

### Phase 6:循环配置汉化

**执行者**:主代理 + 可选子代理并行

**范围**:40 个文件,282 个 `[RotationConfig(Name = "...")]` 属性

**文件分布**:
- Tank:WAR(12)、PLD(15)、GNB(2)、DRK(7)
- Ranged:MCH(7)、DNC(4)、BRD(11)
- Melee:VPR(9)、SAM(4)、RPR(3)、MNK(6)、NIN(5)、DRG(5)
- Magical:SMN(14)、RDM(9)、PCT(8)、BLM_Default(5)、BLM_RP(4)
- Healer:WHM(16)、SGE(22)、SCH(23)、AST(17)
- Limited Jobs:BLU(10)
- Duty:Variant(1)、Phantom(24)、MonsterHunter(4)、Emanation(1)、Bozja(2)
- PVPRotations:22 个文件,合计约 45 个 Name 属性

**翻译原则**:
- `[RotationConfig(Name = "英文", ...)]` → `[RotationConfig(Name = "中文", ...)]`
- 保留其他参数(`PvEFilter`、`PvPFilter`、`Description` 等)
- 职业专属术语参照国服习惯:
  - 坦克:减伤、无敌、复仇、原初、暴乱、深奥、血量、仇恨
  - 治疗:再生、医术、群疗、护盾、无中生有、太阳神、地星、连环计、占卜
  - 近战:连击、身位、龙剑、乱击、风魔、必杀剑、分身、原型
  - 远程:诗人歌、舞步、机工炮塔、野火、过热
  - 魔法:黑魔纹、火4、冰4、三连施法、激情、召唤、召唤物
  - PvP:守护、净化、冲刺、LB(极限技)

**执行方式**:
- 按职业类型分批,每批 1-3 个文件
- 使用 `Read` 读取文件 → 逐个 `Edit` 替换 `Name = "..."` 的字符串
- 可选:启动子代理并行处理 PvP 文件(独立性强)

**验证**:
- `git diff --stat RotationSolver/RebornRotations/`
- 抽查每个职业 1-2 个文件,确认属性参数完整
- 确认 `PvPFilter`、`Description` 等参数未被误改

**提交**(可分 2-3 次提交以控制粒度):
- `git commit -m "zh: localize PvE rotation configs (Phase 6a)"`
- `git commit -m "zh: localize PvP rotation configs (Phase 6b)"`
- (可选)`git commit -m "zh: localize Duty rotation configs (Phase 6c)"`

---

### Phase 7:收尾与最终验证

**执行者**:主代理

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
更新"汉化内容"表格,新增:
- `RotationSolver.Basic/Configuration/Configs.cs` - 配置项 `[UI]` / `[Description]` 翻译
- `RotationSolver.Basic/Configuration/OtherConfiguration.cs` - markdown 说明翻译
- `RotationSolver.Basic/Rotations/CustomRotation_OtherInfo.cs` - 状态显示 `[Description]` 翻译
- `RotationSolver.Basic/Rotations/Duties/DutyRotation.cs` - 副本状态 `[Description]` 翻译
- `RotationSolver/Commands/RSCommands*.cs` - 聊天消息翻译
- `RotationSolver/UI/RotationConfigWindow.cs` - `CNLanguageClient` 分支与提示翻译
- `RotationSolver/UI/FirstStartTutorialWindow.cs` - 新手教程翻译
- `RotationSolver/RebornRotations/**/*.cs` - 282 个 `[RotationConfig]` Name 翻译
- `Resources/IncompatiblePlugins.json` - 冲突插件说明翻译

#### 7.3 最终验证
1. **改动范围统计**:`git diff --stat main..HEAD` 确认所有目标文件均已改动
2. **错误数对比**:
   ```powershell
   dotnet build RotationSolver/RotationSolver.csproj -c Release 2>&1 | Select-String "error CS" | Measure-Object | Select-Object Count
   ```
   错误数应保持在 ~556(预先存在的源生成器 Bug),无新增翻译导致的错误
3. **抽查清单**:
   - Configs.cs:确认 `Filter = DutySpecificXxx` 等参数保留
   - CustomRotation_OtherInfo.cs:确认 `JobBuffs` 字典键未改
   - RotationConfigWindow.cs:确认 `CNLanguageClient` 的 else 分支完整
   - 1 个 Tank / 1 个 Healer / 1 个 PvP 文件:确认 `[RotationConfig]` 参数完整
4. **占位符检查**:随机抽查 5 处 `$"..."` 字符串,确认 `{x}` 占位符保留

#### 7.4 最终提交
- `git commit -m "zh: localize IncompatiblePlugins.json and update README (Phase 7)"`

---

## 假设与决策

1. **子代理 4aabd758 完成质量假设**:假设子代理能正确翻译 Configs.cs 剩余部分。主代理在 Phase 2 收尾时需抽查 5-10 处,若发现质量问题则人工修正。
2. **不重构代码**:仅翻译字符串字面量,不改动任何逻辑、不调整属性参数顺序、不修改方法签名。
3. **不翻译的内容**:
   - 代码注释(`//`、`/// <summary>` 中的英文说明)——除非是 `/// <markdown>` 块(Phase 2 的 OtherConfiguration.cs)
   - 调试日志(`PluginLog.Information` / `PluginLog.Warning`)——这些是开发者向,用户不可见
   - 枚举值名称(如 `DTRType.DTRNormal`)——这些是代码标识符
   - 插件名(`Rotation Solver Reborn`)——保留原英文名
4. **编译验证策略**:鉴于预先存在的 556 错误,采用"错误数不增加"作为通过标准,而非"编译成功"。
5. **提交粒度**:每个 Phase 一次提交,Phase 6 可拆分为 PvE/PvP/Duty 三次提交,便于回滚。
6. **不触碰 sync-upstream.ps1**:汉化工作不影响上游同步脚本,rebase 时可能产生冲突的文件(Configs.cs、CustomRotation_OtherInfo.cs)由 `sync-upstream.ps1` 的冲突处理逻辑兜底。

## 风险与缓解

| 风险 | 缓解措施 |
|---|---|
| 子代理翻译质量不一致 | Phase 2 收尾时抽查 5-10 处,必要时人工修正 |
| `Edit` 大块替换失败 | 拆分为小批量 Edit(1-5 条/次) |
| 误改 `nameof(...)` 引用 | 抽查时重点检查 `Parent = nameof(...)` 参数 |
| 误删 `##` 后的 ImGui ID | UI 层翻译时仅替换 `##` 前的可见文本 |
| 占位符 `{x}` 被翻译破坏 | 抽查 5 处 `$"..."` 字符串确认占位符保留 |
| Phase 6 工作量大(282 条) | 可启动子代理并行处理 PvP 文件(独立性强) |

## 验收标准

1. 所有 7 个 Phase 的文件均已翻译并提交
2. `git diff --stat main..HEAD` 显示改动文件覆盖计划范围
3. 编译错误数保持在 ~556(无新增翻译导致的错误)
4. 抽查 10 处翻译,占位符、ImGui ID、属性参数均保留完整
5. 翻译术语符合 FF14 国服习惯,约定俗成的英文术语(GCD/AoE/PvP 等)保持不译
