import {Avatar, Box, Button, Grid, Paper, TextField, Typography} from "@mui/material";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import {Link} from "react-router-dom";
import {useEffect, useState} from "react";
import {login} from "../../api/authentication-api";
import {getUserId, setToken} from "../../common/token";

const Login = () => {
    const [loading, setLoading] = useState(true);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [userId, setUserId] = useState(null);
    const [validationErrors, setValidationErrors] = useState({ email: '', password: '' });
    const [loginError, setLoginError] = useState('');

    useEffect(() => {
        setUserId(getUserId());
        setLoading(false);
    }, []);

    const validate = () => {
        let tempErrors = { email: '', password: '' };
        let isValid = true;

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

        setValidationErrors(tempErrors);
        return isValid;
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        if (validate()) {
            login(email, password)
                .then((data) => {
                    console.log('Login successful:', data);
                    setLoginError('');
                    setToken(data);
                    window.location.href = '/';
                })
                .catch(error => {
                    console.error('Login failed:', error);
                    setLoginError('Invalid email or password');
                });
        }
    };

    if (loading) {
        return;
    }

    if (userId) {
        return (
            <Typography variant={"h4"}>You are already logged in</Typography>
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
                        Log in
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
                            id={"email"}
                            label={"Email Address"}
                            name={"email"}
                            autoComplete={"email"}
                            autoFocus
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
                        {loginError && (
                            <Typography color="error" variant="body2" sx={{ mt: 2 }}>
                                {loginError}
                            </Typography>
                        )}
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Log In
                        </Button>
                        <Grid container>
                            <Grid item xs>
                            </Grid>
                            <Grid item>
                                <Link to="/register" variant="body2">
                                    {"Register"}
                                </Link>
                            </Grid>
                        </Grid>
                    </Box>
                </Box>
            </Grid>
        </Grid>
    )
}

export default Login;