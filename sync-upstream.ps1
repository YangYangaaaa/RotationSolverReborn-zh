# sync-upstream.ps1
# 同步上游 RSR 更新到 zh-cn 分支
# 用法:
#   cd D:\project\rsr-zh\RotationSolverReborn-zh
#   .\sync-upstream.ps1              # 默认同步 main
#   .\sync-upstream.ps1 -DryRun      # 只看是否有更新,不实际 rebase
#   .\sync-upstream.ps1 -Force       # 强制推送(用于 rebase 后)

[CmdletBinding()]
param(
    [string]$UpstreamBranch = "main",
    [switch]$DryRun,
    [switch]$Force
)

$ErrorActionPreference = "Stop"

function Invoke-Git {
    param([string]$Cmd)
    Write-Host "PS> git $Cmd" -ForegroundColor DarkGray
    $output = git $Cmd.Split(' ') 2>&1
    $exitCode = $LASTEXITCODE
    if ($output) { Write-Host ($output -join "`n") }
    if ($exitCode -ne 0) {
        Write-Error "git $Cmd 失败 (exit $exitCode)"
    }
    return $output
}

Write-Host "=== RSR 中文版上游同步脚本 ===" -ForegroundColor Cyan

# 1. 检查工作区是否干净
$statusOutput = git status --porcelain 2>&1
if ($statusOutput) {
    Write-Host "工作区不干净,请先 commit 或 stash:" -ForegroundColor Yellow
    Write-Host $statusOutput
    exit 1
}

# 2. 切到 zh-cn 分支
$currentBranch = (git rev-parse --abbrev-ref HEAD 2>&1).Trim()
if ($currentBranch -ne "zh-cn") {
    Write-Host "当前分支: $currentBranch,切换到 zh-cn..."
    Invoke-Git "checkout zh-cn"
}

# 3. 拉取上游
Write-Host ""
Write-Host "--- 拉取上游 upstream/$UpstreamBranch ---" -ForegroundColor Cyan
$upstreamHash = (git rev-parse "upstream/$UpstreamBranch" 2>&1).Trim()
Invoke-Git "fetch upstream"
$newUpstreamHash = (git rev-parse "upstream/$UpstreamBranch" 2>&1).Trim()

if ($upstreamHash -eq $newUpstreamHash) {
    Write-Host "上游无更新 (仍是 $newUpstreamHash),无需同步" -ForegroundColor Green
    exit 0
}

Write-Host "上游有更新: $upstreamHash -> $newUpstreamHash" -ForegroundColor Yellow

if ($DryRun) {
    Write-Host "[DryRun] 不实际执行 rebase" -ForegroundColor Yellow
    exit 0
}

# 4. 同步 main 分支
Write-Host ""
Write-Host "--- 同步本地 main 到 upstream/$UpstreamBranch ---" -ForegroundColor Cyan
Invoke-Git "checkout main"
Invoke-Git "merge upstream/$UpstreamBranch"
Invoke-Git "push origin main"

# 5. 切回 zh-cn, rebase 到最新 main
Write-Host ""
Write-Host "--- Rebase zh-cn 到最新 main ---" -ForegroundColor Cyan
Invoke-Git "checkout zh-cn"
$rebaseResult = Invoke-Git "rebase main"

# 检查 rebase 是否有冲突
$statusOutput = git status --porcelain 2>&1
if ($statusOutput -match "^UU|^AA|^DD") {
    Write-Host ""
    Write-Host "!!! Rebase 冲突 !!!" -ForegroundColor Red
    Write-Host "冲突文件:"
    git diff --name-only --diff-filter=U
    Write-Host ""
    Write-Host "解决冲突后运行:"
    Write-Host "  git add ."
    Write-Host "  git rebase --continue"
    Write-Host "  .\sync-upstream.ps1 -Force"
    exit 1
}

# 6. 推送
Write-Host ""
Write-Host "--- 推送 zh-cn 到 origin ---" -ForegroundColor Cyan
if ($Force) {
    Invoke-Git "push origin zh-cn --force-with-lease"
} else {
    Write-Host "rebase 改写了 zh-cn 历史,使用 --force-with-lease 推送" -ForegroundColor Yellow
    Invoke-Git "push origin zh-cn --force-with-lease"
}

Write-Host ""
Write-Host "=== 同步完成 ===" -ForegroundColor Green
Write-Host "GitHub Action 将自动构建并发布新版本" -ForegroundColor Green
Write-Host "查看: https://github.com/YangYangaaaa/RotationSolverReborn-zh/actions" -ForegroundColor Cyan
