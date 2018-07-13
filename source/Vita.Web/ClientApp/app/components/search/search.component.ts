import { Component, OnInit } from '@angular/core';
import "rxjs/add/operator/toPromise";
import { Observable } from "rxjs";

import { trigger, state, style, transition, animate, keyframes } from '@angular/animations';
//import { trigger, state, style, transition, animate, keyframes } from '@angular/platform-browser/animations';
//import { SearchResponse } from './searchResult';
import { SearchService } from './search.service';
import { ChargeClient, Charge, SearchResponse, Company, TransactionType, Locality, Classifier, PaymentMethodType, AustralianState, CategoryType } from "../../vita.api";

//import * as Chartist from 'chartist';

declare var $: any;

@Component({
    selector: 'search-cmp',
    //moduleId: module.id,
    templateUrl: 'search.component.html',
    animations: [
        trigger('myAwesomeAnimation', [
            state('true', style({
                transform: 'scale(1)',
            })),
            state('false', style({
                transform: 'scale(1.2)',
            })),
            transition('false => true', animate('200ms ease-in')),
        ]),
    ]
    // ,template: `
    // <p [@myAwesomeAnimation]='state' (click)="search()">I will animate</p>
    // `,
})

export class SearchComponent implements OnInit {
    public showResults: boolean = false;
    public state: string = 'true';
    public results: Array<SearchResponse>;// Array<SearchResponse>; // Observable<Charge | null>;
    public keyword: string = "";
    public isSearching: boolean = false;

    constructor(
        private searchService: SearchService) {
    }


    ngOnInit(): void {
        // this.getSearchResults();
        this.loadDummyData();
    }

    search() {
        this.getSearchResults();
    }

    getSearchResults() {
        var that = this;
        this.isSearching = true;


        console.info("getSearchResults..." + this.keyword);
        this.searchService.search(this.keyword)
            .subscribe(results => {


                this.isSearching = false;
                console.log("results", results);
                this.results = new Array<SearchResponse>();
                
                if (results){
                    for(var i = 0; i < results.length;i++){
                      this.results.push(results[i]);
                    }
                }

                that.state = 'false';
                that.showResults = true;
            });
    }

    loadDummyData() {
        this.isSearching = false;
        this.showResults = true;

        var sr = new SearchResponse();
        sr.chargeId = "6fdf5fff-4745-47cf-851b-452c3ae4cb64";

        //who
        var c = new Company();
        c.australianBusinessNumber = "006449056";
        c.companyName = "COLESAN";
        c.currentName = "COLES";
        c.dateOfRegistration = "15/07/1985";
        c.companyType = "APTY";
        c.previousStateOfRegistration = "WA";
        sr.who = c;

        //what
        sr.what = TransactionType.Credit;

        //where
        var w = new Locality();
        w.australianState = AustralianState.WA;
        w.placeId = "111111111";
        w.postcode = "6018";
        w.suburb = "Doubleview";
        w.createdUtc = new Date();
        sr.where = w;

        //when
        sr.when = new Date();

        //why

        var kw = [
            "supermarkets",
            "coles",
            "bilo",
            "woolworths",
            "farmerjacks",
            "superiga",
            "iga",
            "greengrocer",
            "farmersmark",
            "aldi",
            "spudshed"
        ];

        var y = new Classifier();
        y.categoryType = CategoryType.FoodDrinks;
        y.id = "some guid";
        y.keywords = kw;
        y.subCategory = "Groceries";

        sr.why = y;

        // build return array
        var arr = new Array<SearchResponse>();
        arr.push(sr);
        arr.push(sr);
        this.results = Observable.create(arr);

        //this.results = [
        //      {
        //          "Id": "6fdf5fff-4745-47cf-851b-452c3ae4cb64",
        //          "who": {
        //              "Id": "003cbe1f-d321-46de-ab41-27ed11ce18eb",
        //              "companyName": "COLESAN PTY. LTD.",
        //              "australianCompanyNumber": "006449056",
        //              "companyType": "APTY",
        //              "companyClass": "LMSH",
        //              "subClass": "PROP",
        //              "status": "REGD",
        //              "dateOfRegistration": "15/07/1985",
        //              "previousStateOfRegistration": "VIC",
        //              "stateOfRegistrationNumber": "C0244837M",
        //              "modifiedSinceLastReport": null,
        //              "currentNameIndicator": "Y",
        //              "australianBusinessNumber": "0",
        //              "currentName": null,
        //              "currentNameStartDate": null,
        //              "companyCurrentInd": "1",
        //              "companyCurrentName": null,
        //              "companyCurrentNameStartDt": null,
        //              "companyModifiedSinceLast": "0",
        //              "createdUtc": "0001-01-01T00:00:00",
        //              "modifiedUtc": null
        //          },
        //          "what": "Unknown",
        //          "where": {
        //              "id": "d68a98c7-5bfe-4677-946e-6f17ce2a8ea4",
        //              "postcode": "5272",
        //              "suburb": "COLES",
        //              "australianState": 4,
        //              "latitude": -37.256253,
        //              "longitude": 140.611845,
        //              "placeId": null,
        //              "createdUtc": "0001-01-01T00:00:00",
        //              "modifiedUtc": null
        //          },
        //          "when": null,
        //          "why": {
        //              "id": "71f17d96-6df9-497f-b68c-520422f5836e",
        //              "categoryType": 3,
        //              "subCategory": "Supermarkets",
        //              "keywords": [
        //                  "supermarkets",
        //                  "coles",
        //                  "bilo",
        //                  "woolworths",
        //                  "farmerjacks",
        //                  "superiga",
        //                  "iga",
        //                  "greengrocer",
        //                  "farmersmark",
        //                  "aldi",
        //                  "spudshed"
        //              ]
        //          },
        //          "paymentMethodType": "Unknown",
        //          "createdUtcDate": "2018-06-10T15:42:47.9869091Z",
        //          "isChargeId": false,
        //          "chargeId": null
        //      }
        //  ];

    }

    //createNewResult(): SearchResponse {
    //    //var searchResponse: SearchResponse = {
    //    //    Id: "1c6e661d-34f8-4b6a-a1bb-f644f0f03891",
    //    //    who: {
    //    //        Id: "00000000-0000-0000-0000-000000000000",
    //    //        companyName: "TEST",
    //    //        australianCompanyNumber: "23423422",
    //    //        australianBusinessNumber: "12342342",
    //    //    },
    //    //    what: "Debit",
    //    //    where: {
    //    //        id: "00000000-0000-0000-0000-000000000000",
    //    //        postcode: "6018",
    //    //        suburb: "Doubleview",
    //    //        australianState: 7,
    //    //        latitude: 0,
    //    //        longitude: 0,
    //    //        placeId: "googleplace123",
    //    //    },
    //    //    when: new Date("2018-05-10T22:23:23.2773537+08:00"),
    //    //    why: {
    //    //        id: "00000000-0000-0000-0000-000000000000",
    //    //        categoryType: 0,
    //    //        subCategory: "ATMWithdrawals",
    //    //        keywords: []
    //    //    },
    //    //    paymentMethodType: "CashWithdrawl"
    //    //};

    //    return searchResponse;
    //}

}
