import * as Mui from '@material-ui/core/';
import { withStyles } from '@material-ui/core/styles';
import MenuIcon from '@material-ui/icons/Menu';
import * as React from 'react';

import ReminderAppMenu from './ReminderAppMenu';

import IMenuItem from '../model/IMenuItem';

const styles = (theme: Mui.Theme) => Mui.createStyles({
    flex: {
        flexGrow: 1,
    },
    menuButton: {
        marginLeft: -12,
        marginRight: 20,
    },
    root: {
        flexGrow: 1,
    }
});

interface IReminderAppMenuBarProps extends Mui.WithStyles<typeof styles> {
    menuItems: IMenuItem[]
}

interface IReminderAppMenuBarState {
    anchorEl: HTMLElement | undefined,
    menuOpen: boolean
}

class ReminderAppMenuBar extends React.Component<IReminderAppMenuBarProps, IReminderAppMenuBarState> {

    constructor(props: IReminderAppMenuBarProps) {
        super(props);

        this.state = {
            anchorEl: undefined,
            menuOpen: false
        };
    }

    public render() {

        return (
            <Mui.AppBar>
                <Mui.Toolbar color="white">
                <Mui.IconButton
                    className={this.props.classes.menuButton}
                    color="inherit"
                    aria-label="Menu"
                    onClick={this.handleOpenMenuClick}
                    >
                    <MenuIcon />
                </Mui.IconButton>
                <Mui.Typography
                    className={this.props.classes.flex}
                    variant="title"
                    color="inherit">
                    Reminders
                </Mui.Typography>
                </Mui.Toolbar>
                <ReminderAppMenu
                    anchorEl={this.state.anchorEl}
                    menuItems={this.props.menuItems}
                    open={this.state.menuOpen}
                    onClosed={this.handleMenuOnClose}
                />
            </Mui.AppBar>
        );
    }

    /**
     * Handle the user having clicked the menu button to
     * open the menu.
     */
    private readonly handleOpenMenuClick = (event: React.SyntheticEvent<HTMLElement>) => {
        this.setState({
            anchorEl: event.currentTarget,
            menuOpen: true
        });
    }

    /**
     * Handle the currently-open menu needing to close.
     */
    private readonly handleMenuOnClose = () => {
        this.setState({
            anchorEl: undefined,
            menuOpen: false
        });
    }
}

export default withStyles(styles)(ReminderAppMenuBar);