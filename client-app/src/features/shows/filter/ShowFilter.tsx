import React, { useContext, useState } from 'react'
import { RootStoreContext } from '../../../app/stores/rootStore';

export const ShowFilter = () => {
    const rootStore = useContext(RootStoreContext);
    const { predicate, setPredicate } = rootStore.showStore;
    const [state, setstate] = useState({value:''})

    const handleSubmit = (predicateKey:string, value:string) => {
        setPredicate(predicateKey, value);
      };
    
      const handleChange = (event:any) => {
            setstate({ value: event.target.value})
    }

    return (
        <div>
            <div className="container">
                <div className='white' >
                    <div className="input-field">
                        <input type="text" name="keywords" value={state.value} onChange={handleChange} required />
                    </div>
                    <div className="input-field"> 
                        <button className="btn blue darken-3" type="submit" onClick={() => handleSubmit('keywords',state.value)}>Search by keywords</button>
                        <button className="btn blue darken-3" type="submit" onClick={() => handleSubmit('language',state.value)}>Search by Language</button>
                        <button className="btn blue darken-3" type="submit" onClick={() => handleSubmit('genere',state.value)}>Search by Genere</button>
                        <button className="btn blue darken-3" type="submit" onClick={() => handleSubmit('channel',state.value)}>Search by Channel</button>
                        <button className="btn blue darken-3" type="submit" onClick={() => handleSubmit('time',state.value)}>Search by Time</button>
                        <button className="btn blue darken-3" type="submit" onClick={() => handleSubmit('sortedRating',state.value)}>Sort by Rating</button>
                        <button className="btn blue darken-3" type="submit" onClick={() => handleSubmit('sortedChannel',state.value)}>Sort by Channel</button>
                        <button className="btn blue darken-3" type="submit" onClick={() => handleSubmit('sortedGenere',state.value)}>Sort by Genere</button>
                    </div>
                </div>
            </div>
        </div>

        
    )
}
