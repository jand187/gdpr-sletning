Usage:


GdprClientConsole.exe delete -f <scanningsfil>
Sletter filer som passer på følgende kriterier: 
- filen er defineret af scanningsresultatet og har ifølge dette en dato, der tilsiger sletning.
- filens aktuelle dato (sidst modificeret dato), tilsiger sletning.

------------------------

GdprClientConsole.exe delete-dry-run -f <scanningsfil>
Rapporterer hvilke filer, en sletning ("delete", se ovenfor) ville slette med kommandoen "delete"

------------------------

GdprClientConsole.exe delete-sharepoint -f <scanningsfil>
Sletter filer som passer på følgende kriterier: 
- filen er defineret af scanningsresultatet og har ifølge dette en dato, der tilsiger sletning.
- filens aktuelle dato (sidst modificeret dato), tilsiger sletning.


------------------------


*** der er ikke nogen dry run option for SharePoint.