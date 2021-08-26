import React from 'react';
import Modal from '../../../components/Modal';

import AdministrationStyles from '../AdministrationStyles.module.css';
import Fetch from "../../../utils/Fetch";
import PromoteSuperuserModal from "./PromoteSuperuserModal";
import DemoteSuperuserModal from "./DemoteSuperuserModal";

class SuperusersAdministration extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            superusers: []
        }
    }

    async componentDidMount() {
        await this.updateSuperusers();
    }

    async updateSuperusers() {
        let superusers;
        try {
            superusers = await Fetch.get("/superusers");
        } catch {
            superusers = [];
        }

        this.setState({
            superusers: superusers
        });
    }
    
    renderSuperusers() {
        let items = this.state.superusers?.map(superuser => ({
            text: superuser,
            onClick: () => Modal.mount(<DemoteSuperuserModal user={superuser} onDone={this.updateSuperusers.bind(this)} />)
        })) || [];

        items.push({
            text: "Promote User to Superuser",
            onClick: () => Modal.mount(<PromoteSuperuserModal onDone={this.updateSuperusers.bind(this)} />)
        });

        return items.map(item => <div className={AdministrationStyles.ListItem} onClick={item.onClick.bind(this)}>
            {item.text}
        </div>)
    }

    render() {
        return <div className={AdministrationStyles.PageContainer}>
            <div className={AdministrationStyles.PageTitle}>Superusers</div>
            <div className={AdministrationStyles.PageDescription}>Manage superusers.</div>
            {this.renderSuperusers()}
        </div>
    }
}

export default SuperusersAdministration;