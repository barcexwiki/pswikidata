---
external help file: PSWikidata.dll-Help.xml
online version: 
schema: 2.0.0
---

# Connect-WDServer

## SYNOPSIS
Connects to Wikidata servers.

## SYNTAX

```
Connect-WDServer [-Credential] <PSCredential> [[-ApiUrl] <String>] [<CommonParameters>]
```

## DESCRIPTION
Connects to Wikidata servers. The cmdlet starts a new session a Wikidata servers. You can optionally set the credentials to be used for the session. If not credentials are set, the cmdlet will ask for a user and password to use. 

By default the Wikidata API URL is used, but a custom API URL can be set to connect to a different Wikibase server.

The session can be closed with the Disconnect-WDServer cmdlet.

## EXAMPLES

### Example 1
```
PS C:\> Connect-WDServer
```

Asks for credentials (username and password) and connects to Wikidata.

### Example 2
```
PS C:\> Connect-WDServer -Credential $cred
```

Connects to Wikidata using the credentials stored in $cred variable.

### Example 3
```
PS C:\>  Connect-WDServer -ApiUrl https://test.wikidata.org/w/api.php
```

Connects to Wikidata using the specified API URL.

## PARAMETERS

### -ApiUrl
A Wikibase API URL. By default https://www.wikidata.org/w/api.php is used.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Credential
Credentials to login to Wikidata.

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### None

## NOTES

## RELATED LINKS

