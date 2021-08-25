import React from 'react';

import Styles from './ErrorPage.module.css'

class ErrorPage extends React.Component {
    render() {
        return <div className={Styles.ErrorPage}>
            <span className={Styles.Title}>{this.props.title}</span>
            <span className={Styles.Message}>{this.props.message}</span>
        </div>
    }
}

export default ErrorPage;