import { NgModule } from '@angular/core';
import { AppModuleShared } from './app.shared.module';
import { AppComponent } from './components/app/app.component';

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        AppModuleShared
    ]
})
export class AppModule {
}
