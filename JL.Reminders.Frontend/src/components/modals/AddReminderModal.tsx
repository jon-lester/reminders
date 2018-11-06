import * as Mui from '@material-ui/core';
import { InlineDatePicker } from 'material-ui-pickers';
import moment, { Moment } from 'moment';
import * as React from 'react';

import IAddReminderRequest from '../../model/IAddReminderRequest';
import IOptions from '../../model/Options';
import TextInputControl from './TextInputControl';

interface IAddReminderModalProps {
    open: boolean;
    occurrenceOptions: IOptions;
    importanceOptions: IOptions;
    onClose(): void;
    onSave(addReminderRequest: IAddReminderRequest): void;
}

interface IAddReminderModalState {
    addReminderRequest: IAddReminderRequest;
    formValid: boolean;
    titleErrorMessage: string | null;
    titleValid: boolean;
}

/**
 * Render a modal dialog to accept data from the user to be used to
 * create a new Reminder.
 */
class AddReminderModal extends React.Component<IAddReminderModalProps, IAddReminderModalState> {

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
            formValid: false,
            titleErrorMessage: null,
            titleValid: true
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
                    <InlineDatePicker
                        label='Next due date'
                        value={this.state.addReminderRequest.forDate}
                        onChange={this.handleDatePickerChange}
                        fullWidth={true}
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

    /**
     * Validate the form data, setting the formValid property
     * in this.state accordingly.
     */
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

        newState.formValid = newState.titleValid;

        this.setState(newState);
    }

    /**
     * Set a property of this.state's IAddReminderRequest
     * to the given value, then trigger a validation check.
     */
    private readonly setAddReminderRequestState = (propName: keyof IAddReminderRequest, value: string | number) => {
        this.setState((state: IAddReminderModalState) => {
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
     * Handle the form's 'next due date' selector being changed by the user.
     */
    private readonly handleDatePickerChange = (date: Moment) => {
        this.setAddReminderRequestState('forDate', date.format());
    }

    /**
     * Handle the form's 'recurrence' select being changed by the user.
     */
    private readonly handleRecurrenceChange = (evt: React.ChangeEvent<HTMLSelectElement>) => {
        const newValue = parseInt(evt.target.value, 10);
        this.setAddReminderRequestState('recurrence', newValue);
    }

    /**
     * Handle the user having clicked the save button by passing the
     * form data down to the onSave callback.
     */
    private readonly handleSave = () => {
        if (this.state.formValid) {
            this.props.onSave(this.state.addReminderRequest);
        }
    }
}

export default AddReminderModal;