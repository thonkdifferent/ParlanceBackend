import React from 'react';
import Modal from '../../../components/Modal';

import AdministrationStyles from '../AdministrationStyles.module.css';
import Styles from './ProjectAdministration.module.css';
import AddProjectModal from './AddProjectModal';
import EditProjectModal from './EditProjectModal';

class ProjectAdministration extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            projects: []
        }

        this.props.projectManager.on("projectsUpdated", this.updateProjects.bind(this));
    }
    
    async componentDidMount() {
        await this.updateProjects();
    }

    async updateProjects() {
        this.setState({
            projects: await this.props.projectManager.getAllProjects()
        });
    }

    renderProjects() {
        let items = this.state.projects?.map(project => ({
            text: project.name,
            onClick: () => Modal.mount(<EditProjectModal projectManager={this.props.projectManager} project={project.name} />)
        })) || [];

        items.push({
            text: "Add New Project",
            onClick: () => Modal.mount(<AddProjectModal projectManager={this.props.projectManager} />)
        });

        return items.map(item => <div className={AdministrationStyles.ListItem} onClick={item.onClick.bind(this)}>
            {item.text}
        </div>)
    }

    render() {
        return <div className={AdministrationStyles.PageContainer}>
            <div className={AdministrationStyles.PageTitle}>Projects</div>
            <div className={AdministrationStyles.PageDescription}>Manage the projects available within Parlance.</div>
            {this.renderProjects()}
        </div>
    }
}

export default ProjectAdministration;