import * as React from 'react';

import { createStyles, withStyles, WithStyles } from '@material-ui/core/styles';

interface IViewContainerProps extends WithStyles<typeof styles> {}

const styles = () => createStyles({
    viewContainer: {
        marginTop: 72,
        padding: 12
    }
});

class ViewContainer extends React.PureComponent<IViewContainerProps> {
    public render() {
        return (
            <div className={this.props.classes.viewContainer}>
                {this.props.children}
            </div>
        );
    }
}

export default withStyles(styles)(ViewContainer);