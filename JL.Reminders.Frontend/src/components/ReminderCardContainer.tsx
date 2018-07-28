import * as Mui from '@material-ui/core/';
import * as React from 'react';

import IReminder from '../interfaces/IReminder';
import ReminderCard from './ReminderCard';

export interface IReminderCardContainerProps {
    reminders: IReminder[]
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
                    <ReminderCard reminder={reminder} />
                </Mui.Grid>
            );
        }

        return (
            <div style={{ padding: 12, marginTop: 72 }}>
                <Mui.Grid container={container} spacing={24}>{cards}</Mui.Grid>
            </div>);
    }
}

export default ReminderCardContainer;