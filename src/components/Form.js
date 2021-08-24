import React from 'react';

import Styles from './Form.module.css';

class Form extends React.Component {
    render() {
        return <div className={Styles.Form}>
            {this.props.children}
        </div>
    }
}

export default Form;