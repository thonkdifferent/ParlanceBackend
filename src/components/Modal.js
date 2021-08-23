import React from 'react';
import ReactDOM from 'react-dom';
import Styles from './Modal.module.css';

class Modal extends React.Component {
    render() {
        return <div className={Styles.ModalBackground}>
            <div className={Styles.ModalContainer}>
                {this.renderHeading()}
                <div className={Styles.ModalText}>
                    {this.props.children}
                </div>
                <div className={Styles.ModalButtonContainer}>
                    {this.props.buttons?.map(button => <div onClick={this.props.onButtonClick.bind(this, button)} className={Styles.ModalButton} key={button}>{button}</div>)}
                </div>
            </div>
        </div>
    }

    renderHeading() {
        if (this.props.heading) {
            return <div className={Styles.ModalHeading}>
                {this.props.heading}
            </div>
        }
        return null;
    }

    static mount(modal) {
        ReactDOM.render(
            <React.StrictMode>
                {modal}
            </React.StrictMode>,
            document.getElementById('modalContainer')
        );
    }

    static unmount() {
        ReactDOM.unmountComponentAtNode(document.getElementById('modalContainer'));
    }
}

export default Modal;