import * as Mui from '@material-ui/core/';
import { createStyles, withStyles } from '@material-ui/core/styles';
import * as React from 'react';

import ReminderCard from './ReminderCard';
import ReminderFAB from './ReminderFAB';

import IReminder from '../model/IReminder';

const styles = () => createStyles({
    wrappingDiv: {
        marginTop: 72,
        padding: 12
    }
});

interface IReminderCardContainerProps extends Mui.WithStyles<typeof styles> {
    reminders: IReminder[];
    onAddReminder?: () => void;
    onMarkActioned?: (reminder: IReminder) => void;
    onMarkArchived?: (reminder: IReminder) => void;
}

class ReminderCardContainer extends React.Component<IReminderCardContainerProps> {

    constructor(props: IReminderCardContainerProps) {
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
            <div className={this.props.classes.wrappingDiv}>
                {this.props.reminders.length ? (
                    <Mui.Grid container={true} spacing={24}>{cards}</Mui.Grid>
                ) : (
                    <Mui.Typography variant="caption">Loading reminders...</Mui.Typography>
                )}
                <ReminderFAB
                    onAddReminder={this.props.onAddReminder}/>
            </div>
        );
    }
}

export default withStyles(styles)(ReminderCardContainer);