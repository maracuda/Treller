var TaskListCardAvatarList = React.createClass({
    render(){
        return <div className="clearfix task-block-members-list">
            <span></span>
            {this.props.children}
        </div>
    }
});