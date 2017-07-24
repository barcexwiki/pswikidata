---
external help file: PSWikidata.dll-Help.xml
online version: 
schema: 2.0.0
---

# Get-WDItem

## SYNOPSIS
Retrieves an item or a set of items from Wikidata.

## SYNTAX

### qid
```
Get-WDItem [-QId] <String[]> [<CommonParameters>]
```

### sitelink
```
Get-WDItem -SitelinkSite <String> -SitelinkTitle <String> [<CommonParameters>]
```

## DESCRIPTION
Retrieves an item or a set of items from Wikidata. Given a list of 

## EXAMPLES

### Example 1
```
PS C:\>  Get-WDItem q1

QId: q1 (Loaded)
Labels: 167   Descriptions: 70   Aliases: 96   Sitelinks: 172   Claims: 48

Language Label    Description
-------- -----    -----------
en       universe totality of planets, stars, galaxies, intergalactic space,
                  or all matter or all energy
es       universo totalidad del espacio-tiempo, la materia y la energía
                  existentes

Site   Title
----   -----
dewiki Universum
enwiki Universe
eswiki Universo
frwiki Univers
```

Gets a PSWDItem object for the identifier q1.

### Example 2   
```
PS C:\>  Get-WDItem -SitelinkSite enwiki -SitelinkTitle Universe

QId: q1 (Loaded)
Labels: 167   Descriptions: 70   Aliases: 96   Sitelinks: 172   Claims: 48

Language Label    Description
-------- -----    -----------
en       universe totality of planets, stars, galaxies, intergalactic space,
                  or all matter or all energy
es       universo totalidad del espacio-tiempo, la materia y la energía
                  existentes

Site   Title
----   -----
dewiki Universum
enwiki Universe
eswiki Universo
frwiki Univers
```

Gets the PSWDItem object for the item with the sitelink "Universe" in enwiki.

### Example 3   
```
PS C:\> "q414","q8" |  Get-WDItem

QId: q414 (Loaded)
Labels: 261   Descriptions: 55   Aliases: 48   Sitelinks: 274   Claims: 269

Language Label     Description                       Aliases
-------- -----     -----------                       -------
en       Argentina federal republic in South America Argentine Republic
                                                     AR
                                                     ar
                                                     🇦🇷
es       Argentina país de América del Sur           República Argentina
                                                     Provincias Unidas del Río
                                                     de la Plata
                                                     Confederación Argentina

Site   Title
----   -----
dewiki Argentinien
enwiki Argentina
eswiki Argentina
frwiki Argentine



QId: q8 (Loaded)
Labels: 112   Descriptions: 47   Aliases: 46   Sitelinks: 124   Claims: 26

Language Label     Description
-------- -----     -----------
en       happiness mental or emotional state of well-being characterized by
                   pleasant emotions
es       felicidad estado de ánimo

Site   Title
----   -----
dewiki Glück
enwiki Happiness
eswiki Felicidad
frwiki Bonheur
```

Takes a list of identifiers on the pipeline and retrieves their corresponding PSWDItem objects.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String[]

## OUTPUTS

### PSWDItem[]

## NOTES

## RELATED LINKS

