import * as Mui from '@material-ui/core';
import * as moment from 'moment';
import * as React from 'react';

import IAddReminderRequest from '../../model/IAddReminderRequest';
import IOptions from '../../model/Options';
import TextInputControl from './TextInputControl';

interface IAddReminderDialogProps {
    open: boolean;
    occurrenceOptions: IOptions;
    importanceOptions: IOptions;
    onClose(): void;
    onSave(addReminderRequest: IAddReminderRequest): void;
}

interface IAddReminderDialogState {
    addReminderRequest: IAddReminderRequest;
    forDateErrorMessage: string | null;
    forDateValid: boolean;
    formValid: boolean;
    titleErrorMessage: string | null;
    titleValid: boolean;
}

class AddReminderModal extends React.Component<IAddReminderDialogProps, IAddReminderDialogState> {

    constructor(props: any) {
        super(props);

        this.state = {
            addReminderRequest: {
                description: '',
                forDate: moment().format(),
                importance: 0,
                recurrence: 0,
                title: ''
            },
            forDateErrorMessage: null,
            forDateValid: true,
            formValid: false,
            titleErrorMessage: null,
            titleValid: true,
        };
    }

    public render() {

        const occurrenceOptions = [];

        for (const key in this.props.occurrenceOptions) {
            if (this.props.occurrenceOptions.hasOwnProperty(key)) {
                occurrenceOptions.push(
                    <Mui.MenuItem
                        key={key}
                        value={key}>
                        {this.props.occurrenceOptions[key]}
                    </Mui.MenuItem>
                )
            }
        }

        return (
            <Mui.Dialog
                disableBackdropClick={true}
                disableEscapeKeyDown={true}
                open={this.props.open}
                onClose={this.props.onClose}>
                <Mui.DialogTitle>Add a new reminder</Mui.DialogTitle>
                <Mui.DialogContent>
                    <TextInputControl
                        id="title"
                        onChange={this.handleTitleChange}
                        autoFocus={true}
                        defaultValue={''}
                        errorMessage={this.state.titleErrorMessage}
                        label="Title"
                        valid={this.state.titleValid}
                        maxLength={64}
                    />
                    <Mui.TextField
                        id="reminder-description"
                        onChange={this.handleDescriptionChange}
                        label="Description"
                        fullWidth={true}
                        margin="normal"
                    />
                    <Mui.TextField
                        id="reminder-fordate"
                        onChange={this.handleDateChange}
                        defaultValue={moment().format('YYYY-MM-DD')}
                        label="For Date"
                        fullWidth={true}
                        InputLabelProps={{shrink: true}}
                        type="date"
                        margin="normal"
                    />
                    <Mui.FormControl margin="normal" style={{minWidth: 120}}>
                        <Mui.InputLabel htmlFor="reminder-recurrence">Recurrence</Mui.InputLabel>
                        <Mui.Select
                            inputProps={{name: 'recurrence', id: 'reminder-recurrence' }}
                            onChange={this.handleRecurrenceChange}
                            value={this.state.addReminderRequest.recurrence.toString()}>
                            {occurrenceOptions}
                        </Mui.Select>
                    </Mui.FormControl>
                </Mui.DialogContent>
                <Mui.DialogActions>
                    <Mui.Button
                        color="primary"
                        onClick={this.props.onClose}>
                        Cancel
                    </Mui.Button>
                    <Mui.Button
                        color="primary"
                        onClick={this.handleSave}
                        disabled={!this.state.formValid}>
                        Save
                    </Mui.Button>
                </Mui.DialogActions>
            </Mui.Dialog>
        );
    }

    private readonly validate = () => {

        const newState = { ...this.state };

        if (this.state.addReminderRequest.title.length === 0
            || this.state.addReminderRequest.title.length > 64
            || this.state.addReminderRequest.title === null
            || this.state.addReminderRequest.title === undefined) {
            newState.titleValid = false;
            newState.titleErrorMessage = 'Required, 1-64 characters.';
        } else {
            newState.titleValid = true;
            newState.titleErrorMessage = null;
        }

        if (this.state.addReminderRequest.forDate.length === 0) {
            newState.forDateValid = false;
            newState.forDateErrorMessage = 'Not a valid date.';
        } else {
            newState.forDateValid = true;
            newState.forDateErrorMessage = null;
        }

        newState.formValid = newState.titleValid && newState.forDateValid;

        this.setState(newState);
    }

    private readonly setAddReminderRequestState = (propName: string, value: string | number) => {
        this.setState((state: IAddReminderDialogState) => {
            const newReminderDetails = { ...state.addReminderRequest };
            newReminderDetails[propName] = value;
            return {
                ...state,
                addReminderRequest: newReminderDetails
            };
        }, this.validate);
    }

    /**
     * Handle the form's 'title' field being changed by the user.
     */
    private readonly handleTitleChange = (value: string, id: string) => {
        this.setAddReminderRequestState('title', value || '');
    }

    /**
     * Handle the form's 'description' field being changed by the user.
     */
    private readonly handleDescriptionChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = evt.currentTarget.value;
        this.setAddReminderRequestState('description', newValue || '');
    }

    /**
     * Handle the form's 'for date' selector being changed by the user.
     */
    private readonly handleDateChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
        const userMoment = moment.utc(evt.target.value, 'YYYY-MM-DD', true);
        this.setAddReminderRequestState('forDate', userMoment.format());
    }

    /**
     * Handle the form's 'recurrence' select being changed by the user.
     */
    private readonly handleRecurrenceChange = (evt: React.ChangeEvent<HTMLSelectElement>) => {
        const newValue = parseInt(evt.target.value, 10);
        this.setAddReminderRequestState('recurrence', newValue);
    }

    /**
     * Handle the user having clicked the save button; pass the
     * form data down to the onSave callback.
     */
    private readonly handleSave = () => {
        this.props.onSave(this.state.addReminderRequest);
    }
}

export default AddReminderModal;