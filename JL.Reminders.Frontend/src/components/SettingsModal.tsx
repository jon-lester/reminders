import * as Mui from '@material-ui/core';
import * as React from 'react';

import IUrgencyConfiguration from '../model/IUrgencyConfiguration';

interface ISettingsModalState {
    urgencyConfiguration: IUrgencyConfiguration;
}

interface ISettingsModalProps {
    onCancel: () => void;
    onSave: (settings: IUrgencyConfiguration) => void;
    open: boolean;
    urgencyConfiguration: IUrgencyConfiguration;
}

class SettingsModal extends React.Component<ISettingsModalProps, ISettingsModalState> {

    constructor(props: ISettingsModalProps) {
        super(props);

        this.state = {
            urgencyConfiguration: { ...this.props.urgencyConfiguration }
        }
    }

    public render() {
        return (
            <Mui.Dialog open={this.props.open}>
                <Mui.DialogTitle>
                    Settings
                </Mui.DialogTitle>
                <Mui.DialogContent>
                    <Mui.TextField
                        id="imminent-setting"
                        onChange={this.handleValueChange}
                        label="How soon is 'imminent' in days?"
                        fullWidth={true}
                        autoFocus={true}
                        defaultValue={this.props.urgencyConfiguration.imminentDays}
                    />
                    <Mui.TextField
                        id="soon-setting"
                        onChange={this.handleValueChange}
                        label="How soon is 'soon' in days?"
                        fullWidth={true}
                        defaultValue={this.props.urgencyConfiguration.soonDays}
                    />
                </Mui.DialogContent>
                <Mui.DialogActions>
                    <Mui.Button onClick={this.props.onCancel}>Cancel</Mui.Button>
                    <Mui.Button onClick={this.saveSettings}>Save</Mui.Button>
                </Mui.DialogActions>
            </Mui.Dialog>
        );
    }

    private readonly handleValueChange = (event: React.ChangeEvent<HTMLInputElement>) => {

        // must cache these values as React may re-use the
        // SyntheticEvent before setState below has run
        const fieldId = event.currentTarget.id;
        const newValue = Number(event.currentTarget.value);

        this.setState((state: ISettingsModalState) => {

            const newState = {
                urgencyConfiguration: {...state.urgencyConfiguration}
            };

            switch (fieldId) {
                case 'imminent-setting': {
                    newState.urgencyConfiguration.imminentDays = newValue;
                    break;
                }
                case 'soon-setting': {
                    newState.urgencyConfiguration.soonDays = newValue;
                    break;
                }
            }

            return newState;
        });
    }

    private readonly saveSettings = (event: React.MouseEvent<HTMLElement>) => {
        this.props.onSave({
            // TODO: get the actual values from the form instead of the
            // current values from the user's config
            imminentDays: this.state.urgencyConfiguration.imminentDays,
            soonDays: this.state.urgencyConfiguration.soonDays
        });
    }
}

export default SettingsModal;