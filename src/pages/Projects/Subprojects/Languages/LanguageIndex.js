import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";
import tags from "language-tags";

import Index from "../../../../components/Index";
import Modal from "../../../../components/Modal";

class ProjectLanguageSelect extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            projects: []
        }
    }

    async componentDidMount() {
        this.setState({
            parentSubproject: (await this.props.projectManager.getAllProjects()).find(project => project.name === this.props.match.params.project).subprojects.find(subproject => subproject.slug === this.props.match.params.subproject)
        });
    }

    addLanguage() {
        let buttonClick = button => {
            if (button === "Cancel") {
                Modal.unmount();
            }
        };

        Modal.mount(<Modal heading="Add a language" buttons={["Cancel", "Add Language"]} onButtonClick={buttonClick}>
            Which language do you want to add?
        </Modal>)
    }

    renderLanguages() {
        return [
            ...(this.state.parentSubproject?.languages.filter(lang => lang.identifier !== this.state.parentSubproject.baseLang).map(lang => <Link to={`${this.props.match.url}/${lang.identifier}`}>{tags(lang.identifier.replace("_", "-")).language().descriptions()[0]} ({lang.identifier})</Link>) || []),
            <div onClick={this.addLanguage.bind(this)}>Add a language</div>
        ]
    }

    render() {
        return <Index title={this.state.parentSubproject?.name}>
            {this.renderLanguages()}
        </Index>;
    }
}

export default withRouter(ProjectLanguageSelect);