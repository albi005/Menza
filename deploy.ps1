Get-ChildItem -include bin,obj,.vs -recurse -force | Remove-Item -recurse -force

dotnet publish -c Release -r linux-x64 --self-contained Menza.Server/Menza.Server.csproj

ssh albi@192.168.0.7 'systemctl --user stop menza.service'

scp -r Menza.Server/bin/Release/net7.0/linux-x64/publish/* albi@192.168.0.7:/home/albi/www/Menza/bin

ssh albi@192.168.0.7 'systemctl --user restart menza.service'
