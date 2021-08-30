import React from 'react';
import Styles from './Index.module.css';

class Index extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <div className={Styles.Index}>
            <h1 className={Styles.Header}>{this.props.title}</h1>
            <div className={Styles.IndexContainer}>
                {this.props.children?.map(el => <div className={Styles.IndexItem} onClick={el.props.onClick}>{el}</div>)}
            </div>
        </div>
    }
}

export default Index;