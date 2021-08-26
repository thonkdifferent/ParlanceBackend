import React from 'react';

import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter,
    NavLink
} from "react-router-dom";

import Styles from './AdministrationSidebar.module.css';

class AdministrationSidebar extends React.Component {
    render() {
        return <div className={Styles.AdministrationSidebar}>
            <div className={Styles.SidebarHeader}>Parlance Administration</div>
            <NavLink to="/admin/projects" activeClassName={Styles.Selected} className={Styles.SidebarItem}>Projects</NavLink>
            <NavLink to="/admin/permissions" activeClassName={Styles.Selected} className={Styles.SidebarItem}>Permissions</NavLink>
            <NavLink to="/admin/ssh" activeClassName={Styles.Selected} className={Styles.SidebarItem}>SSH</NavLink>
            <NavLink to="/admin/superusers" activeClassName={Styles.Selected} className={Styles.SidebarItem}>Superusers</NavLink>
        </div>
    }
}

export default AdministrationSidebar;