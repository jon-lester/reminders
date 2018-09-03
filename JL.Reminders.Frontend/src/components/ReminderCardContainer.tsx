import * as Mui from '@material-ui/core/';
import * as React from 'react';

import ReminderCard from './ReminderCard';

import IReminder from '../model/IReminder';

export interface IReminderCardContainerProps {
    reminders: IReminder[];
    onMarkActioned?: (reminder: IReminder) => void;
    onMarkArchived?: (reminder: IReminder) => void;
}

class ReminderCardContainer extends React.Component<IReminderCardContainerProps> {

    constructor(props: IReminderCardContainerProps) {
        super(props);
    }

    public render() {

        const container: boolean = true;
        const item: boolean = true;

        const cards = [];

        for(const reminder of this.props.reminders) {
            cards.push(
                <Mui.Grid key={reminder.id} item={item}>
                    <ReminderCard
                        onMarkActioned={this.props.onMarkActioned}
                        onMarkArchived={this.props.onMarkArchived}
                        reminder={reminder} />
                </Mui.Grid>
            );
        }

        return (
            <div style={{ padding: 12, marginTop: 72 }}>
                <Mui.Grid container={container} spacing={24}>{cards}</Mui.Grid>
            </div>
        );
    }
}

export default ReminderCardContainer;