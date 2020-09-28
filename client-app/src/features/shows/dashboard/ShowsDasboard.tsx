import React, { useContext } from 'react'
import { Grid } from 'semantic-ui-react'
import { ShowList } from '../shows-list/ShowList'
import ShowStoreContext from '../../../app/stores/showStore';
import { observer } from 'mobx-react-lite';

export const ShowsDasboard: React.FC = () => {
    const showStore = useContext(ShowStoreContext);
    const shows = showStore.showsEnvelop;

    return (
        <div>
            <Grid>
                <Grid.Row>
                    {/* Filters */}
                </Grid.Row>
                <Grid.Row>
                    {shows && <ShowList />}
                </Grid.Row>
            </Grid>
        </div>
    )
}

export default observer(ShowsDasboard);