import React, {  Fragment, useContext, useEffect} from 'react';
import { Navbar } from '../../features/nav/Navbar';
import { Container } from 'semantic-ui-react';
import { ShowsDasboard } from '../../features/shows/dashboard/ShowsDasboard';
import {observer} from 'mobx-react-lite';
import {
  Route,
  withRouter,
  RouteComponentProps,
  Switch
} from 'react-router-dom';
import HomePage from '../../features/home/HomePage';
import ShowDetails from '../../features/shows/details/ShowDetails';
import NotFound from './NotFound';
import LoginForm from '../../features/user/LoginForm';
import { RootStoreContext } from '../stores/rootStore';
import LoadingComponent from './LoadingComponent';
import ModalContainer from '../common/modals/ModalContainer';

const App: React.FC<RouteComponentProps> = () => {
  const rootStore = useContext(RootStoreContext);
  const {setAppLoaded, token, appLoaded} = rootStore.commonStore;
  const {getUser} = rootStore.userStore;
  
  useEffect(() => {
    if (token) {
      getUser().finally(() => setAppLoaded())
    } else {
      setAppLoaded();
    }
  }, [getUser, setAppLoaded, token])

  if (!appLoaded)  return <LoadingComponent content='Loading app...' />

  return (
   <Fragment>
      <ModalContainer />
      <Route exact path='/' component={HomePage} />
      <Route
        path={'/(.+)'}
        render={() => (
          <Fragment>
            <Navbar />
            <Container style={{ marginTop: '7em' }}>
              <Switch>
                <Route exact path='/shows' component={ShowsDasboard} />
                <Route path='/shows/:id' component={ShowDetails} />
                <Route path='/login' component={LoginForm} />
                <Route component={NotFound} />
              </Switch>
            </Container>
          </Fragment>
        )}
      />
    </Fragment>
  );
}

export default withRouter(observer(App));
