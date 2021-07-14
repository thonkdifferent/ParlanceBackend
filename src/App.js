import React from "react";
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

import './App.css';

import ProjectManager from './utils/ProjectManager';

import Header from './components/Header';
import Footer from './components/Footer';
import Home from './pages/Home';
import Projects from './pages/Projects';

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
        return <Router>
            <div>
                <Header />
                
                <Switch>
                    <Route exact path="/">
                        <Home projectManager={this.state.projectManager} />
                    </Route>
                    <Route path="/projects">
                        <Projects projectManager={this.state.projectManager} />
                    </Route>
                </Switch>

                <Footer />
            </div>
        </Router>
    }
}

export default App;