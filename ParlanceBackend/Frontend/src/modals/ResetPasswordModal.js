import React from 'react';
import Fetch from "../utils/Fetch";
import Modal from "../components/Modal";
import Form from "../components/Form";
import ProgressSpinner from "../components/ProgressSpinner";
import ModalList from "../components/ModalList";


class ResetPasswordModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "username",
            username: "",
            resetMethods: [],
            resetEmailAddress: ""
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

    componentWillUnmount() {
        this.props.onDone();
    }

    async acquireResetOptions() {
        this.changeStage("acquiring")

        try {
            this.setState({
                stage: "selectMethod",
                resetMethods: await Fetch.post("/users/resetMethods", {
                    username: this.state.username
                })
            })
        } catch {
            this.changeStage("error");
        }
    }
    
    async performReset() {
        this.changeStage("resetting")

        try {
            let data;
            
            if (this.state.resetType === "email") {
                data = {
                    email: this.state.resetEmailAddress
                }
            }
            await Fetch.post("/users/reset", {
                username: this.state.username,
                resetType: this.state.resetType,
                resetProperties: data
            });
            this.changeStage("resetComplete");
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            username: <Modal heading={"Reset Password"} buttons={[
                {
                    text: "Cancel",
                    onClick: this.props.onDone
                },
                {
                    text: "Next",
                    onClick: this.acquireResetOptions.bind(this)
                }
            ]}>
                What's your username?
                <Form>
                    <label>Username</label>
                    <input type="text" value={this.state.username} onChange={this.updateStateText.bind(this, "username")} />
                </Form>
            </Modal>,
            acquiring: <Modal heading="Reset Password">
                <ProgressSpinner message={"One moment..."} />
            </Modal>,
            selectMethod: <Modal heading={"Reset Password"} buttons={[
                {
                    text: "Cancel",
                    onClick: this.props.onDone
                }
                ]}>
                Choose a method to obtain a recovery password
                <ModalList>
                    {this.state.resetMethods.map(method => {
                        if (method.type === "email") {
                            return {
                                text: `Send email to ${method.data.user}∙∙∙@${method.data.domain}∙∙∙`,
                                onClick: () => {
                                    this.setState({
                                        stage: "emailMethod",
                                        resetType: method.type,
                                        resetData: method.data
                                    })
                                }
                            }
                        }
                    })}
                </ModalList>
            </Modal>,
            emailMethod: <Modal heading={"Reset Password"} buttons={[
                {
                    text: "Back",
                    onClick: () => this.changeStage("selectMethod")
                },
                {
                    text: "Reset Password",
                    onClick: this.performReset.bind(this)
                }
            ]}>
                Enter the full email address
                <Form>
                    <label>Email Address</label>
                    <input type="text" placeholder={`${this.state.resetData?.user}∙∙∙@${this.state.resetData?.domain}∙∙∙`} value={this.state.resetEmailAddress} onChange={this.updateStateText.bind(this, "resetEmailAddress")} />
                </Form>
            </Modal>,
            resetting: <Modal heading="Reset Password">
                <ProgressSpinner message={"Resetting your password..."} />
            </Modal>,
            resetComplete: <Modal heading="Reset Password" buttons={[
                {
                    text: "OK",
                    onClick: this.props.onDone
                }
            ]}>
                If the data you entered was correct, information has been sent to you about how to reset your password.
            </Modal>,
            error: <Modal heading="Reset Password" buttons={[
                Modal.OkButton
            ]}>
                Sorry, we couldn't reset your password. Try again later.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default ResetPasswordModal;