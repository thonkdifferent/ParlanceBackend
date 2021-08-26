import React from 'react';

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';
import ProgressSpinner from "../../../components/ProgressSpinner";


class DemoteSuperuserModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "promote",
            user: ""
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

    async demoteUser() {
        this.changeStage("demoting")

        try {
            await Fetch.delete(`/superusers/${this.props.user}`);

            Modal.unmount();
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            promote: <Modal heading={"Demote User"} buttons={[
                Modal.CancelButton,
                {
                    text: "Demote",
                    onClick: this.demoteUser.bind(this)
                }
            ]}>
                Do you want to demote {this.props.user}?
            </Modal>,
            demoting: <Modal heading="Demote User">
                <ProgressSpinner message={"Demoting the user..."} />
            </Modal>,
            error: <Modal heading="Demote User" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the user could not be demoted.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default DemoteSuperuserModal;