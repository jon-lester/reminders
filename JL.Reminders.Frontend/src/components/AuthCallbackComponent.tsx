import * as React from 'react';

import { withAuthService, WithAuthServiceProps } from '../services/AuthService';

/**
 * Renders nothing; provides a means to handle an HTTP callback directed to a specific route.
 */
class AuthCallbackComponent extends React.Component<WithAuthServiceProps> {

    public componentDidMount() {
        this.props.auth.onHandleCallback();
    }

    public render() {
        return null;
    }
}

export default withAuthService(AuthCallbackComponent);