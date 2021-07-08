import React from 'react';
import Styles from './TranslationTextEntry.module.css';

class TranslationTextEntry extends React.Component {
    constructor(props) {
        super(props);
    }

    getTitleString() {
        let pluralForms = this.props.translation.msgstr.length;
        if (pluralForms === 1) {
            return `Translation into ${this.props.poManager.getTargetName()}`;
        } else if (pluralForms === 2) {
            return `${this.props.index === 0 ? "Singular" : "Plural"} translation into ${this.props.poManager.getTargetName()}`
        } else {
            return `Translation into ${this.props.poManager.getTargetName()} (Plural form ${this.props.index + 1})`
        }
    }

    render() {
        let changeEvent = (e) => {
            this.props.poManager.setTranslation(this.props.selection.context, this.props.selection.key, this.props.index, e.target.value)
        }

        return <>
            <span className={Styles.Introduction}>{this.getTitleString()}</span>
            <textarea value={this.props.translation.msgstr[this.props.index]} className={Styles.TranslatedText} onChange={changeEvent} />
        </>
    }
}

export default TranslationTextEntry;