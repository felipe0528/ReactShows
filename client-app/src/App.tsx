import React, {Component} from 'react';
import logo from './logo.svg';
import './App.css';


class App extends Component{
  state = {
    values:[]
  }

  componentDidMount() {
    this.setState(
      {
        values: [{id:1, name :"name 1"},{id:2, name :"name 2"}]
      }
    )
  }

  render(){
    return (
      <div>
        <ul>
          {this.state.values.map((value: any) =>(
            <li>{value.name}</li>
          ))}
        </ul>
      </div>
    );
  }

}


export default App;
