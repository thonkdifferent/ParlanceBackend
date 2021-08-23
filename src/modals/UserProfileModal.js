import React from 'react';

import Modal from '../components/Modal';
import userManager from '../utils/UserManager';

class UserProfileModal extends React.Component {
    constructor(props) {
        super(props);
    }

    buttonClick(button) {
        let buttonActions = {
            "Close": () => Modal.unmount(),
            "Log Out": async () => {
                await userManager.logout()
                Modal.unmount();
            }
        }

        buttonActions[button]();
    }

    render() {
        return <Modal heading="User Management" buttons={["Close", "Log Out"]} onButtonClick={this.buttonClick.bind(this)}>
            
        </Modal>
    }
}

export default UserProfileModal;