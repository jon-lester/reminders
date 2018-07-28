import * as React from 'react';

class ReminderLoader extends React.Component {

    public render() {
        return '';
    }

    public componentDidMount() {
        fetch('http://localhost:49900/api/reminders/')
            .then(response => response.json())
            .then(json => this.setState(json));
    }
}

export default ReminderLoader;