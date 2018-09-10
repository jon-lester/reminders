import * as Mui from '@material-ui/core';
import { createStyles, withStyles } from '@material-ui/core/styles';
import * as React from 'react';

const styles = () => createStyles({
    time: {
    },
    timeImminent: {
        color: 'orange'
    },
    timeOverdue: {
        color: 'red'
    },
    timeSoon: {
        color: 'yellow'
    }
});

interface IReminderCardDaysProps extends Mui.WithStyles<typeof styles> {
    days: number;
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

    private readonly getClass = () => {
        
        if (this.props.days < 0) {
            return this.props.classes.timeOverdue;
        }

        if (this.props.days <= 7) {
            return this.props.classes.timeImminent;
        }

        if (this.props.days <= 30) {
            return this.props.classes.timeSoon;
        }

        return this.props.classes.time;
    }
}

export default withStyles(styles)(ReminderCardDays);