import React from "react";
import Styles from "./ProgressSpinner.module.css";

import { Spinner } from 'spin.js';
import 'spin.js/spin.css'

class ProgressSpinner extends React.Component {
    constructor(props) {
        super(props)

        this.spinnerRef = React.createRef();
    }

    componentDidMount() {
        this.spinner = new Spinner({
            lines: 12,
            width: 2,
            radius: 10,
            length: 10,
            color: "#FFF",
            fadeColor: "transparent",
            animation: "spinner-line-fade-quick",
            direction: 1,
            top: "25px",
            left: "25px",
            position: "relative",
            shadow: '0 0 1px transparent',
            zIndex: 30
        })
        this.spinner.spin(this.spinnerRef.current);
    }

    componentWillUnmount() {
        this.spinner.stop();
    }

    render() {
        return <div className={Styles.ProgressSpinner}>
            <div className={Styles.Spinner} ref={this.spinnerRef} />
            {this.props?.message}
        </div>
    }
}

export default ProgressSpinner;