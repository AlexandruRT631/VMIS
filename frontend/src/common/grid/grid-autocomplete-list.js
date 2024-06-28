import React, {useEffect, useState} from "react";
import {Autocomplete, Checkbox, Grid, TextField} from "@mui/material";

const GridAutocompleteList = ({ list, setSelect, select, name, error }) => {
    const [inputValue, setInputValue] = useState('');
    const [localSelect, setLocalSelect] = useState([]);
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);

    useEffect(() => {
        if (select) {
            setLocalSelect(select);
        }
    }, [select]);

    const handleDropdownClose = () => {
        setIsDropdownOpen(false);
        setSelect(localSelect);
    };

    const handleDropdownOpen = () => {
        setIsDropdownOpen(true);
    };

    const handleInputChange = (event, newValue) => {
        setInputValue(newValue);
        if (newValue === '' && !isDropdownOpen) {
            setLocalSelect([]);
            setSelect([]);
        }
    };

    const handleChange = (event, newValue) => {
        setLocalSelect(newValue);
        if (newValue.length === 0 && !isDropdownOpen) {
            setSelect([]);
        }
    };

    return (
        <Grid item xs={3}>
            <Autocomplete
                multiple
                options={list}
                disableCloseOnSelect
                getOptionLabel={(option) => option.name}
                inputValue={inputValue}
                isOptionEqualToValue={(option, value) => option.id === value.id}
                onInputChange={handleInputChange}
                onChange={handleChange}
                value={localSelect}
                onClose={handleDropdownClose}
                onOpen={handleDropdownOpen}
                renderOption={(props, option, { selected }) => {
                    const { key, ...rest } = props;
                    return (
                        <li key={key} {...rest}>
                            <Checkbox
                                sx={{
                                    color: option.hexCode,
                                    '&.Mui-checked': {
                                        color: option.hexCode,
                                    },
                                }}
                                checked={selected}
                                style={{ marginRight: 8 }}
                            />
                            {option.name}
                        </li>
                    );
                }}
                renderTags={(value, getTagProps) => {
                    if (inputValue || value.length === 0) return null;
                    const firstOption = value[0];
                    const additionalCount = value.length - 1;

                    return (
                        <div style={{ display: 'flex', alignItems: 'center' }}>
                            <span>{firstOption.name}</span>
                            {additionalCount > 0 && (
                                <span style={{ marginLeft: 4 }}>
                                    {`+ ${additionalCount}`}
                                </span>
                            )}
                        </div>
                    );
                }}
                renderInput={(params) => <TextField
                    {...params}
                    label={name}
                    error={!!error}
                    helperText={error}
                />}
                disabled={list.length === 0}
            />
        </Grid>
    );
}

export default GridAutocompleteList;