---
external help file: PSWikidata.dll-Help.xml
online version: 
schema: 2.0.0
---

# Add-WDReference

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

```
Add-WDReference -Statement <PSWDStatement> -Snaks <PSWDSnak[]> [-DoNotSave] [-WhatIf] [-Confirm]
```

## DESCRIPTION
{{Fill in the Description}}

## EXAMPLES

### Example 1
```
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

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
Add the reference but do not save the changes to Wikidata.

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

### -Snaks
Snaks that make the reference.

```yaml
Type: PSWDSnak[]
Parameter Sets: (All)
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Statement
Statement to be modified.

```yaml
Type: PSWDStatement
Parameter Sets: (All)
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
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

## INPUTS

### PSWikidata.PSWDStatement


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

