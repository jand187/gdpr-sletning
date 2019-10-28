remove-item "D:\Gdpr-sletning-test\*" -force

$file = New-Item "D:\Gdpr-sletning-test\should be deleted.txt"
$file.LastWriteTime = '01/11/2005 06:01:36'

New-Item "D:\Gdpr-sletning-test\should NOT be deleted.txt"
New-Item "D:\Gdpr-sletning-test\should NOT be deleted-modified.txt"

$file2 = New-Item "D:\Gdpr-sletning-test\should fail.txt"
$file2.LastWriteTime = '01/11/2005 06:01:36'
Set-ItemProperty -Path "D:\Gdpr-sletning-test\should fail.txt" -Name IsReadOnly -Value $true

