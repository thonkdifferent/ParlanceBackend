import React from 'react';
import Style from "./TranslationItem.module.css";

import MessageText from '../components/MessageText';

import UnfinishedIcon from '../assets/unfinished.svg';
import WarningIcon from '../assets/warning.svg';
import CompleteIcon from '../assets/complete.svg';

class TranslationItem extends React.Component {
    constructor(props) {
        super(props);
    }

    isSelected() {
        return this.props.selection?.context === this.props.context && this.props.selection?.key == this.props.translationKey;
    }

    icon() {
        let translation = this.props.poManager.getTranslation(this.props.context, this.props.translationKey);
        if (translation.checks.filter(checkArray => checkArray.length !== 0).length !== 0) {
            return WarningIcon;
        } else if (translation.msgstr.includes("")) {
            return UnfinishedIcon;
        } else {
            return CompleteIcon;
        }
    }

    shouldRender(translation) {
        //Figure out if we should render
        if (this.isSelected()) return true;

        let searchQuery = this.props.searchQuery.toLowerCase();
        if (searchQuery !== "") {
            if (!translation.msgid.toLowerCase().includes(searchQuery) && !translation.msgstr.map(str => str.toLowerCase().includes(searchQuery)).includes(true)) return false;
        }

        if (this.props.flags.unfinishedOnly) {
            if (!translation.msgstr.includes("")) return false;
        }

        return true;
    }

    render() {
        let translation = this.props.poManager.getTranslation(this.props.context, this.props.translationKey);

        if (!this.shouldRender(translation)) return null;

        let clickEvent = () => {
            this.props.onSelect({
                context: this.props.context,
                key: this.props.translationKey
            })
        }

        return <div className={this.isSelected() ? Style.Selected : ""}>
            <div className={Style.TranslationItem} onClick={clickEvent} >
                <img className={Style.Icon} src={this.icon()} width={24}/>
                <MessageText className={Style.SourceText} text={translation.msgid} />
                <MessageText className={Style.TranslationText} text={translation.msgstr[0]} />
            </div>
        </div>
    }
}

export default TranslationItem;