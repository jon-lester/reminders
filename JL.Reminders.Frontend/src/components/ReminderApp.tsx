import Snackbar from '@material-ui/core/Snackbar';
import Typography from '@material-ui/core/Typography';
import * as React from 'react';

import { IWithApiProps, withApi } from '../services/ApiService';

import AddReminderModal from './AddReminderModal';
import ReminderCardContainer from './ReminderCardContainer';

import IAddReminderRequest from '../model/IAddReminderRequest';
import IReminder from '../model/IReminder';
import IReminderOptions from '../model/IReminderOptions';

export interface IAppState {
    addDialogOpen: boolean;
    reminders: IReminder[];
    reminderOptions: IReminderOptions;
    snackbarMessage: string | undefined;
    snackbarOpen: boolean;
}

class ReminderApp extends React.Component<{} & IWithApiProps, IAppState> {

    constructor(props: any) {
        super(props);
        this.state = {
            addDialogOpen: false,
            reminderOptions: {
                importances: {},
                recurrences: {}
            },
            reminders: [],
            snackbarMessage: undefined,
            snackbarOpen: false
        };
    }

    public render() {
        return (
            <React.Fragment>
                <ReminderCardContainer
                    reminders={this.state.reminders}
                    onMarkActioned={this.handleMarkActioned}
                    onMarkArchived={this.handleMarkArchived}
                    onAddReminder={this.handleAddReminder}
                />
                {!this.state.addDialogOpen ||
                <AddReminderModal
                    importanceOptions={this.state.reminderOptions.importances}
                    occurrenceOptions={this.state.reminderOptions.recurrences}
                    onClose={this.handleAddReminderDialogClosed}
                    onSave={this.handleAddReminderDialogSave}
                    open={this.state.addDialogOpen}
                />}
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
     * When the app component mounts, load options and all reminders from the API.
     */
    public componentDidMount() {

        this.props.onGetOptions()
            .then(options => {
                this.setState({...this.state, reminderOptions: options});
            })
            // tslint:disable-next-line:no-console
            .catch(reason => console.log(reason));

        this.refreshAllReminders();
    }

    private readonly refreshAllReminders = () => {
        this.props.onGetAllReminders()
            .then(reminders => this.setState({reminders: reminders.sort((a, b) => a.daysToGo - b.daysToGo)}))
            // tslint:disable-next-line:no-console
            .catch(reason => console.log(reason));
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
     * Handle the user having clicked to add a new reminder.
     */
    private readonly handleAddReminder = () => {
        this.setState({
            ...this.state,
            addDialogOpen: true
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
    private readonly handleMarkActioned = (reminder: IReminder) => {

        this.props.onActionReminder({
            notes: '',
            reminderId: reminder.id
        }).then(success => {
            this.showToast(`${reminder.title} was marked as actioned.`);
            this.refreshAllReminders();
        });
    }

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

export default withApi(ReminderApp);