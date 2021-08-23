import React from 'react';
import Styles from './Header.module.css';
import {
  Link,
  withRouter
} from "react-router-dom";
import UserIndicator from './UserIndicator';

import Icon from "../assets/icon-dark.svg";

class Header extends React.Component {
    constructor(props) {
        super(props);
    }

    renderSnackbar() {
        let params = this.props.match.params;
        if (params.project) {
            <Link to={`/projects/${params.project}`}>{params.project}</Link>
        }
    }

    render() {
        return <div className={Styles.Header}>
            <Link to="/"><img src={Icon} className={Styles.Icon} /></Link>
            <Link to="/projects">Projects</Link>
            {this.renderSnackbar()}
            <div style={{flexGrow: "1"}}></div>
            <UserIndicator />
        </div>
    }
}

export default withRouter(Header);