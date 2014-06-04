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