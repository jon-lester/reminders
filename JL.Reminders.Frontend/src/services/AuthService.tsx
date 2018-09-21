import * as Auth0 from 'auth0-js';
import * as React from 'react';

import { RouteComponentProps, withRouter } from 'react-router';

export interface IWithAuthServiceProps {
    onAuthenticate: () => {};
    onHandleCallback: () => {};
    onLogout: () => {};
    onCheckAuthenticated: () => boolean;
    onGetAccessToken: () => string | undefined;
}

export const withAuthService = <P extends object>(Component: React.ComponentType<P & IWithAuthServiceProps>) =>
    withRouter(class AuthService extends React.Component<P & IWithAuthServiceProps & RouteComponentProps> {

        private readonly auth = new Auth0.WebAuth({
            audience: process.env.REACT_APP_AUTH_API_AUDIENCE || '',
            clientID: process.env.REACT_APP_AUTH_CLIENT_ID || '',
            domain: process.env.REACT_APP_AUTH_AUTHORITY_DOMAIN || '',
            redirectUri: process.env.REACT_APP_AUTH_CALLBACK || '',
            responseType: 'token id_token',
            scope: 'openid email'
        });

        public render() {
            return (
                <Component
                onAuthenticate={this.authenticate}
                onHandleCallback={this.handleCallback}
                onLogout={this.logout}
                onCheckAuthenticated={this.isAuthenticated}
                onGetAccessToken={this.getAccessToken}
                {...this.props} />
            );
        }

        private readonly authenticate = (): void => {
            this.auth.authorize();
        }

        private readonly handleCallback = (): void => {
            this.auth.parseHash((err, authResult) => {
                if (authResult && authResult.accessToken && authResult.idToken) {
                    this.setSession(authResult);
                    this.props.history.replace('/app');
                } else if (err) {
                    // tslint:disable-next-line:no-console
                    console.log(err);
                    this.props.history.replace('/');
                }
            });
        }

        private readonly setSession = (authResult: Auth0.Auth0DecodedHash): void => {

            if (authResult.expiresIn && authResult.accessToken && authResult.idToken) {
                // Set the time that the Access Token will expire at
                const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
                localStorage.setItem('access_token', authResult.accessToken);
                localStorage.setItem('id_token', authResult.idToken);
                localStorage.setItem('expires_at', expiresAt);
                // navigate to the home route
                this.props.history.replace('/app');
            }
        }

        private readonly getAccessToken = (): string | undefined => {
            const accessToken = localStorage.getItem('access_token');

            if (accessToken) {
                return accessToken;
            } else {
                return undefined;
            }
        }

        private readonly logout = (): void => {
            // Clear Access Token and ID Token from local storage
            localStorage.removeItem('access_token');
            localStorage.removeItem('id_token');
            localStorage.removeItem('expires_at');
            // navigate to the home route
            this.props.history.replace('/');
        }
        
        private readonly isAuthenticated = (): boolean => {
            // Check whether the current time is past the 
            // Access Token's expiry time
            const expiry = localStorage.getItem('expires_at');

            if (!expiry) {
                return false;
            }

            return new Date().getTime() < JSON.parse(expiry);
        }
    });