import * as React from 'react';

import IAddReminderActionRequest from '../model/IAddReminderActionRequest';
import IAddReminderRequest from '../model/IAddReminderRequest';
import IReminder from '../model/IReminder';
import IReminderOptions from '../model/IReminderOptions';
import IUserPreferences from '../model/IUserPreferences';

import { withAuthService, WithAuthServiceProps } from '../services/AuthService';

interface IInjectedApiProps {
    api: {
        onActionReminder: (addReminderActionRequest: IAddReminderActionRequest) => Promise<boolean>;
        onAddReminder: (addReminderRequest: IAddReminderRequest) => Promise<number>;
        onArchiveReminder: (reminderId: number) => Promise<boolean>;
        onGetAllReminders: () => Promise<IReminder[]>;
        onGetOptions: () => Promise<IReminderOptions>;
        onGetUserSettings: () => Promise<IUserPreferences>;
        onSaveUserSettings: (userSettings: IUserPreferences) => Promise<boolean>;
        onUnarchiveReminder: (reminderId: number) => Promise<boolean>;
    }
}

export type WithApiServiceProps<P = {}> = P & IInjectedApiProps;

export const withApi = <P extends {}>(Component: React.ComponentType<WithApiServiceProps<P>>) => {

    return withAuthService(
        class ApiService extends React.Component<WithAuthServiceProps<P>> {

            private static readonly API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

            private static reminderOptions: IReminderOptions | undefined = undefined;

            constructor(props: WithAuthServiceProps<P>) {
                super(props);
            }

            public render() {
                const apiProp = {
                    onActionReminder: this.actionReminder,
                    onAddReminder: this.addReminder,
                    onArchiveReminder: this.archiveReminder,
                    onGetAllReminders: this.getAllReminders,
                    onGetOptions: this.getOptions,
                    onGetUserSettings: this.getUserSettings,
                    onSaveUserSettings: this.saveUserSettings,
                    onUnarchiveReminder: this.unarchiveReminder
                }

                const { api, ...props } = this.props as any;

                return (
                    <Component
                        {...props}
                        api={apiProp}
                    />
                );
            }

            private readonly makeHeaders = (withJsonContentType: boolean = false): Headers => {
                const headers = new Headers();

                const accessToken = this.props.auth.onGetAccessToken();

                if (accessToken) {
                    headers.append('Authorization', `Bearer ${accessToken}`);
                }

                if (withJsonContentType) {
                    headers.append('Content-Type', 'application/json');
                }

                return headers;
            }

            private readonly getUserSettings = () => {
                return fetch(this.makeUri('api/preferences'), {
                    headers: this.makeHeaders()
                })
                .then(response => response.json())
                .then((json: IUserPreferences) => json);
            }

            private readonly saveUserSettings = (userSettings: IUserPreferences) => {
                return fetch(this.makeUri('api/preferences'), {
                    // right now the api only supports urgency config..
                    // change this PATCH later if/when more settings exist
                    body: JSON.stringify(userSettings.urgencyConfiguration),
                    headers: this.makeHeaders(true),
                    method: 'PATCH'
                })
                .then(response => true);
            }

            private readonly getOptions = () => {
                if (ApiService.reminderOptions) {
                    return Promise.resolve(ApiService.reminderOptions);
                } else {
                    return fetch(this.makeUri('api/reminders/options'), {
                        headers: this.makeHeaders()
                    })
                    .then(response => response.json())
                    .then(json => json);
                }
            }

            private readonly getAllReminders = () => {
                return fetch(this.makeUri('api/reminders'), {
                    headers: this.makeHeaders()
                })
                .then(response => response.json())
                .then(json => json);
            }

            private readonly addReminder = (addReminderRequest: IAddReminderRequest): Promise<number> => {

                // todo - handle errors, and get the id for the new record
                // from the location header to return from this function
                return fetch(this.makeUri('api/reminders'), {
                    body: JSON.stringify(addReminderRequest),
                    headers: this.makeHeaders(true),
                    method: 'POST'
                })
                .then(response => 0); // todo - extract the id from the location header
            }

            private readonly actionReminder = (addReminderActionRequest: IAddReminderActionRequest): Promise<boolean> => {

                // todo - handle errors, and get the id for the new record
                // from the location header to return from this function
                return fetch(this.makeUri(`api/reminders/${addReminderActionRequest.reminderId}/actions`), {
                    body: JSON.stringify(addReminderActionRequest),
                    headers: this.makeHeaders(true),
                    method: 'POST'
                })
                .then(response => true); // todo - extract whether or not success from return code
            }

            private readonly archiveReminder = (reminderId: number): Promise<boolean> => {
                return fetch(this.makeUri(`api/reminders/${reminderId}`), {
                    body: JSON.stringify({
                        status: 1
                    }),
                    headers: this.makeHeaders(true),
                    method: 'PATCH'
                })
                .then(response => true);
            }

            private readonly unarchiveReminder = (reminderId: number): Promise<boolean> => {
                return fetch(this.makeUri(`api/reminders/${reminderId}`), {
                    body: JSON.stringify({
                        status: 0
                    }),
                    headers: this.makeHeaders(true),
                    method: 'PATCH'
                })
                .then(response => true);
            }

            private makeUri(endpoint: string) {
                return `${ApiService.API_BASE_URL}${endpoint}`;
            }
        }
    ) as React.ComponentType<P>;
}