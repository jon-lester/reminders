import CssBaseline from '@material-ui/core/CssBaseline';
import * as React from 'react';
import { Route, Switch } from 'react-router-dom';

import { IWithAuthServiceProps, withAuthService } from './services/AuthService';

import AboutModal from './components/AboutModal';
import AuthCallbackComponent from './components/AuthCallbackComponent';
import HomeView from './components/HomeView';
import ReminderApp from './components/ReminderApp';
import ReminderAppMenuBar from './components/ReminderAppMenuBar';

import IMenuItem from './model/IMenuItem';

interface IAppState {
    aboutModalOpen: boolean;
}

class App extends React.Component<IWithAuthServiceProps, IAppState> {

    private readonly menuItems: IMenuItem[];

    constructor(props: any) {
        super(props);

        this.state = {
            aboutModalOpen: false
        };

        // set up items for the main menu.
        this.menuItems = [
            {
                action: () => {
                    this.setState({
                        ...this.state,
                        aboutModalOpen: true
                    });
                },
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
                <AboutModal
                    open={this.state.aboutModalOpen}
                    onClose={this.handleAboutModalClosed}
                />
            </React.Fragment>
        );
    }

    /**
     * Handle the user having close the About modal.
     */
    private readonly handleAboutModalClosed = () => {
        this.setState({
            ...this.state,
            aboutModalOpen: false
        });
    }
}

export default withAuthService(App);