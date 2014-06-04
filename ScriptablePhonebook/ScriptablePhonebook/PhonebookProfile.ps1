function New-Contact {
	<#
		.Synopsis
			Creates a new contact
		.Description
			The New-Contact function creates a new phonebook contact and sets both name and number
		.Example
			New-Contact "John Doe" "99887766"
	#>

	param (
		[Parameter(ValueFromPipelineByPropertyName=$true)]
		[string]$Name,

		[Parameter(ValueFromPipelineByPropertyName=$true)]
		[string]$Number
	)

	$contact = New-Object ScriptablePhonebook.Models.Contact
	
	$contact.Name = $Name
	$contact.Number = $Number

	$contact
}

function Clear-Contacts {
	<#
		.Synopsis
			Deletes all contacts in the phonebook
		.Description
			The Clear-Contacts function deletes all contacts from the phonebook
		.Example
			Clear-Contacts
	#>

	$repository.Clear()
}

function Add-Contact {
	<#
		.Synopsis
			Adds contacts to the phonebook
		.Description
			The Add-Contact function adds contacts to the phonebook.
		.Example
			New-Contact "John Doe" "99887766" | Add-Contact
		.Example
			Add-Contact(New-Contact "John Doe" "99887766")
	#>

	param (
		[Parameter(ValueFromPipeline=$true)]
		$Contact
	)

	$repository.AddContact($Contact)
}

function Search-Number {
	<#
		.Synopsis
			Searches the web for phone numbers
		.Description
			The Search-Number function searches the web for phone numbers of specified name
		.Example
			Search-Number "John Doe"
	#>

	param (
		[string]$Name
	)

	$url = "http://www.gulesider.no/person/resultat/$Name"
	$result = Invoke-WebRequest $url
	$number = $result.AllElements |
		Where Class -match "hit-phone-number" |
		Select -First 1 -ExpandProperty innerText

	if ($number -eq $null) {
		"not found"
	} else {
		$number
	}
}

function New-ContactBySearch {
	<#
		.Synopsis
			Creates a new contact, searching the web for the number
		.Description
			The New-ContactBySearch function creates a new phonebook contact, where the number is found by making a web search.
		.Example
			New-ContactBySearch "John Doe"
	#>

	param (
		[string]$Name
	)

	New-Contact $Name (Search-Number $Name)
}