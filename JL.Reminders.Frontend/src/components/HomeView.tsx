import * as Mui from '@material-ui/core';
import { createStyles, withStyles } from '@material-ui/core/styles';
import * as React from 'react';
import { Redirect } from 'react-router';

import ViewContainer from './ViewContainer';

import { IWithAuthServiceProps, withAuthService } from '../services/AuthService';

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

class HomeView extends React.PureComponent<Mui.WithStyles<typeof styles> & IWithAuthServiceProps> {
    public render() {
        return (
            !this.props.onCheckAuthenticated() ? (
            <ViewContainer>
                <Mui.Paper className={this.props.classes.homePaper}>
                    <Mui.Typography variant="display3">
                        Reminders
                    </Mui.Typography>
                    <hr />
                    <Mui.Typography variant="caption">
                        Version {process.env.REACT_APP_VERSION}
                    </Mui.Typography>
                </Mui.Paper>
            </ViewContainer>) : (
                <Redirect to="/app" />
            )
        );
    }
}

export default withAuthService(withStyles(styles)(HomeView));