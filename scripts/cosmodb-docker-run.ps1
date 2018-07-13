#docker rm -f $(docker ps -aq)

docker run --name azure-cosmosdb-emulator-container --memory 2GB --mount type=bind,source=c:/tools/CosmosDbEmulatorCert,destination=C:/CosmosDB.Emulator/CosmosDbEmulatorCert -P --interactive --tty microsoft/azure-cosmosdb-emulator

explorer https://172.20.229.193:8081/
