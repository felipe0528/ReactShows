import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite';
import { RootStoreContext } from '../../../app/stores/rootStore';
import { Link } from 'react-router-dom';
import { Button, Card, Grid, Icon, Image } from 'semantic-ui-react'
import {IShow} from '../../../app/models/show'

const ShowList: React.FC = () => {
    const rootStore = useContext(RootStoreContext);
    const {showsDefault} = rootStore.showStore
    return (
        <Card.Group itemsPerRow={5}>
            {showsDefault.map((show: IShow) =>(
                <Card key={show.idAPI}>
                    <Image src={show.photoURL} wrapped ui={false} />
                    <Card.Content>
                        <Card.Header>{show.name}</Card.Header>
                        <Card.Meta>
                            <span className='date'>{show.genere}</span>
                        </Card.Meta>
                        <Card.Description>
                            <b>Channel:</b> {show.channel}<br/>
                            <b>Time:</b> {show.time}<br/>
                            <b>Days:</b> {show.days}
                        </Card.Description>
                    </Card.Content>
                    <Card.Content extra>
                    <Grid columns={2} divided>
                    <Grid.Column>
                    <span>
                        <Icon name='language' />
                        {show.language}
                    </span>
                    <br/>
                    <span>
                        <Icon name='star' />
                        {show.rating}
                    </span>
                    </Grid.Column>
                    <Grid.Column>
                    <Button
                            as={Link} to={`/shows/${show.idAPI}`}
                            basic
                            color='blue'
                        content='View'
                        />
                    </Grid.Column>
                    
                   
                        </Grid>
                    </Card.Content>
              </Card>
            ))}
        </Card.Group>
    )
}

export default observer(ShowList);