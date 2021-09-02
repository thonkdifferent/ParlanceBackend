import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter,
    Prompt
} from "react-router-dom";
import Fetch from "../utils/Fetch";

import Styles from "./TranslationArea.module.css";
import TranslationTextEntry from "./TranslationTextEntry";
import ErrorPage from "../components/ErrorPage";

class TranslationArea extends React.Component {
    constructor(props) {
        super(props);

        this.props.poManager.on("translationsChanged", () => {
            this.forceUpdate()
        });
    }

    async componentDidMount() {

    }

    componentDidUpdate(prevProps) {
        if (this.props.selection != prevProps.selection && prevProps.selection) this.save(prevProps.selection);
    }

    renderTextEntries(translation) {
        return translation.msgstr.map((item, index) => {
            return <TranslationTextEntry key={index} editable={this.props.editable} translation={translation} poManager={this.props.poManager} selection={this.props.selection} index={index} />
        });
    }

    renderMainArea() {
        if (this.props.selection) {
            let translation = this.props.poManager.getTranslation(this.props.selection.context, this.props.selection.key);

            return <>
                <div className={Styles.SourceText}>{translation.msgid}</div>
                {this.renderTextEntries(translation)}
            </>
        } else {
            return <ErrorPage title={"Get Started"} message={"Select a string to begin"} />;
        }
    }

    async save(selection) {
        if (!this.props.editable) return;
        
        let params = this.props.match.params;
        let translation = this.props.poManager.getTranslation(selection.context, selection.key);
        try {
            await Fetch.post(`/projects/${params.project}/${params.subproject}/${params.language}`, {
                context: selection.context,
                key: selection.key,
                translations: translation.msgstr,
                unfinished: false
            });
        } catch (err) {
            alert(`Error while saving. ${err}`)
        }
    }

    render() {
        return <div className={Styles.TranslationAreaRoot}>
            <div className={Styles.TranslationAreaInnerRoot}>
                {this.renderMainArea()}
            </div>
        </div>
    }
}

export default withRouter(TranslationArea);