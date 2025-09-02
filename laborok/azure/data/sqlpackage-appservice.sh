wget https://aka.ms/sqlpackage-linux -O /tmp/sqlpackage-linux.zip
cd /home
mkdir sqlpackage
unzip /tmp/sqlpackage-linux.zip -d /home/sqlpackage 
chmod a+x /home/sqlpackage/sqlpackage
/home/sqlpackage/sqlpackage /Action:Publish /SourceFile:"/home/site/wwwroot/imdbtitles_sample_azure.dacpac" /TargetConnectionString:"$SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING"
