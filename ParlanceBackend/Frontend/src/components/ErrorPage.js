import React from 'react';

class ErrorPage extends React.Component {
    render() {
        return <div>
            {this.props.title}
            {this.props.message}
        </div>
    }
}

export default ErrorPage;