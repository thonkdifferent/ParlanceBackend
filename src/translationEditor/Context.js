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
import MessageText from '../components/MessageText';

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

    isSelected(key) {
        return this.props.selection?.context === this.props.context && this.props.selection?.key == key;
    }

    render() {
        return <>
            <div className={Style.ContextHeader}>{this.props.context}</div>
            {this.props.poManager.getKeys(this.props.context).map(key => {
                let translation = this.props.poManager.getTranslation(this.props.context, key);

                let clickEvent = () => {
                    this.props.onSelect({
                        context: this.props.context,
                        key: key
                    })
                }

                return <div className={`${Style.TranslationItem} ${this.isSelected(key) ? Style.Selected : ""}`} onClick={clickEvent} >
                    <MessageText className={Style.SourceText} text={translation.msgid} />
                    <MessageText className={Style.TranslationText} text={translation.msgstr[0]} />
                </div>
            })}
        </>
    }
}

export default withRouter(Context);