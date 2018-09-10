import * as React from 'react';

import IAddReminderActionRequest from '../model/IAddReminderActionRequest';
import IAddReminderRequest from '../model/IAddReminderRequest';
import IReminder from '../model/IReminder';
import IReminderOptions from '../model/IReminderOptions';

export interface IWithApiProps {
    onGetOptions: () => Promise<IReminderOptions>;
    onGetAllReminders: () => Promise<IReminder[]>;
    onAddReminder: (addReminderRequest: IAddReminderRequest) => Promise<number>;
    onActionReminder: (addReminderActionRequest: IAddReminderActionRequest) => Promise<boolean>;
}

export const withApi = <P extends object>(Component: React.ComponentType<P & IWithApiProps>) => {

    return class ApiComponent extends React.Component<P & IWithApiProps> {

        private static readonly API_BASE_URL = 'http://localhost:49900/';
        // private static readonly API_BASE_URL = 'https://2d410672-d82b-4642-918f-d96db1e140e1.mock.pstmn.io/';

        private static reminderOptions: IReminderOptions | undefined = undefined;

        constructor(props: P & IWithApiProps) {
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

        private getOptions = () => {
            if (ApiComponent.reminderOptions) {
                return Promise.resolve(ApiComponent.reminderOptions);
            } else {
                console.log('Making API call for options..');
                return fetch(this.makeUri('api/reminders/options'))
                .then(response => response.json())
                .then(json => json);
            }
        }

        private getAllReminders = () => {
            return fetch(this.makeUri('api/reminders'))
                .then(response => response.json())
                .then(json => {
                    return json;
                });
        }

        private addReminder = (addReminderRequest: IAddReminderRequest): Promise<number> => {

            const headers = new Headers();
            headers.append('Content-Type', 'application/json');

            // todo - handle errors, and get the id for the new record
            // from the location header to return from this function
            return fetch(this.makeUri('api/reminders'), {
                body: JSON.stringify(addReminderRequest),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                method: 'POST'
            })
            .then(response => response.json)
            .then(json => 0); // todo - extract the id from the location header
        }

        private actionReminder = (addReminderActionRequest: IAddReminderActionRequest): Promise<boolean> => {
            const headers = new Headers();
            headers.append('Content-Type', 'application/json');

            // todo - handle errors, and get the id for the new record
            // from the location header to return from this function
            return fetch(this.makeUri(`api/reminders/${addReminderActionRequest.reminderId}/actions`), {
                body: JSON.stringify(addReminderActionRequest),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                method: 'POST'
            })
            .then(response => response.json)
            .then(json => true); // todo - extract whether or not success from return code
        }

        private makeUri(endpoint: string) {
            return `${ApiComponent.API_BASE_URL}${endpoint}`;
        }
    };
}