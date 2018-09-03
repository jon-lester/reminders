import * as React from 'react';

import IReminder from '../model/IReminder';

export interface IApiComponentProps {
    getAllReminders: () => IReminder[]
}

const withApi = <P extends object>(Component: React.ComponentType<P>) => {

    return class ApiComponent extends React.Component<P & IApiComponentProps> {

        constructor(props: any) {
            super(props);
        }

        public render() {
            return <Component {...this.props}/>;
        }
    };
}

export default withApi;