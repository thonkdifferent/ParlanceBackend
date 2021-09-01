import React from "react";

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';
import ProgressSpinner from "../../../components/ProgressSpinner";
import languageManager from "../../../utils/LanguageManager";
import ModalList from "../../../components/ModalList";

class EditLanguagePermissionsModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "loading",
            lang: ""
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
    
    async componentDidMount() {
        await this.fetchPermissions();
    }
    
    async fetchPermissions(nextStage = "langSelect") {
        this.changeStage("loading");
        
        try {
            this.setState({
                permissions: await Fetch.get("/permissions/languages")
            })
            this.changeStage(nextStage);
        } catch {
            this.changeStage("error")
        }
    }

    async performGrantPermissions() {
        if (!this.state.user) return;
        this.changeStage("processing")

        try {
            await Fetch.post(`/permissions/languages/${this.state.lang.identifier}/${this.state.user}`, {});
            await this.fetchPermissions("userSelect");
        } catch {
            this.changeStage("grantError");
        }
    }

    async performRevokePermissions() {
        this.changeStage("processing")

        try {
            await Fetch.delete(`/permissions/languages/${this.state.lang.identifier}/${this.state.user}`);
            await this.fetchPermissions("userSelect");
        } catch {
            this.changeStage("denyError");
        }
    }

    render() {
        const stages = {
            loading: <Modal heading="Edit Language Permissions">
                <ProgressSpinner message={"Loading permissions..."} />
            </Modal>,
            langSelect: <Modal heading="Edit Language Permissions" buttons={[
                Modal.CancelButton
            ]}>
                Select a language to edit
                <ModalList>
                    {Object.values(languageManager.sortedLanguages()).map(language => ({
                        text: language.name,
                        onClick: () => {
                            this.setState({
                                lang: language
                            }, this.changeStage.bind(this, "userSelect"))
                        }
                    }))}
                </ModalList>
            </Modal>,
            userSelect: <Modal heading={`Edit permissions for ${this.state.lang.name}`} buttons={[
                {
                    text: "Back",
                    onClick: this.changeStage.bind(this, "langSelect")
                },
                {
                    text: "Grant to new user",
                    onClick: () => {
                        this.setState({
                            stage: "grant",
                            user: ""
                        })
                    }
                }
            ]}>
                Select a user to remove permissions for, or grant permissions to a new user
                <ModalList>
                    {this.state.permissions?.filter(permission => permission.language === this.state.lang.identifier)
                        .map(permission => ({
                            text: permission.userName,
                            onClick: () => {
                                this.setState({
                                    stage: "deny",
                                    user: permission.userName
                                })
                            }
                        }))}
                </ModalList>
            </Modal>,
            grant: <Modal heading={`Grant permissions to edit ${this.state.lang.name}`} buttons={[
                {
                    text: "Back",
                    onClick: this.changeStage.bind(this, "userSelect")
                },
                {
                    text: "Grant",
                    onClick: this.performGrantPermissions.bind(this)
                }
            ]}>
                Granting this permission to a user will allow the user to make edits to the {this.state.lang.name} language files across the entire website.
                <Form>
                    <label>Username</label>
                    <input type="text" value={this.state.user} onChange={this.updateStateText.bind(this, "user")} />
                </Form>
            </Modal>,
            deny: <Modal heading={`Revoke permissions to edit ${this.state.lang.name}`} buttons={[
                {
                    text: "Back",
                    onClick: this.changeStage.bind(this, "userSelect")
                },
                {
                    text: "Revoke",
                    type: "destructive",
                    onClick: this.performRevokePermissions.bind(this)
                }
            ]}>
                Revoking this permission from {this.state.user} will no longer allow the user to make edits to the {this.state.lang.name} language files across the entire website.
            </Modal>,
            processing: <Modal heading={`Grant permissions to edit ${this.state.lang.name}`}>
                <ProgressSpinner message={"Processing permissions changes..."} />
            </Modal>,
            grantError: <Modal heading={`Grant permissions to edit ${this.state.lang.name}`} buttons={[
                {
                    text: "Back",
                    onClick: this.changeStage.bind(this, "grant")
                }
            ]}>
                Sorry, permissions could not be granted.
            </Modal>,
            denyError: <Modal heading={`Revoke permissions to edit ${this.state.lang.name}`} buttons={[
                {
                    text: "Back",
                    onClick: this.changeStage.bind(this, "userSelect")
                }
            ]}>
                Sorry, permissions could not be revoked.
            </Modal>,
            error: <Modal heading="Edit Language Permissions" buttons={[
                Modal.OkButton
            ]}>
                Sorry, there was a problem.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default EditLanguagePermissionsModal;