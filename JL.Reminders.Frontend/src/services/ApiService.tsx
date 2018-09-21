import * as React from 'react';

import IAddReminderActionRequest from '../model/IAddReminderActionRequest';
import IAddReminderRequest from '../model/IAddReminderRequest';
import IReminder from '../model/IReminder';
import IReminderOptions from '../model/IReminderOptions';

import { IWithAuthServiceProps, withAuthService } from '../services/AuthService';

export interface IWithApiProps {
    onGetOptions: () => Promise<IReminderOptions>;
    onGetAllReminders: () => Promise<IReminder[]>;
    onAddReminder: (addReminderRequest: IAddReminderRequest) => Promise<number>;
    onActionReminder: (addReminderActionRequest: IAddReminderActionRequest) => Promise<boolean>;
}

export const withApi = <P extends object>(Component: React.ComponentType<P & IWithApiProps>) => {

    return withAuthService(class ApiService extends
        React.Component<P & IWithApiProps & IWithAuthServiceProps> {

        private static readonly API_BASE_URL = process.env.REACT_APP_API_BASE_URL;
        // private static readonly API_BASE_URL = 'https://2d410672-d82b-4642-918f-d96db1e140e1.mock.pstmn.io/';

        private static reminderOptions: IReminderOptions | undefined = undefined;

        constructor(props: P & IWithApiProps & IWithAuthServiceProps) {
            super(props);
        }

        public render() {
            return <Component
                onGetOptions={this.getOptions}
                onGetAllReminders={this.getAllReminders}
                onAddReminder={this.addReminder}
                onActionReminder={this.actionReminder}
                {...this.props}/>;
        }

        private readonly makeHeaders = (withJsonContentType: boolean = false): Headers => {
            const headers = new Headers();

            const accessToken = this.props.onGetAccessToken();

            if (accessToken) {
                headers.append('Authorization', `Bearer ${accessToken}`);
            }

            if (withJsonContentType) {
                headers.append('Content-Type', 'application/json');
            }

            return headers;
        }

        private readonly getOptions = () => {
            if (ApiService.reminderOptions) {
                return Promise.resolve(ApiService.reminderOptions);
            } else {
                console.log('Making API call for options..');
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
            .then(json => {
                return json;
            });
        }

        private readonly addReminder = (addReminderRequest: IAddReminderRequest): Promise<number> => {

            // todo - handle errors, and get the id for the new record
            // from the location header to return from this function
            return fetch(this.makeUri('api/reminders'), {
                body: JSON.stringify(addReminderRequest),
                headers: this.makeHeaders(true),
                method: 'POST'
            })
            .then(response => response.json)
            .then(json => 0); // todo - extract the id from the location header
        }

        private readonly actionReminder = (addReminderActionRequest: IAddReminderActionRequest): Promise<boolean> => {

            // todo - handle errors, and get the id for the new record
            // from the location header to return from this function
            return fetch(this.makeUri(`api/reminders/${addReminderActionRequest.reminderId}/actions`), {
                body: JSON.stringify(addReminderActionRequest),
                headers: this.makeHeaders(true),
                method: 'POST'
            })
            .then(response => response.json)
            .then(json => true); // todo - extract whether or not success from return code
        }

        private makeUri(endpoint: string) {
            return `${ApiService.API_BASE_URL}${endpoint}`;
        }
    });
}