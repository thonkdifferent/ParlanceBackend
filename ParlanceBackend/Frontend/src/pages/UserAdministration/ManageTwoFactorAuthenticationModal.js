import React from 'react';
import { withTranslation } from 'react-i18next';
import QRCode from 'qrcode.react';

import Modal from "../../components/Modal";
import ProgressSpinner from "../../components/ProgressSpinner";
import Fetch from "../../utils/Fetch";
import ModalList from "../../components/ModalList";
import Form from "../../components/Form";
import userManager from "../../utils/UserManager";
import CurrentPasswordModal from "./CurrentPasswordModal";

import Styles from "./ManageTwoFactorAuthenticationModal.module.css"

function chunk(string, spacing) {
    if (!string) return [];
    
    let arr = [];
    for (let i = 0; i < string.length; i += spacing) {
        arr.push(string.substr(i, spacing));
    }
    return arr;
}

class ManageTwoFactorAuthenticationModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "password",
            currentPassword: "",
            otpDetails: {},
            otpKey: ""
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

    async loadData() {
        this.changeStage("loading")

        try {
            let otpDetails = await Fetch.post("/users/me/otp", {
                currentPassword: this.state.currentPassword
            });
            
            this.setState({
                otpDetails: otpDetails
            });
            
            if (otpDetails.enabled) {
                this.changeStage("enabled");
            } else {
                this.changeStage("setup");
            }
        } catch (error) {
            this.changeStage(error.status === 401 ? "passwordInvalid" : "error");
        }
    }
    
    async performEnable() {
        this.changeStage("loading")
        try {
            let otpDetails = await Fetch.post("/users/me/otp/enable", {
                currentPassword: this.state.currentPassword,
                otpCode: this.state.otpKey
            });

            await this.loadData();
        } catch (error) {
            this.changeStage(error.status === 401 ? "setup" : "error");
        }
    }

    async performDisable() {
        this.changeStage("loading")
        try {
            let otpDetails = await Fetch.post("/users/me/otp/disable", {
                currentPassword: this.state.currentPassword
            });

            Modal.unmount();
        } catch (error) {
            this.changeStage(error.status === 401 ? "passwordInvalid" : "error");
        }
    }

    async performRegenerate() {
        this.changeStage("loading")
        try {
            let otpDetails = await Fetch.post("/users/me/otp/regenerate", {
                currentPassword: this.state.currentPassword
            });

            await this.loadData();
        } catch (error) {
            this.changeStage(error.status === 401 ? "passwordInvalid" : "error");
        }
    }

    render() {
        const stages = {
            password: <CurrentPasswordModal onContinue={this.loadData.bind(this)} onCancel={() => Modal.unmount()} onPasswordChange={password => this.setState({currentPassword: password})} continueButton={this.props.t("PROFILE_NEXT_BUTTON")} />,
            loading: <Modal heading={this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION")}>
                <ProgressSpinner message={this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_PROCESSING")} />
            </Modal>,
            setup: <Modal heading={this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION")} buttons={[
                Modal.CancelButton,
                {
                    text: this.props.t("PROFILE_NEXT_BUTTON"),
                    onClick: this.performEnable.bind(this)
                }
            ]}>
                {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_INTRODUCTION")}
                <div className={Styles.SetupContainer}>
                    <div className={Styles.SetupStepNumber}>1</div>
                    <div className={Styles.SetupStep}>
                        {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_STEP_ONE")}
                    </div>
                    <div className={Styles.SetupStepNumber}>2</div>
                    <div className={Styles.SetupStep}>
                        {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_STEP_TWO")}
                        <div className={Styles.SetupQrCode}><QRCode value={`otpauth://totp/Victor%20Tran%20Account?secret=${this.state.otpDetails?.key}&issuer=Victor%20Tran%20Account`} size={512} /></div>
                        {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_STEP_TWO_CODE")}
                        <div className={Styles.SetupCode}>{chunk(this.state.otpDetails?.key, 4).map(chunk => <span className={Styles.SetupCodeChunk}>{chunk}</span>)}</div>
                    </div>
                    <div className={Styles.SetupStepNumber}>3</div>
                    <div className={Styles.SetupStep}>
                        {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_STEP_THREE")}
                    </div>
                </div>
                <Form errorText={this.state.errorText}>
                    <label>{this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_CODE")}</label>
                    <input type="text" value={this.state.otpKey} onChange={this.updateStateText.bind(this, "otpKey")} />
                </Form>
            </Modal>,
            enabled: <Modal heading={this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION")} buttons={[
                {
                    text: this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_DONE"),
                    onClick: () => Modal.unmount()
                }
            ]}>
                {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_ENABLED_INTRODUCTION")}<br />
                {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_ENABLED_BACKUP_CODES_PROMPT")}
                <div className={Styles.BackupCodeContainer}>
                    {this.state.otpDetails?.backupCodes?.map(code => 
                        <div key={code.code} className={[Styles.BackupCode, code.used ? Styles.UsedBackupCode : ""].join(" ")}>
                            {chunk(code.code, 4).map(chunk => <span className={Styles.BackupCodeChunk}>{chunk}</span>)}
                        </div>
                    )}
                </div>
                {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_ENABLED_BACKUP_CODES_INFORMATION")}
                <ModalList>
                    {[
                        {
                            text: this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_PRINT_BACKUP_CODES"),
                            onClick: () => {}
                        },
                        {
                            text: this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_REGENERATE_BACKUP_CODES"),
                            onClick: () => this.changeStage("confirmRegenerate")
                        },
                        {
                            text: this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_DISABLE"),
                            type: "destructive",
                            onClick: () => this.changeStage("confirmDisable")
                        }
                    ]}
                </ModalList>
            </Modal>,
            confirmDisable: <Modal heading={this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_DISABLE")} buttons={[
                {
                    text: this.props.t("BUTTON_CANCEL"),
                    onClick: () => this.changeStage("enabled")
                },
                {
                    text: this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_DISABLE_BUTTON"),
                    type: "destructive",
                    onClick: this.performDisable.bind(this)
                }
            ]}>
                {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_DISABLE_PROMPT")}
            </Modal>,
            confirmRegenerate: <Modal heading={this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_REGENERATE_BACKUP_CODES")} buttons={[
                {
                    text: this.props.t("BUTTON_CANCEL"),
                    onClick: () => this.changeStage("enabled")
                },
                {
                    text: this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_REGENERATE_BACKUP_CODES_BUTTON"),
                    onClick: this.performRegenerate.bind(this)
                }
            ]}>
                {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_REGENERATE_BACKUP_CODES_PROMPT")}
            </Modal>,
            passwordInvalid: <Modal heading={this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("PROFILE_CURRENT_PASSWORD_INCORRECT")}
            </Modal>,
            error: <Modal heading={this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION")} buttons={[
                Modal.OkButton
            ]}>
                {this.props.t("PROFILE_MANAGE_TWO_FACTOR_AUTHENTICATION_ERROR")}
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default withTranslation()(ManageTwoFactorAuthenticationModal);