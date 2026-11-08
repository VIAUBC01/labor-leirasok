#!/bin/bash
# DACPAC betöltése az App Service SSH konzoljáról.
# Akkor használd, ha a saját gépedről nem éred el az Azure SQL-t (pl. a hálózat tiltja az 1433-as portot).
# Az App Service-ben beállított, SQLAzure típusú connection stringet használja.
set -e

CONN="$(printenv | grep -m1 '^SQLAZURECONNSTR_' | cut -d= -f2-)"
if [ -z "$CONN" ]; then
    echo "Nem található SQLAzure típusú connection string. Állítsd be a Web App Environment variables » Connection strings oldalán."
    exit 1
fi

echo "sqlpackage letöltése..."
wget -q https://aka.ms/sqlpackage-linux -O /tmp/sqlpackage-linux.zip
mkdir -p /home/sqlpackage
unzip -o -q /tmp/sqlpackage-linux.zip -d /home/sqlpackage
chmod a+x /home/sqlpackage/sqlpackage

echo "DACPAC letöltése..."
wget -q https://viaubc01.github.io/labor-leirasok/laborok/azure/data/imdbtitles_sample_azure.dacpac -O /home/imdbtitles_sample_azure.dacpac

echo "Betöltés az adatbázisba..."
/home/sqlpackage/sqlpackage /Action:Publish /SourceFile:"/home/imdbtitles_sample_azure.dacpac" /TargetConnectionString:"$CONN"
