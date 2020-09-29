import { observable, action, computed,  runInAction, reaction } from 'mobx';
import agent from '../api/agent';
import { IEnvelope } from '../models/envelope';
import { IShow } from '../models/show';
import {RootStore} from './rootStore';

const LIMIT = 20;

export default class ShowStore {
    
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
    reaction(
      () => this.predicate.keys(),
      () => {
        this.page = 0;
        this.showRegistry.clear();
        this.loadShows();
      }
    )
  }

    @observable showRegistry = new Map();
    @observable show :IShow | undefined;
    @observable showsEnvelop:IEnvelope | undefined;
    @observable loadingInitial = false;
    @observable submitting = false;
    @observable target = '';
    @observable showsCount = 0;
    @observable page = 0;
    @observable predicate = new Map();

    @action setPredicate = (predicate: string, value: string) => {
      this.predicate.clear();
      if (predicate !== 'all') {
        this.predicate.set(predicate, value);
      }
    }

    @computed get axiosParams() {
      const params = new URLSearchParams();
      params.append('limit', String(LIMIT));
      params.append('offset', `${this.page ? this.page * LIMIT : 0}`);
      this.predicate.forEach((value, key) => {
        params.append(key, value)
      })
      return params;
    }


    @computed get totalPages() {
      return Math.ceil(this.showsCount / LIMIT);
    }
  
    @action setPage = (page: number) => {
      this.page = page;
    }

    @computed get showsDefault() {
      return Array.from(this.showRegistry.values());
    }

    @action loadShows = async () => {
        this.loadingInitial = true;
        try {
            const showEnvelope = await agent.Shows.list(this.axiosParams);
            const {shows,showsCount} = showEnvelope;
            runInAction('loading shows',()=>{
              this.showsEnvelop = showEnvelope;
              shows.forEach(show => {
                this.showRegistry.set(show.id, show);
              });
              this.showsCount=showsCount;
              this.loadingInitial = false
            })
        } catch(e){
            runInAction('loading error',()=>{
                console.log(e);
            })
        }
    }

    @action loadShow = async (id: string) => {
        this.loadingInitial = true;
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
