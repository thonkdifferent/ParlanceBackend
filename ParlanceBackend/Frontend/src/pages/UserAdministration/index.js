import React from 'react';
import ErrorPage from '../../components/ErrorPage';

import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";
import { withTranslation } from 'react-i18next';

import userManager from '../../utils/UserManager';
import Styles from './UserAdministration.module.css';
import ResendVerificationEmailModal from "./ResendVerificationEmailModal";
import Modal from "../../components/Modal";
import EnterVerificationCodeModal from "./EnterVerificationCodeModal";

class SiteAdministration extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            accessible: false
        }

        userManager.on('userChanged', this.updateAccessibility.bind(this))
    }
    
    async componentDidMount() {
        await this.updateAccessibility();
    }

    async componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps !== this.props) await this.updateAccessibility();
    }

    async updateAccessibility() {
        this.setState({
            accessible: userManager.loggedIn()
        });
    }
    
    renderEmailVerificationPrompt() {
        if (!userManager.isVerified()) {
            return <div className={Styles.VerificationRequest}>
                <h2 className={Styles.SectionHeader}>{this.props.t("VERIFY_EMAIL_HEADER")}</h2>
                <span>{this.props.t("VERIFY_EMAIL_EXPLANATION")}</span>
                <div className={Styles.VerificationRequestButtonBox}>
                    <button onClick={() => {Modal.mount(<ResendVerificationEmailModal />)}}>{this.props.t("RESEND_VERIFICATION_EMAIL_BUTTON")}</button>
                    <button onClick={() => {Modal.mount(<EnterVerificationCodeModal />)}}>{this.props.t("ENTER_VERIFICATION_CODE_BUTTON")}</button>
                </div>
            </div>
        }
    }

    render() {
        if (!this.state.accessible) {
            return <ErrorPage message="To change user settings, you'll need to log in." title="Log In" />
        }

        return <div className={Styles.SiteAdministration}>
            <div className={Styles.SiteAdministrationContents}>
                <div className={Styles.UserSplash}>
                    <img className={Styles.ProfilePicture} src={userManager.profilePictureUrl()} />
                    <span className={Styles.UserName}>{userManager.username()}</span>
                    <span className={Styles.EmailAddress}>{userManager.emailAddress()}</span>
                </div>
                {this.renderEmailVerificationPrompt()}
                <div>
                    <h2 className={Styles.SectionHeader}>{this.props.t("PROFILE_HEADER")}</h2>
                    <div className={Styles.SectionItem}>{this.props.t("PROFILE_CHANGE_USERNAME")}</div>
                    <div className={Styles.SectionItem}>{this.props.t("PROFILE_CHANGE_PROFILE_PICTURE")}</div>
                    <div className={Styles.SectionItem}>{this.props.t("PROFILE_CHANGE_EMAIL_ADDRESS")}</div>
                </div>
                <div>
                    <h2 className={Styles.SectionHeader}>{this.props.t("PROFILE_SECURITY")}</h2>
                    <div className={Styles.SectionItem}>{this.props.t("PROFILE_CHANGE_PASSWORD")}</div>
                    <div className={Styles.SectionItem}>{this.props.t("PROFILE_TWO_FACTOR_AUTHENTICATION")}</div>
                </div>
            </div>
        </div>
    }
}

export default withRouter(withTranslation()(SiteAdministration));