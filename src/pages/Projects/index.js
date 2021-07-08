import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";

import ProjectIndex from "./ProjectIndex";
import Subprojects from "./Subprojects";

class Projects extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <Switch>
            <Route exact path={`${this.props.match.path}/`}>
                <ProjectIndex projectManager={this.props.projectManager} />
            </Route>
            <Route path={`${this.props.match.path}/:project`}>
                <Subprojects projectManager={this.props.projectManager} />
            </Route>
        </Switch>
    }
}

export default withRouter(Projects);