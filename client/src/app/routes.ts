import { ProductDetailsComponent } from './product-details/product-details.component';
import { Routes } from "@angular/router";
import { MainPageComponent } from "./main/main.component";
import { ProductComponent } from "./Product/product.component";
import { LoginPageComponent } from "./login/login.component";
import { RegisterComponent } from './register/register.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserSettingComponent } from './user-setting/user-setting.component';
import { UserChangePasswordComponent } from './user-change-password/user-change-password.component';
import { AddProductComponent } from './add-product/add-product.component';
export const appRoutes: Routes =
  [
    { path: "home", component: MainPageComponent },
    { path: "products", component: ProductComponent },
    { path: "products/tags/:tagId", component: ProductComponent },
    { path: "products/:id", component: ProductComponent },
    { path: "productDetail/:idx", component: ProductDetailsComponent },
    { path: "AddProduct", component: AddProductComponent },
    { path: "login", component: LoginPageComponent },
    { path: "register", component: RegisterComponent },
    { path: "profile", component: UserProfileComponent },
    { path: "profile/setting/password", component: UserChangePasswordComponent },
    { path: "profile/setting", component: UserSettingComponent },
    { path: "profile/:username", component: UserProfileComponent },
    { path: "**", component: MainPageComponent },
  ];
