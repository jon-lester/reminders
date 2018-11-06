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

interface IAddReminderFABProps extends Mui.WithStyles<typeof styles> {
    onAddReminder?: () => void;
}

/**
 * Render a Floating Action Button allowing the user to add a new reminder
 * in a fixed position at bottom-right of the UI.
 */
class AddReminderFAB extends React.PureComponent<IAddReminderFABProps> {
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

export default withStyles(styles)(AddReminderFAB);