import React from "react";
import Styles from "./MessageText.module.css";

class MessageText extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <span dir="auto" className={`${this.props.className} ${Styles.Text}`}>{this.props.text}</span>;
    }
}

export default MessageText;