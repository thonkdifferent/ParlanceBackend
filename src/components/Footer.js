import React from 'react';
import Styles from './Footer.module.css';

class Footer extends React.Component {
    render() {
        return <div className={Styles.Footer}>
            <div className={Styles.FooterInner}>
                Parlance 0.1
            </div>
        </div>
    }
}

export default Footer;