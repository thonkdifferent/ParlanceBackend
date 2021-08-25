import React from 'react';
import ReactDOM from 'react-dom';
import Styles from './Modal.module.css';

class Modal extends React.Component {
    render() {
        return <div className={Styles.ModalBackground}>
            <div className={Styles.ModalContainer}>
                {this.renderHeading()}
                {this.renderModalText()}
                {this.renderModalList()}
                <div className={Styles.ModalButtonContainer}>
                    {this.props.buttons?.map(button => {
                        if (typeof button === "object") {
                            return <div onClick={button.onClick} className={Styles.ModalButton} key={button.text}>{button.text}</div>
                        } else {
                            //deprecated
                            return <div onClick={this.props.onButtonClick.bind(this, button)} className={Styles.ModalButton} key={button}>{button}</div>
                        }
                    })}
                </div>
            </div>
        </div>
    }

    renderModalList() {
        let children = React.Children.toArray(this.props.children).filter(child => child.type?.displayName === "ModalList")

        return children.length !== 0 && <>{children}</>
    }

    renderModalText() {
        let children = React.Children.toArray(this.props.children).filter(child => child.type?.displayName !== "ModalList")

        return children.length !== 0 && <div className={Styles.ModalText}>
            {children}
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

Modal.CancelButton = {
    text: "Cancel",
    onClick: () => Modal.unmount()
};
Modal.OkButton = {
    text: "OK",
    onClick: () => Modal.unmount()
};

export default Modal;