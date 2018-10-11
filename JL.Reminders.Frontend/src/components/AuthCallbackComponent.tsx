import * as React from 'react';

import { withAuthService, WithAuthServiceProps } from '../services/AuthService';

class AuthCallbackComponent extends React.Component<WithAuthServiceProps> {

    public componentDidMount() {
        this.props.auth.onHandleCallback();
    }

    public render() {
        return null;
    }
}

export default withAuthService(AuthCallbackComponent);