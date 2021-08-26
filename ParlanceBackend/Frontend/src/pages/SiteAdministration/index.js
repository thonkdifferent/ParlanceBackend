import React from 'react';
import ErrorPage from '../../components/ErrorPage';

import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";

import userManager from '../../utils/UserManager';
import AdministrationSidebar from './AdministrationSidebar';
import ProjectAdministration from './ProjectAdministration';
import PermissionAdministration from './PermissionAdministration';

import Styles from './SiteAdministration.module.css';

class SiteAdministration extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            accessible: false
        }

        userManager.on('userChanged', this.updateAccessibility.bind(this))
    }
    
    async componentDidMount() {
        await this.updateAccessibility();
    }

    async updateAccessibility() {
        this.setState({
            accessible: userManager.isSuperuser()
        });
    }

    render() {
        if (!this.state.accessible) {
            return <ErrorPage message="You don't have access to this page." title="Access Denied" />
        }

        return <div className={Styles.SiteAdministration}>
            <AdministrationSidebar />
            <Switch>
                <Route exact path={`${this.props.match.path}/`}>
                    Hi!
                </Route>
                <Route path={`${this.props.match.path}/projects`}>
                    <ProjectAdministration projectManager={this.props.projectManager} />
                </Route>
                <Route path={`${this.props.match.path}/permissions`}>
                    <PermissionAdministration />
                </Route>
            </Switch>
        </div>
    }
}

export default withRouter(SiteAdministration);