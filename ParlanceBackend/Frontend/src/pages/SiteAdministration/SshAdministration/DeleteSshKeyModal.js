import React from 'react';

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';
import ProgressSpinner from "../../../components/ProgressSpinner";


class DeleteSshKeyModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "confirmDelete"
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
            await Fetch.delete("/ssh/publicKey");
            Modal.unmount();
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            confirmDelete: <Modal heading={"Delete SSH Key"} buttons={[
                Modal.CancelButton,
                {
                    text: "Delete",
                    onClick: this.deleteKey.bind(this)
                }
            ]}>
                Deleting this SSH key will allow you to generate a new SSH key within Parlance, but the old SSH key will no longer be used and cannot be recovered.
                <br />
                <br />
                Delete this SSH key?
            </Modal>,
            deleting: <Modal heading="Delete SSH Key">
                <ProgressSpinner message={"Deleting the SSH key..."} />
            </Modal>,
            error: <Modal heading="Delete SSH Key" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the SSH key could not be deleted.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default DeleteSshKeyModal;