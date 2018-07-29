import * as Mui from '@material-ui/core/';
import { withStyles } from '@material-ui/core/styles';
import MenuIcon from '@material-ui/icons/Menu';
import * as React from 'react';

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

interface IReminderAppMenuBarProps extends Mui.WithStyles<typeof styles> { }

class ReminderAppMenuBar extends React.Component<IReminderAppMenuBarProps> {

    public render() {
        return (
            <Mui.AppBar>
                <Mui.Toolbar color="white">
                <Mui.IconButton className={this.props.classes.menuButton} color="inherit" aria-label="Menu">
                    <MenuIcon />
                </Mui.IconButton>
                <Mui.Typography className={this.props.classes.flex} variant="title" color="inherit">Reminders</Mui.Typography>
                </Mui.Toolbar>
            </Mui.AppBar>
        );
    }
}

export default withStyles(styles)(ReminderAppMenuBar);