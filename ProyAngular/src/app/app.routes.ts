import { Routes } from '@angular/router';
<<<<<<< HEAD
import { MainComponent } from './main/main.component';
import { LoginComponent } from './login/login.component';
=======
import { MainComponent } from './pages/main/main.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
>>>>>>> origin/main

export const routes: Routes = [
    {
        path: '',
        component: MainComponent,
    },
<<<<<<< HEAD

    {
        path: 'login',
        component: LoginComponent,
<<<<<<< HEAD
    },

=======
=======
    {
        path: 'login',
        component: LoginComponent,
    },
    {
        path: 'register',
        component: RegisterComponent,
>>>>>>> origin/main
    }
>>>>>>> development
];
