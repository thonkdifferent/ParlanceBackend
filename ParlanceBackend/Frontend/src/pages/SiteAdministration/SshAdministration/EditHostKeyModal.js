import React from 'react';

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';
import ProgressSpinner from "../../../components/ProgressSpinner";

import Styles from './EditHostKeyModal.module.css'

class EditHostKeyModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "edit",
            host: ""
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

    componentWillUnmount() {
        this.props.onDone();
    }

    async deleteKey() {
        this.changeStage("deleting")

        try {
            await Fetch.delete(`/ssh/hostKeys/${this.props.index}`, {
                host: this.state.host
            });

            Modal.unmount();
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            edit: <Modal heading={"View Host Key"} buttons={[
                Modal.CancelButton,
                {
                    text: "Delete Host Key",
                    type: "destructive",
                    onClick: this.changeStage.bind(this, "confirmDelete")
                }
            ]}>
                <code className={Styles.SshKey}>{this.props.hostKey}</code>
            </Modal>,
            confirmDelete: <Modal heading={"Delete Host Key"} buttons={[
                {
                    text: "Cancel",
                    onClick: this.changeStage.bind(this, "edit")
                },
                {
                    text: "Delete",
                    type: "destructive",
                    onClick: this.deleteKey.bind(this)
                }
            ]}>
                Delete this host key?
            </Modal>,
            deleting: <Modal heading="Delete Host Key">
                <ProgressSpinner message={"Deleting the host key..."} />
            </Modal>,
            error: <Modal heading="Delete Host Key" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the host key could not be deleted.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default EditHostKeyModal;