---
external help file: PSWikidata.dll-Help.xml
online version: 
schema: 2.0.0
---

# New-WDItem

## SYNOPSIS
Creates a new item.

## SYNTAX

```
New-WDItem [-DoNotSave] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Creates a new item. Te item is created empty right away on the server unless the -DoNotSave parameter is used, in which case the item is returned unsaved.

## EXAMPLES

### Example 1
```
PS C:\> New-WDItem

Confirm
Are you sure you want to perform this action?
Performing the operation "Create" on target "new item".
[Y] Yes  [A] Yes to All  [N] No  [L] No to All  [S] Suspend  [?] Help
(default is "Y"):Y

QId: q33258952 (Loaded)
Labels: 0   Descriptions: 0   Aliases: 0   Sitelinks: 0   Claims: 0
```

Creates an empty item. The item identifier is returned. 
It is show in the 'loaded' state because it is synchronized with the contents on the server.

### Example 2
```
PS C:\> $newItem = New-WDItem -DoNotSave -Confirm:$false
PS C:\> $newItem

QId:  (New)
Labels: 0   Descriptions: 0   Aliases: 0   Sitelinks: 0   Claims: 0
```

Creates an empty item and does not save it on the server. Stores the object in a variable to be used lated by other commands. 
The item identifier is not returned because it will be assigned by the server once the item is actually created.
It is show in the 'new' state because it is not created on the server yet.
In order to create it the Save-WDItem cmdlet could be used.

## PARAMETERS

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DoNotSave
Create the item but do not save the changes to Wikidata.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### PSWikidata.PSWDItem

## NOTES

## RELATED LINKS

