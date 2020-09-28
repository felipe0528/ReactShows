import React, { useContext, useEffect } from 'react';
import { Card, Image, Button } from 'semantic-ui-react';
import ShowStore from '../../../app/stores/showStore';
import { observer } from 'mobx-react-lite';
import { RouteComponentProps } from 'react-router';
import LoadingComponent from '../../../app/layout/LoadingComponent';

interface DetailParams {
  id: string;
}

const ShowDetails: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history
}) => {
  const showStore = useContext(ShowStore);
  const {
    show,
    loadShow,
    loadingInitial
  } = showStore;

  useEffect(() => {
    loadShow(match.params.id);
  }, [loadShow, match.params.id]);

   if (!show) return <LoadingComponent content='Loading show...' />
    
  return (<div>
    <Card fluid>
        <Image src={show!.photoURL!} wrapped ui={false} />
        <Card.Content>
            <Card.Header>{show!.name!}</Card.Header>
            <Card.Meta>
                <span className='date'>{show!.genere!}</span>
            </Card.Meta>
            <Card.Description>
                {
                 show.summary
                }
            </Card.Description>
        </Card.Content>
        <Card.Content extra>
        <Button
          onClick={() => history.push('/shows')}
          basic
          color='grey'
          content='Cancel'
        />
        </Card.Content>
    </Card>
    <h3>Cast</h3>
    
  </div>
    
  );
};

export default observer(ShowDetails);
