import CssBaseline from '@material-ui/core/CssBaseline';
import * as React from 'react';
import { Route, Switch } from 'react-router-dom';

import { withAuthService, WithAuthServiceProps } from './services/AuthService';

import AboutModal from './components/AboutModal';
import AuthCallbackComponent from './components/AuthCallbackComponent';
import HomeView from './components/HomeView';
import ReminderApp from './components/ReminderApp';
import ReminderAppMenuBar from './components/ReminderAppMenuBar';

import IMenuItem from './model/IMenuItem';

interface IAppState {
    aboutModalOpen: boolean;
    menuItems: IMenuItem[];
}

class App extends React.Component<WithAuthServiceProps, IAppState> {

    private readonly aboutMenuItem: IMenuItem = {
        action: () => {
            this.setState({
                ...this.state,
                aboutModalOpen: true
            });
        },
        text: 'About'
    }

    constructor(props: WithAuthServiceProps<{}>) {
        super(props);

        this.state = {
            aboutModalOpen: false,
            menuItems: [this.aboutMenuItem]
        };
    }

    public render() {
        return (
            <>
                <CssBaseline />
                <ReminderAppMenuBar
                    menuItems={this.state.menuItems}
                    onLogin={this.props.auth.onAuthenticate}
                    onLogout={this.props.auth.onLogout}
                    isLoggedIn={this.props.auth.onCheckAuthenticated()}
                />
                <Switch>
                    <Route path="/callback" component={AuthCallbackComponent} />
                    { this.props.auth.onCheckAuthenticated() &&
                        <Route path="/app">
                            <ReminderApp onConfigureAppMenu={this.configureMenu} />
                        </Route>
                    }
                    <Route component={HomeView} />
                </Switch>
                <AboutModal
                    open={this.state.aboutModalOpen}
                    onClose={this.handleAboutModalClosed}
                />
            </>
        );
    }

    /**
     * Handle the user having closed the About modal.
     */
    private readonly handleAboutModalClosed = () => {
        this.setState({
            ...this.state,
            aboutModalOpen: false
        });
    }

    private readonly configureMenu = (menuItems: IMenuItem[]) => {
        this.setState({
            menuItems: [...menuItems, this.aboutMenuItem]
        });
    }
}

export default withAuthService(App);