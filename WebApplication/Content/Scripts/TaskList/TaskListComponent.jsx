var TaskListComponent = React.createClass({
    getInitialState(){
        return {
            taskList: this.props.data.TaskList,
            boardsBlock: this.props.data.BoardsBlock
        };
    },
    componentWillMount(){
        if (typeof window !== "undefined") {
            setInterval(this.loadFromServer, this.props.pollInterval);  
        };        
    },
    loadFromServer(){
        $.get(this.props.updateUrl).done(function(data){
            this.setState({
                taskList: data.TaskList,
                boardsBlock: data.BoardsBlock
            });
        }.bind(this));
    },
    render() {
        var taskListGroups = this.state.taskList ? this.state.taskList.OverallStateCards.map(group => <TaskGroupComponent {...group} key={group.State} />) : null;

        return <div>
            <SiteHeader title={this.props.title}>
                <TaskListBoardsBlock {...this.state.boardsBlock} />
            </SiteHeader>
            {taskListGroups}
        </div>;
    }
});