import { Component, OnInit } from "@angular/core";
import { AuthService } from "./auth.service";
import { UserDto } from "./userDto.model";

@Component({
  selector: "app-root",
  templateUrl: "app.component.html",
})
export class AppComponent implements OnInit {
  isAuthenticated: boolean = false;
  userDto: UserDto | null;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.identity();

    this.authService.getAccountState().subscribe((userDto) => {
      this.userDto = userDto;
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
