function Test-WDInstanceOf
{
    param
    (

        # Item to be tested
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true)]
        [ValidateNotNullOrEmpty()]
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDEntity]
        $Entity,

        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$false)]
        [ValidateNotNullOrEmpty()]
        [Alias("v")] 
        [string]
        $QId
    )

    $statements = $Entity.Claims | Where-Object {$_.Property -eq "p31" -and $_.Value.Id -eq $QId}

    return $statements.Count -gt 0
}

function Test-WDHuman
{
    param
    (

        # Item to be tested
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true)]
        [ValidateNotNullOrEmpty()]
        [Alias("i")] 
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem]
        $Item
    )


    return Test-WDInstanceOf -Entity $Item -QId "Q5"
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
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem]
        $Item,

        # Sex that the item will be tested against
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$false)]        
        [ValidateSet("Male", "Female")]
        $Sex
    )

    switch ($Sex)
    {
        'Female' { 
            $sexStatements = $Item.Claims | Where-Object {$_.Property -eq "p21" -and $_.Value.Id -eq "Q6581072"}
         }
        'Male' {
            $sexStatements = $Item.Claims | Where-Object {$_.Property -eq "p21" -and $_.Value.Id -eq "q6581097"}
        }
    }
    
    return $sexStatements.Count -gt 0
}

function Set-WDRelative
{
    [CmdletBinding(SupportsShouldProcess=$true, 
                  PositionalBinding=$false,
                  ConfirmImpact='Medium')]
    [OutputType([PSWikidata.PSWDItem])]
    Param
    (
         # Item to be modified
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true,
                   Position = 0)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [Alias("i")] 
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem[]]
        $Item,

        # Item that represents the mother of the subject item
        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem]
        $Mother,

        # Item that represents the father of the subject item        
        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem]
        $Father,

        # Item that represents the spouse of the subject item        
        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem]
        $Spouse,

        # Items that represent the children of the subject item 
        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem[]]
        $Children,

        # Items that represent the siblings of the subject item 
        [Parameter(Mandatory= $false)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem[]]
        $Siblings
    )

    Process
    {
        foreach ($i in $Item)
        {
            # checks that there are no duplicate Qids
            $allQids = @();
            if ($pscmdlet.MyInvocation.BoundParameters["Children"]) 
                { $allQids += $Children.Id;}
            if ($pscmdlet.MyInvocation.BoundParameters["Siblings"]) 
                { $allQids += $Siblings.Id;}
            if ($pscmdlet.MyInvocation.BoundParameters["Spouse"]) 
                {$allQids += $Spouse.Id;}
            if ($pscmdlet.MyInvocation.BoundParameters["Father"]) 
                {$allQids += $Father.Id;}
            $allQids += $i.Id;

            if ($allQids | Group-Object | Where-Object {$_.count -gt 1})
            {
                throw "Parameters are inconsistent."
            }

            if ($pscmdlet.ShouldProcess($i.Id, "Set relatives"))
            {
        
              if ($pscmdlet.MyInvocation.BoundParameters["Mother"])
              {                  
                  Add-WDStatement -Entity $i -Property p25 -ValueItem $Mother -ErrorAction Stop | Out-Null
                  Add-WDStatement -Entity $Mother -Property p40 -ValueItem $i -ErrorAction Stop | Out-Null
              }

              if ($pscmdlet.MyInvocation.BoundParameters["Father"])
              {
                  Add-WDStatement -Entity $i -Property p22 -ValueItem $Father -ErrorAction Stop | Out-Null
                  Add-WDStatement -Entity $Father -Property p40 -ValueItem $i -ErrorAction Stop | Out-Null
              }

              if ($pscmdlet.MyInvocation.BoundParameters["Spouse"])
              {
                  Add-WDStatement -Entity $i -Property p26 -ValueItem $Spouse -ErrorAction Stop | Out-Null
                  Add-WDStatement -Entity $Spouse -Property p26 -ValueItem $i -ErrorAction Stop | Out-Null
              }


              foreach ($s in $Siblings)
              {                  
                  setSibling $i $s
                  setSibling $s $i
              }


              if ($pscmdlet.MyInvocation.BoundParameters["Children"])
              {
                  $ChildrenQId = $Children.Id | Sort-Object | Get-Unique
                  $children = $ChildrenQId | Get-WDEntity -ErrorAction Stop 

                  foreach ($child in $Children)
                  {

                      Add-WDStatement -Entity $i -Property p40 -ValueItem $child -ErrorAction Stop | Out-Null

                      if (Test-WDSex -Item $i -Sex Male) {
                          Add-WDStatement -Entity $child -Property p22 -ValueItem $i -ErrorAction Stop | Out-Null
                      } 
                      elseif (Test-WDSex -Item $i -Sex Female)  {
                          Add-WDStatement -Entity $child -Property P25 -ValueItem $i -ErrorAction Stop | Out-Null
                      } else {
                          throw "The sex of the parent is unknown"
                      }
                  
                      # Children are sibling between them
                      $siblings = $children | Where-Object {$_.Id -ne $child.Id}
                      foreach ($s in $siblings) 
                      {
                        setSibling $child $s
                      }
                   }
               }

               Write-Output $Item
            }
        }
    }
}

function setSibling ([PSWikidata.PSWDItem]$a, [PSWikidata.PSWDItem] $b)
{
    Add-WDStatement -Entity $a -Property "p3373" -ValueItem $b | Out-Null
}


function Set-WDHuman
{
    [CmdletBinding(SupportsShouldProcess=$true, 
                  PositionalBinding=$false,
                  ConfirmImpact='Medium')]
    [OutputType([PSWikidata.PSWDItem])]
    Param
    (
         # Item to be modified
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true,
		   Position=0)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [Alias("i")] 
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem[]]
        $Item,

        [Parameter(Mandatory= $false)]
        [ValidateNotNullOrEmpty()]
        [ValidateSet("Male", "Female")]
        $Sex,

        [Parameter(Mandatory=$false)]
        [switch]$SetInstanceOf = $false
    )

    DynamicParam {

        $valueItemParameters = @{
            "PlaceOfBirth" = "P19"
            "CauseOfDeath" = "P509"
            "PlaceOfDeath" = "P20"
            "MannerOfDeath" = "P1196"
            "Occupation" = "P106"
            "CountryOfCitizenship" = "P27"
            "PlaceOfBurial" = "P119"
            "KilledBy" = "P157"
            "NativeLanguage" = "P103"
            "AlmaMater" = "P69"
            "FieldOfWork" = "P101"
            "NotableWork" = "P800"
            "Employer" = "P108"
            "PositionHeld" = "P39"
            "Religion" = "P140"
            "HonorifixPrefix" = "P511"
            "FamilyName" = "P734"
            "GivenName" = "P735"
            "EyeColor" = "P1340"
            "LanguagesSpoken" = "P1412"
            "Affiliation" = "P1416"
        }

        $stringParameters = @{
            "Pseudonym" = "P742"
        }


        $paramDictionary = New-Object System.Management.Automation.RuntimeDefinedParameterDictionary

        foreach ($paramName in $valueItemParameters.Keys)
        {
             $attribute = New-Object System.Management.Automation.ParameterAttribute
             $attribute.Mandatory = $false
             $attributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute]
             $attributeCollection.Add($attribute)

             $attribute = New-Object System.Management.Automation.ValidateNotNullOrEmptyAttribute
             $attributeCollection.Add($attribute)

             $attribute = New-Object PSWikidata.PSWDEntityArgumentTransformationAttribute
             $attributeCollection.Add($attribute)
         
             $param = New-Object System.Management.Automation.RuntimeDefinedParameter($paramName, [PSWikidata.PSWDItem], $attributeCollection)
             $paramDictionary.Add($paramName, $param)
        }

        foreach ($paramName in $stringParameters.Keys)
        {
             $attribute = New-Object System.Management.Automation.ParameterAttribute
             $attribute.Mandatory = $false
             $attributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute]
             $attributeCollection.Add($attribute)

             $attribute = New-Object System.Management.Automation.ValidateNotNullOrEmptyAttribute
             $attributeCollection.Add($attribute)
         
             $param = New-Object System.Management.Automation.RuntimeDefinedParameter($paramName, [string], $attributeCollection)
             $paramDictionary.Add($paramName, $param)
        }

        return $paramDictionary
    }

    Process
    {
        $pInstanceOf = $null;
        $qHuman = $null;
                
        foreach ($i in $Item)
        {
            if (!$pInstanceOf) { $pInstanceOf = Get-WDEntity -Entity p31 }
            if (!$qHuman) { $qHuman = Get-WDEntity -Entity q5 }
            if ($pscmdlet.ShouldProcess($i.Id, "Set-WDHuman"))
            {
                if (!(Test-WDHuman $i))
                {
                    if ($SetInstanceOf)
                    {
                        Add-WDStatement -Entity $i -Property $pInstanceOf -ValueItem $qHuman -ErrorAction Stop | Out-Null
                    } 
                    else
                    {
                        throw "Not instance of human"
                    }
                }

                foreach ($parameterName in $valueItemParameters.Keys)
                {
                    $parameterValue = $pscmdlet.MyInvocation.BoundParameters[$parameterName]
                    if ($parameterValue)
                    {
                        Add-WDStatement -Entity $i -Property $valueItemParameters[$parameterName] -ValueItem $parameterValue -ErrorAction Stop | Out-Null
                    }                    
                }


                foreach ($parameterName in $stringParameters.Keys)
                {
                    $parameterValue = $pscmdlet.MyInvocation.BoundParameters[$parameterName]
                    if ($parameterValue)
                    {
                        Add-WDStatement -Entity $i -Property $stringParameters[$parameterName] -ValueString $parameterValue -ErrorAction Stop | Out-Null
                    }                    
                }

                if ($pscmdlet.MyInvocation.BoundParameters["Sex"])
                {
                    switch ($Sex)
                    {
                        'Female'{ $sexQId = "Q6581072" }
                        'Male' { $sexQId = "q6581097" }
                    }

                    $sexStatements = $i.Claims | Where-Object {$_.Property -eq "p21"}
                    if ($sexStatements.Count -le 1)
                    {
                        if ($sexStatements.Count -eq 1)
                        {
                            $sexStatements | Set-WDStatement -ValueItem $sexQId -ErrorAction Stop | Out-Null
                        } 
                        else
                        {
                            Add-WDStatement -Entity $i -Property p21 -ValueItem $sexQId -ErrorAction Stop | Out-Null
                        }
                    }
                    else 
                    {
                        throw "Multiple Sex statements. Cannot determine which one to set." 
                    }

                }

            }

            $i
        }
    }
}

function Get-WDCountry
{

 
    [CmdletBinding(DefaultParameterSetName='Parameter Set 1', 
                  PositionalBinding=$true,
                  ConfirmImpact='Low')]
    [OutputType([PSWikidata.PSWDItem])]
    Param
    (
         # Item to be modified
        [Parameter(Mandatory=$true, 
                   Position = 0,
                   ParameterSetName='Parameter Set 1')]
        [ValidateNotNullOrEmpty()]
        [Alias("iso")] 
        [string]
        $ISO3166   
    )


    Process
    {
        $iso = $ISO3166.ToUpper()

        $query= @"
SELECT ?country
WHERE
{
    ?country  wdt:P31 wd:Q6256 .
    ?country  wdt:P297 "$iso"
}
"@
       $escapedQuery = [System.Uri]::EscapeDataString($query)
       $uri = "https://query.wikidata.org/sparql?query=$escapedQuery&format=json"
       $restOutput = Invoke-RestMethod -Method Get -Uri $uri
                    
       if ($restOutput.results.bindings.country.value -match "^http://www.wikidata.org/entity/(Q.*)$")
       {
           $qId = $Matches[1]
           Get-WDEntity -Id $qId
       }
    }
}


function Get-WDLanguage
{

 
    [CmdletBinding(DefaultParameterSetName='Parameter Set 1', 
                  PositionalBinding=$true,
                  ConfirmImpact='Low')]
    [OutputType([PSWikidata.PSWDItem])]
    Param
    (
         # Item to be modified
        [Parameter(Mandatory=$true, 
                   Position = 0,
                   ParameterSetName='Parameter Set 1')]
        [ValidateNotNullOrEmpty()]
        [Alias("iso")] 
        [string]
        $ISO618   
    )


    Process
    {
        $iso = $ISO618.ToLower()

        $query= @"
SELECT ?country
WHERE
{
    ?country  wdt:P31 wd:Q34770 .
    ?country  wdt:P218 "$iso"
}
"@
       $escapedQuery = [System.Uri]::EscapeDataString($query)
       $uri = "https://query.wikidata.org/sparql?query=$escapedQuery&format=json"
       $restOutput = Invoke-RestMethod -Method Get -Uri $uri
                    
       if ($restOutput.results.bindings.country.value -match "^http://www.wikidata.org/entity/(Q.*)$")
       {
           $qId = $Matches[1]
           Get-WDEntity -Id $qId
       }
    }
}

function Get-WDItemFromIdentifier
{

 
    [CmdletBinding(DefaultParameterSetName='Parameter Set 1', 
                  PositionalBinding=$true,
                  ConfirmImpact='Low')]
    [OutputType([PSWikidata.PSWDItem])]
    Param
    (
         # Item to be modified
        [Parameter(Mandatory=$true, 
                   Position = 0,
                   ValueFromPipeline=$true,
                   ParameterSetName='Parameter Set 1')]
        [ValidateNotNullOrEmpty()]
        [Alias("id")] 
        [string[]]
        $Identifier,
        [ValidateNotNullOrEmpty()]
        [ValidatePattern('^p[0-9]+$')]
        [Alias("p")] 
        [string]
        $Property
    )


    Process
    {
        foreach($id in $Identifier)
        {
            $id = $id.ToLower()

            $query= @"
SELECT ?i
WHERE
{
    ?i  wdt:$($Property.ToUpper()) "$id"
}
"@
           $escapedQuery = [System.Uri]::EscapeDataString($query)
           $uri = "https://query.wikidata.org/sparql?query=$escapedQuery&format=json"
           $restOutput = Invoke-RestMethod -Method Get -Uri $uri
      
           if ($restOutput.results.bindings.i.Count -gt 1) 
           {
                Write-Warning "More than one wikidata item for $id $($restOutput.results.bindings.i.Value)"
           }              

           $entityUrl = $restOutput.results.bindings.i.Value | Select-Object -First 1
                     
           if (($entityUrl -match "^http://www.wikidata.org/entity/(Q.*)$") -eq $true)
           {
               $qId = $Matches[1]
               Get-WDEntity -Id $qId
           }
       }
    }
}


function Get-WDItemFromImdb
{

 
    [CmdletBinding(DefaultParameterSetName='Parameter Set 1', 
                  PositionalBinding=$true,
                  ConfirmImpact='Low')]
    [OutputType([PSWikidata.PSWDItem])]
    Param
    (
         # Item to be modified
        [Parameter(Mandatory=$true, 
                   Position = 0,
                   ValueFromPipeline=$true,
                   ParameterSetName='Parameter Set 1')]
        [ValidateNotNullOrEmpty()]
        [Alias("imdb")] 
        [string[]]
        $ImdbId   
    )


    Process
    {
        foreach($id in $ImdbId)
        {
            Get-WDItemFromIdentifier -Property P345 -Identifier $id
        }
    }
}


function _hasIMDBReference
{
    param ($Statement)

    foreach ($r in $Statement.references)
    {
        $s = $r.Snaks  | Where-Object {($_.Property -eq "p248" -and $_.Value.Id -eq "q37312") -or ($_.Property -eq "p854" -and $_.Value.Value -match 'http://(.*\.)?imdb\.com/')}

        if ($s) {
            return $true
        }
    }

    return $false

}

function _hasCinenacionalReference
{
    param ($Statement)

    foreach ($r in $Statement.references)
    {
        $s = $r.Snaks  | Where-Object {($_.Property -eq "p248" -and $_.Value.Id -eq "q3610461") -or ($_.Property -eq "p854" -and $_.Value.Value -match 'http://(.*\.)?cinenacional\.com/')}

        if ($s) {
            return $true
        }
    }

    return $false

}


function _hasWikiReference
{
    param ($Statement,$WikiQId)

    foreach ($r in $Statement.references)
    {
        $s = $r.Snaks  | Where-Object {($_.Property -eq "p248" -and $_.Value.Id -eq $WikiQId)}

        if ($s) {
            return $true
        }
    }

    return $false

}


function Add-WDCastMember
{
    [CmdletBinding()]
    param 
    (
        #Movie (item)
        [Parameter(Mandatory= $true,ValueFromPipeline=$true)]
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem[]]$Item, 

        #Actors and actresses (items)
        [Parameter(Mandatory= $true)]
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDItem[]]$CastMember,

        #Source
        [ValidateSet("IMDb", "ESwiki", "ENwiki", "cinenacional")]
        [string[]]$Source,

        #Access date
        [ValidateNotNull()]
        [DateTime]$AccessDate
    );
    begin
    {
        switch($Source)
        {
            {"ESwiki" -in $_} {$esWikiItem = Get-WDEntity q8449}
            {"ENwiki" -in $_} {$enWikiItem = Get-WDEntity q328}
            {"IMDb" -in $_} {$imdbItem = Get-WDEntity q37312}
            {"cinenacional" -in $_} {$cinenacionalItem = Get-WDEntity q3610461}
        }    
    }
    process
    {    
        foreach ($i in $Item) 
        {
            if (($i.Claims | Where-Object {$_.Property -eq "p31" -and $_.Value.Id -in ("q11424","q506240")}).Count -lt 1)
            {
                Write-Error "$($i.Id) is not a movie";
                continue;
            }

            foreach ($member in $CastMember)
            {
                if (Test-WDHuman -Item $member)
                {
                    $statement = $null

                    $existingClaims = $i.Claims | Where-Object {$_.Property -eq "p161" -and $_.Value.Id -eq $member.Id} 

                    switch ($existingClaims.Count)
                    {
                        0 { $statement = $i | Add-WDStatement -Property p161 -ValueItem $member  -DoNotSave   -OutputStatement }
                        1 { $statement = $existingClaims[0] }                            
                        default 
                        { 
                            Write-Error "$($member.Id) is in multiple cast memeber claims"
                            continue
                        }     
                    }                                        
                } 
                else
                {
                    Write-Error "$($member.Id) is not a human being"
                    continue
                }
                if ($statement -ne $null) 
                {
                    $referenceSnaks = @()
                    switch($Source)
                    {
                        {"ESwiki" -in $_ -and !(_hasWikiReference -Statement $statement -WikiQId $esWikiItem.Id )} 
                            { $referenceSnaks += New-WDSnak -Property p143 -ValueItem $esWikiItem }
                        {"ENwiki" -in $_ -and !(_hasWikiReference -Statement $statement -WikiQId $enWikiItem.Id )} 
                            { $referenceSnaks += New-WDSnak -Property p143 -ValueItem $enWikiItem }
                        {"IMDb" -in $_ -and !(_hasIMDBReference -Statement $statement)}   
                            { $referenceSnaks += New-WDSnak -Property p248 -ValueItem $imdbItem }
                        {"cinenacional" -in $_ -and !(_hasCinenacionalReference -Statement $statement)}   
                            { $referenceSnaks += New-WDSnak -Property p248 -ValueItem $cinenacionalItem }
                    }    
                    if ($referenceSnaks.Count -gt 0)
                    {
                        if ($AccessDate) 
                        { 
                            $referenceSnaks += New-WDSnak -Property p813 -ValueTime (ConvertTo-WDTimeValueString $AccessDate) -ValueTimePrecision Day -ValueCalendarModel GregorianCalendar
                        }
                        Add-WDReference -Statement $statement -Snaks $referenceSnaks  | Out-Null
                    }

                    if ($i.Status -eq "Changed")
                    {
                        Save-WDEntity -Entity $i
                    }
                    Write-Verbose "Processed $($member.Id)"                
                } 
                else 
                {
                    Write-Verbose "Skipped $($member.Id) $($i.Id)"
                }        
            }
        }
    }
    end
    {
        Write-Output $Item
    }
}
