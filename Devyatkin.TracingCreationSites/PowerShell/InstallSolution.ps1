Add-PSSnapin "Microsoft.SharePoint.PowerShell"
$solutionName = "Devyatkin.TracingCreationSites.wsp"
$path ="C:\Users\fedor\Documents\WSP\"+$solutionName
$siteCollection = http://intranet.home-tst.it
Add-SPSolution $path
Install-SPSolution -Identity $solutionName -GACDeployment
Get-SPSolution -Identity $solutionName
iisreset
restart-service sptimerv4