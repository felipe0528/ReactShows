import ShowStore from './showStore';
import UserStore from './userStore';
import { createContext } from 'react';
import { configure } from 'mobx';

configure({enforceActions: 'always'});

export class RootStore {
    // showstore: ShowStore;
    // userStore: UserStore;

    // constructor() {
    //     this.showstore = new ShowStore(this);
    //     this.userStore = new UserStore(this);
    // }
}

export const RootStoreContext = createContext(new RootStore());