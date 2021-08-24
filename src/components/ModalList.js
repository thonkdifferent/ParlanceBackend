import React from 'react';
import Styles from './ModalList.module.css';

class ModalList extends React.Component {
    static displayName = "ModalList";
    
    render() {
        return <div className={Styles.ModalList}>
            {this.props.children?.map((item, index) => <div key={index} className={Styles.ModalListItem} onClick={item.onClick}>{item.text}</div>)}
        </div>
    }
}

export default ModalList;