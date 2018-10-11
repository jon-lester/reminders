import Snackbar from '@material-ui/core/Snackbar';
import Typography from '@material-ui/core/Typography';
import * as React from 'react';

import { withApi, WithApiServiceProps } from '../services/ApiService';

import AddReminderModal from './AddReminderModal';
import ErrorView from './ErrorView';
import LoadingView from './LoadingView';
import ReminderCardView from './ReminderCardView';
import SettingsModal from './SettingsModal';
import ViewWrapper from './ViewWrapper';

import AppState from '../model/AppState';
import IAddReminderRequest from '../model/IAddReminderRequest';
import IMenuItem from '../model/IMenuItem';
import IReminder from '../model/IReminder';
import IReminderOptions from '../model/IReminderOptions';
import IUrgencyConfiguration from '../model/IUrgencyConfiguration';
import IUserPreferences from '../model/IUserPreferences';

interface IReminderAppState {
    addReminderModalOpen: boolean;
    appState: AppState;
    reminders: IReminder[];
    reminderOptions: IReminderOptions;
    settingsModalOpen: boolean;
    snackbarMessage: string | undefined;
    snackbarOpen: boolean;
    userSettings: IUserPreferences;
}

interface IReminderAppProps {
    onConfigureAppMenu: (menuItems: IMenuItem[]) => void;
}

class ReminderApp extends React.Component<WithApiServiceProps<IReminderAppProps>, IReminderAppState> {

    constructor(props: IReminderAppProps) {
        super(props as any);
        this.state = {
            addReminderModalOpen: false,
            appState: AppState.Loading,
            reminderOptions: {
                importances: {},
                recurrences: {}
            },
            reminders: [],
            settingsModalOpen: false,
            snackbarMessage: undefined,
            snackbarOpen: false,
            userSettings: {
                urgencyConfiguration: {
                    imminentDays: 7,
                    soonDays: 30
                }
            }
        };

        this.props.onConfigureAppMenu([{
            action: () => {
                this.setState({
                    settingsModalOpen: true
                });
            },
            text: "Settings"
        }]);
    }

    public render() {

        let view;
        switch (this.state.appState) {
            case AppState.Loading:
                view = <LoadingView />
                break;
            case AppState.Loaded:
                view = <ReminderCardView
                            reminders={this.state.reminders}
                            onMarkActioned={this.handleMarkActioned}
                            onMarkArchived={this.handleMarkArchived}
                            onAddReminder={this.handleAddReminder}
                            imminentDays={this.state.userSettings.urgencyConfiguration.imminentDays}
                            soonDays={this.state.userSettings.urgencyConfiguration.soonDays}
                        />
                break;
            case AppState.Error:
                view = <ErrorView />
        }

        return (
            <React.Fragment>
                <ViewWrapper>
                    {view}
                </ViewWrapper>
                {!this.state.addReminderModalOpen ||
                <AddReminderModal
                    importanceOptions={this.state.reminderOptions.importances}
                    occurrenceOptions={this.state.reminderOptions.recurrences}
                    onClose={this.handleAddReminderModalClosed}
                    onSave={this.handleAddReminderModalSave}
                    open={this.state.addReminderModalOpen}
                />}
                {!this.state.settingsModalOpen ||
                <SettingsModal
                    open={this.state.settingsModalOpen}
                    onSave={this.handleSettingsModalSave}
                    onCancel={this.handleSettingsModalCancel}
                    urgencyConfiguration={this.state.userSettings.urgencyConfiguration}
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
     * When the app component mounts, load options and all reminders from the API,
     * then set Loaded state.
     */
    public componentDidMount() {

        Promise.all([
            this.props.api.onGetOptions(),
            this.props.api.onGetUserSettings(),
            this.props.api.onGetAllReminders()
        ])
        .then(results => {
            this.setState({
                ...this.state,
                appState: AppState.Loaded,
                reminderOptions: results[0],
                reminders: results[2].sort((a, b) => a.daysToGo - b.daysToGo),
                userSettings: results[1]
            });
        })
        .catch(reason => {
            console.log(reason);
            this.setState({
                ...this.state,
                appState: AppState.Error
            });
        });

        this.props.api.onGetOptions()
            .then((options) => {
                return Promise.all([options, this.props.api.onGetAllReminders()])
            })
            .then(results => {
                this.setState({
                    ...this.state,
                    appState: AppState.Loaded,
                    reminderOptions: results[0],
                    reminders: results[1].sort((a, b) => a.daysToGo - b.daysToGo)
                });
            })
            // tslint:disable-next-line:no-console
            .catch(reason => {
                console.log(reason);
                this.setState({
                    ...this.state,
                    appState: AppState.Error
                });
            });

        // this.refreshAllReminders();
    }

    private readonly refreshAllReminders = () => {
        this.props.api.onGetAllReminders()
            .then((reminders: IReminder[]) => this.setState({reminders: reminders.sort((a: IReminder, b: IReminder) => a.daysToGo - b.daysToGo)}))
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
            addReminderModalOpen: true
        });
    }

    /**
     * Handle the add-reminder modal requesting that it be closed.
     */
    private readonly handleAddReminderModalClosed = () => {
        this.setState({
            ...this.state,
            addReminderModalOpen: false
        });
    }

    /**
     * Handle the user having requested to save a new reminder.
     */
    private readonly handleAddReminderModalSave = (saveRequest: IAddReminderRequest) => {
        this.props.api.onAddReminder(saveRequest)
            .then((id: number) => {
                this.showToast(`${saveRequest.title} was added.`);
                this.refreshAllReminders();
            });
        this.handleAddReminderModalClosed();
    }

    private readonly handleSettingsModalSave = (urgencyConfiguration: IUrgencyConfiguration) => {
        this.props.api.onSaveUserSettings({ urgencyConfiguration })
            .then(result => {
                this.setState({
                    settingsModalOpen: false,
                    userSettings: { urgencyConfiguration }
                });
            });
    }

    private readonly handleSettingsModalCancel = () => {
        this.setState({
            settingsModalOpen: false
        });
    }

    /**
     * Handle the user having requested to mark an existing reminder as actioned.
     */
    private readonly handleMarkActioned = (reminder: IReminder) => {

        this.props.api.onActionReminder({
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
    private readonly handleMarkArchived = (reminder: IReminder) => {
        this.props.api.onArchiveReminder(reminder.id)
        .then(success => {
            this.showToast(`${reminder.title} was archived.`);
            this.refreshAllReminders();
        });
    }

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