import React from 'react';

import Modal from '../components/Modal';
import ModalList from '../components/ModalList';
import userManager from '../utils/UserManager';

import { withRouter } from 'react-router';

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

    availableItems() {
        let items = [];

        if (userManager.isSuperuser()) {
            items.push({
                text: "Parlance Administration",
                onClick: () => {
                    this.props.history.push("/admin");
                    Modal.unmount();
                }
            });
        }

        items.push({
            text: "Account",
            onClick: () => {
                this.props.history.push("/account");
                Modal.unmount();
            }
        });
        
        return items;
    }

    render() {
        return <Modal heading="User Management" buttons={["Close", "Log Out"]} onButtonClick={this.buttonClick.bind(this)}>
            <span>Hi, {userManager.username()}!</span>
            <ModalList>
                {this.availableItems()}
            </ModalList>
        </Modal>
    }
}

export default UserProfileModal;