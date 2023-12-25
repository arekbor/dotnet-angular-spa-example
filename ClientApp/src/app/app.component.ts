import { Component, OnInit } from "@angular/core";
import { Account } from "./account.model";
import { AuthService } from "./auth.service";

@Component({
  selector: "app-root",
  templateUrl: "app.component.html",
})
export class AppComponent implements OnInit {
  isAuthenticated: boolean = false;
  account: Account | null;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.identity();

    this.authService.getAccountState().subscribe((account) => {
      this.account = account;
    });

    this.authService.isAuthenticated().subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
    });
  }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout();
  }
}
