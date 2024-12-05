import { Routes } from '@angular/router';
import { MainComponent } from './pages/main/main.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
import { CatalogComponent } from './pages/catalog/catalog.component';
import { AboutusComponent } from './pages/aboutus/aboutus.component';
import { CartComponent } from './pages/cart/cart.component';
import { ProductDetailComponent } from './pages/product-detail/product-detail.component';
import { CheckoutComponent } from './pages/checkout/checkout.component';
<<<<<<< HEAD
import { PaymentComponent } from './pages/payment/payment.component';

=======
>>>>>>> origin/Fitin
export const routes: Routes = [
    {
        path: '',
        component: MainComponent,
    },
    {
        path: 'login',
        component: LoginComponent,
    },
    {
        path: 'register',
        component: RegisterComponent,
    },
    {
        path:'catalog',
        component: CatalogComponent,
    },
    {
        path: 'aboutus',
        component: AboutusComponent,
    },
    {
        path: 'cart',
        component: CartComponent,
    },
    {
        path: 'product-detail/:id',
        component: ProductDetailComponent,
    },
    {
        path: 'checkout',
        component: CheckoutComponent,
<<<<<<< HEAD
    },
    {
        path: 'pay',
        component: PaymentComponent,
=======
>>>>>>> origin/Fitin
    }
];
