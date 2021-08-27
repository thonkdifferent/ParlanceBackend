import React from 'react';
import { withTranslation } from 'react-i18next';

import Modal from "../../components/Modal";
import ProgressSpinner from "../../components/ProgressSpinner";
import Fetch from "../../utils/Fetch";
import ModalList from "../../components/ModalList";
import Form from "../../components/Form";
import userManager from "../../utils/UserManager";
import CurrentPasswordModal from "./CurrentPasswordModal";

class ChangePasswordModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "newPassword",
            password: "",
            passwordConfirm: "",
            currentPassword: "",
            errorText: ""
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

    setPasswordStage() {
        this.setState({
            stage: "password",
            currentPassword: ""
        })
    }

    async performChange() {
        this.changeStage("applying")

        try {
            await Fetch.post("/users/me/password", {
                newPassword: this.state.password,
                currentPassword: this.state.currentPassword
            });
            await userManager.refreshUserData();
            Modal.unmount();
        } catch (error) {
            this.changeStage(error.status === 401 ? "passwordInvalid" : "error");
        }
    }

    render() {
        const stages = {
            newPassword: <Modal heading={this.props.t("PROFILE_CHANGE_PASSWORD")} buttons={[
                Modal.CancelButton,
                {
                    text: this.props.t("PROFILE_NEXT_BUTTON"),
                    onClick: () => {
                        if (this.state.password !== this.state.passwordConfirm) {
                            this.setState({
                                errorText: this.props.t("PROFILE_CHANGE_PASSWORD_NOT_MATCH")
                            })
                            return;
                        }
                        
                        this.setPasswordStage()
                    }
                }
            ]}>
                {this.props.t("PROFILE_CHANGE_PASSWORD_PROMPT")}
                <Form errorText={this.state.errorText}>
                    <label>{this.props.t("PROFILE_CHANGE_PASSWORD_NEW_PASSWORD")}</label>
                    <input type="password" value={this.state.password} onChange={this.updateStateText.bind(this, "password")} />

                    <label>{this.props.t("PROFILE_CHANGE_PASSWORD_CONFIRM_NEW_PASSWORD")}</label>
                    <input type="password" value={this.state.passwordConfirm} onChange={this.updateStateText.bind(this, "passwordConfirm")} />
                </Form>
            </Modal>,
            password: <CurrentPasswordModal onContinue={this.performChange.bind(this)} onCancel={this.changeStage.bind(this, "newPassword")} onPasswordChange={password => this.setState({currentPassword: password})} continueButton={this.props.t("PROFILE_CHANGE_PASSWORD_ACTION")} />,
            applying: <Modal heading={this.props.t("PROFILE_CHANGE_PASSWORD")}>
                <ProgressSpinner message={this.props.t("PROFILE_CHANGE_PASSWORD_PROCESSING")} />
            </Modal>,
            passwordInvalid: <Modal heading={this.props.t("PROFILE_CHANGE_PASSWORD")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("PROFILE_CURRENT_PASSWORD_INCORRECT")}
            </Modal>,
            error: <Modal heading={this.props.t("PROFILE_CHANGE_PASSWORD")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("CHANGE_PASSWORD_ERROR")}
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default withTranslation()(ChangePasswordModal);