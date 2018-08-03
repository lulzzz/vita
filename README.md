# Work in Progress (Demoware -- series of posts & talks coming to show how I built this)

## Introduction

A solution that uses ML.Net to classify spending patterns. Utlising Azure features to be self updating (aka online machine learning)



#### For more info

<a href="http://mckelt.com/blog/?p=460" target="_blank">Charge Id – scratching the tech itch [ part 1 ]</a>

<a href="http://mckelt.com/blog/?p=485" target="_blank">Charge Id – lean canvas [ part 2 ]</a>

<a href="http://mckelt.com/blog/?p=505" target="_blank">Charge Id – solution overview [ part 3 ]</a>

<a href="http://mckelt.com/blog/?p=507" target="_blank">Charge Id – analysing the data [ part 4 ]</a>

<a href="http://mckelt.com/blog/?p=668" target="_blank">Charge Id – the prediction model [ part 5 ]</a>

<a href="http://mckelt.com/blog/?p=705" target="_blank">Charge Id – deploying a ML.Net Model to Azure [ part 6 ]</a>



#### Code

<a href="https://github.com/chrismckelt/vita" target="_blank">https://github.com/chrismckelt/vita</a>

![image](https://user-images.githubusercontent.com/662868/42613185-0f6a0ae4-85d2-11e8-90b0-335f87a5ee1f.png)


### Technology

- ML.Net classifier for NLP description [done]
- ML.Net regression for unclassified transactions [to do]
- Azure Service Bus & Queues to update the prediction model [to do]
- Azure app service with a Swagger REST API to run the predictions [done]
- Azure FUNCTION app service with a Swagger REST API to run the predictions [blocked]
- Angular 6 with SSR [doing]
- Application Insights [to do]
- local docker dev with CosmosDB  [to do]
- CosmosDB backing onto Azure Search for quick lookup of data (Australian companies, suburbs and Google place information) [doing]
- 

Project board

<a href="https://github.com/chrismckelt/vita/projects/1" target="_blank">https://github.com/chrismckelt/vita/projects/1</a>

## License

[MIT](LICENSE).