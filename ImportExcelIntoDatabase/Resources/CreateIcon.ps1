# PowerShell script to create a simple application icon
# This creates a 256x256 icon with Excel-to-Database theme

Add-Type -AssemblyName System.Drawing

# Create a 256x256 bitmap
$size = 256
$bitmap = New-Object System.Drawing.Bitmap($size, $size)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic

# Fill background with gradient (Excel green to SQL blue)
$brush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
    (New-Object System.Drawing.Rectangle(0, 0, $size, $size)),
    [System.Drawing.Color]::FromArgb(33, 115, 70),  # Excel green
    [System.Drawing.Color]::FromArgb(0, 120, 212),  # SQL blue
    45
)
$graphics.FillRectangle($brush, 0, 0, $size, $size)

# Draw Excel sheet representation (left side)
$excelColor = [System.Drawing.Color]::FromArgb(255, 255, 255, 200)
$excelPen = New-Object System.Drawing.Pen($excelColor, 3)
$cellSize = 30
$startX = 30
$startY = 60

# Draw grid cells
for ($i = 0; $i -lt 4; $i++) {
    for ($j = 0; $j -lt 3; $j++) {
        $rect = New-Object System.Drawing.Rectangle(
            $startX + ($j * $cellSize),
            $startY + ($i * $cellSize),
            $cellSize,
            $cellSize
        )
        $graphics.DrawRectangle($excelPen, $rect)
    }
}

# Draw arrow (middle)
$arrowPen = New-Object System.Drawing.Pen([System.Drawing.Color]::White, 8)
$arrowPen.EndCap = [System.Drawing.Drawing2D.LineCap]::ArrowAnchor
$arrowPen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
$graphics.DrawLine($arrowPen, 120, $size/2, 180, $size/2)

# Draw database cylinder (right side)
$dbColor = [System.Drawing.Color]::White
$dbPen = New-Object System.Drawing.Pen($dbColor, 3)
$dbBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(100, 255, 255, 255))

$cylinderX = 190
$cylinderY = 80
$cylinderWidth = 50
$cylinderHeight = 100

# Draw database cylinder
$graphics.DrawEllipse($dbPen, $cylinderX, $cylinderY, $cylinderWidth, 20)
$graphics.DrawLine($dbPen, $cylinderX, $cylinderY + 10, $cylinderX, $cylinderY + $cylinderHeight)
$graphics.DrawLine($dbPen, $cylinderX + $cylinderWidth, $cylinderY + 10, $cylinderX + $cylinderWidth, $cylinderY + $cylinderHeight)
$graphics.DrawEllipse($dbPen, $cylinderX, $cylinderY + $cylinderHeight - 10, $cylinderWidth, 20)

# Add "XLS ? DB" text at bottom
$font = New-Object System.Drawing.Font("Arial", 20, [System.Drawing.FontStyle]::Bold)
$textBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::White)
$text = "XLS ? DB"
$textSize = $graphics.MeasureString($text, $font)
$textX = ($size - $textSize.Width) / 2
$textY = $size - 40
$graphics.DrawString($text, $font, $textBrush, $textX, $textY)

# Save as PNG first (ICO format requires additional work)
$pngPath = "$PSScriptRoot\AppIcon.png"
$bitmap.Save($pngPath, [System.Drawing.Imaging.ImageFormat]::Png)

Write-Host "? Icon created at: $pngPath"
Write-Host ""
Write-Host "To convert to ICO format, use one of these free online tools:"
Write-Host "  • https://convertio.co/png-ico/"
Write-Host "  • https://www.icoconverter.com/"
Write-Host "  • https://redketchup.io/icon-converter"
Write-Host ""
Write-Host "Upload AppIcon.png and download as app.ico"
Write-Host "Then save app.ico to: $PSScriptRoot"

# Cleanup
$graphics.Dispose()
$bitmap.Dispose()

# Try to create ICO using .NET (simplified version with single size)
try {
    # Create a 48x48 version for ICO
    $iconSize = 48
    $iconBitmap = New-Object System.Drawing.Bitmap($iconSize, $iconSize)
    $iconGraphics = [System.Drawing.Graphics]::FromImage($iconBitmap)
    $iconGraphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $iconGraphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    
    # Draw scaled version
    $iconGraphics.DrawImage([System.Drawing.Image]::FromFile($pngPath), 0, 0, $iconSize, $iconSize)
    
    # Save as ICO
    $iconPath = "$PSScriptRoot\app.ico"
    $iconStream = [System.IO.File]::Create($iconPath)
    $iconBitmap.Save($iconStream, [System.Drawing.Imaging.ImageFormat]::Icon)
    $iconStream.Close()
    
    $iconGraphics.Dispose()
    $iconBitmap.Dispose()
    
    Write-Host ""
    Write-Host "? ICO file created successfully at: $iconPath"
    Write-Host ""
    Write-Host "The icon has been created and is ready to use!"
}
catch {
    Write-Host ""
    Write-Host "??  Could not create ICO directly. Please use the PNG to ICO converter mentioned above."
    Write-Host "Error: $($_.Exception.Message)"
}
