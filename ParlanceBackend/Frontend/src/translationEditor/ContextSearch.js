import React from 'react';
import Style from './ContextSearch.module.css';

class ContextSearch extends React.Component {
    searchBoxChanged(e) {
        this.props.onSearch(e.target.value);
    }

    setFlag(flag, e) {
        let flags = this.props.flags;
        flags[flag] = e.target.checked;
        this.props.onSetFlags(flags)
    }

    render() {
        return <div className={Style.ContextSearch}>
            <input id="unfinishedOnly" type="checkbox" value={this.props.flags.unfinishedOnly} onChange={this.setFlag.bind(this, "unfinishedOnly")} />
            <label htmlFor="unfinishedOnly">Only show unfinished translations</label>

            <input className={Style.SearchBox} type="text" placeholder="Search" value={this.props.searchQuery} onChange={this.searchBoxChanged.bind(this)} />
        </div>
    }
}

export default ContextSearch;