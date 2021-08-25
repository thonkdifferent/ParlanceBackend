import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";

import Styles from "./index.module.css";
import { withTranslation } from 'react-i18next';

import SubprojectIndex from "./SubprojectIndex";
import Languages from "./Languages";
import SubprojectSidebar from "./SubprojectSidebar";
import ErrorPage from "../../../components/ErrorPage";

class Subprojects extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <div className={Styles.Subprojects}>
            <Switch>
                <Route exact path={`${this.props.match.path}/`}>
                    <SubprojectSidebar projectManager={this.props.projectManager} />
                    {/*<SubprojectIndex projectManager={this.props.projectManager} />*/}
                    <ErrorPage title={this.props.t("SELECT_LANGUAGE")} message="Select a language to get started!" />
                </Route>
                <Route path={`${this.props.match.path}/:subproject`}>
                    <Languages projectManager={this.props.projectManager} />
                </Route>
            </Switch>
        </div>
    }
}

export default withRouter(withTranslation()(Subprojects));