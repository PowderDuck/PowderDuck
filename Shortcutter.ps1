param([string]$source, [string]$destination);
#$source = $args[0];
#$destination = $args[1];

$wShell = New-Object -ComObject WScript.Shell;
$shortCut = $wShell.CreateShortcut($destination);
$shortCut.TargetPath = $source;
$shortCut.Save();