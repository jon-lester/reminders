import * as Mui from '@material-ui/core/';
import * as React from 'react';

import { createStyles, withStyles } from '@material-ui/core/styles';
import MoreVertIcon from '@material-ui/icons/MoreVert'

import IMenuItem from '../model/IMenuItem';
import IReminder from '../model/IReminder';
import ReminderAppMenu from './ReminderAppMenu';

const styles = () => createStyles({
    card: {
        minWidth: 250
    },
    time: {
        marginBottom: 30
    }
});

interface IReminderCardProps extends Mui.WithStyles<typeof styles> {
    onMarkActioned?: (reminder: IReminder) => void,
    onMarkArchived?: (reminder: IReminder) => void,
    reminder: IReminder,
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
            id: 1,
            text: 'Mark Actioned'
        }, {
            action: () => this.props.onMarkArchived
                ? this.props.onMarkArchived(this.props.reminder)
                : {},
            id: 2,
            text: 'Archive'
        }];
    }

    public render() {

        const formattedReminder = this.getFormattedReminderProp();

        return (
            <div>
            <Mui.Card className={this.props.classes.card}>
                <Mui.CardHeader
                    title = {formattedReminder.title ? formattedReminder.title : 'Untitled'}
                    action={<Mui.IconButton onClick={this.handleMenuOpen}>
                                <MoreVertIcon />
                            </Mui.IconButton>}/>
                <Mui.CardContent>
                <Mui.Typography
                    className = {this.props.classes.time}
                    align = "center"
                    variant = "display4">
                    {formattedReminder.daysToGo}
                </Mui.Typography>
                <Mui.Typography component="p">
                    {formattedReminder.description}
                </Mui.Typography>
                </Mui.CardContent>
            </Mui.Card>
            <ReminderAppMenu open={this.state.menuOpen} onClosed={this.handleMenuClosed} menuItems={this.menuItems} anchorEl={this.state.menuElement} />
            </div>
        );
    }

    private readonly getFormattedReminderProp = (): IReminder => {
        return {
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