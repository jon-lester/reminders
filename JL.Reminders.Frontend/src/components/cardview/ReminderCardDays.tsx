import * as Mui from '@material-ui/core';
import * as React from 'react';

import blue from '@material-ui/core/colors/blue';
import orange from '@material-ui/core/colors/orange';
import red from '@material-ui/core/colors/red';
import yellow from '@material-ui/core/colors/yellow';

import { createStyles, withStyles } from '@material-ui/core/styles';

import Urgency from '../../model/Urgency';

const styles = () => {

    const time = {
        color: 'rgba(0, 0, 0, 0.54)',
        fontSize: '7rem'
    };

    return createStyles({
        time,
        timeImminent: {
            ...time,
            color: orange[400]
        },
        timeNow: {
            ...time,
            color: blue[400]
        },
        timeOverdue: {
            ...time,
            color: red[400]
        },
        timeSoon: {
            ...time,
            color: yellow[400] // dubious.. kind of hard to read.. needs something different
        }
    });
}


interface IReminderCardDaysProps extends Mui.WithStyles<typeof styles> {
    days: number;
    urgency: Urgency;
}

/**
 * Render the large formatted Days To Go value used in the centre of a ReminderCard.
 */
class ReminderCardDays extends React.PureComponent<IReminderCardDaysProps> {
    public render() {
        return (
            <Mui.Typography
                className = {this.getClass()}
                align = "center"
                variant = "h1">
                {this.props.days}
            </Mui.Typography>
        )
    }

    private readonly getClass = (): string => {
        switch (this.props.urgency) {
            case Urgency.Imminent: return this.props.classes.timeImminent;
            case Urgency.Now: return this.props.classes.timeNow;
            case Urgency.Soon: return this.props.classes.timeSoon;
            case Urgency.Overdue: return this.props.classes.timeOverdue;
            default: return this.props.classes.time;
        }
    }
}

export default withStyles(styles)(ReminderCardDays);