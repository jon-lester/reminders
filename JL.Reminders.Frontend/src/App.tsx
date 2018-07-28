import * as Mui from '@material-ui/core/';
import CssBaseline from '@material-ui/core/CssBaseline';
import * as React from 'react';
import './App.css';

import ReminderCardContainer from './components/ReminderCardContainer';
import IReminder from './interfaces/IReminder';

export interface IAppState {
    reminders: IReminder[]
}

class App extends React.Component<any, IAppState> {

    constructor(props: any) {
        super(props);
        this.state = {
            reminders: []
        };
    }

    public render() {
        return (
            <React.Fragment>
                <Mui.AppBar>
                    <Mui.Toolbar color="white">
                        <Mui.Typography variant="title" color="inherit">Reminders</Mui.Typography>
                    </Mui.Toolbar>
                </Mui.AppBar>
                <CssBaseline />
                <ReminderCardContainer reminders={this.state.reminders} />
            </React.Fragment>
        );
    }

    public componentDidMount() {
        fetch('http://localhost:49900/api/reminders/')
            .then(response => response.json())
            .then(json => {
                this.setState({
                    reminders: json
                });
                this.render();
            });
    }
}

export default App;