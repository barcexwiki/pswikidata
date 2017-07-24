---
external help file: PSWikidata.dll-Help.xml
online version: 
schema: 2.0.0
---

# Add-WDStatement

## SYNOPSIS
Adds an statement to an item.

## SYNTAX

### item
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueItem <PSWDItem> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### monolingual
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueText <String> -ValueLanguage <String> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### string
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueString <String> [-WhatIf] [-Confirm] [<CommonParameters>]
```

### quantity
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> -ValueAmount <Decimal> [-ValuePlusMinus <Decimal>]
 [-ValueUnit <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### time
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueTime <String> [-ValueTimeZoneOffset <Int32>] [-ValueBefore <Int32>] [-ValueAfter <Int32>]
 [-ValueCalendarModel <CalendarModel>] [-ValueTimePrecision <TimeValuePrecision>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### globecoordinate
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueLatitude <Double> -ValueLongitude <Decimal> [-ValueCoordinatePrecision <Decimal>] [-ValueGlobe <Globe>]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

### novalue
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave] [-NoValue]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

### somevalue
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave] [-SomeValue]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Adds an statement to an item.

## EXAMPLES

### Example 1
```
PS C:\> Add-WDStatement -Item Q33258952 -Property p18 -ValueString "IAAF World Challenge - Meeting Madrid 2017 - 170714 19
3212.jpg"

QId: q33258952 (Loaded)
Labels: 2   Descriptions: 2   Aliases: 0   Sitelinks: 0   Claims: 5

Language Label         Description   Aliases
-------- -----         -----------   -------
en       Santiago Cova Cuban athlete
es       Santiago Cova atleta cubano
```

### Example 2
```
PS C:\> Add-WDStatement -Item Q33258952 -Property p31 -ValueItem q5

QId: q33258952 (Loaded)
Labels: 2   Descriptions: 2   Aliases: 0   Sitelinks: 0   Claims: 1

Language Label         Description   Aliases
-------- -----         -----------   -------
en       Santiago Cova Cuban athlete
es       Santiago Cova atleta cubano
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

### -DoNotSave
Add the statement but do not save the changes to Wikidata.

```yaml
Type: SwitchParameter
Parameter Sets: item, monolingual, string, time, globecoordinate, novalue, somevalue
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Item
Item to be modified.

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

### -Multiple
Add the statement even if there is already an statement for this property.

```yaml
Type: SwitchParameter
Parameter Sets: item, monolingual, string, time, globecoordinate, novalue, somevalue
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoValue
The property has no value.

```yaml
Type: SwitchParameter
Parameter Sets: novalue
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputStatement
Outputs the new statement instead of the modified item.

```yaml
Type: SwitchParameter
Parameter Sets: item, monolingual, string, time, globecoordinate, novalue, somevalue
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Property
Property for the statement.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SomeValue
The property has some unknown value.

```yaml
Type: SwitchParameter
Parameter Sets: somevalue
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueAfter
How many units after the given time could it be.
The unit is given by the precision.

```yaml
Type: Int32
Parameter Sets: time
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueAmount
Quantity to be assigned to the property.

```yaml
Type: Decimal
Parameter Sets: quantity
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueBefore
How many units before the given time could it be.
The unit is given by the precision.

```yaml
Type: Int32
Parameter Sets: time
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueCalendarModel
Calendar Model.

```yaml
Type: CalendarModel
Parameter Sets: time
Aliases: 
Accepted values: Unknown, GregorianCalendar, JulianCalendar

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueCoordinatePrecision
Precision of the coordinate

```yaml
Type: Decimal
Parameter Sets: globecoordinate
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueGlobe
Globe of the coordinate

```yaml
Type: Globe
Parameter Sets: globecoordinate
Aliases: 
Accepted values: Unknown, Earth

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueItem
Item that will be the value of the property.

```yaml
Type: PSWDItem
Parameter Sets: item
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueLanguage
Language code for the text that will be the value of the property.

```yaml
Type: String
Parameter Sets: monolingual
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueLatitude
Latitude of the coordinate

```yaml
Type: Double
Parameter Sets: globecoordinate
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueLongitude
Longitude of the coordinate

```yaml
Type: Decimal
Parameter Sets: globecoordinate
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValuePlusMinus
Range around the value.

```yaml
Type: Decimal
Parameter Sets: quantity
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueString
Text that will be the value of the property.

```yaml
Type: String
Parameter Sets: string
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueText
Text that will be the value of the property.

```yaml
Type: String
Parameter Sets: monolingual
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueTime
Time in the format \[+|-\]yyyyyyyyyyyy-mm-ddThh:mm:ssZ.

```yaml
Type: String
Parameter Sets: time
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueTimePrecision
Time precision.

```yaml
Type: TimeValuePrecision
Parameter Sets: time
Aliases: 
Accepted values: GigaYear, HundredMegaYears, TenMegaYears, MegaYear, HundredKiloYears, TenKiloYears, Millennium, Century, Decade, Year, Month, Day, Hour, Minute, Second

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueTimeZoneOffset
Offset in minutes from UTC.

```yaml
Type: Int32
Parameter Sets: time
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ValueUnit
Unit of the quantity value.

```yaml
Type: String
Parameter Sets: quantity
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

### PSWikidata.PSWDItem

## OUTPUTS

### PSWikidata.PSWDItem

## NOTES

## RELATED LINKS

