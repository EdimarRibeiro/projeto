import { Injectable } from '@angular/core';


@Injectable()
export class UuidService {


    constructor() { }

    getUuidv4() {
        // @ts-ignore 
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c: any) =>
            // tslint:disable-next-line:no-bitwise  
            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        );
    }

}
