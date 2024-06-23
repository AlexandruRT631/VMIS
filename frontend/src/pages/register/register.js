import React, {useEffect, useState} from "react";
import {getUserId} from "../../common/token";
import {
    Avatar,
    Box,
    Button,
    FormControl,
    Grid,
    InputLabel,
    MenuItem,
    Paper,
    Select,
    TextField,
    Typography
} from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import {createThumbnail} from "../../common/create-thumbnail";
import {register} from "../../api/authentication-api";

const roles = [
    {
        name: 'Client',
        value: 1
    },
    {
        name: 'Seller',
        value: 2
    }
]

const Register = () => {
    const [loading, setLoading] = useState(true);
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [role, setRole] = useState(1);
    const [uploadingImage, setUploadingImage] = useState(false);
    const [image, setImage] = useState(null);
    const [thumbnail, setThumbnail] = useState(null);
    const [userId, setUserId] = useState(null);
    const [validationErrors, setValidationErrors] = useState({
        name: '',
        email: '',
        password: '',
        role: '',
    });
    const [registerError, setRegisterError] = useState('');

    useEffect(() => {
        setUserId(getUserId());
        setLoading(false);
    }, [])

    const validate = () => {
        let tempErrors = {
            name: '',
            email: '',
            password: '',
            role: '',
        };
        let isValid = true;

        if (!name) {
            tempErrors.name = 'Name is required';
            isValid = false;
        }
        else if (name.length < 3) {
            tempErrors.name = 'Name should be of minimum 3 characters length';
            isValid = false;
        }

        if (!email) {
            tempErrors.email = 'Email is required';
            isValid = false;
        }
        else if (!/\S+@\S+\.\S+/.test(email)) {
            tempErrors.email = 'Enter a valid email';
            isValid = false;
        }

        if (!password) {
            tempErrors.password = 'Password is required';
            isValid = false;
        }
        else if (password.length < 8) {
            tempErrors.password = 'Password should be of minimum 8 characters length';
            isValid = false;
        }

        if (!role) {
            tempErrors.role = 'Role is required';
            isValid = false;
        }
        else if (role !== 1 && role !== 2) {
            tempErrors.role = 'Invalid role';
            isValid = false;
        }

        setValidationErrors(tempErrors);
        return isValid;
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        if (uploadingImage) {
            setRegisterError('Please wait for the image to upload');
        }
        else if (validate()) {
            register({
                Name: name,
                Email: email,
                Password: password,
                Role: role
            }, image)
                .then((data) => {
                    console.log('Registration successful:', data);
                    setRegisterError('');
                    window.location.href = '/login';
                })
                .catch(error => {
                    console.error('Registration failed:', error);
                    setRegisterError('Registration failed');
                });
        }
    }

    const handleImageChange = async (event) => {
        setUploadingImage(true);
        setImage(event.target.files[0]);
        const thumbnail = await createThumbnail(event.target.files[0]);
        setThumbnail(thumbnail);
        setUploadingImage(false);
    };

    if (loading) {
        return;
    }

    if (userId) {
        return (
            <Typography variant={"h4"}>You are already registered</Typography>
        )
    }

    return (
        <Grid container justifyContent={"center"}>
            <Grid item xs={12} sm={8} md={5} component={Paper}>
                <Box sx={{
                    my: 8,
                    mx: 4,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}>
                    <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
                        <LockOutlinedIcon />
                    </Avatar>
                    <Typography variant={"h5"}>
                        Register
                    </Typography>
                    <Box
                        component={"form"}
                        noValidate
                        onSubmit={handleSubmit}
                    >
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            id={"name"}
                            label={"Name"}
                            name={"name"}
                            autoComplete={"name"}
                            autoFocus
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            error={!!validationErrors.name}
                            helperText={validationErrors.name}
                        />
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            id={"email"}
                            label={"Email Address"}
                            name={"email"}
                            autoComplete={"email"}
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            error={!!validationErrors.email}
                            helperText={validationErrors.email}
                        />
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            id={"password"}
                            label={"Password"}
                            name={"password"}
                            type={"password"}
                            autoComplete={"current-password"}
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            error={!!validationErrors.password}
                            helperText={validationErrors.password}
                        />
                        <FormControl fullWidth margin="normal">
                            <InputLabel id="role-label">Role</InputLabel>
                            <Select
                                labelId="role-label"
                                id="role"
                                value={role}
                                label="Role"
                                onChange={(e) => setRole(e.target.value)}
                            >
                                {roles.map((role) => (
                                    <MenuItem key={role.value} value={role.value}>
                                        {role.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                        {validationErrors.role && (
                            <Typography color="error" variant="body2" sx={{ mt: 2 }}>
                                {validationErrors.role}
                            </Typography>
                        )}
                        <Box sx={{ display: 'flex', alignItems: 'center', mt: 2 }}>
                            <Button variant="contained" component="label">
                                Select Image
                                <input
                                    type="file"
                                    hidden
                                    accept="image/*"
                                    onChange={handleImageChange}
                                />
                            </Button>
                            {thumbnail && (
                                <Box
                                    component="img"
                                    src={thumbnail}
                                    alt="Thumbnail"
                                    sx={{ width: 64, height: 64, ml: 2, borderRadius: '4px' }}
                                />
                            )}
                        </Box>

                        {registerError && (
                            <Typography color="error" variant="body2" sx={{ mt: 2 }}>
                                {registerError}
                            </Typography>
                        )}
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Register
                        </Button>
                    </Box>
                </Box>
            </Grid>
        </Grid>
    );
}

export default Register;