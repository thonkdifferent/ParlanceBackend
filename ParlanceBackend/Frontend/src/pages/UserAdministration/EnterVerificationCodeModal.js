import React from 'react';
import { withTranslation } from 'react-i18next';

import Modal from "../../components/Modal";
import ProgressSpinner from "../../components/ProgressSpinner";
import Fetch from "../../utils/Fetch";
import ModalList from "../../components/ModalList";
import Form from "../../components/Form";
import userManager from "../../utils/UserManager";

class EnterVerificationCodeModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "code",
            code: ""
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

    async performVerify() {
        this.changeStage("verifying")

        try {
            await Fetch.post("/users/me/verify", {
                verificationCode: this.state.code
            });
            await userManager.refreshUserData();
            this.changeStage("verifySuccess");
        } catch (error) {
            this.changeStage(error.status === 401 ? "verifyInvalid" : "verifyError");
        }
    }

    render() {
        const stages = {
            code: <Modal heading={this.props.t("ENTER_VERIFICATION_CODE_BUTTON")} buttons={[
                Modal.CancelButton,
                {
                    text: this.props.t("VERIFY_EMAIL_VERIFY_BUTTON"),
                    onClick: this.performVerify.bind(this)
                }
            ]}>
                {this.props.t("VERIFY_EMAIL_PROMPT")}
                <Form>
                    <label>{this.props.t("VERIFY_EMAIL_VERIFICATION_CODE")}</label>
                    <input type="text" value={this.state.code} onChange={this.updateStateText.bind(this, "code")} />
                </Form>
            </Modal>,
            verifying: <Modal heading={this.props.t("ENTER_VERIFICATION_CODE_BUTTON")}>
                <ProgressSpinner message={this.props.t("VERIFY_EMAIL_PROCESSING")} />
            </Modal>,
            verifySuccess:<Modal heading={this.props.t("ENTER_VERIFICATION_CODE_BUTTON")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("VERIFY_EMAIL_SUCCESS")}
            </Modal>,
            verifyInvalid: <Modal heading={this.props.t("ENTER_VERIFICATION_CODE_BUTTON")} buttons={[
                Modal.OkButton
            ]}>
                <span style={{whiteSpace: "pre-line"}}>{this.props.t("VERIFY_EMAIL_INVALID")}</span>
            </Modal>,
            verifyError: <Modal heading={this.props.t("ENTER_VERIFICATION_CODE_BUTTON")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("VERIFY_EMAIL_ERROR")}
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default withTranslation()(EnterVerificationCodeModal);