import React from 'react';

import AdministrationStyles from '../AdministrationStyles.module.css';
import Styles from './PermissionAdministration.module.css';
import Modal from "../../../components/Modal";
import EditLanguagePermissionsModal from "./EditLanguagePermissionsModal";

class PermissionAdministration extends React.Component {
    editLanguagePermissions() {
        Modal.mount(<EditLanguagePermissionsModal />);
    }
    
    render() {
        return <div className={AdministrationStyles.PageContainer}>
            <div className={AdministrationStyles.PageTitle}>Permissions</div>
            <div className={AdministrationStyles.PageDescription}>Grant and deny users permissions on Parlance.</div>
            <div className={AdministrationStyles.ListItem} onClick={this.editLanguagePermissions.bind(this)}>
                Edit Language Permissions
            </div>
        </div>
    }
}

export default PermissionAdministration;