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
import userManager from "../../../../utils/UserManager";

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
        await this.updateParentSubproject();
    }
    
    async componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps !== this.props) {
            await this.updateParentSubproject();
        }
    }
    
    async updateParentSubproject() {
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
        Modal.mount(<AddLanguageModal project={this.props.match.params.project} subproject={this.state.parentSubproject.slug} languages={Object.values(userManager.allowedLanguages()).filter(language => {
            let knownLanguages = this.state.parentSubproject.languages.map(language => language.identifier);
            for (let lang of knownLanguages) {
                if (lang.toLowerCase() === language.identifier.toLowerCase()) return false;
                if (lang.toLowerCase() === language.identifier.replace("-", "_").toLowerCase()) return false;
            }
            return true;
        })} history={this.props.history} />)
    }

    renderLanguages() {
        return [
            ...(this.state.parentSubproject?.languages.filter(lang => lang.identifier !== this.state.parentSubproject.baseLang).map(lang => <Link to={`${this.props.match.url}/${lang.identifier}`}>{languageManager.getLanguage(lang.identifier)?.name}</Link>) || []),
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