# push-zh.ps1
# 提交并推送所有汉化改动到 zh-cn 分支
# 用法:
#   .\push-zh.ps1                              # 使用自动生成的 commit message
#   .\push-zh.ps1 -Message "zh: 你的提交说明"   # 指定提交说明

param(
    [string]$Message = ""
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

# 1. 检查当前分支
$currentBranch = (git rev-parse --abbrev-ref HEAD 2>&1).Trim()
if ($currentBranch -ne "zh-cn") {
    Write-Host "当前分支: $currentBranch, 切换到 zh-cn..." -ForegroundColor Yellow
    Invoke-Git "checkout zh-cn"
}

# 2. 检查是否有改动
$statusOutput = git status --porcelain 2>&1
if (-not $statusOutput) {
    Write-Host "没有未提交的改动" -ForegroundColor Green
    exit 0
}

# 3. 暂存所有改动
Write-Host ""
Write-Host "--- 暂存改动 ---" -ForegroundColor Cyan
$files = @()
$statusOutput -split "`n" | ForEach-Object {
    $files += $_.Substring(3)
}
foreach ($f in $files) {
    Invoke-Git "add `"$f`""
}

# 4. 提交
Write-Host ""
Write-Host "--- 提交 ---" -ForegroundColor Cyan
if ($Message) {
    Invoke-Git "commit -m `"$Message`""
} else {
    $count = ($statusOutput -split "`n").Count
    Invoke-Git "commit -m 'zh: update translations' -m 'Auto commit: $count file(s) changed'"
}

# 5. 推送
Write-Host ""
Write-Host "--- 推送 zh-cn 到 origin ---" -ForegroundColor Cyan
Invoke-Git "push origin zh-cn"

Write-Host ""
Write-Host "=== 推送完成 ===" -ForegroundColor Green
Write-Host "GitHub Action 将自动构建并发布新版本" -ForegroundColor Green
Write-Host "查看: https://github.com/YangYangaaaa/RotationSolverReborn-zh/actions" -ForegroundColor Cyan
