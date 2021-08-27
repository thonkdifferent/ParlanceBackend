import React from 'react';
import { withTranslation } from 'react-i18next';

import Modal from "../../components/Modal";
import ProgressSpinner from "../../components/ProgressSpinner";
import Fetch from "../../utils/Fetch";
import ModalList from "../../components/ModalList";
import Form from "../../components/Form";
import userManager from "../../utils/UserManager";
import CurrentPasswordModal from "./CurrentPasswordModal";

class ChangeEmailModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "email",
            email: "",
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
            await Fetch.post("/users/me/email", {
                newEmail: this.state.email,
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
            email: <Modal heading={this.props.t("PROFILE_CHANGE_EMAIL")} buttons={[
                Modal.CancelButton,
                {
                    text: this.props.t("PROFILE_NEXT_BUTTON"),
                    onClick: this.setPasswordStage.bind(this)
                }
            ]}>
                <Form>
                    <label>{this.props.t("PROFILE_CHANGE_EMAIL_EMAIL")}</label>
                    <input type="text" value={this.state.email} onChange={this.updateStateText.bind(this, "email")} />
                </Form>
            </Modal>,
            password: <CurrentPasswordModal onContinue={this.performChange.bind(this)} onCancel={this.changeStage.bind(this, "email")} onPasswordChange={password => this.setState({currentPassword: password})} continueButton={this.props.t("PROFILE_CHANGE_EMAIL_ACTION")} />,
            applying: <Modal heading={this.props.t("PROFILE_CHANGE_EMAIL")}>
                <ProgressSpinner message={this.props.t("PROFILE_CHANGE_EMAIL_PROCESSING")} />
            </Modal>,
            passwordInvalid: <Modal heading={this.props.t("PROFILE_CHANGE_EMAIL")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("PROFILE_CURRENT_PASSWORD_INCORRECT")}
            </Modal>,
            error: <Modal heading={this.props.t("PROFILE_CHANGE_EMAIL")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("CHANGE_EMAIL_ERROR")}
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default withTranslation()(ChangeEmailModal);