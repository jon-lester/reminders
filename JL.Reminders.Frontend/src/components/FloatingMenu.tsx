import * as Mui from '@material-ui/core';
import * as React from 'react';

import IMenuItem from '../model/IMenuItem';

interface IFloatingMenuProps {
    anchorEl: HTMLElement | undefined, 
    menuItems: IMenuItem[],
    onClosed: () => void,
    open: boolean
};

/**
 * Render a floating menu anchored to a given element to be used
 * as a drop-down / pop-up. (See IMenuItem.)
 */
class FloatingMenu extends React.Component<IFloatingMenuProps> {

    constructor(props: IFloatingMenuProps) {
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
     * Handle one of the menu items having been clicked - run its
     * action then close the menu.
     */
    private readonly handleMenuItemClick = (menuActionFunction: () => void) => (evt: any)  => {
        menuActionFunction();
        this.props.onClosed();
    }
}

export default FloatingMenu;