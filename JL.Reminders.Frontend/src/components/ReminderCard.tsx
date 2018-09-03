import * as Mui from '@material-ui/core/';
import * as React from 'react';

import IReminder from '../model/IReminder';

export interface IReminderCardProps {
    onMarkActioned: (reminder: IReminder) => void,
    reminder: IReminder,
}

class ReminderCard extends React.Component<IReminderCardProps> {

    constructor(props: IReminderCardProps) {
        super(props);
    }

    public render() {

        const cleanCardContent : IReminder = {
            created: this.props.reminder.created,
            daysToGo: this.props.reminder.daysToGo,
            description: this.props.reminder.description,
            forDate: this.props.reminder.forDate,
            id: this.props.reminder.id,
            importance: this.props.reminder.importance,
            lastActioned: this.props.reminder.lastActioned,
            recurrence: this.props.reminder.recurrence,
            title: this.props.reminder.title
                ? (this.props.reminder.title.length > 16
                    ? this.props.reminder.title.substr(0, 14) + '..'
                    : this.props.reminder.title)
                : 'Untitled'
        };

        return (
            <Mui.Card>
                <Mui.CardHeader
                    title = {cleanCardContent.title ? cleanCardContent.title : 'Untitled'}
                    subheader = {cleanCardContent.description}/>
                <Mui.CardContent>
                    <Mui.Typography
                        align = "center"
                        variant = "display4">
                        {cleanCardContent.daysToGo}
                    </Mui.Typography>
                </Mui.CardContent>
                <Mui.CardActions>
                    <Mui.Button onClick={this.markActionedClickHandler}>
                        Mark Actioned
                    </Mui.Button>
                </Mui.CardActions>
            </Mui.Card>
        );
    }

    private readonly markActionedClickHandler = () => {
        this.props.onMarkActioned(this.props.reminder);
    }
}

export default ReminderCard;