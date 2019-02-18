import * as Mui from '@material-ui/core';
import * as React from 'react';

import red from '@material-ui/core/colors/red';
import { createStyles, withStyles, WithStyles } from '@material-ui/core/styles';
import ErrorIcon from '@material-ui/icons/ErrorOutlineRounded'

const styles = (theme: Mui.Theme) => createStyles({
    errorIcon: {
        color: red[500],
        height: 100,
        width: 100
    }
});

/**
 * Render a view that shows the user an error message explaining
 * that the API could not be contacted.
 */
class ErrorView extends React.PureComponent<WithStyles<typeof styles>> {
    public render() {
        return (
            <Mui.Grid
                container={true}
                spacing={24}
                direction="column"
                justify="center"
                alignItems="center">
                <Mui.Grid item={true}>
                    <ErrorIcon
                        className={this.props.classes.errorIcon}
                    />
                </Mui.Grid>
                <Mui.Grid item={true}>
                    <Mui.Typography
                        variant="h4">
                        Server Error!
                    </Mui.Typography>
                </Mui.Grid>
                <Mui.Grid item={true}>
                    <Mui.Typography>
                        Sorry - something went wrong. Your reminders are safe,
                        but the server isn't responding right now. Please try
                        again later.
                    </Mui.Typography>
                </Mui.Grid>
            </Mui.Grid>
        );
    }
}

export default withStyles(styles)(ErrorView);