import { Location } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, Subject, map, tap } from "rxjs";
import { UserDto } from "./userDto.model";

@Injectable()
export class AuthService {
  private accountState$ = new Subject<UserDto | null>();
  constructor(private httpClient: HttpClient, private location: Location) {}

  login() {
    location.href = `${location.origin}${this.location.prepareExternalUrl(
      "api/oauth2/authorize/google"
    )}`;
  }

  logout() {
    this.httpClient.post("/api/oauth2/signout/google", {}).subscribe();
    this.accountState$.next(null);
  }

  identity(): void {
    this.fetch()
      .pipe(
        tap((userDto: UserDto) => {
          this.authenticate(userDto);
        })
      )
      .subscribe();
  }

  isAuthenticated(): Observable<boolean> {
    return this.accountState$.pipe(map((userDto) => !!userDto));
  }

  getAccountState(): Observable<UserDto | null> {
    return this.accountState$;
  }

  private authenticate(identity: UserDto | null) {
    this.accountState$.next(identity);
  }

  private fetch(): Observable<UserDto> {
    return this.httpClient.get<UserDto>("api/account");
  }
}
