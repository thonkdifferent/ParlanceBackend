import React from 'react';
import { withTranslation } from 'react-i18next';

import Modal from "../../components/Modal";
import ProgressSpinner from "../../components/ProgressSpinner";
import Fetch from "../../utils/Fetch";
import ModalList from "../../components/ModalList";
import Form from "../../components/Form";
import userManager from "../../utils/UserManager";
import CurrentPasswordModal from "./CurrentPasswordModal";

class ChangeUsernameModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "username",
            username: "",
            currentPassword: ""
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
            await Fetch.post("/users/me/username", {
                newUsername: this.state.username,
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
            username: <Modal heading={this.props.t("PROFILE_CHANGE_USERNAME")} buttons={[
                Modal.CancelButton,
                {
                    text: this.props.t("PROFILE_NEXT_BUTTON"),
                    onClick: this.setPasswordStage.bind(this)
                }
            ]}>
                <Form>
                    <label>{this.props.t("PROFILE_CHANGE_USERNAME_USERNAME")}</label>
                    <input type="text" value={this.state.username} onChange={this.updateStateText.bind(this, "username")} />
                </Form>
            </Modal>,
            password: <CurrentPasswordModal onContinue={this.performChange.bind(this)} onCancel={this.changeStage.bind(this, "username")} onPasswordChange={password => this.setState({currentPassword: password})} continueButton={this.props.t("PROFILE_CHANGE_USERNAME_ACTION")} />,
            applying: <Modal heading={this.props.t("PROFILE_CHANGE_USERNAME")}>
                <ProgressSpinner message={this.props.t("PROFILE_CHANGE_USERNAME_PROCESSING")} />
            </Modal>,
            passwordInvalid: <Modal heading={this.props.t("PROFILE_CHANGE_USERNAME")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("PROFILE_CURRENT_PASSWORD_INCORRECT")}
            </Modal>,
            error: <Modal heading={this.props.t("PROFILE_CHANGE_USERNAME")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("CHANGE_USERNAME_ERROR")}
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default withTranslation()(ChangeUsernameModal);