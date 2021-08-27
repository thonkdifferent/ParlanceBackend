import React from 'react';

import Styles from './Form.module.css';

class Form extends React.Component {
    renderErrorText() {
        if (this.props.errorText) {
            return <span className={Styles.ErrorText}>{this.props.errorText}</span>
        }
    }
    
    render() {
        return <div className={Styles.Form}>
            {this.props.children}
            {this.renderErrorText()}
        </div>
    }
}

export default Form;