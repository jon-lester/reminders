import * as React from 'react';

import { IWithAuthServiceProps, withAuthService } from '../services/AuthService';

class AuthCallbackComponent extends React.Component<IWithAuthServiceProps> {

    public componentDidMount() {
        this.props.onHandleCallback();
    }

    public render() {
        return null;
    }
}

export default withAuthService(AuthCallbackComponent);