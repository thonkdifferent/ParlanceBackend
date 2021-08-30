import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";
import { withTranslation } from 'react-i18next';

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
        return this.state.projects.map(project => <div onClick={() => this.props.history.push(`${this.props.match.url}/${project.name}`)}>{project.name}</div>)
    }

    render() {
        return <Index title={this.props.t("PROJECT_SELECT_PROMPT")}>
            {this.renderProjects()}
        </Index>;
    }
}

export default withRouter(withTranslation()(ProjectIndex));