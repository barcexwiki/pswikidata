---
external help file: PSWikidata.dll-Help.xml
online version: 
schema: 2.0.0
---

# Get-WDAlias

## SYNOPSIS
Retrieves a list of alias for an item in a specific language.

## SYNTAX

```
Get-WDAlias [-Item] <PSWDItem> [-Language] <String> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of alias for an item in a specific language.

## EXAMPLES

### Example 1
```
PS C:\> Get-WDAlias q1 en
space
Cosmos
outer space
universe (class)
universe, a
```

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

### -Item
Item to get the aliases from.

```yaml
Type: PSWDItem
Parameter Sets: (All)
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Language
Language of the aliases to be retrieved.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: 2
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

### PSWikidata.PSWDItem

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

