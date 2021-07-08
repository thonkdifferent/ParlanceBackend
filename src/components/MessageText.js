import React from "react";

class MessageText extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <span className={this.props.className}>{this.props.text}</span>;
    }
}

export default MessageText;