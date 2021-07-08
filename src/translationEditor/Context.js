import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter,
    Prompt
} from "react-router-dom";

import Style from "./Context.module.css";
import TranslationItem from './TranslationItem';

class Context extends React.Component {
    constructor(props) {
        super(props);
        
        this.props.poManager.on("translationsChanged", () => {
            this.forceUpdate()
        });
    }

    async componentDidMount() {
        //Grab the po file from the server. Here we'll hardcode it.
    }

    render() {
        return <>
            <div className={Style.ContextHeader}>{this.props.context}</div>
            {this.props.poManager.getKeys(this.props.context).map(key => <TranslationItem key={key} searchQuery={this.props.searchQuery} flags={this.props.flags} poManager={this.props.poManager} onSelect={this.props.onSelect} context={this.props.context} translationKey={key} selection={this.props.selection} />)}
        </>
    }
}

export default withRouter(Context);