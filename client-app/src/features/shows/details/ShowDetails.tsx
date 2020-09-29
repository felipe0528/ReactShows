import React, { useContext, useEffect } from 'react';
import { Card, Image, Button } from 'semantic-ui-react';
import { RootStoreContext } from '../../../app/stores/rootStore';
import { observer } from 'mobx-react-lite';
import { RouteComponentProps } from 'react-router';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { IActor } from '../../../app/models/actor';
import { ISeason } from '../../../app/models/season';
import { IEpisodes } from '../../../app/models/episodes';

interface DetailParams {
  id: string;
}

const ShowDetails: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history
}) => {
  const showStore = useContext(RootStoreContext);
  const {
    show,
    loadShow,
    loadingInitial
  } = showStore.showStore;

  useEffect(() => {
    loadShow(match.params.id);
  }, [loadShow, match.params.id]);

   if (loadingInitial ||!show) return <LoadingComponent content='Loading show...' />
    
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
    <table>
      <thead>
          <th>Character</th>
          <th>Person</th>
      </thead>
      <tbody>
        {show.cast.map((actor: IActor) =>(
          <tr>
            <td>{actor.character.name}</td>
            <td>{actor.person.name}</td>
          </tr>
        ))}
      </tbody>
    </table>
    <h3>Seasons</h3>
    <table>
      <thead>
        <th>Season</th>
        <th>Episodes</th>
      </thead>
      <tbody>
        {show.seasons.map((season: ISeason) =>(
        <tr>
          <td>{season.seasonNumber}</td>
          <td>
            <ul>
              {season.episodes.map((episode: IEpisodes) =>(
                  <li>{episode.id} - {episode.name}</li>
              ))}
            </ul>
          </td>
        </tr>
      ))}
      </tbody>
    </table>
  </div>
  );
};

export default observer(ShowDetails);
