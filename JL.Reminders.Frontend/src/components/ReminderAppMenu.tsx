import * as Mui from '@material-ui/core';
import * as React from 'react';

import IMenuItem from '../interfaces/IMenuItem';

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

        for (const item of this.props.menuItems) {
            menuItems.push(
                <Mui.MenuItem
                    key={item.id}
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

    private handleMenuItemClick = (func: () => void) => (evt: any)  => {
        func();
        this.props.onClosed();
    }
}

export default ReminderAppMenu;