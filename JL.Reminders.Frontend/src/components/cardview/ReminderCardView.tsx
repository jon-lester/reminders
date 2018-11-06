import * as Mui from '@material-ui/core/';
import * as React from 'react';

import { createStyles, withStyles } from '@material-ui/core/styles';
import AddIcon from '@material-ui/icons/Add';

import IReminder from '../../model/IReminder';
import AddReminderFAB from './AddReminderFAB';
import ReminderCard from './ReminderCard';

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
    soonDays: number;
    imminentDays: number;
}

/**
 * Render a Grid of ReminderCards, including a FAB so that new cards can be added.
 * If no cards exist, show an introductory message instead.
 */
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
                        reminder={reminder}
                        imminentDays={this.props.imminentDays}
                        soonDays={this.props.soonDays}
                    />
                </Mui.Grid>
            );
        }

        return (
            <>
                {this.props.reminders.length ? (
                    <Mui.Grid container={true} spacing={24}>{cards}</Mui.Grid>
                ) : (<>
                        <Mui.Typography
                            align="center"
                            variant="display1">
                            You have no reminders!
                        </Mui.Typography>
                        <Mui.Typography
                            align="center"
                            variant="display1">
                            Click the <AddIcon className={this.props.classes.addIcon} /> button to get started.
                        </Mui.Typography>
                    </>
                )}
                <AddReminderFAB
                    onAddReminder={this.props.onAddReminder}
                />
            </>
        );
    }
}

export default withStyles(styles)(ReminderCardView);