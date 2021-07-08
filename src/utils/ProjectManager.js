import Fetch from './Fetch';

class ProjectManager {
    projects;

    constructor() {
        this.projects = null;
    }

    async getAllProjects() {
        if (this.projects) return this.projects;
        
        this.projects = await Fetch.get("/projects");
        return this.projects;
    }
}

export default ProjectManager;