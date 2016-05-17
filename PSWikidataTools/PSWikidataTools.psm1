<#
.Synopsis
   Descripción corta
.DESCRIPTION
   Descripción larga
.EXAMPLE
   Ejemplo de cómo usar este cmdlet
.EXAMPLE
   Otro ejemplo de cómo usar este cmdlet
.INPUTS
   Entradas a este cmdlet (si hay)
.OUTPUTS
   Salidas de este cmdlet (si hay)
.NOTES
   Notas generales
.COMPONENT
   El componente al que pertenece este cmdlet
.ROLE
   El rol al que pertenece este cmdlet
.FUNCTIONALITY
   La funcionalidad que mejor describe a este cmdlet
#>
function Copy-WDLabel
{
    [CmdletBinding(DefaultParameterSetName='Parameter Set 1', 
                  SupportsShouldProcess=$true, 
                  PositionalBinding=$false,
                  ConfirmImpact='Medium')]
    [Alias("cplabel")]
    [OutputType([PSWikidata.PSWDItem])]
    Param
    (
        # Item to be modified
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true,
                   ParameterSetName='Parameter Set 1')]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [Alias("i")] 
        [PSWikidata.PSWDItem[]]
        $Item,

        # Source language
        [Parameter(Mandatory=$true, 
                   ParameterSetName='Parameter Set 1')]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [Alias("sl")] 
        [string]
        $SourceLanguage,

        # Destination languages
        [Parameter(Mandatory=$true, 
                   ParameterSetName='Parameter Set 1')]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [Alias("dl")] 
        [string[]]
        $DestinationLanguages

    )

    Begin
    {
    }
    Process
    {
        foreach ($i in $Item)
        { 
            if ($pscmdlet.ShouldProcess($i.QId, "Copying labels from $SourceLanguage to $($DestinationLanguages -join " ")"))
            {               

                $label = ($i.Labels|? Language -eq $SourceLanguage).Label

                if ($label)
                {
                    foreach ($dl in $DestinationLanguages)
                    {
                        Write-Verbose "Copying label '$label' of $($i.QId) from [$SourceLanguage] to [$dl]"
                        Set-WDItem -Item $i -Label $label -Language $dl | Out-Null
                    }
                }
                else
                {
                    Write-Warning "$($i.QId) has no label for language $SourceLanguage"
                } 

                # sends the item to the output stream
                $i
            }

        }
    }
    End
    {
    }
}

function Test-WDSex
{
    param
    (

        # Item to be tested
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true)]
        [ValidateNotNullOrEmpty()]
        [Alias("i")] 
        [PSWikidata.PSWDItem[]]
        $Item,

        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true)]        
        [ValidateSet("Male", "Female")]
        $Sex
    )

    switch ($Sex)
    {
        'Female' { 
            $sexStatements = $Item.Claims | ? {$_.Property -eq "p21" -and $_.Value.Id -eq "Q6581072"}
         }
        'Male' {
            $sexStatements = $Item.Claims | ? {$_.Property -eq "p21" -and $_.Value.Id -eq "q6581097"}
        }
    }
    
    return $sexStatements.Count -gt 0
}

<#
.Synopsis
   Descripción corta
.DESCRIPTION
   Descripción larga
.EXAMPLE
   Ejemplo de cómo usar este cmdlet
.EXAMPLE
   Otro ejemplo de cómo usar este cmdlet
.INPUTS
   Entradas a este cmdlet (si hay)
.OUTPUTS
   Salidas de este cmdlet (si hay)
.NOTES
   Notas generales
.COMPONENT
   El componente al que pertenece este cmdlet
.ROLE
   El rol al que pertenece este cmdlet
.FUNCTIONALITY
   La funcionalidad que mejor describe a este cmdlet
#>
function Set-WDRelatives
{
    [CmdletBinding(DefaultParameterSetName='Parameter Set 1', 
                  SupportsShouldProcess=$true, 
                  PositionalBinding=$false,
                  ConfirmImpact='Medium')]
    [Alias()]
    [OutputType([String])]
    Param
    (
         # Item to be modified
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true,
                   ParameterSetName='Parameter Set 1')]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [Alias("i")] 
        [PSWikidata.PSWDItem[]]
        $Item,

        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [string]$MotherQId,
        
        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [string]$FatherQId,

        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [string]$SpouseQId,


        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [string[]]$ChildrenQId,

        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [string[]]$SibligsQId

    )

    Begin
    {
        
        
    }
    Process
    {
        foreach ($i in $Item)
        {
            # checks that there are no duplicate Qids
            $allQids = @();
            if ($ChildrenQId) 
                { $allQids += $ChildrenQid;}
            if ($SibligsQId) 
                { $allQids += $SibligsQId;}
            if ($SpouseQId) 
                {$allQids += $SpouseQId;}
            if ($FatherQId) 
                {$allQids += $FatherQId;}
            $allQids += $i.QId;

            if ($allQids | group | ? {$_.count -gt 1})
            {
                throw "Parameters are inconsistent."
            }

            if ($pscmdlet.ShouldProcess("Target", "Operation"))
            {
        
              if ($pscmdlet.MyInvocation.BoundParameters["MotherQId"])
              {
                  $mother = Get-WDItem $MotherQId -ErrorAction Stop 

                  Add-WDStatement -Item $i -Property p25 -ValueItem $mother.QId -ErrorAction Stop | Out-Null
                  Add-WDStatement -Item $mother -Property p40 -ValueItem $i.QId -ErrorAction Stop | Out-Null
              }

              if ($pscmdlet.MyInvocation.BoundParameters["FatherQId"])
              {
                  $father = Get-WDItem $FatherQId -ErrorAction Stop 

                  Add-WDStatement -Item $i -Property p22 -ValueItem $father.QId -ErrorAction Stop | Out-Null
                  Add-WDStatement -Item $father -Property p40 -ValueItem $i.QId -ErrorAction Stop | Out-Null
              }

              if ($pscmdlet.MyInvocation.BoundParameters["SpouseQId"])
              {
                  $spouse = Get-WDItem $SpouseQId -ErrorAction Stop 

                  Add-WDStatement -Item $i -Property p26 -ValueItem $spouse.QId -ErrorAction Stop | Out-Null
                  Add-WDStatement -Item $spouse -Property p26 -ValueItem $i.QId -ErrorAction Stop | Out-Null
              }


              foreach ($sQId in $SibligsQId)
              {
                  $sibling = Get-WDItem $sQId -ErrorAction Stop
                  
                  setSibling $i $sibling
                  setSibling $sibling $i
              }


              if ($pscmdlet.MyInvocation.BoundParameters["ChildrenQId"])
              {
                  $ChildrenQId = $ChildrenQId | Sort-Object | Get-Unique
                  $children = $ChildrenQId | % { Get-WDItem $_ -ErrorAction Stop }

                  foreach ($child in $children)
                  {

                      Add-WDStatement -Item $i -Property p40 -ValueItem $child.QId -ErrorAction Stop | Out-Null

                      if (Test-WDSex -Item $i -Sex Male) {
                          Add-WDStatement -Item $child -Property p22 -ValueItem $i.QId -ErrorAction Stop | Out-Null
                      } 
                      elseif (Test-WDSex -Item $i -Sex Female)  {
                          Add-WDStatement -Item $child -Property P25 -ValueItem $i.QId -ErrorAction Stop | Out-Null
                      } else {
                          throw "The sex of the parent is unknown"
                      }
                  
                      # Children are sibling between them
                      $siblings = $children | Where-Object {$_.QId -ne $child.QId}
                      foreach ($s in $siblings) 
                      {
                        setSibling $child $s
                      }
                  }
              }


            }

            $Item
        }
    }
    End
    {
    }
}

function setSibling ([PSWikidata.PSWDItem]$a, [PSWikidata.PSWDItem] $b)
{
    if (Test-WDSex -Item $b -Sex Male) {
        Add-WDStatement -Item $a -Property "p7" -ValueItem $b.QId | Out-Null
    } 
    elseif (Test-WDSex -Item $b -Sex Female)  {
        Add-WDStatement -Item $a -Property "p9" -ValueItem $b.QId | Out-Null
    } else {
        throw "The sex of the sibling is unknown"
    }
}
