import { Location } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, Subject, map, tap } from "rxjs";
import { Account } from "./account.model";

@Injectable()
export class AuthService {
  private accountState$ = new Subject<Account | null>();
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
        tap((userDto: Account) => {
          this.authenticate(userDto);
        })
      )
      .subscribe();
  }

  isAuthenticated(): Observable<boolean> {
    return this.accountState$.pipe(map((userDto) => !!userDto));
  }

  getAccountState(): Observable<Account | null> {
    return this.accountState$;
  }

  private authenticate(identity: Account | null) {
    this.accountState$.next(identity);
  }

  private fetch(): Observable<Account> {
    return this.httpClient.get<Account>("api/account");
  }
}
