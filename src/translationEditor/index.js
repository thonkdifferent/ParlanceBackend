import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter,
    Prompt
} from "react-router-dom";
import hotkeys from "react-keyboard-shortcuts";
import Fetch from "../utils/Fetch";

import ContextSearch from './ContextSearch';

import PoManager from "./PoManager";
import Styles from "./index.module.css";
import Context from "./Context";
import TranslationArea from './TranslationArea';
import TranslationItem from './TranslationItem';

class TranslationEditor extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            po: null,
            selection: null,
            searchQuery: "",
            flags: {
                unfinishedOnly: false
            }
        }
    }

    async componentDidMount() {
        //Grab the po file from the server. Here we'll hardcode it.
        let params = this.props.match.params;
        let fileContents = await (await fetch(`/api/projects/${params.project}/${params.subproject}/${params.language}/gettext`, {
            method: "GET"
        })).text();

        let po = new PoManager(fileContents, this.props.match.params.language);
        po.on("translationsChanged", () => {
            this.forceUpdate()
        });

        this.setState({
            po
        });
    }

    hot_keys = {
        'ctrl+s': {
            priority: 1,
            handler: (e) => {
                alert("control S")
                e.preventDefault();
            }
        },
        'alt+down': {
            priority: 1,
            handler: (e) => {
                e.preventDefault();
                this.setState(oldState => {
                    return {
                        selection: this.getNextItem()
                    }
                });
            }
        },
        'alt+up': {
            priority: 1,
            handler: (e) => {
                e.preventDefault();
                this.setState(oldState => {
                    return {
                        selection: this.getPreviousItem()
                    }
                });
            }
        }
    }
    
    select(selection) {
        this.setState({
            selection
        });
    }

    search(query) {
        this.setState({
            searchQuery: query
        })
    }

    setFlags(flags) {
        this.setState({
            flags
        })
    }

    getNextItem() {
        let nextSelection = this.state.selection;
        let firstSelection = this.state.selection;
        do {
            nextSelection = this.state.po.nextSelection(nextSelection?.context, nextSelection?.key);
            if (JSON.stringify(nextSelection) === JSON.stringify(firstSelection)) return firstSelection;
        } while (!TranslationItem.shouldRender(this.state.po, nextSelection.context, nextSelection.key, this.state.searchQuery, this.state.flags));

        return nextSelection;
    }

    getPreviousItem() {
        let previousSelection = this.state.selection;
        let firstSelection = this.state.selection;
        do {
            previousSelection = this.state.po.previousSelection(previousSelection?.context, previousSelection?.key);
            if (JSON.stringify(previousSelection) === JSON.stringify(firstSelection)) return firstSelection;
        } while (!TranslationItem.shouldRender(this.state.po, previousSelection.context, previousSelection.key, this.state.searchQuery, this.state.flags));

        return previousSelection;
    }

    render() {
        if (this.state.po) {
            if (this.state.po.hasError) {
                // return "There was an error loading the translation file.";
                return this.state.po.hasError.message;
            } else {
                return <div className={Styles.EditorRoot}>
                    <div className={Styles.ContextListWrapper}>
                        <ContextSearch searchQuery={this.state.searchQuery} onSearch={this.search.bind(this)} flags={this.state.flags} onSetFlags={this.setFlags.bind(this)} />
                        <div className={Styles.ContextList}>
                            {this.state.po.contexts().map(context => <Context key={context} searchQuery={this.state.searchQuery} flags={this.state.flags} context={context} poManager={this.state.po} selection={this.state.selection} onSelect={this.select.bind(this)} />)}
                        </div>
                    </div>
                    <TranslationArea selection={this.state.selection} poManager={this.state.po} />
                </div>
            }
        } else {
            return "Hang on...";
        }
    }
}

export default withRouter(hotkeys(TranslationEditor));