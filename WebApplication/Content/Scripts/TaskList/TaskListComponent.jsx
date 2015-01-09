var TaskListComponent = React.createClass({
    getInitialState(){
        return {
            taskList: this.props.data
        };
    },
    componentWillMount(){
        if (typeof window !== "undefined") {
            setInterval(this.loadFromServer, this.props.pollInterval);  
        };        
    },
    loadFromServer(){
        $.get(this.props.updateUrl).done(function(data){
            this.setState({ taskList: data });
        }.bind(this));
    },
    render() {
        return <div>{ this.state.taskList && this.state.taskList.OverallStateCards.map(group => <TaskGroupComponent {...group} key={group.State} />) }</div>;
    }
});