# Work in Progress (Demoware -- series of posts & talks coming to show how I built this)

## Introduction

A WIP series of demos showing a sample application that takes your bank statement (hopefully through Open Banking) and classifies your expenditure

End solution will

Take a consumers bank statement (OPEN BANKING, Yodlee, Basiq, BankStatements) and

- classify expenditure
- predict outgoing payments or 'savings time'
- search for better deals on products (mortgages, insurance, loans, fees)


![image](https://user-images.githubusercontent.com/662868/42613185-0f6a0ae4-85d2-11e8-90b0-335f87a5ee1f.png)


### Technology

- ML.Net classifier to group bank statement description into categorical labels
- CosmosDB backing onto Azure Search for quick lookup of data (Australian companies, suburbs and Google place information)
- Azure Service Bus & Queues to update the prediction model
- Azure app service with a Swagger REST API to run the predictions
- Angular 6 with SSR
- Application Insights
- local docker dev with CosmosDB

## License

[MIT](LICENSE).