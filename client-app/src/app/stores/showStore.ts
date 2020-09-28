import { observable, action , configure, runInAction} from 'mobx';
import { createContext } from 'react';
import agent from '../api/agent';
import { IEnvelope } from '../models/envelope';
import { IShow } from '../models/show';

configure({enforceActions: 'always'});

class ShowStore {
    
    @observable showsEnvelop:IEnvelope | null = null;
    @observable show :IShow | undefined;
    @observable loadingInitial = false;

    @action loadShows = async () => {
        this.loadingInitial = true;
        try {
            const shows = await agent.Shows.list();
            runInAction('loading',()=>{
                this.showsEnvelop = shows;
                this.loadingInitial = false
            })
            
        } catch(e){
            runInAction('loading error',()=>{
                console.log(e);
            })
        }
    }

    @action loadShow = async (id: string) => {
      
        this.loadingInitial = false;
        try {
          let show = await agent.Shows.details(id);
          runInAction('getting show',() => {
            this.show = show;
            this.loadingInitial = false;
          })
        } catch (error) {
          runInAction('get show error', () => {
            this.loadingInitial = false;
          })
          console.log(error);
        }
      }
}

export default createContext(new ShowStore());