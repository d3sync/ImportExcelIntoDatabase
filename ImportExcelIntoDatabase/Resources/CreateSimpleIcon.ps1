# Simple icon creator - creates a basic ICO file
Add-Type -AssemblyName System.Drawing

Write-Host "Creating application icon..."

# Create multiple sizes for better quality
$sizes = @(16, 32, 48, 256)
$icons = @()

foreach ($size in $sizes) {
    # Create bitmap
    $bitmap = New-Object System.Drawing.Bitmap($size, $size)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    
    # Background gradient (Excel green to SQL blue)
    $rect = New-Object System.Drawing.Rectangle(0, 0, $size, $size)
    $startColor = [System.Drawing.Color]::FromArgb(33, 115, 70)   # Excel green
    $endColor = [System.Drawing.Color]::FromArgb(0, 120, 212)     # SQL blue
    $brush = New-Object System.Drawing.Drawing2D.LinearGradientBrush($rect, $startColor, $endColor, 45)
    $graphics.FillRectangle($brush, $rect)
    
    # Draw simple Excel sheet icon (white grid)
    $pen = New-Object System.Drawing.Pen([System.Drawing.Color]::White, [Math]::Max(1, $size / 64))
    $gridSize = [Math]::Floor($size * 0.3)
    $cellSize = [Math]::Floor($gridSize / 3)
    $startX = [Math]::Floor($size * 0.15)
    $startY = [Math]::Floor($size * 0.25)
    
    for ($i = 0; $i -le 3; $i++) {
        $x = $startX + ($i * $cellSize)
        $graphics.DrawLine($pen, $x, $startY, $x, $startY + $gridSize)
    }
    for ($i = 0; $i -le 3; $i++) {
        $y = $startY + ($i * $cellSize)
        $graphics.DrawLine($pen, $startX, $y, $startX + $gridSize, $y)
    }
    
    # Draw arrow
    $arrowPen = New-Object System.Drawing.Pen([System.Drawing.Color]::White, [Math]::Max(2, $size / 32))
    $arrowPen.EndCap = [System.Drawing.Drawing2D.LineCap]::ArrowAnchor
    $arrowPen.CustomEndCap = New-Object System.Drawing.Drawing2D.AdjustableArrowCap(3, 3)
    $midY = [Math]::Floor($size / 2)
    $arrowStart = [Math]::Floor($size * 0.45)
    $arrowEnd = [Math]::Floor($size * 0.60)
    $graphics.DrawLine($arrowPen, $arrowStart, $midY, $arrowEnd, $midY)
    
    # Draw database cylinder
    $dbPen = New-Object System.Drawing.Pen([System.Drawing.Color]::White, [Math]::Max(1, $size / 64))
    $dbX = [Math]::Floor($size * 0.65)
    $dbY = [Math]::Floor($size * 0.30)
    $dbWidth = [Math]::Floor($size * 0.25)
    $dbHeight = [Math]::Floor($size * 0.40)
    $ellipseHeight = [Math]::Max(3, [Math]::Floor($dbHeight * 0.15))
    
    $graphics.DrawEllipse($dbPen, $dbX, $dbY, $dbWidth, $ellipseHeight)
    $graphics.DrawLine($dbPen, $dbX, $dbY + $ellipseHeight/2, $dbX, $dbY + $dbHeight)
    $graphics.DrawLine($dbPen, $dbX + $dbWidth, $dbY + $ellipseHeight/2, $dbX + $dbWidth, $dbY + $dbHeight)
    $graphics.DrawEllipse($dbPen, $dbX, $dbY + $dbHeight - $ellipseHeight, $dbWidth, $ellipseHeight)
    
    $graphics.Dispose()
    $icons += $bitmap
}

# Save as PNG for preview
$icons[3].Save("$PSScriptRoot\AppIcon.png", [System.Drawing.Imaging.ImageFormat]::Png)
Write-Host "? PNG icon created: AppIcon.png"

# Create simple ICO with 48x48 size
try {
    $icoPath = "$PSScriptRoot\app.ico"
    $icon48 = $icons[2] # 48x48
    
    # Create memory stream for ICO
    $ms = New-Object System.IO.MemoryStream
    $writer = New-Object System.IO.BinaryWriter($ms)
    
    # ICO header (6 bytes)
    $writer.Write([UInt16]0)      # Reserved
    $writer.Write([UInt16]1)      # Type (1 = icon)
    $writer.Write([UInt16]1)      # Number of images
    
    # Image directory entry (16 bytes)
    $writer.Write([byte]48)       # Width
    $writer.Write([byte]48)       # Height  
    $writer.Write([byte]0)        # Color palette
    $writer.Write([byte]0)        # Reserved
    $writer.Write([UInt16]1)      # Color planes
    $writer.Write([UInt16]32)     # Bits per pixel
    
    # Convert PNG to bytes
    $pngStream = New-Object System.IO.MemoryStream
    $icon48.Save($pngStream, [System.Drawing.Imaging.ImageFormat]::Png)
    $pngBytes = $pngStream.ToArray()
    
    $writer.Write([UInt32]$pngBytes.Length)  # Image size
    $writer.Write([UInt32]22)                 # Image offset (6 + 16)
    
    # Write PNG data
    $writer.Write($pngBytes)
    
    # Write to file
    [System.IO.File]::WriteAllBytes($icoPath, $ms.ToArray())
    
    $writer.Close()
    $ms.Close()
    $pngStream.Close()
    
    Write-Host "? ICO file created: app.ico"
    Write-Host ""
    Write-Host "Icon is ready to use!"
}
catch {
    Write-Host "??  Could not create ICO: $($_.Exception.Message)"
    Write-Host "Please use an online converter to convert AppIcon.png to app.ico"
}

# Cleanup
foreach ($icon in $icons) {
    $icon.Dispose()
}

Write-Host ""
Write-Host "Next steps:"
Write-Host "1. The icon files are in the Resources folder"
Write-Host "2. The .csproj file will be updated to include the icon"
Write-Host "3. Build the project to see the icon in action!"
