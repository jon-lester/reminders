import * as React from 'react';

import IAddReminderRequest from '../model/IAddReminderRequest';
import IReminder from '../model/IReminder';

export interface IWithApiProps {
    onGetAllReminders: () => Promise<IReminder[]>;
    onAddReminder: (addReminderRequest: IAddReminderRequest) => Promise<number>;
}

export const withApi = <P extends object>(Component: React.ComponentType<P & IWithApiProps>) => {

    return class ApiComponent extends React.Component<P & IWithApiProps> {

        private static readonly API_BASE_URL = 'http://localhost:49900/';
        // private static readonly API_BASE_URL = 'https://2d410672-d82b-4642-918f-d96db1e140e1.mock.pstmn.io/';

        constructor(props: P & IWithApiProps) {
            super(props);
        }

        public render() {
            return <Component
                onGetAllReminders={this.getAllReminders}
                onAddReminder={this.addReminder}
                {...this.props}/>;
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
            .then(json => 0);
        }

        private makeUri(endpoint: string) {
            return `${ApiComponent.API_BASE_URL}${endpoint}`;
        }
    };
}