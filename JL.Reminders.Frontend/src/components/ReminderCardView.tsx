import * as Mui from '@material-ui/core/';
import { createStyles, withStyles } from '@material-ui/core/styles';
import AddIcon from '@material-ui/icons/Add';
import * as React from 'react';

import ReminderCard from './ReminderCard';
import ReminderFAB from './ReminderFAB';

import IReminder from '../model/IReminder';

const styles = (theme: Mui.Theme) => createStyles({
    addIcon: {
        backgroundColor: theme.palette.secondary.main,
        borderRadius: "0.75em",
        color: 'white',
        height: '1.5em',
        padding: '0.3em',
        position: 'relative',
        top: '0.3em',
        width: '1.5em'
    }
});

interface IReminderCardViewProps extends Mui.WithStyles<typeof styles> {
    reminders: IReminder[];
    onAddReminder?: () => void;
    onMarkActioned?: (reminder: IReminder) => void;
    onMarkArchived?: (reminder: IReminder) => void;
}

class ReminderCardView extends React.Component<IReminderCardViewProps> {

    constructor(props: IReminderCardViewProps) {
        super(props);
    }

    public render() {

        const cards = [];

        for(const reminder of this.props.reminders) {
            cards.push(
                <Mui.Grid key={reminder.id} item={true}>
                    <ReminderCard
                        onMarkActioned={this.props.onMarkActioned}
                        onMarkArchived={this.props.onMarkArchived}
                        reminder={reminder} />
                </Mui.Grid>
            );
        }

        return (
            <>
                {this.props.reminders.length ? (
                    <Mui.Grid container={true} spacing={24}>{cards}</Mui.Grid>
                ) : (<>
                    <Mui.Typography align="center" variant="display1">You have no reminders!</Mui.Typography>,
                    <Mui.Typography align="center" variant="display1">Click <AddIcon className={this.props.classes.addIcon} /> below to get started.</Mui.Typography>
                    </>
                )}
                <ReminderFAB
                    onAddReminder={this.props.onAddReminder}
                />
            </>
        );
    }
}

export default withStyles(styles)(ReminderCardView);