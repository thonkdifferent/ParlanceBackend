import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter,
    Prompt
} from "react-router-dom";

import LanguageIndex from "./LanguageIndex";
import TranslationEditor from "../../../../translationEditor";
import SubprojectSidebar from "../SubprojectSidebar";

class Languages extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <Switch>
            <Route exact path={`${this.props.match.path}/`}>
                <SubprojectSidebar projectManager={this.props.projectManager} />
                <LanguageIndex projectManager={this.props.projectManager} />
            </Route>
            <Route path={`${this.props.match.path}/:language`}>
                {/* <Languages projectManager={this.props.projectManager} /> */}
                <TranslationEditor projectManager={this.props.projectManager} />
            </Route>
        </Switch>
    }
}

export default withRouter(Languages);