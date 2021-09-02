import React from 'react';
import Styles from './Header.module.css';
import {
  Link,
  withRouter
} from "react-router-dom";
import UserIndicator from './UserIndicator';
import { matchPath } from "react-router";

import languageManager from "../utils/LanguageManager";

import Icon from "../assets/icon.svg";

function SnackbarPart(props) {
    return <Link to={props.to} className={Styles.SnackbarPart}>
        <div className={Styles.SnackbarPointer} />
        <div className={Styles.SnackbarInnerPointer} />
        <div className={Styles.SnackbarInner}>
            {props.children}
        </div>
    </Link>
}

class Header extends React.Component {
    constructor(props) {
        super(props);
        
        this.state = {
            path: window.location.pathname,
            projects: []
        };
        
    }
    
    async componentDidMount() {
        this.setState({
            projects: await this.props.projectManager.getAllProjects()
        })
    }

    renderSnackbar() {
        if (!this.state.projects) return [];
        // let params = this.props.match.params;
        let param = type => {
            let match = matchPath(window.location.pathname, {
                path: "/projects/:project?/:subproject?/:language?",
                exact: false,
                strict: false
            });
            if (match?.params[type]) return match.params[type];
        };
        
        let parts = [];
        
        let project = param("project");
        if (project) {
            let currentProject = this.state.projects.find(proj => proj.name === project);
            parts.push(<SnackbarPart to={`/projects/${project}`}>{project}</SnackbarPart>);
            
            let subproject = param("subproject");
            if (subproject && currentProject) {
                let currentSubproject = currentProject.subprojects.find(subproj => subproj.slug === subproject);
                parts.push(<SnackbarPart to={`/projects/${project}/${subproject}`}>{currentSubproject.name || "Unidentified Subproject"}</SnackbarPart>)
                
                let language = param("language");
                if (language && currentSubproject) {
                    parts.push(<SnackbarPart to={`/projects/${project}/${subproject}/${language}`}>{languageManager.getLanguage(language)?.name || "Unidentified Language"}</SnackbarPart>)
                }
            }
        }
        
        return parts;
    }

    render() {
        console.log(this.props);
        return <div className={Styles.Header}>
            <div className={Styles.SnackbarContainer}>
                <SnackbarPart to={"/"}><img src={Icon} className={Styles.Icon} /></SnackbarPart>
                <SnackbarPart to={"/projects"}>Projects</SnackbarPart>
                {this.renderSnackbar()}
            </div>
            <div style={{flexGrow: "1"}}></div>
            <UserIndicator />
        </div>
    }
}

export default withRouter(Header);