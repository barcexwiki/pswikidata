Write-Verbose "Loading PSWikidata module"

function ConvertTo-WDTimeValueString
{

    [CmdletBinding(PositionalBinding=$true,
                  ConfirmImpact='Low')]
    [OutputType([string])]
    Param
    (
         # Item to be modified
        [Parameter(Mandatory=$true, 
                   Position = 0)]
        [ValidateNotNullOrEmpty()]
        [System.DateTime]
        $Date
    )

    Process
    {
        $result = [String]::Format("{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}Z", $date.Year, $date.Month, $date.Day, $date.Hour, $date.Minute, $date.Second);
        if ($date.year -gt 0) 
        {
            $result = "+" + $result;
        }
        Write-Output $result
    }
}

function Copy-WDLabel
{
    [CmdletBinding(SupportsShouldProcess=$true, 
                  PositionalBinding=$false,
                  ConfirmImpact='Medium')]
    [OutputType([PSWikidata.PSWDItem])]
    param
    (
        # Entity to be modified
        [Parameter(Mandatory=$true, 
                   ValueFromPipeline=$true)]
        [ValidateNotNullOrEmpty()]
        [Alias("e")] 
        [PSWikidata.PSWDEntityArgumentTransformation()]
        [PSWikidata.PSWDEntity[]]
        $Entity,

        # Source language
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [Alias("sl")] 
        [string]
        $SourceLanguage,

        # Destination languages
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [Alias("dl")] 
        [string[]]
        $DestinationLanguages,

        [Parameter(Mandatory=$false)]
        [switch]$DoNotSave = $false
    )

    process
    {
        foreach ($e in $Entity)
        { 
            if ($pscmdlet.ShouldProcess($e.Id, "Copying labels from $SourceLanguage to $($DestinationLanguages -join " ")"))
            {               

                $label = ($e.Labels | Where-Object Language -eq $SourceLanguage).Label

                if ($label)
                {
                    foreach ($dl in $DestinationLanguages)
                    {
                        Write-Verbose "Copying label '$label' of $($e.Id) from [$SourceLanguage] to [$dl]"
                        Set-WDEntity -Entity $e -Label $label -Language $dl -DoNotSave -ErrorAction Stop | Out-Null
                    }
                }
                else
                {
                    Write-Warning "$($e.Id) has no label for language $SourceLanguage"
                } 

                try
                {
                    if (!$DoNotSave)
                    {                   
                        Save-WDEntity -Entity $e -ErrorAction Stop | Out-Null
                    }
                    # sends the entity to the output stream
                    Write-Output $e
                } 
                catch 
                {
                    Write-Error $_
                }
             }
        }

    }
    
}