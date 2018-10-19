import * as Mui from '@material-ui/core/';
import * as React from 'react';

import blue from '@material-ui/core/colors/blue';
import orange from '@material-ui/core/colors/orange';
import red from '@material-ui/core/colors/red';
import yellow from '@material-ui/core/colors/yellow';

import { createStyles, withStyles } from '@material-ui/core/styles';
import MoreVertIcon from '@material-ui/icons/MoreVert'

import IMenuItem from '../../model/IMenuItem';
import IReminder from '../../model/IReminder';
import Urgency from '../../model/Urgency';
import ReminderAppMenu from '../ReminderAppMenu';
import ReminderCardDays from './ReminderCardDays';

const styles = () => {

    const card = {
        width: 250
    };

    return createStyles({
        card,
        cardImminent: {
            ...card,
            backgroundColor: orange[50]
        },
        cardNow: {
            ...card,
            backgroundColor: blue[50] // 'rgba(0, 0, 255, 0.1)'
        },
        cardOverdue: {
            ...card,
            backgroundColor: red[50]
        },
        cardSoon: {
            ...card,
            backgroundColor: yellow[50]
        },
    });
};

interface IReminderCardProps extends Mui.WithStyles<typeof styles> {
    imminentDays: number;
    onMarkActioned?: (reminder: IReminder) => void;
    onMarkArchived?: (reminder: IReminder) => void;
    reminder: IReminder;
    soonDays: number;

}

interface IReminderCardState {
    menuElement: HTMLElement | undefined;
    menuOpen: boolean;
}

class ReminderCard extends React.Component<IReminderCardProps & Mui.WithStyles<typeof styles>, IReminderCardState> {

    private menuItems : IMenuItem[];

    constructor(props: any) {
        super(props);

        this.state = {
            menuElement: undefined,
            menuOpen: false
        }

        this.menuItems = [{
            action: () => this.props.onMarkActioned
                ? this.props.onMarkActioned(this.props.reminder)
                : {},
            text: 'Mark Actioned'
        }, {
            action: () => this.props.onMarkArchived
                ? this.props.onMarkArchived(this.props.reminder)
                : {},
            text: 'Archive'
        }];
    }

    public render() {

        const formattedReminder = this.getFormattedReminderProp();

        return (
            <div>
            <Mui.Card>
                <Mui.CardHeader
                    className={this.getCardClass()}
                    title = {formattedReminder.title ? formattedReminder.title : 'Untitled'}
                    subheader = {formattedReminder.subTitle}
                    action={<Mui.IconButton onClick={this.handleMenuOpen}>
                                <MoreVertIcon />
                            </Mui.IconButton>}/>
                <Mui.CardContent>
                    <ReminderCardDays
                        days={this.props.reminder.daysToGo}
                        urgency={this.getUrgency()}
                    />
                </Mui.CardContent>
            </Mui.Card>
            <ReminderAppMenu open={this.state.menuOpen} onClosed={this.handleMenuClosed} menuItems={this.menuItems} anchorEl={this.state.menuElement} />
            </div>
        );
    }

    private readonly getUrgency = (): Urgency => {
        if (this.props.reminder.daysToGo < 0) {
            return Urgency.Overdue;
        }

        if (this.props.reminder.daysToGo === 0) {
            return Urgency.Now;
        }

        if (this.props.reminder.daysToGo <= this.props.imminentDays) {
            return Urgency.Imminent;
        }

        if (this.props.reminder.daysToGo <= this.props.soonDays) {
            return Urgency.Soon;
        }

        return Urgency.Normal;
    }

    private readonly getCardClass = (): string => {
        switch (this.getUrgency()) {
            case Urgency.Imminent: return this.props.classes.cardImminent;
            case Urgency.Now: return this.props.classes.cardNow;
            case Urgency.Soon: return this.props.classes.cardSoon;
            case Urgency.Overdue: return this.props.classes.cardOverdue;
            default: return this.props.classes.card;
        }
    }

    private readonly getFormattedReminderProp = (): IReminder => {
        return {
            created: this.props.reminder.created,
            daysToGo: this.props.reminder.daysToGo,
            description: this.props.reminder.description,
            forDate: this.props.reminder.forDate,
            id: this.props.reminder.id,
            imminentDaysPreference: this.props.reminder.imminentDaysPreference,
            importance: this.props.reminder.importance,
            lastActioned: this.props.reminder.lastActioned,
            nextDueDate: this.props.reminder.nextDueDate,
            recurrence: this.props.reminder.recurrence,
            soonDaysPreference: this.props.reminder.soonDaysPreference,
            subTitle: this.props.reminder.subTitle,
            title: this.props.reminder.title
                ? (this.props.reminder.title.length > 16
                    ? this.props.reminder.title.substr(0, 14) + '..'
                    : this.props.reminder.title)
                : 'Untitled'
        };
    }

    /**
     * Handle the vert-icon button being clicked to open the card's menu.
     */
    private readonly handleMenuOpen = (evt: React.SyntheticEvent<HTMLElement>) => {
        this.setState({
            ...this.state,
            menuElement: evt.currentTarget,
            menuOpen: true
        });
    }

    /**
     * Handle the vert-icon's currently-open menu needing to close.
     */
    private readonly handleMenuClosed = () => {
        this.setState({
            ...this.state,
            menuElement: undefined,
            menuOpen: false
        });
    }
}

export default withStyles(styles)(ReminderCard);