var TaskListComponent = React.createClass({
    render: function () {
        console.log(this.props);
        return React.createElement("div", { className: "TaskList" }, this.props.data.InternalName);
    }
});