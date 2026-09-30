# 1. Clean old coverage results
Write-Host "Cleaning old coverage folders..."
Get-ChildItem -Recurse -Directory -Filter "TestResults" | ForEach-Object {
    Remove-Item $_.FullName -Recurse -Force
}

# 2. Run tests with coverage
Write-Host "Running tests..."
dotnet test LibraryDDD.sln --collect:"XPlat Code Coverage"

# 3. Find the newest coverage file
Write-Host "Locating newest coverage file..."
$latestCoverage = Get-ChildItem -Recurse -Filter "coverage.cobertura.xml" |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if (-not $latestCoverage) {
    Write-Host "No coverage file found."
    exit 1
}

Write-Host "Using coverage file: $($latestCoverage.FullName)"

# 4. Generate report
dotnet tool run reportgenerator `
    -reports:$latestCoverage.FullName `
    -targetdir:"coverage-report" `
    -reporttypes:Html

Write-Host "Coverage report generated."
