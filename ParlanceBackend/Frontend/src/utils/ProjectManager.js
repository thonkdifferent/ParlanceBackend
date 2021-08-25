import Fetch from './Fetch';

import EventEmitter from 'eventemitter3';

class ProjectManager extends EventEmitter {
    projects;
    getter;

    constructor() {
        super();

        this.projects = null;
        this.getter = null;
    }

    async getAllProjects() {
        if (this.projects) return this.projects;
        
        if (this.getter) {
            return await this.getter;
        } else {
            this.getter = (async () => {
                this.projects = await Fetch.get("/projects");
                this.getter = null;
                return this.projects;
            })();
            return await this.getter;
        }
    }

    invalidateProjects() {
        this.projects = null;

        this.emit("projectsUpdated");
    }
}

export default ProjectManager;