import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";

import Index from "../../components/Index";

class ProjectIndex extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            projects: []
        }
    }

    async componentDidMount() {
        this.setState({
            projects: await this.props.projectManager.getAllProjects()
        });
    }

    renderProjects() {
        return this.state.projects.map(project => <Link to={`${this.props.match.url}/${project.name}`}>{project.name}</Link>)
    }

    render() {
        return <Index title="Select a project">
            {this.renderProjects()}
        </Index>;
    }
}

export default withRouter(ProjectIndex);