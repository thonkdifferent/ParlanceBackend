import React from "react";
import Modal from "../../components/Modal";
import Form from "../../components/Form";
import { withTranslation } from 'react-i18next';

class CurrentPasswordModal extends React.Component {
    render() {
        return <Modal heading={this.props.t("PROFILE_CURRENT_PASSWORD")} buttons={[
            {
                text: this.props.t("BUTTON_CANCEL"),
                onClick: this.props.onCancel
            },
            {
                text: this.props.continueButton,
                onClick: this.props.onContinue
            }
        ]}>
            {this.props.t("PROFILE_CURRENT_PASSWORD_PROMPT")}
            <Form>
                <label>{this.props.t("PROFILE_CURRENT_PASSWORD_PASSWORD")}</label>
                <input type="password" value={this.props.password} onChange={event => this.props.onPasswordChange(event.target.value)} />
            </Form>
        </Modal>
    }
}

export default withTranslation()(CurrentPasswordModal);