import { Routes } from '@angular/router';
<<<<<<< HEAD
<<<<<<< HEAD
import { MainComponent } from './main/main.component';
import { LoginComponent } from './login/login.component';
=======
import { MainComponent } from './pages/main/main.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
>>>>>>> origin/main
=======
import { MainComponent } from './pages/main/main.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
>>>>>>> origin/gonza

export const routes: Routes = [
    {
        path: '',
        component: MainComponent,
    },
<<<<<<< HEAD
<<<<<<< HEAD

    {
        path: 'login',
        component: LoginComponent,
<<<<<<< HEAD
    },

=======
=======
=======
>>>>>>> origin/gonza
    {
        path: 'login',
        component: LoginComponent,
    },
    {
        path: 'register',
        component: RegisterComponent,
<<<<<<< HEAD
>>>>>>> origin/main
=======
>>>>>>> origin/gonza
    }
>>>>>>> development
];
