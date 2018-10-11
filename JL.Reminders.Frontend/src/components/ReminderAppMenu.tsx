import * as Mui from '@material-ui/core';
import * as React from 'react';

import IMenuItem from '../model/IMenuItem';

interface IReminderAppMenuProps {
    anchorEl: HTMLElement | undefined, 
    menuItems: IMenuItem[],
    onClosed: () => void,
    open: boolean
};

class ReminderAppMenu extends React.Component<IReminderAppMenuProps> {

    constructor(props: IReminderAppMenuProps) {
        super(props);
    }

    public render() {

        const menuItems = [];

        let id = 0;

        for (const item of this.props.menuItems) {
            menuItems.push(
                <Mui.MenuItem
                    key={id++}
                    onClick={this.handleMenuItemClick(item.action)}>
                    {item.text}
                </Mui.MenuItem>
            );
        }

        return (
            <Mui.Menu
                id="simple-menu"
                anchorEl={this.props.anchorEl}
                open={Boolean(this.props.open)}
                onClose={this.props.onClosed}>
                {menuItems}
            </Mui.Menu>
        );
    }

    /**
     * Handle one of the menu items being clicked - run its
     * action then close the menu.
     */
    private readonly handleMenuItemClick = (menuActionFunction: () => void) => (evt: any)  => {
        menuActionFunction();
        // close the menu after calling the action
        this.props.onClosed();
    }
}

export default ReminderAppMenu;