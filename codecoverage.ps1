$ErrorActionPreference = "Stop"

$root = $PSScriptRoot
$resultsDirectory = Join-Path $root "TestResults/Coverage"
$reportDirectory = Join-Path $root "coverage-report"
$solution = Join-Path $root "LibraryDDD.sln"

Push-Location $root
try {
    Remove-Item $resultsDirectory -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item $reportDirectory -Recurse -Force -ErrorAction SilentlyContinue

    New-Item $resultsDirectory -ItemType Directory -Force | Out-Null

    Write-Host "Running tests with coverage..."
    dotnet test $solution `
        --collect:"XPlat Code Coverage" `
        --results-directory $resultsDirectory

    $testExitCode = $LASTEXITCODE

    $coverageFiles = @(
        Get-ChildItem $resultsDirectory -Filter "coverage.cobertura.xml" -Recurse -File
    )

    if ($coverageFiles.Count -eq 0) {
        Write-Error "No coverage files were produced."
        exit 1
    }

    $reports = ($coverageFiles.FullName -join ";")

    Write-Host "Generating merged coverage report from $($coverageFiles.Count) file(s)..."
    dotnet tool run reportgenerator `
        "-reports:$reports" `
        "-targetdir:$reportDirectory" `
        "-reporttypes:Html"

    if ($LASTEXITCODE -ne 0) {
        Write-Error "ReportGenerator failed."
        exit $LASTEXITCODE
    }

    Write-Host "Coverage report: $(Join-Path $reportDirectory 'index.html')"

    if ($testExitCode -ne 0) {
        Write-Error "Tests failed with exit code $testExitCode. The coverage report was still generated."
        exit $testExitCode
    }
}
finally {
    Pop-Location
}