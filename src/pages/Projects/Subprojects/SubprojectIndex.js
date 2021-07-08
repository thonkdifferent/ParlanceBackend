import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";

import Index from "../../../components/Index";

class ProjectLanguageSelect extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            parentProject: null
        }
    }

    async componentDidMount() {
        this.setState({
            parentProject: (await this.props.projectManager.getAllProjects()).find(project => project.name === this.props.match.params.project)
        });
    }

    renderSubprojects() {
        return this.state.parentProject?.subprojects.map(subproject => <Link to={`${this.props.match.url}/${subproject.slug}`}>{subproject.name}</Link>)
    }

    render() {
        return <Index title={this.props.match.params.project}>
            {this.renderSubprojects()}
        </Index>;
    }
}

export default withRouter(ProjectLanguageSelect);