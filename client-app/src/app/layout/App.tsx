import React, { useEffect, Fragment, useContext} from 'react';
import { Navbar } from '../../features/nav/Navbar';
import { Container } from 'semantic-ui-react';
import { ShowsDasboard } from '../../features/shows/dashboard/ShowsDasboard';
import LoadingComponent from './LoadingComponent';
import {observer} from 'mobx-react-lite';
import ShowStoreContext from '../stores/showStore';
import { Route, withRouter, RouteComponentProps } from 'react-router-dom';
import HomePage from '../../features/home/HomePage';
import ShowDetails from '../../features/shows/details/ShowDetails';

const App = () =>{
  const showStore = useContext(ShowStoreContext)
  useEffect(()=>
    {
      showStore.loadShows();
    }
  ,[showStore])

  if (showStore.loadingInitial) return <LoadingComponent content='Loading shows...' />

  return (
    <Fragment>
      <Route exact path='/' component={HomePage} />
      <Navbar/>
      <Container style={{marginTop:'3em'}}>
        <Route exact path='/shows' component={ShowsDasboard} />
        <Route exact path='/shows/:id' component={ShowDetails} />
      </Container>
    </Fragment>
  );
}

export default observer(App);
