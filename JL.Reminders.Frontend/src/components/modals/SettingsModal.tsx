import * as Mui from '@material-ui/core';
import red from '@material-ui/core/colors/red';
import { createStyles, withStyles } from '@material-ui/core/styles/'
import * as React from 'react';

import IUrgencyConfiguration from '../../model/IUrgencyConfiguration';
import TextInputControl from './TextInputControl';

interface ISettingsModalState {
    imminentDaysErrorMessage: string | null;
    imminentDaysValid: boolean;
    soonDaysErrorMessage: string | null;
    soonDaysValid: boolean;
    urgencyConfiguration: IUrgencyConfiguration;
}

interface ISettingsModalProps {
    onCancel: () => void;
    onSave: (settings: IUrgencyConfiguration) => void;
    open: boolean;
    urgencyConfiguration: IUrgencyConfiguration;
}

const styles = (theme: Mui.Theme) => createStyles({
    errorText: {
        color: red[500]
    }
});

class SettingsModal extends React.Component<ISettingsModalProps & Mui.WithStyles<typeof styles>, ISettingsModalState> {

    constructor(props: ISettingsModalProps & Mui.WithStyles<typeof styles>) {
        super(props);

        this.state = {
            imminentDaysErrorMessage: null,
            imminentDaysValid: true,
            soonDaysErrorMessage: null,
            soonDaysValid: true,
            urgencyConfiguration: { ...this.props.urgencyConfiguration },
        }
    }

    public render() {
        return (
            <Mui.Dialog open={this.props.open}>
                <Mui.DialogTitle>
                    Settings
                </Mui.DialogTitle>
                <Mui.DialogContent>
                    <TextInputControl
                        id="soon"
                        autoFocus={false}
                        defaultValue={this.props.urgencyConfiguration.soonDays.toString()}
                        errorMessage={this.state.soonDaysErrorMessage}
                        onChange={this.handleChange}
                        label="Display reminders as 'soon' when they are due within:"
                        valid={this.state.soonDaysValid}
                        endAdornment="days"
                    />
                    <TextInputControl
                        id="imminent"
                        autoFocus={true}
                        defaultValue={this.props.urgencyConfiguration.imminentDays.toString()}
                        errorMessage={this.state.imminentDaysErrorMessage}
                        onChange={this.handleChange}
                        label="Display reminders as 'imminent' when they are due within:"
                        valid={this.state.imminentDaysValid}
                        endAdornment="days"
                    />
                </Mui.DialogContent>
                <Mui.DialogActions>
                    <Mui.Button
                        onClick={this.props.onCancel}>
                        Cancel
                    </Mui.Button>
                    <Mui.Button
                        disabled={this.formIsInvalid()}
                        onClick={this.saveSettings}>
                        Save
                    </Mui.Button>
                </Mui.DialogActions>
            </Mui.Dialog>
        );
    }

    private readonly isValidDayRange = (value: number) => !isNaN(value) && Number.isInteger(value) && value > 0 && value <= 365;

    private readonly formIsInvalid = (): boolean => !this.state.imminentDaysValid || !this.state.soonDaysValid;

    private readonly validate = () => {

        let imminentDaysValid = this.isValidDayRange(this.state.urgencyConfiguration.imminentDays);
        let soonDaysValid = this.isValidDayRange(this.state.urgencyConfiguration.soonDays);

        let imminentDaysErrorMessage = imminentDaysValid ? null : 'Must be a value from 1 to 365.';
        let soonDaysErrorMessage = soonDaysValid ? null : 'Must be a value from 1 to 365.';

        if (this.state.urgencyConfiguration.imminentDays >= this.state.urgencyConfiguration.soonDays) {
            imminentDaysValid = false;
            soonDaysValid = false;
            imminentDaysErrorMessage = 'Must be lower than the value for soon.'
            soonDaysErrorMessage = 'Must be higher than the value for imminent.'
        }

        this.setState((state) => ({
            imminentDaysErrorMessage,
            imminentDaysValid,
            soonDaysErrorMessage,
            soonDaysValid,
        }));
    }

    private readonly handleChange = (value: string, id: string) => {

        switch (id) {
            case 'imminent':
                this.setState((state) => ({
                    urgencyConfiguration: {
                        ...state.urgencyConfiguration,
                        imminentDays: Number(value)
                    }
                }), this.validate);
                break;
            case 'soon':
            this.setState((state) => ({
                urgencyConfiguration: {
                    ...state.urgencyConfiguration,
                    soonDays: Number(value)
                }
            }), this.validate);
                break;
        }
    }

    private readonly saveSettings = (event: React.MouseEvent<HTMLElement>) => {
        if (this.state.imminentDaysValid && this.state.soonDaysValid) {
            this.props.onSave({
                imminentDays: this.state.urgencyConfiguration.imminentDays,
                soonDays: this.state.urgencyConfiguration.soonDays
            });
        }
    }
}

export default withStyles(styles)(SettingsModal);