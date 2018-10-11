import * as React from 'react';

import { createStyles, withStyles, WithStyles } from '@material-ui/core/styles';

const styles = () => createStyles({
    viewContainer: {
        marginTop: 72,
        padding: 12
    }
});

class ViewWrapper extends React.PureComponent<WithStyles<typeof styles>> {
    public render() {
        return (
            <div className={this.props.classes.viewContainer}>
                {this.props.children}
            </div>
        );
    }
}

export default withStyles(styles)(ViewWrapper);