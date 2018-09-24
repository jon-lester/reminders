import blue from '@material-ui/core/colors/blue';
import orange from '@material-ui/core/colors/orange';
import red from '@material-ui/core/colors/red';
import yellow from '@material-ui/core/colors/yellow';

import * as Mui from '@material-ui/core';
import { createStyles, withStyles } from '@material-ui/core/styles';
import * as React from 'react';

import Urgency from '../model/Urgency';

const styles = () => createStyles({
    time: {
    },
    timeImminent: {
        color: orange[400]
    },
    timeNow: {
        color: blue[400]
    },
    timeOverdue: {
        color: red[400]
    },
    timeSoon: {
        color: yellow[400]
    }
});

interface IReminderCardDaysProps extends Mui.WithStyles<typeof styles> {
    days: number;
    urgency: Urgency;
}

class ReminderCardDays extends React.PureComponent<IReminderCardDaysProps> {
    public render() {
        return (
            <Mui.Typography
                className = {this.getClass()}
                align = "center"
                variant = "display4">
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