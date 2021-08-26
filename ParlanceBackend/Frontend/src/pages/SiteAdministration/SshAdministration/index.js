import React from 'react';
import Modal from '../../../components/Modal';

import AdministrationStyles from '../AdministrationStyles.module.css';
import Styles from './SshAdministration.module.css';
import Fetch from "../../../utils/Fetch";
import GenerateSshKeyModal from "./GenerateSshKeyModal";
import DeleteSshKeyModal from "./DeleteSshKeyModal";
import AcquireHostKeyModal from "./AcquireHostKeyModal";
import EditHostKeyModal from "./EditHostKeyModal";

class SshAdministration extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            sshKey: null
        }
    }
    
    async componentDidMount() {
        await this.updateKeys();
    }

    async updateKeys() {
        let sshKey;
        let hostKeys;
        try {
            sshKey = await Fetch.get("/ssh/publicKey");
            hostKeys = await Fetch.get("/ssh/hostKeys");
        } catch {
            sshKey = null;
            hostKeys = [];
        }
        
        
        this.setState({
            sshKey: sshKey?.sshKeyContents,
            hostKeys: hostKeys
        });
    }

    renderHostKeys() {
        let items = this.state.hostKeys?.map((key, index) => {
            let parts = key.split(" ");
            let name = parts.shift();
            return {
                text: name,
                subText: parts.join(" "),
                onClick: () => Modal.mount(<EditHostKeyModal hostKey={key} index={index} onDone={this.updateKeys.bind(this)}/>)
            };
        }) || [];

        items.push({
            text: "Add New Host Key",
            onClick: () => Modal.mount(<AcquireHostKeyModal onDone={this.updateKeys.bind(this)} />)
        });

        return <div className={AdministrationStyles.Section}>
            <h2 className={AdministrationStyles.SectionHeader}>Host Keys</h2>
            <span className={AdministrationStyles.SectionExplanation}>Before connecting to an SSH host, Parlance will check to ensure that the host key has been imported.</span>
            {items.map(item => <div className={AdministrationStyles.ListItem} onClick={item.onClick.bind(this)}>
                {item.text}
                <span className={Styles.KeyValue}>{item.subText}</span>
            </div>)}
        </div>
    }
    
    renderSshKey() {
        let keyContents;
        if (this.state.sshKey) {
            keyContents = <>
                <code className={Styles.SshKey}>{this.state.sshKey}</code>
                <div className={AdministrationStyles.ListItem} onClick={() => navigator.clipboard.writeText(this.state.sshKey)}>
                    Copy SSH key
                </div>
                <div className={AdministrationStyles.ListItem} onClick={() => Modal.mount(<DeleteSshKeyModal onDone={this.updateKeys.bind(this)} />)}>
                    Delete SSH key
                </div>
            </>
        } else {
            keyContents = <>
                <div className={AdministrationStyles.ListItem} onClick={() => Modal.mount(<GenerateSshKeyModal onDone={this.updateKeys.bind(this)} />)}>
                    Generate a new SSH key
                </div>
            </>
        }
        
        return <div className={AdministrationStyles.Section}>
            <h2 className={AdministrationStyles.SectionHeader}>SSH Key</h2>
            <span className={AdministrationStyles.SectionExplanation}>When using Git over SSH, Parlance will present an SSH key to the server.</span>
            {keyContents}
        </div>
    }

    render() {
        return <div className={AdministrationStyles.PageContainer}>
            <div className={AdministrationStyles.PageTitle}>SSH</div>
            <div className={AdministrationStyles.PageDescription}>Manage SSH keys for connecting to Git repositories.</div>
            {this.renderSshKey()}
            {this.renderHostKeys()}
        </div>
    }
}

export default SshAdministration;