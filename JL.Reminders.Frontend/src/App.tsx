import CssBaseline from '@material-ui/core/CssBaseline';
import Snackbar from '@material-ui/core/Snackbar';
import Typography from '@material-ui/core/Typography';
import * as React from 'react';

import AddReminderModal from './components/AddReminderModal';
import { IWithApiProps, withApi } from './components/ApiComponent';
import ReminderAppMenuBar from './components/ReminderAppMenuBar';
import ReminderCardContainer from './components/ReminderCardContainer';

import IAddReminderRequest from './model/IAddReminderRequest';
import IMenuItem from './model/IMenuItem';
import IReminder from './model/IReminder';

export interface IAppState {
    addDialogOpen: boolean;
    reminders: IReminder[];
    snackbarMessage: string | undefined;
    snackbarOpen: boolean;
}

class App extends React.Component<{} & IWithApiProps, IAppState> {

    private readonly menuItems: IMenuItem[];

    constructor(props: any) {
        super(props);
        this.state = {
            addDialogOpen: false,
            reminders: [],
            snackbarMessage: undefined,
            snackbarOpen: false
        };

        // set up items for the main menu.
        this.menuItems = [
            {
                action: () => {
                    this.setState({
                        ...this.state,
                        addDialogOpen: true
                    })
                },
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
                    onMarkArchived={this.handleMarkArchived}
                />
                <AddReminderModal
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

    /**
     * When the app component mounts, load all reminders from the API.
     */
    public componentDidMount() {
        this.refreshAllReminders();
    }

    private readonly refreshAllReminders = () => {
        this.props.onGetAllReminders().then(reminders => this.setState({reminders}));
    }

    /**
     * Handle the snackbar toast requesting that it be closed.
     */
    private readonly handleSnackbarClosed = () => {
        this.setState({
            ...this.state,
            snackbarOpen: false
        });
    }

    /**
     * Handle the add-reminder modal requesting that it be closed.
     */
    private readonly handleAddReminderDialogClosed = () => {
        this.setState({
            ...this.state,
            addDialogOpen: false
        });
    }

    /**
     * Handle the user having requested to save a new reminder.
     */
    private readonly handleAddReminderDialogSave = (saveRequest: IAddReminderRequest) => {
        this.props.onAddReminder(saveRequest)
            .then((id: number) => {
                console.log(`Saved new reminder with id = ${id}`);
                this.refreshAllReminders()
            });
        this.handleAddReminderDialogClosed();
    }

    /**
     * Handle the user having requested to mark an existing reminder as actioned.
     */
    private readonly handleMarkActioned = (reminder: IReminder) =>
        this.showToast(`${reminder.title} was marked as actioned.`);

    /**
     * Handle the user having requested to archive an existing reminder.
     */
    private readonly handleMarkArchived = (reminder: IReminder) =>
        this.showToast(`${reminder.title} was archived.`);

    /**
     * Show a toast message via the SnackBar component.
     */
    private readonly showToast = (message: string) => {
        this.setState({
            ...this.state,
            snackbarMessage: message,
            snackbarOpen: true
        });
    }
}

export default withApi(App);