import * as Mui from '@material-ui/core';
import { createStyles, withStyles } from '@material-ui/core/styles';
import * as React from 'react';

const styles = () => createStyles({
    homePaper: {
        display: 'table',
        margin: 'auto',
        marginTop: 250,
        padding: 15,
        textAlign: 'center'
    },
    newPara: {
        marginTop: '2em'
    }
});

class HomePage extends React.PureComponent<Mui.WithStyles<typeof styles>> {
    public render() {
        return (
            <Mui.Paper className={this.props.classes.homePaper}>
                <Mui.Typography variant="display3">
                    Reminders
                </Mui.Typography>
                <hr />
                <Mui.Typography variant="caption">
                    Version {process.env.REACT_APP_VERSION}
                </Mui.Typography>
            </Mui.Paper>
        );
    }
}

export default withStyles(styles)(HomePage);