import React from 'react';

import Fetch from '../../../utils/Fetch';
import Modal from '../../../components/Modal';
import Form from '../../../components/Form';



class AddProjectModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            stage: "create",
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

    async addProject() {
        this.changeStage("adding")

        try {
            let projectDetails = await Fetch.post("/projects", {
                name: this.state.name,
                gitCloneUrl: this.state.gitCloneUrl,
                branch: this.state.branch
            });

            this.props.projectManager.invalidateProjects();
            Modal.unmount();
        } catch {
            this.changeStage("error");
        }
    }

    render() {
        const stages = {
            create: <Modal heading="Add New Project" buttons={[
                Modal.CancelButton,
                {
                    text: "Add",
                    onClick: this.addProject.bind(this)
                }
            ]}>
                Ensure that the project contains a <code>.parlance.json</code> file in the root.
                <Form>
                    <label>Project Name</label>
                    <input type="text" value={this.state.name} onChange={this.updateStateText.bind(this, "name")} />

                    <label>Git Clone URL</label>
                    <input type="text" value={this.state.gitCloneUrl} onChange={this.updateStateText.bind(this, "gitCloneUrl")} />

                    <label>Branch</label>
                    <input type="text" value={this.state.branch} onChange={this.updateStateText.bind(this, "branch")} />
                </Form>
            </Modal>,
            adding: <Modal heading="Add New Project">
                <Modal.ModalProgressSpinner message={"Cloning Repository..."} />
            </Modal>,
            error: <Modal heading="Add New Project" buttons={[
                Modal.OkButton
            ]}>
                Sorry, the project could not be added.
            </Modal>
        }

        return stages[this.state.stage];
    }
}

export default AddProjectModal;