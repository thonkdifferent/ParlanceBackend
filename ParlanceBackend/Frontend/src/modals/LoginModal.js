import React from 'react';

import Styles from './LoginModal.module.css';

import Fetch from '../utils/Fetch';
import Modal from '../components/Modal';
import userManager from '../utils/UserManager';
import ProgressSpinner from "../components/ProgressSpinner";
import ResetPasswordModal from "./ResetPasswordModal";
import Form from "../components/Form";

class LoginModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "login",
            username: "",
            password: "",
            email: "",
            otpToken: "",
            newPassword: "",
            passwordConfirm: "",
            error: ""
        }
    }
    
    async performLogin() {
        //Attempt to log in with the current details
        this.changeStage("loginProcessing");

        let provisionTokenData = {
            username: this.state.username,
            password: this.state.password
        }

        if (this.state.otpToken !== "") provisionTokenData.otpToken = this.state.otpToken;
        if (this.state.newPassword !== "") provisionTokenData.newPassword = this.state.newPassword;

        try {
            let response = await Fetch.post("/users/provisionToken", provisionTokenData);
            await userManager.performLogin(response.token);
            Modal.unmount();
        } catch (errorResponse) {
            try {
                let response = await errorResponse.json();
                console.log("Login error!");

                switch (response.error) {
                    case "incorrectCredentials":
                        this.setState({
                            error: "Check your credentials and try again."
                        });
                        this.changeStage("login");
                        break;
                    case "passwordResetRequired":
                        this.changeStage("reset");
                        break;
                    case "disabledAccount":
                        this.changeStage("disabled");
                        break;
                    case "twofactorRequired":
                        this.changeStage("twoFactor");
                        break;
                    case "passwordResetRequestRequired":
                        this.changeStage("passwordResetRequestRequired");
                        break;
                    default:
                        this.setState({
                            error: "Sorry, we couldn't log you in."
                        });
                        this.changeStage("login");
                }
            } catch {
                this.setState({
                    error: "Sorry, we couldn't log you in."
                });
                this.changeStage("login");
            }
        }
    }

    async performRegister() {
        this.changeStage("registerProcessing");

        let registerData = {
            username: this.state.username,
            password: this.state.password,
            email: this.state.email
        };

        try {
            let response = await Fetch.post("/users/create", registerData);
            await userManager.performLogin(response.token);
            Modal.unmount();
        } catch (errorResponse) {
            let response = await errorResponse.json();
            console.log("Login error!");

            switch (response.error) {
                default:
                    this.setState({
                        error: "Sorry, we couldn't register an account for you."
                    });
                    this.changeStage("register");
            }
        }
    }

    updateStateText(stateVar, event) {
        this.setState({
            [stateVar]: event.target.value
        })
    }

    changeStage(stage) {
        this.setState({
            stage: stage
        })
    };

    render() {
        const stages = {
            login: <Modal heading="Log In" buttons={[
                Modal.CancelButton,
                {
                    text: "Register",
                    onClick: () => {
                        this.setState({
                            email: "",
                            passwordConfirm: ""
                        }, () => this.changeStage("register"))
                    }
                },
                {
                    text: "Forgot",
                    onClick: () => this.changeStage("forgot")
                },
                {
                    text: "Log In",
                    onClick: () => {
                        this.setState({
                            otpToken: "",
                            newPassword: ""
                        }, this.performLogin.bind(this))
                    }
                }
                ]}>
                <div className={Styles.LoginContainer}>
                    Log in to your Victor Tran account to access Parlance.
                    <input className={Styles.LoginTextEntry} type="text" placeholder="Username" value={this.state.username} onChange={this.updateStateText.bind(this, "username")} />
                    <input className={Styles.LoginTextEntry} type="password" placeholder="Password" value={this.state.password} onChange={this.updateStateText.bind(this, "password")} />
                    {this.state.error && <span className={Styles.ErrorText}>{this.state.error}</span>}
                </div>
            </Modal>,
            register: <Modal heading="Register" buttons={[
                {
                    text: "Cancel",
                    onClick: () => this.changeStage("login")
                },
                {
                    text: "Register",
                    onClick: async () => {
                        if (!this.state.username || !this.state.password || !this.state.email) {
                            this.setState({
                                error: "Please fill in all fields."
                            });
                            return;
                        }

                        if (this.state.password !== this.state.passwordConfirm) {
                            this.setState({
                                error: "Check that your passwords match."
                            });
                            return;
                        }

                        await this.performRegister();
                    }
                }
                ]}>
                <div className={Styles.LoginContainer}>
                    Register for a Victor Tran account to make edits to translations. 
                    <div className={Styles.LoginContainer}>
                        <input className={Styles.LoginTextEntry} type="text" placeholder="Username" value={this.state.username} onChange={this.updateStateText.bind(this, "username")} />
                        <input className={Styles.LoginTextEntry} type="email" placeholder="Email Address" value={this.state.email} onChange={this.updateStateText.bind(this, "email")} />
                    </div>
                    Make it a good password and save it for this account. You don't want to be reusing this password.
                    <div className={Styles.LoginContainer}>
                        <input className={Styles.LoginTextEntry} type="password" placeholder="Password" value={this.state.password} onChange={this.updateStateText.bind(this, "password")} />
                        <input className={Styles.LoginTextEntry} type="password" placeholder="Confirm Password" value={this.state.passwordConfirm} onChange={this.updateStateText.bind(this, "passwordConfirm")} />
                    </div>
                    {this.state.error && <span className={Styles.ErrorText}>{this.state.error}</span>}
                </div>
            </Modal>,
            loginProcessing: <Modal heading="Log In" >
                <ProgressSpinner message={"Logging in..."} />
            </Modal>,
            registerProcessing: <Modal heading="Register" >
                <ProgressSpinner message={"Creating account..."} />
            </Modal>,
            disabled: <Modal heading="Disabled Account" buttons={[Modal.OkButton]}>
                Sorry, you can't be logged in as this account is disbled.
            </Modal>,
            passwordResetRequestRequired: <Modal heading="Password Reset Required" buttons={[
                Modal.OkButton
            ]}>
                You need to reset your password.
            </Modal>,
            twoFactor: <Modal heading="Two Factor Authentication" buttons={[
                {
                    text: "Cancel",
                    onClick: () => this.changeStage("login")
                },
                {
                    text: "OK",
                    onClick: this.performLogin.bind(this),
                }
                ]}>
                <div className={Styles.LoginContainer}>
                    Enter your Two Factor Authentication code
                    <span className={Styles.HelpText}>You can also use a 12 digit backup code</span>
                    <input className={Styles.LoginTextEntry}  type="text" placeholder="Two Factor Authentication code" value={this.state.otpToken} onChange={this.updateStateText.bind(this, "otpToken")} />
                </div>
            </Modal>,
            reset: <Modal heading="Reset Password" buttons={[
                {
                    text: "Cancel",
                    onClick: () => this.changeStage("login")
                },
                {
                    text: "OK",
                    onClick: () => {
                        if (this.state.newPassword !== this.state.passwordConfirm) {
                            return;
                        }
                        
                        this.performLogin();
                    }
                }
            ]}>
                <div className={Styles.LoginContainer}>
                    You need to set a new password for your account.
                    <span className={Styles.HelpText}>Make it a good password and save it for this account. You don't want to be reusing this password.</span>
                    <Form>
                        <label>New Password</label>
                        <input type="password" value={this.state.newPassword} onChange={this.updateStateText.bind(this, "newPassword")} />
                        
                        <label>Confirm New Password</label>
                        <input type="password" value={this.state.passwordConfirm} onChange={this.updateStateText.bind(this, "passwordConfirm")} />
                    </Form>
                </div>
            </Modal>,
            forgot: <ResetPasswordModal onDone={this.changeStage.bind(this, "login")} />
        }

        return stages[this.state.stage];
    }
}

export default LoginModal;