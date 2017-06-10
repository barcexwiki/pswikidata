---
external help file: PSWikidata.dll-Help.xml
online version: 
schema: 2.0.0
---

# Get-WDItem

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

### qid
```
Get-WDItem [-QId] <String[]>
```

### sitelink
```
Get-WDItem -SitelinkSite <String> -SitelinkTitle <String>
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

### -QId
Q identifier for the item.

```yaml
Type: String[]
Parameter Sets: qid
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -SitelinkSite
Site of the sitelink

```yaml
Type: String
Parameter Sets: sitelink
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SitelinkTitle
Title on the specified SitelinkSite

```yaml
Type: String
Parameter Sets: sitelink
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### System.String[]


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

