# RotationSolverReborn 中文汉化版

基于 [FFXIV-CombatReborn/RotationSolverReborn](https://github.com/FFXIV-CombatReborn/RotationSolverReborn) 的 UI 中文汉化 Fork。

## 汉化内容

| 文件 | 改动 |
|---|---|
| `RotationSolver/Data/UiString.cs` | 222 条 `[Description]` 字符串翻译为中文 |
| `RotationSolver/UI/RotationConfigWindowTab.cs` | 侧边栏 Tab 中文名(上游已有,保留) |

翻译基于:
- [RSR 官方 Wiki](https://github.com/FFXIV-CombatReborn/RotationSolverReborn/wiki)(Actions / Auto / Basic / Extra / List 5 个页面)
- FF14 国服通行术语(GCD / oGCD / AoE / 坦克死刑 / 击退 / 凝视 / 减伤 / 无敌 / 复活 / 转嫁仇恨 / 身位 / 驱散)
- 国服副本类型官方译名(零式 / 绝境战 / 极蛮神 / 联盟突袭 / 深层迷宫 / 异闻迷宫 / 宝物迷宫 / 假面狂欢 等)

**不修改任何逻辑代码**,功能与上游完全一致,仅 UI 文案为中文。

## 安装(普通用户)

### 方式 A: 添加自定义插件源(推荐,支持自动更新)

1. 打开游戏,聊天框输入 `/xlsettings`
2. 切到 `Experimental` 选项卡,找到 `Custom Plugin Repositories` 区段
3. 在空文本框粘贴:
   ```
   https://raw.githubusercontent.com/YangYangaaaa/RotationSolverReborn-zh/gh-pages/pluginmaster.json
   ```
4. 点 `+` 按钮添加,确保旁边的复选框勾上
5. 点右下角保存图标
6. 关闭官方源里 RSR 的自动更新(避免冲突):
   - Dalamud Settings → Repository → 找到官方 RSR 所在源 → 关闭
   - 或在 Plugin Installer 里卸载官方版 RSR
7. 在 Plugin Installer 的 `Available Plugins` 里找到中文版 RSR,安装

之后每次启动游戏 Dalamud 会自动从你的源拉取最新版。

### 方式 B: 手动覆盖 dll(临时使用)

1. 去 [Releases](https://github.com/YangYangaaaa/RotationSolverReborn-zh/releases) 下载最新 zip
2. 关闭游戏
3. 解压覆盖到 `%APPDATA%\XIVLauncher\installedPlugins\RotationSolverReborn\latest\`
4. 关闭 Dalamud 中 RSR 所在源的自动更新

## 上游同步(维护者)

### 一次性配置

```powershell
git clone https://github.com/YangYangaaaa/RotationSolverReborn-zh.git D:\project\rsr-zh\RotationSolverReborn-zh
cd D:\project\rsr-zh\RotationSolverReborn-zh
git remote add upstream https://github.com/FFXIV-CombatReborn/RotationSolverReborn.git
git fetch upstream
```

### 检查是否有更新

```powershell
.\sync-upstream.ps1 -DryRun
```

### 同步并发布

```powershell
.\sync-upstream.ps1
```

脚本会:
1. fetch 上游
2. 同步 main 分支到 upstream/main
3. rebase zh-cn 分支到最新 main
4. force-with-lease 推送 zh-cn 到 origin
5. 触发 GitHub Action 自动构建并发布

GitHub Action 跑完后:
- 新版本出现在 Releases 页
- gh-pages 分支的 pluginmaster.json 自动更新
- 下次启动游戏时 Dalamud 自动拉取新版

### 冲突处理

若 rebase 冲突,脚本会停止并提示冲突文件。冲突通常只在 `UiString.cs`,因为上游可能新增 enum 项:

```
<<<<<<< HEAD
[Description("技能条件")]              # 你的汉化
=======
[Description("Action Condition")]      # 上游未改
[Description("New Feature XYZ")]       # 上游新增项
ConfigWindow_NewFeature,
>>>>>>> main
```

解决方法:
```powershell
# 编辑 UiString.cs,保留你的中文 + 翻译新增项
# 然后:
git add RotationSolver/Data/UiString.cs
git rebase --continue
.\sync-upstream.ps1 -Force
```

## 翻译约定

| 英文 | 中文 | 备注 |
|---|---|---|
| GCD / oGCD / AoE | 不译 | 国服玩家通用 |
| Rotation | 循环 | 跟上游 CNString 一致 |
| Action | 技能 | 不是"动作" |
| Tank Buster | 坦克死刑 | 国服通行 |
| Knockback | 击退 | |
| Gaze/Stop | 凝视/止息 | Stop 在此语境为止息类 debuff |
| Dispel | 驱散 | |
| Stance | 姿态 | |
| Positional | 身位 | 近战身位机制 |
| Shirk | 转嫁仇恨 | |
| Raise | 复活 | |
| Burst | 爆发 | |
| Anti-Knockback | 防击退 | |
| Mitigation | 减伤 | |
| Invulnerability | 无敌 | |
| HoT / DoT | 持续恢复 / 持续伤害 | |
| Ultimate | 绝境战 | 国服官方 |
| Savage | 零式 | 国服官方 |
| Extreme | 极蛮神 | 国服官方 |
| Dungeon | 地下城 | 国服官方 |
| Deep Dungeon | 深层迷宫 | 国服官方 |
| Variant Dungeon | 异闻迷宫 | 国服官方 |
| Alliance Raid | 联盟突袭 | 国服官方 |
| The Masked Carnivale | 假面狂欢 | 国服官方(蓝魔) |
| Forced Condition | 强制条件 | RSR 特有 |
| Disabled Condition | 禁用条件 | RSR 特有 |

## 升级频率建议

不要每次上游 commit 都跟,只在以下情况 rebase:
- 上游发 Release
- 7.x 游戏补丁后 RSR 适配版本
- BossModReborn / AutoDuty 联动 API 变更

## License

跟上游一致,见 [LICENSE](LICENSE)。

## 致谢

- [FFXIV-CombatReborn/RotationSolverReborn](https://github.com/FFXIV-CombatReborn/RotationSolverReborn) 原作者
- RSR Wiki 提供术语上下文
- FF14 国服社区提供术语参考
