import * as Mui from '@material-ui/core';
import { createStyles, withStyles } from '@material-ui/core/styles';
import * as React from 'react';
import { Redirect } from 'react-router';

import { withAuthService, WithAuthServiceProps } from '../services/AuthService';

import ViewWrapper from './ViewWrapper';

const styles = () => createStyles({
    homePaper: {
        display: 'table',
        margin: 'auto',
        marginTop: '3em',
        padding: 15,
        textAlign: 'center'
    },
    newPara: {
        marginTop: '2em'
    }
});

/**
 * Render a view that shows the user some explanation of what they're looking
 * at before they're authenticated and logged in.
 */
class HomeView extends React.PureComponent<Mui.WithStyles<typeof styles> & WithAuthServiceProps> {
    public render() {
        return (
            !this.props.auth.onCheckAuthenticated() ? (
            <ViewWrapper>
                <Mui.Paper className={this.props.classes.homePaper}>
                    <Mui.Typography variant="display3">
                        Reminders
                    </Mui.Typography>
                    <hr />
                    <Mui.Typography variant="caption">
                        Version {process.env.REACT_APP_VERSION}
                    </Mui.Typography>
                </Mui.Paper>
            </ViewWrapper>) : (
                <Redirect to="/app" />
            )
        );
    }
}

export default withAuthService(withStyles(styles)(HomeView));