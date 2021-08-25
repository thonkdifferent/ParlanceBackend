import React from "react";
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

import './App.css';

import ProjectManager from './utils/ProjectManager';
import SiteAdministration from "./pages/SiteAdministration";
import UserAdministration from "./pages/UserAdministration";

import Header from './components/Header';
import Footer from './components/Footer';
import Home from './pages/Home';
import Projects from './pages/Projects';

import Styles from "./App.module.css"
import ProgressSpinner from "./components/ProgressSpinner";

// This site has 3 pages, all of which are rendered
// dynamically in the browser (not server rendered).
//
// Although the page does not ever refresh, notice how
// React Router keeps the URL up to date as you navigate
// through the site. This preserves the browser history,
// making sure things like the back button and bookmarks
// work properly.

class App extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            projectManager: new ProjectManager()
        };
    }

    render() {
        return <React.Suspense fallback={<ProgressSpinner style={{height: "100vh"}} message="Parlance" />}>
            <Router>
                <div className={Styles.MainContainer}>
                    <Header projectManager={this.state.projectManager} />
                    <Switch>
                        <Route exact path="/">
                            <Home projectManager={this.state.projectManager} />
                        </Route>
                        <Route path="/projects">
                            <Projects projectManager={this.state.projectManager} />
                        </Route>
                        <Route path="/account">
                            <UserAdministration />
                        </Route>
                        <Route path="/admin">
                            <SiteAdministration projectManager={this.state.projectManager} />
                        </Route>
                    </Switch>

                    <Footer />
                </div>
            </Router>
        </React.Suspense>
    }
}

export default App;