import React from 'react';

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';
import ProgressSpinner from "../../../components/ProgressSpinner";


class AcquireHostKeyModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "acquire",
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

    async acquireKey() {
        this.changeStage("acquiring")

        try {
            await Fetch.post("/ssh/hostKeys", {
                host: this.state.host
            });
            
            Modal.unmount();
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            acquire: <Modal heading={"Add Host Key"} buttons={[
                Modal.CancelButton,
                {
                    text: "Retrieve Host Key",
                    onClick: this.acquireKey.bind(this)
                }
            ]}>
                Enter the host for which to retrieve and add the key. Once the host key is added, Parlance will trust any host that presents that key.
                <Form>
                    <label>Host</label>
                    <input type="text" placeholder={"github.com"} value={this.state.host} onChange={this.updateStateText.bind(this, "host")} />
                </Form>
            </Modal>,
            acquiring: <Modal heading="Add Host Key">
                <ProgressSpinner message={"Retrieving the host key..."} />
            </Modal>,
            error: <Modal heading="Add Host Key" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the host key could not be added.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default AcquireHostKeyModal;