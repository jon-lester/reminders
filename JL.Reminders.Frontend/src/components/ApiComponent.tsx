import * as React from 'react';

import IReminder from '../model/IReminder';

export interface IWithApiProps {
    onGetAllReminders: () => Promise<IReminder[]>
}

export const withApi = <P extends object>(Component: React.ComponentType<P & IWithApiProps>) => {

    return class ApiComponent extends React.Component<P & IWithApiProps> {

        constructor(props: P & IWithApiProps) {
            super(props);
        }

        public render() {
            return <Component
                onGetAllReminders={this.getAllReminders}
                {...this.props}/>;
        }

        private getAllReminders = () => {
            const uri = 'https://2d410672-d82b-4642-918f-d96db1e140e1.mock.pstmn.io/api/reminders/';
            // const uri = 'http://localhost:49900/api/reminders/';
            return fetch(uri)
                .then(response => response.json())
                .then(json => {
                    return json;
                });
        }
    };
}