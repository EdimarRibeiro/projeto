import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments';
import { Observable } from 'rxjs';

@Injectable()
export class HomeService {

    private URL = environment.baseServer + 'home';

    constructor(private http: HttpClient) {
    }

    getVersao(): Observable<any> {
        return this.http.get<any[]>(this.URL + '/versao');
    }

}
