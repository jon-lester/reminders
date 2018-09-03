import * as Mui from '@material-ui/core';
import * as moment from 'moment';
import * as React from 'react';

import IAddReminderRequest from '../model/IAddReminderRequest';

interface IAddReminderDialogComponentProps {
    open: boolean;
    onClose(): void;
    onSave(addReminderRequest: IAddReminderRequest): void;
}

class AddReminderDialogComponent extends React.Component<IAddReminderDialogComponentProps, IAddReminderRequest> {

    constructor(props: any) {
        super(props);

        this.state = {
            description: '',
            forDate: new Date(),
            importance: 0,
            recurrence: 0,
            title: ''
        };
    }

    public render() {
        return (
            <Mui.Dialog
                disableBackdropClick={true}
                disableEscapeKeyDown={true}
                open={this.props.open}
                onClose={this.handleOnClose}>
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
                            value={this.state.recurrence}>
                            <Mui.MenuItem value={0}>One-off</Mui.MenuItem>
                            <Mui.MenuItem value={1}>Annual</Mui.MenuItem>
                            <Mui.MenuItem value={2}>Six-Monthly</Mui.MenuItem>
                            <Mui.MenuItem value={3}>Quarterly</Mui.MenuItem>
                            <Mui.MenuItem value={4}>Monthly</Mui.MenuItem>
                        </Mui.Select>
                    </Mui.FormControl>
                </Mui.DialogContent>
                <Mui.DialogActions>
                    <Mui.Button
                        color="primary"
                        onClick={this.handleOnClose}>
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

    private handleTitleChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            title: evt.target.value || ''
        });
    }

    private handleDescriptionChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            description: evt.target.value || ''
        });
    }

    private handleDateChange = (evt: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            forDate: moment(evt.target.value, 'YYYY-MM-DD').toDate()
        });
    }

    private handleRecurrenceChange = (evt: React.ChangeEvent<HTMLSelectElement>) => {
        console.log(evt);
        this.setState({
            ...this.state,
            recurrence: parseInt(evt.target.value, 10)
        });
    }

    private handleOnClose = () => {
        this.props.onClose();
    }

    private handleSave = () => {
        this.props.onSave({
            description: this.state.description,
            forDate: this.state.forDate,
            importance: this.state.importance,
            recurrence: this.state.recurrence,
            title: this.state.title
        });
    }
}

export default AddReminderDialogComponent;