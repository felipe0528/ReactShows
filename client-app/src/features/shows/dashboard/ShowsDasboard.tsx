import React, { useContext, useEffect, useState } from 'react';
import { Button, Grid } from 'semantic-ui-react';
import  ShowList  from '../shows-list/ShowList'
import { RootStoreContext } from '../../../app/stores/rootStore';
import { observer } from 'mobx-react-lite';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import InfiniteScroll from 'react-infinite-scroller';

export const ShowsDasboard: React.FC = () => {

    const rootStore = useContext(RootStoreContext);
    const {loadShows, loadingInitial, setPage, page, totalPages} = rootStore.showStore;
    const [loadingNext, setLoadingNext] = useState(false);

    const handleGetNext = () => {
      setLoadingNext(true);
      setPage(page + 1);
      loadShows().then(() => setLoadingNext(false));
    };

    useEffect(() => {
        loadShows();
      }, [loadShows]);
    
      if (loadingInitial && page === 0)
    return <LoadingComponent content='Loading activities' />;

    return (
    <Grid>
      <Grid.Row  >
        <h2>Show filters</h2>
      </Grid.Row>
      <Grid.Row >
        <InfiniteScroll
            pageStart={0}
            loadMore={handleGetNext}
            hasMore={!loadingNext && page + 1 < totalPages}
            initialLoad={false}
          >
            <ShowList />
        </InfiniteScroll>
      </Grid.Row>
    </Grid>
  );
}

export default observer(ShowsDasboard);