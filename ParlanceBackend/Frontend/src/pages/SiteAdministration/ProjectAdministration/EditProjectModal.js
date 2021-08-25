import React from 'react';

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';
import ModalList from '../../../components/ModalList';
import ProgressSpinner from "../../../components/ProgressSpinner";

class EditProjectModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "edit",
            name: "",
            gitCloneUrl: "",
            branch: "master"
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

    async deleteProject() {
        this.changeStage("deleting")

        try {
            let projectDetails = await Fetch.delete(`/projects/${this.props.project}`);
            this.props.projectManager.invalidateProjects();
            Modal.unmount();
        } catch {
            this.changeStage("deleteError");
        }
    }

    render() {
        const stages = {
            edit: <Modal heading={this.props.project} buttons={[
                Modal.CancelButton,
                {
                    text: "Delete",
                    onClick: () => this.changeStage("delete")
                }
            ]}>
                If you're looking for subproject settings, they can be found inside the <code>.parlance.json</code> file in the root of the repository.
                <ModalList>
                    {[
                        {
                            text: "Visit Project",
                            onClick: () => window.location = `/projects/${this.props.project}`
                        }
                    ]}
                </ModalList>
            </Modal>,
            delete: <Modal heading="Delete Project" buttons={[
                {
                    text: "Cancel",
                    onClick: () => this.changeStage("edit")
                },
                {
                    text: "Delete",
                    onClick: () => this.deleteProject()
                }
            ]}>
                Delete {this.props.project} from Parlance? You'll need to add it again to use it.
            </Modal>,
            deleting: <Modal heading="Delete Project">
                <ProgressSpinner message={"Deleting the project..."} />
            </Modal>,
            deleteError: <Modal heading="Delete Project" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the project could not be deleted.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default EditProjectModal;