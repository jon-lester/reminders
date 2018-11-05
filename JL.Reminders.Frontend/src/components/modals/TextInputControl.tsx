import * as Mui from '@material-ui/core';
import * as React from 'react';

import { red } from '@material-ui/core/colors/';
import { createStyles, withStyles } from '@material-ui/core/styles';


interface ITextInputControlProps {
    autoFocus: boolean;
    defaultValue: string;
    endAdornment?: string;
    errorMessage: string | null;
    id: string;
    label: string;
    maxLength?: number;
    onChange: (value: string, id: string) => void;
    valid: boolean;
}

const styles = () => createStyles({
    formHelperErrorText: {
        color: red[500]
    }
});

class TextInputControl extends React.Component<ITextInputControlProps & Mui.WithStyles<typeof styles>> {
    
    constructor(props: ITextInputControlProps & Mui.WithStyles<typeof styles>) {
        super(props);
    }

    public render() {
        return (
            <Mui.FormControl
                margin="normal"
                fullWidth={true}
                aria-describedby={`${this.props.id}-helper-text`}>
                <Mui.InputLabel
                    htmlFor={this.props.id}>
                    {this.props.label}
                </Mui.InputLabel>
                <Mui.Input
                    id={this.props.id}
                    onChange={this.handleValueChange}
                    fullWidth={true}
                    autoFocus={this.props.autoFocus}
                    required={true}
                    error={!this.props.valid}
                    defaultValue={this.props.defaultValue}
                    endAdornment={this.props.endAdornment}
                    inputProps={this.props.maxLength ? {maxLength: this.props.maxLength} : {}}
                />
                {!this.props.valid &&
                    <Mui.FormHelperText
                        id={this.props.id + "-helper-text"}
                        className={this.props.classes.formHelperErrorText}>
                        {this.props.errorMessage || 'Error'}
                    </Mui.FormHelperText>}
            </Mui.FormControl>
        );
    }

    private readonly handleValueChange = (event: React.ChangeEvent<HTMLInputElement>) =>
        this.props.onChange(event.currentTarget.value, this.props.id);
}

export default withStyles(styles)(TextInputControl);