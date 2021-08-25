import React from 'react';

import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter,
    NavLink
} from "react-router-dom";

import Styles from './SubprojectSidebar.module.css';

class SubprojectSidebar extends React.Component {
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
        let match = this.props.match.params;
        return this.state.parentProject?.subprojects.map(subproject => <NavLink className={Styles.SidebarItem} activeClassName={Styles.Selected} to={`/projects/${match.project}/${subproject.slug}`}>{subproject.name}</NavLink>)
    }
    
    render() {
        if (this.props.match.params.language) {
            return null
        } else {
            return <div className={Styles.ProjectSidebar}>
                <div className={Styles.SidebarHeader}>{this.state.parentProject?.name || "Parlance"}</div>
                {this.renderSubprojects()}
            </div>
        }
    }
}

export default withRouter(SubprojectSidebar);