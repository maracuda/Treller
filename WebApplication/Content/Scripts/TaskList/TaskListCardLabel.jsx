var TaskListCardLabel = React.createClass({
    render(){
        return <span className={"task-block-label card-label " + this.props.ColorText} title={this.props.Name}></span>
    }
});