import React from 'react';

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';
import ProgressSpinner from "../../../components/ProgressSpinner";


class PromoteSuperuserModal extends React.Component {
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

    async promoteUser() {
        this.changeStage("promoting")

        try {
            await Fetch.post(`/superusers/${this.state.user}`, {});

            Modal.unmount();
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            promote: <Modal heading={"Promote User"} buttons={[
                Modal.CancelButton,
                {
                    text: "Promote to Superuser",
                    onClick: this.promoteUser.bind(this)
                }
            ]}>
                Enter the user to be promoted to a superuser. Ensure you trust this user entirely, as superusers have the ability to perform any action on Parlance, including removing you as a superuser.
                <Form>
                    <label>Username</label>
                    <input type="text" value={this.state.user} onChange={this.updateStateText.bind(this, "user")} />
                </Form>
            </Modal>,
            promoting: <Modal heading="Promote User">
                <ProgressSpinner message={"Promoting the user..."} />
            </Modal>,
            error: <Modal heading="Promote User" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the user could not be promoted.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default PromoteSuperuserModal;