param([string] $language, [string] $operation);

$layoutList = Get-WinUserLanguageList;

function Addition()
{
   $layoutList.Add($language);
}

if($operation -eq "add")
{
   Addition;
}
elseif($operation -eq "remove")
{
   foreach($lang in $layoutList)
   {
      if($lang.LanguageTag -eq $language -or $lang.LanguageTag -eq $language.Split('-')[0] -or $lang.LanguageTag -eq $language.Split('-')[1])
      {
         $layoutList.Remove($lang);
         break;
      }
   }
}
else
{
   Addition;
}

Set-WinUserLanguageList -LanguageList $layoutList;