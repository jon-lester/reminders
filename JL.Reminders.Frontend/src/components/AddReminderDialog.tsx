import * as Mui from '@material-ui/core';
import * as React from 'react';

import IAddReminderRequest from '../interfaces/IAddReminderRequest';

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
                        id="reminder.fordate"
                        onChange={this.handleDateChange}
                        label="For Date"
                        fullWidth={true}
                        InputLabelProps={{shrink: true}}
                        type="date"/>
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
        console.log(evt.target.value);
        this.setState({
            ...this.state,
            forDate: new Date()
        });
    }

    private handleOnClose = () => {
        this.props.onClose();
    }

    private handleSave = () => {
        this.props.onSave({
            description: this.state.description,
            forDate: new Date(),
            importance: 1,
            recurrence: 2,
            title: this.state.title
        });
    }
}

export default AddReminderDialogComponent;