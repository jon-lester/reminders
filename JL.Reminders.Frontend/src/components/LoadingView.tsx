import * as Mui from '@material-ui/core';
import * as React from 'react';

class LoadingView extends React.PureComponent<{}> {
    public render() {
        return (
            <Mui.Grid container={true} direction="row" justify="center" alignItems="center">
                <Mui.CircularProgress variant="indeterminate" />
            </Mui.Grid>
        );
    }
}

export default LoadingView;