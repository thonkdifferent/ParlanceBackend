import React from 'react';
import { withTranslation } from 'react-i18next';

import Modal from "../../components/Modal";
import ProgressSpinner from "../../components/ProgressSpinner";
import Fetch from "../../utils/Fetch";

class ResendVerificationEmailModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "resending"
        }
    }

    changeStage(stage) {
        this.setState({
            stage: stage
        })
    };
    
    async componentDidMount() {
        await this.performResend();
    }

    async performResend() {
        this.changeStage("resending")

        try {
            await Fetch.post("/users/me/verify/resend", {});
            this.changeStage("resendSuccess");
            // let projectDetails = await Fetch.delete(`/projects/${this.props.project}`);
            // this.props.projectManager.invalidateProjects();
            // Modal.unmount();
        } catch {
            this.changeStage("resendError");
        }
    }

    render() {
        const stages = {
            resending: <Modal heading={this.props.t("RESEND_VERIFICATION_EMAIL_BUTTON")}>
                <ProgressSpinner message={this.props.t("RESEND_VERIFICATION_EMAIL_PROCESSING")} />
            </Modal>,
            resendSuccess:<Modal heading={this.props.t("RESEND_VERIFICATION_EMAIL_BUTTON")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("RESEND_VERIFICATION_EMAIL_SUCCESS")}
            </Modal>,
            resendError: <Modal heading={this.props.t("RESEND_VERIFICATION_EMAIL_BUTTON")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("RESEND_VERIFICATION_EMAIL_ERROR")}
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default withTranslation()(ResendVerificationEmailModal);