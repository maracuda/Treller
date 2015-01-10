var SiteHeader = React.createClass({
    propTypes: {
        title: React.PropTypes.string.isRequired
    },
    render(){
        return <header className="layout-header">
            <h1 className="layout-title">{this.props.title}</h1>
            { this.props.children ? this.props.children : null }
        </header>
    }
});