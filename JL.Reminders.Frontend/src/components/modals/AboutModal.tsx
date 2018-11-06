import * as Mui from '@material-ui/core';
import * as React from 'react';

interface IAboutModalProps {
    open: boolean;
    onClose: () => void;
}

/**
 * Render a modal dialog to display version/author info.
 */
class AboutModal extends React.PureComponent<IAboutModalProps> {
    public render() {
        return (
            <Mui.Dialog
                disableBackdropClick={false}
                disableEscapeKeyDown={false}
                open={this.props.open}
                onClose={this.props.onClose}>
                <Mui.DialogTitle>Reminders</Mui.DialogTitle>
                <Mui.DialogContent>
                    <Mui.DialogContentText>Version {process.env.REACT_APP_VERSION}</Mui.DialogContentText>
                    <Mui.DialogContentText>by Jon Lester</Mui.DialogContentText>
                </Mui.DialogContent>
                <Mui.DialogActions>
                    <Mui.Button
                        color="primary"
                        onClick={this.props.onClose}>
                        Ok
                    </Mui.Button>
                </Mui.DialogActions>
            </Mui.Dialog>
        );
    }
}

export default AboutModal;