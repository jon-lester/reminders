import CssBaseline from '@material-ui/core/CssBaseline';
import Snackbar from '@material-ui/core/Snackbar';
import Typography from '@material-ui/core/Typography';
import * as React from 'react';

import ReminderAppMenuBar from './components/ReminderAppMenuBar';
import ReminderCardContainer from './components/ReminderCardContainer';

import IReminder from './interfaces/IReminder';

export interface IAppState {
    reminders: IReminder[],
    snackbarMessage: string | undefined,
    snackbarOpen: boolean
}

class App extends React.Component<any, IAppState> {

    constructor(props: any) {
        super(props);
        this.state = {
            reminders: [],
            snackbarMessage: undefined,
            snackbarOpen: false
        };
    }

    public render() {
        return (
            <React.Fragment>
                <CssBaseline />
                <ReminderAppMenuBar />
                <ReminderCardContainer
                    reminders={this.state.reminders}
                    onMarkActioned={this.handleMarkActioned}
                />
                <Snackbar
                    autoHideDuration={3000}
                    open={this.state.snackbarOpen}
                    message={<Typography align = "center" color="inherit">{this.state.snackbarMessage}</Typography>}
                    onClose={this.handleSnackbarClosed}
                />
            </React.Fragment>
        );
    }

    public componentDidMount() {
        const uri = 'https://2d410672-d82b-4642-918f-d96db1e140e1.mock.pstmn.io/api/reminders/';
        // const uri = 'http://localhost:49900/api/reminders/';
        fetch(uri)
            .then(response => response.json())
            .then(json => {
                this.setState({
                    reminders: json
                });
                this.render();
            });
    }

    private handleSnackbarClosed = () => {
        this.setState({
            ...this.state,
            snackbarMessage: undefined,
            snackbarOpen: false
        });
    }

    private handleMarkActioned = (reminder: IReminder) => {
        this.setState({
            ...this.state,
            snackbarMessage: `Reminder "${reminder.title}" was marked as actioned.`,
            snackbarOpen: true
        });
    }
}

export default App;