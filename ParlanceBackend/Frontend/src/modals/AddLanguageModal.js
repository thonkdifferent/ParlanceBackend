import React from 'react';

import Fetch from '../utils/Fetch';
import Modal from '../components/Modal';
import ModalList from '../components/ModalList';

import userManager from '../utils/UserManager';

class AddLanguageModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: userManager.loggedIn() ? "add" : "login"
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

    async addLanguage(language) {
        this.changeStage("adding");

        try {
            let projectDetails = await Fetch.post(`/projects/${this.props.project}/${this.props.subproject}/${language}/create`, {});
            Modal.unmount();
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            login: <Modal heading="Add a language" buttons={[
                Modal.OkButton
            ]}>
                To add a language, you'll need to log in to Parlance.
            </Modal>,
            add: <Modal heading="Add a language" buttons={[
                Modal.CancelButton
            ]}>
                Which language do you want to add?
                <ModalList>
                {this.props.languages.map(language => ({
                    text: language.name,
                    onClick: async () => {
                        this.addLanguage(language.identifier)
                    }
                }))}
                </ModalList>
            </Modal>,
            adding: <Modal heading="Add a language">
                <Modal.ModalProgressSpinner message={"Adding the language..."} />
            </Modal>,
            error: <Modal heading="Add a language" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the language could not be added.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default AddLanguageModal;