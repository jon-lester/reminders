import CssBaseline from '@material-ui/core/CssBaseline';
import * as React from 'react';
import { Route, Switch } from 'react-router-dom';

import { IWithAuthServiceProps, withAuthService } from './services/AuthService';

import AuthCallbackComponent from './components/AuthCallbackComponent';
import HomeView from './components/HomeView';
import ReminderApp from './components/ReminderApp';
import ReminderAppMenuBar from './components/ReminderAppMenuBar';

import IMenuItem from './model/IMenuItem';

class App extends React.Component<IWithAuthServiceProps> {

    private readonly menuItems: IMenuItem[];

    constructor(props: any) {
        super(props);

        // set up items for the main menu.
        this.menuItems = [
            {
                action: () => console.log('action 2'),
                id: 2,
                text: 'About'
            }];
    }

    public render() {
        return (
            <React.Fragment>
                <CssBaseline />
                <ReminderAppMenuBar
                    menuItems={this.menuItems}
                    onLogin={this.props.onAuthenticate}
                    onLogout={this.props.onLogout}
                    isLoggedIn={this.props.onCheckAuthenticated()}
                />
                <Switch>
                    <Route path="/callback" component={AuthCallbackComponent} />
                    { this.props.onCheckAuthenticated() &&
                        <Route path="/app" component={ReminderApp} />
                    }
                    <Route component={HomeView} />
                </Switch>
            </React.Fragment>
        );
    }
}

export default withAuthService(App);