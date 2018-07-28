import * as Mui from '@material-ui/core/';
import * as React from 'react';

import IReminder from '../interfaces/IReminder';


export interface IReminderCardProps {
    reminder: IReminder
}

class ReminderCard extends React.Component<IReminderCardProps> {

    constructor(props: IReminderCardProps) {
        super(props);
    }

    public render() {
        return (
            <Mui.Card>
                <Mui.CardHeader
                    title = {this.props.reminder.title}
                    subheader = {this.props.reminder.description}/>
                <Mui.CardContent>
                    <Mui.Typography
                        align = "center"
                        variant = "display4">
                        {this.props.reminder.daysToGo}
                    </Mui.Typography>
                </Mui.CardContent>
                <Mui.CardActions>
                    <Mui.Button>
                        Mark Actioned
                    </Mui.Button>
                </Mui.CardActions>
            </Mui.Card>);
    }
}

export default ReminderCard;