import React from 'react';
import Styles from './UserIndicator.module.css';
import {
  Link,
  withRouter
} from "react-router-dom";

import userManager from '../utils/UserManager';

class UserIndicator extends React.Component {
    constructor(props) {
        super(props);

        userManager.on("userChanged", this.userChanged.bind(this));

        this.state = {
            loggedIn: false,
            username: "Log In"
        }

        this.userChanged();
    }

    userChanged() {
        this.setState({
            loggedIn: userManager.loggedIn(),
            username: userManager.username()
        });
    }

    renderContents() {
        if (this.state.loggedIn) {
            return <>
                <img className={Styles.ProfilePicture} src={userManager.profilePictureUrl()} />
                {this.state.username}
            </>;
        } else {
            return "Log In";
        }
    }

    render() {
        return <div className={Styles.UserIndicator} onClick={userManager.openLoginModal.bind(userManager, this.props.history)}>
            {this.renderContents()}
        </div>
    }
}

export default withRouter(UserIndicator);