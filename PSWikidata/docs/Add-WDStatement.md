---
external help file: PSWikidata.dll-Help.xml
online version: 
schema: 2.0.0
---

# Add-WDStatement

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

### item
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueItem <PSWDItem> [-WhatIf] [-Confirm]
```

### monolingual
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueText <String> -ValueLanguage <String> [-WhatIf] [-Confirm]
```

### string
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueString <String> [-WhatIf] [-Confirm]
```

### quantity
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> -ValueAmount <Decimal> [-ValuePlusMinus <Decimal>]
 [-ValueUnit <String>] [-WhatIf] [-Confirm]
```

### time
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueTime <String> [-ValueTimeZoneOffset <Int32>] [-ValueBefore <Int32>] [-ValueAfter <Int32>]
 [-ValueCalendarModel <CalendarModel>] [-ValueTimePrecision <TimeValuePrecision>] [-WhatIf] [-Confirm]
```

### globecoordinate
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave]
 -ValueLatitude <Double> -ValueLongitude <Decimal> [-ValueCoordinatePrecision <Decimal>] [-ValueGlobe <Globe>]
 [-WhatIf] [-Confirm]
```

### novalue
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave] [-NoValue]
 [-WhatIf] [-Confirm]
```

### somevalue
```
Add-WDStatement [-Item] <PSWDItem> -Property <String> [-Multiple] [-OutputStatement] [-DoNotSave] [-SomeValue]
 [-WhatIf] [-Confirm]
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

## INPUTS

### PSWikidata.PSWDItem


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

