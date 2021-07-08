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

    renderLanguages() {
        return [
            ...(this.state.parentSubproject?.languages.map(lang => <Link to={`${this.props.match.url}/${lang.identifier}`}>{tags(lang.identifier.replace("_", "-")).language().descriptions()[0]} ({lang.identifier})</Link>) || []),
            <div>Add a language</div>
        ]
    }

    render() {
        return <Index title={this.state.parentSubproject?.name}>
            {this.renderLanguages()}
        </Index>;
    }
}

export default withRouter(ProjectLanguageSelect);