import * as Mui from '@material-ui/core';
import * as React from 'react';

/**
 * Render a view which contains a wait/loading spinner.
 */
class LoadingView extends React.PureComponent<{}> {
    public render() {
        return (
            <Mui.Grid
                container={true}
                direction="row" 
                justify="center"
                alignItems="center">
                <Mui.CircularProgress variant="indeterminate" />
            </Mui.Grid>
        );
    }
}

export default LoadingView;