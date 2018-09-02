import CssBaseline from '@material-ui/core/CssBaseline';
import Snackbar from '@material-ui/core/Snackbar';
import Typography from '@material-ui/core/Typography';
import * as React from 'react';

import AddReminderDialogComponent from './components/AddReminderDialog';
import ReminderAppMenuBar from './components/ReminderAppMenuBar';
import ReminderCardContainer from './components/ReminderCardContainer';

import IAddReminderRequest from './interfaces/IAddReminderRequest';
import IMenuItem from './interfaces/IMenuItem';
import IReminder from './interfaces/IReminder';

export interface IAppState {
    addDialogOpen: boolean;
    reminders: IReminder[];
    snackbarMessage: string | undefined;
    snackbarOpen: boolean;
}

class App extends React.Component<any, IAppState> {

    private readonly menuItems: IMenuItem[];

    constructor(props: any) {
        super(props);
        this.state = {
            addDialogOpen: false,
            reminders: [],
            snackbarMessage: undefined,
            snackbarOpen: false
        };

        this.menuItems = [
            {
                action: this.handleAddNew,
                id: 1,
                text: 'Add new reminder'
            }, {
                action: () => console.log('action 2'),
                id: 2,
                text: 'About'
            }];
    }

    public render() {
        return (
            <React.Fragment>
                <CssBaseline />
                <ReminderAppMenuBar menuItems={this.menuItems} />
                <ReminderCardContainer
                    reminders={this.state.reminders}
                    onMarkActioned={this.handleMarkActioned}
                />
                <AddReminderDialogComponent
                    onClose={this.handleAddReminderDialogClosed}
                    onSave={this.handleAddReminderDialogSave}
                    open={this.state.addDialogOpen} />
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

    private handleAddReminderDialogClosed = () => {
        this.setState({
            ...this.state,
            addDialogOpen: false
        });
    }

    private handleAddReminderDialogSave = (saveRequest: IAddReminderRequest) => {
        console.log(saveRequest);
        this.handleAddReminderDialogClosed();
    }

    private handleMarkActioned = (reminder: IReminder) => {
        this.setState({
            ...this.state,
            snackbarMessage: `Reminder "${reminder.title}" was marked as actioned.`,
            snackbarOpen: true
        });
    }

    private handleAddNew = () => {
        this.setState({
            ...this.state,
            addDialogOpen: true
        })
    }
}

export default App;