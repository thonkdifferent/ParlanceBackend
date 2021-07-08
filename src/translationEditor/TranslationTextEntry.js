import React from 'react';
import Styles from './TranslationTextEntry.module.css';

class TranslationTextEntry extends React.Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        if (this.props.index === 0) this.translatedTextArea.focus();
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

    renderChecks() {
        return this.props.translation.checks[this.props.index].map(check => <div className={Styles.FailingCheck}>
            <span className={Styles.CheckTitle}>{check.title}</span>
            <span className={Styles.CheckMessage}>{check.message}</span>
        </div>)
    }

    render() {
        let changeEvent = (e) => {
            this.props.poManager.setTranslation(this.props.selection.context, this.props.selection.key, this.props.index, e.target.value)
        }

        return <>
            <span className={Styles.Introduction}>{this.getTitleString()}</span>
            <textarea ref={element => this.translatedTextArea = element} value={this.props.translation.msgstr[this.props.index]} className={Styles.TranslatedText} onChange={changeEvent} />
            {this.renderChecks()}
        </>
    }
}

export default TranslationTextEntry;