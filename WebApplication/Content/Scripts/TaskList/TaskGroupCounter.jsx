var TaskGroupCounter = React.createClass({
    render: function() {
        return <span className="parrots-info-action"  title={ this.props.title }>
            { this.props.children
                ? this.props.children
                : <span>{ this.props.count } <span className={ "fa text-success " + this.props.iconClass}></span></span>
            }
        </span>;
    }
});