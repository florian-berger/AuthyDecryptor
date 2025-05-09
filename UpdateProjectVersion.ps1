param (
    [Parameter(Mandatory = $true)]
    [string]$NewVersion
)

# Function to update the version in a project file
function Update-VersionInFile {
    param (
        [string]$FilePath,
        [string]$Version
    )
    Write-Host "Updating version in project file: $FilePath" -ForegroundColor Green

    # Load the file content
    $content = Get-Content -Path $FilePath

    # Update the <Version> tag
    $updatedContent = $content -replace '<Version>.*?</Version>', "<Version>$Version</Version>"

    # Save the updated content back to the file
    Set-Content -Path $FilePath -Value $updatedContent
}

# Function to update the version in the Package.wxs file
function Update-VersionInPackageWxs {
    param (
        [string]$FilePath,
        [string]$Version
    )

    Write-Host "Updating version in Package.wxs file: $FilePath" -ForegroundColor Green

    # Auto-expand version to X.Y.0 if it's in the format X.Y
    if ($Version -match '^\d+\.\d+$') {
        $Version = "$Version.0"
    }

    # Load the file content
    $content = Get-Content -Path $FilePath

    # Update the Version attribute in the Package node
    $updatedContent = $content -replace '(?<=<Package[^>]*\sVersion=")[^"]*', "$Version"

    # Save the updated content back to the file
    Set-Content -Path $FilePath -Value $updatedContent
}

# Get all .csproj files in the current directory and subdirectories
$csprojFiles = Get-ChildItem -Path . -Recurse -Include *.csproj

# Update version in all .csproj files
foreach ($file in $csprojFiles) {
    Update-VersionInFile -FilePath $file.FullName -Version $NewVersion
}

# Find the Package.wxs file
$packageWxsFile = Get-ChildItem -Path . -Recurse -Include Package.wxs

# Update version in the Package.wxs file if it exists
if ($packageWxsFile) {
    Update-VersionInPackageWxs -FilePath $packageWxsFile.FullName -Version $NewVersion
} else {
    Write-Host "Package.wxs file not found." -ForegroundColor Yellow
}

Write-Host "Version update completed for all project files." -ForegroundColor Cyan
