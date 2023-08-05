$SERVER = "albi@iron"

Get-ChildItem -include bin,obj,.vs -recurse -force | Remove-Item -recurse -force

dotnet publish -c Release -r linux-x64 --self-contained

ssh $SERVER 'systemctl --user stop menza.service'

scp -r bin/Release/net7.0/linux-x64/publish/* ${SERVER}:/home/albi/www/Menza/bin

ssh $SERVER 'systemctl --user restart menza.service'
