Get-ChildItem -include bin,obj,.vs -recurse -force | Remove-Item -recurse -force

dotnet publish -c Release -r linux-x64 --self-contained

ssh albi@100.80.80.33 'systemctl --user stop menza.service'

scp -r bin/Release/net7.0/linux-x64/publish/* albi@100.80.80.33:/home/albi/www/Menza/bin

ssh albi@100.80.80.33 'systemctl --user restart menza.service'
