# -*- mode: shell-script -*-
#
# Script for updating the checked in OpsMgr assemblies the SCX builds are dependent on
# to specified build number - debug, retail and signed builds.
#
# Execute from an VS command line (or make sure tf.exe is in path) like 
# C:\path\to\my\source\ext\OpsMgrSDKFiles> powershell GetDependentAssemblies.ps1 <buildnr>
# where <buildnr> is the build number in OpsMgr tree.
#
param([string]$srcBuildNr)

$opsMgrBuildDropRoot = "\\smbuilds\builds\OM10\en\"    # "

# Root of referenced assemblies structure
$scxExtAssemblyPath = "solutionRoot:\source\ext\OpsMgrSDKFiles"

# The path is relative to the OpsMgr drop directory, but the target dir will be a flat structure
$assemblyList = @(
	"Corgent.Diagramming.DesktopHost.dll",
	"Corgent.Diagramming.Dom.dll",
	"Corgent.Diagramming.EditorBase.dll",
	"Corgent.Diagramming.EditorComponents.dll",
	"Corgent.Diagramming.Export.Visio.dll",
	"Corgent.Diagramming.Serialization.dll",
	"Corgent.Diagramming.Toolbox.dll",
	"Corgent.Diagramming.Utility.dll",
	"Corgent.Toolbox.Core.dll",
	"DundasWinChart.dll",
	"Microsoft.EnterpriseManagement.Core.dll",
	"Monad\OperationsManager\OM10.CoreCommands\Microsoft.EnterpriseManagement.Core.Cmdlets.dll",
	"Monad\OperationsManager\OM10.CoreCommands\Microsoft.EnterpriseManagement.Core.SdkUtilities.dll"
	"Monad\OperationsManager\OM10.CoreCommands\Microsoft.EnterpriseManagement.SqmBase.dll"
    "Microsoft.EnterpriseManagement.HealthService.dll",
    "Microsoft.EnterpriseManagement.Modules.ModuleDebug.dll",
	"Microsoft.EnterpriseManagement.OperationsManager.dll",
    "Microsoft.EnterpriseManagement.Packaging.dll",
	"Microsoft.EnterpriseManagement.Runtime.dll",
	"Microsoft.EnterpriseManagement.UI.Administration.dll",
	"Microsoft.EnterpriseManagement.UI.Authoring.dll",
	"Microsoft.EnterpriseManagement.UI.Console.Common.dll",
	"Microsoft.EnterpriseManagement.UI.ConsoleFramework.dll",
	"Microsoft.EnterpriseManagement.UI.Controls.dll",
	"Microsoft.EnterpriseManagement.UI.Extensibility.dll",
	"Microsoft.EnterpriseManagement.UI.Foundation.dll",
	"Microsoft.EnterpriseManagement.UI.ViewFramework.dll",
	"Microsoft.EnterpriseManagement.UI.WpfViews.dll",
	"Microsoft.Mom.Common.dll",
	"Microsoft.Mom.Isam.Interop.dll",
	"Microsoft.Mom.RecorderBar.dll",
	"Microsoft.Mom.UI.Common.dll",
	"Microsoft.Mom.UI.Components.dll",
	"Microsoft.Mom.UI.Wrappers.dll",
	"Microsoft.Office.Interop.Word.dll",
	"Microsoft.ReportViewer.WinForms.dll",
	"Monad\OperationsManager\OM10.Commands\Microsoft.SystemCenter.OperationsManagerV10.Commands.dll",
	"Microsoft.VisualStudio.Modeling.Sdk.Diagrams.GraphObject.dll",
	"Microsoft.VisualStudio.Modeling.Sdk.Diagrams.dll",
	"Microsoft.VisualStudio.Modeling.Sdk.dll"
	"AntiXssLibrary.dll"

)

function CreateSolutionRootDrive {
	$dir = Get-Location
	while ( !(test-path -path "$dir/.SolutionRoot") ) {
		$dir = resolve-path "$dir/.."
		if ($dir.Path -eq $dir.Drive.Root) {
				write-error "Cannot locate SolutionRoot (are you really executing from within TFS structure?)"
				exit -1
			}
	}
    
	new-psdrive -name solutionRoot -PSProvider filesystem -Root $dir -Scope script
    write "Created drive $dir"
}


function CheckOutAssemblies ([string]$flavor, $assemblyListArg) {
	foreach ($assembly in $assemblyListArg) {
        $filename = split-path "$assembly" -leaf
        $f = ("$scxExtAssemblyPath\$flavor\$filename").ProviderPath
		write "Checking out $f"
		tf checkout (resolve-path "$scxExtAssemblyPath\$flavor\$filename").ProviderPath
	}
	tf checkout (resolve-path "$scxExtAssemblyPath\$flavor\README.txt").ProviderPath
}

function ValidateSrcDir([string]$buildNr, [string]$srcFlavor) {
	$testDir = $opsMgrBuildDropRoot + $buildNr + "\" + $srcFlavor + "\amd64\" 
	if ( !(Test-Path -path $testDir)) {
	   Write-Error "Missing source directory: $testDir"
	   exit -1
	}
}

function CopyAssemblies ([string]$buildNr, [string]$srcFlavor, [string]$destFlavor, $assemblyListArg) {
	$srcDir = $opsMgrBuildDropRoot + $buildNr + "\" + $srcFlavor + "\amd64\" 
	foreach ($assembly in $assemblyListArg) {
       $filename = split-path "$assembly" -leaf
	   copy-item -path ($srcDir + "$assembly") -destination $scxExtAssemblyPath/$destFlavor/$filename
	}
	UpdatePedigreeReadme ($scxExtAssemblyPath + "/$destFlavor/README.txt") $srcDir
}

function UpdatePedigreeReadme([string]$filename, [string]$srcDir) {
    $scriptname = $MyInvocation.ScriptName
    $date = get-date
	Set-Content -path $filename -value ""
    Add-Content -path $filename -value "This file updated using $scriptname on $date"
    Add-Content -path $filename -value ""
    Add-Content -path $filename -value "Source directory $srcDir"
}



#--------------------------------------------------------------------------------
# Man program start

CreateSolutionRootDrive

if ( $srcBuildNr -eq "") {
	Write-Error "No source build number provided"
	exit -1
}

ValidateSrcDir $srcBuildNr "debug"
ValidateSrcDir $srcBuildNr "retail"
ValidateSrcDir $srcBuildNr "signed"

CheckOutAssemblies "debug" $assemblyList
CopyAssemblies $srcBuildNr "debug" "debug" $assemblyList

CheckOutAssemblies "retail" $assemblyList
CopyAssemblies $srcBuildNr "retail" "retail" $assemblyList

CheckOutAssemblies "signed" $assemblyList
CopyAssemblies $srcBuildNr "signed" "signed" $assemblyList
write "All done."
exit 0


