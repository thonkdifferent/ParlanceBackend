import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";

import SubprojectIndex from "./SubprojectIndex";
import Languages from "./Languages";

class Subprojects extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <Switch>
            <Route exact path={`${this.props.match.path}/`}>
                <SubprojectIndex projectManager={this.props.projectManager} />
            </Route>
            <Route path={`${this.props.match.path}/:subproject`}>
                <Languages projectManager={this.props.projectManager} />
            </Route>
        </Switch>
    }
}

export default withRouter(Subprojects);