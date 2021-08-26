import React from 'react';

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';
import ProgressSpinner from "../../../components/ProgressSpinner";

import Styles from './GenerateSshKeyModal.module.css';
import ModalList from "../../../components/ModalList";

class GenerateSshKeyModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "generating"
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
        await this.generateKey();
    }
    
    componentWillUnmount() {
        this.props.onDone();
    }

    async generateKey() {
        this.changeStage("generating")

        try {
            let key = await Fetch.post("/ssh/publicKey", {});
            
            this.setState({
                key: key.sshKeyContents,
                stage: "done"
            })
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            generating: <Modal heading="Generate SSH Key">
                <ProgressSpinner message={"Generating a new SSH key..."} />
            </Modal>,
            done: <Modal heading="Generate SSH Key" buttons={[
                Modal.OkButton
            ]}>
                The SSH key has been generated.
                <code className={Styles.SshKey}>{this.state.key}</code>
                Register this public key with the Git servers you'd like to use.
                <ModalList>
                    {[
                        {
                            text: "Copy Key",
                            onClick: () => navigator.clipboard.writeText(this.state.key)
                        }
                    ]}
                </ModalList>
            </Modal>,
            error: <Modal heading="Generate SSH Key" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the SSH key could not be generated.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default GenerateSshKeyModal;