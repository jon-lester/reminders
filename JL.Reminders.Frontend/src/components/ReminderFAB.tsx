import * as React from 'react';

import * as Mui from '@material-ui/core';
import { createStyles, withStyles } from '@material-ui/core/styles';
import AddIcon from '@material-ui/icons/Add';

const styles = () => createStyles({
    fab: {
        bottom: 50,
        position: 'fixed',
        right: 50
    }
});

interface IReminderFABProps extends Mui.WithStyles<typeof styles> {
    onAddReminder?: () => void;
}

class ReminderFAB extends React.PureComponent<IReminderFABProps> {
    public render() {
        return (
            <Mui.Tooltip
                title="Add a new reminder"
                enterDelay={700}
                leaveDelay={200}>
                <Mui.Button
                    variant="fab"
                    color="secondary"
                    className={this.props.classes.fab}
                    onClick={this.handleAddReminderClick}>
                    <AddIcon />
                </Mui.Button>
            </Mui.Tooltip>
        );
    }

    private readonly handleAddReminderClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        if (typeof(this.props.onAddReminder)  === 'function') {
            this.props.onAddReminder();
        }
    }
}

export default withStyles(styles)(ReminderFAB);