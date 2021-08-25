import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";
import tags from "language-tags";

import languageManager from '../../../../utils/LanguageManager';

import Index from "../../../../components/Index";
import Modal from "../../../../components/Modal";
import ModalList from '../../../../components/ModalList';
import Fetch from '../../../../utils/Fetch';
import AddLanguageModal from '../../../../modals/AddLanguageModal';

class ProjectLanguageSelect extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            projects: [],
            error: ""
        }
    }

    async componentDidMount() {
        try {
            this.setState({
                parentSubproject: (await this.props.projectManager.getAllProjects()).find(project => project.name === this.props.match.params.project).subprojects.find(subproject => subproject.slug === this.props.match.params.subproject)
            });
        } catch {
            this.setState({
                error: "notFound"
            });
        }
    }

    addLanguage() {
        let buttonClick = button => {
            if (button === "Cancel") {
                Modal.unmount();
            }
        };

        Modal.mount(<AddLanguageModal project={this.props.match.params.project} subproject={this.state.parentSubproject.slug} languages={Object.values(languageManager.languages)} />)
    }

    renderLanguages() {
        return [
            ...(this.state.parentSubproject?.languages.filter(lang => lang.identifier !== this.state.parentSubproject.baseLang).map(lang => <Link to={`${this.props.match.url}/${lang.identifier}`}>{tags(lang.identifier.replace("_", "-")).language().descriptions()[0]} ({lang.identifier})</Link>) || []),
            <div onClick={this.addLanguage.bind(this)}>Add a language</div>
        ]
    }

    render() {
        if (this.state.error) {
            return <div>
                We couldn't find that project.
            </div>
        } else {
            return <Index title={this.state.parentSubproject?.name}>
                {this.renderLanguages()}
            </Index>;
        }
    }
}

export default withRouter(ProjectLanguageSelect);