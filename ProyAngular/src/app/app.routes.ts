import { Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';

export const routes: Routes = [
    {
        path: '',
        component: MainComponent,
    },
    {
        path: 'login',
        component:LoginComponent,
    },
    {
        path: 'register',
        component: RegisterComponent,
    }
];
