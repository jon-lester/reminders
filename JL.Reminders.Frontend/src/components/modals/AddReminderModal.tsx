import * as Mui from '@material-ui/core';
import * as moment from 'moment';
import * as React from 'react';

import IAddReminderRequest from '../../model/IAddReminderRequest';
import IOptions from '../../model/Options';

interface IAddReminderDialogComponentProps {
    open: boolean;
    occurrenceOptions: IOptions;
    importanceOptions: IOptions;
    onClose(): void;
    onSave(addReminderRequest: IAddReminderRequest): void;
}

class AddReminderModal extends React.Component<IAddReminderDialogComponentProps, IAddReminderRequest> {

    constructor(props: any) {
        super(props);

        this.state = {
            description: '',
            forDate: moment().format(),
            importance: 0,
            recurrence: 0,
            title: ''
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
                <Mui.DialogTitle>Add Reminder</Mui.DialogTitle>
                <Mui.DialogContent>
                    <Mui.DialogContentText>Add a new reminder to the dashboard.</Mui.DialogContentText>
                </Mui.DialogContent>
                <Mui.DialogContent>
                    <Mui.TextField
                        id="reminder-title"
                        onChange={this.handleTitleChange}
                        label="Title"
                        fullWidth={true}
                        autoFocus={true}/>
                    <Mui.TextField
                        id="reminder-description"
                        onChange={this.handleDescriptionChange}
                        label="Description"
                        fullWidth={true}/>
                    <Mui.TextField
                        id="reminder-fordate"
                        style={{marginTop: 20}}
                        onChange={this.handleDateChange}
                        defaultValue={moment().format('YYYY-MM-DD')}
                        label="For Date"
                        fullWidth={true}
                        InputLabelProps={{shrink: true}}
                        type="date"/>
                    <Mui.FormControl style={{minWidth: 120}}>
                        <Mui.InputLabel htmlFor="reminder-recurrence">Recurrence</Mui.InputLabel>
                        <Mui.Select
                            inputProps={{name: 'recurrence', id: 'reminder-recurrence' }}
                            onChange={this.handleRecurrenceChange}
                            value={this.state.recurrence.toString()}>
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
                        onClick={this.handleSave}>
                        Save
                    </Mui.Button>
                </Mui.DialogActions>
            </Mui.Dialog>
        );
    }

    /**
     * Handle the form's 'title' field being changed by the user.
     */
    private readonly handleTitleChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            title: evt.target.value || ''
        });
    }

    /**
     * Handle the form's 'description' field being changed by the user.
     */
    private readonly handleDescriptionChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            description: evt.target.value || ''
        });
    }

    /**
     * Handle the form's 'for date' selector being changed by the user.
     */
    private readonly handleDateChange = (evt: React.ChangeEvent<HTMLInputElement>) => {

        const userMoment = moment.utc(evt.target.value, 'YYYY-MM-DD', true);

        this.setState({
            ...this.state,
            forDate: userMoment.format()
        });
    }

    /**
     * Handle the form's 'recurrence' select being changed by the user.
     */
    private readonly handleRecurrenceChange = (evt: React.ChangeEvent<HTMLSelectElement>) => {
        console.log(evt.target.value);
        this.setState({
            ...this.state,
            recurrence: parseInt(evt.target.value, 10)
        });
        console.log(this.state.recurrence);
    }

    /**
     * Handle the user having clicked the save button; pass the
     * form data down to the onSave callback.
     */
    private readonly handleSave = () => {
        this.props.onSave({
            description: this.state.description,
            forDate: this.state.forDate,
            importance: this.state.importance,
            recurrence: this.state.recurrence,
            title: this.state.title
        });
    }
}

export default AddReminderModal;