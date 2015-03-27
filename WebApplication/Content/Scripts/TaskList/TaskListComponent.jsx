var BugsLink = React.createClass({
    render(){
        return <a title={this.props.Description} href={this.props.Link}>{this.props.Count}</a>
    }
});

var TaskListComponent = React.createClass({
    getInitialState(){
        return {
            taskList: this.props.data.TaskList,
            boardsBlock: this.props.data.BoardsBlock,
            bugsBlock: this.props.data.BugsBlock
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
                boardsBlock: data.BoardsBlock,
                bugsBlock: data.BugsBlock
            });
        }.bind(this));
    },
    render() {
        var taskListGroups = this.state.taskList ? this.state.taskList.OverallStateCards.map(group => <TaskGroupComponent {...group} key={group.State} />) : null;

        return <div>
            <SiteHeader title={this.props.title}>
                <TaskListBoardsBlock {...this.state.boardsBlock} />
                <h4>
                    Информация по багам: Battle (<BugsLink {...this.state.bugsBlock.BattleUnassigned} />/<BugsLink {...this.state.bugsBlock.BattleAssigned} />),
					Billy (<BugsLink {...this.state.bugsBlock.BillyCurrent} />/<BugsLink {...this.state.bugsBlock.BillyAll} />),
					CS <BugsLink {...this.state.bugsBlock.CsCurrent} />,
					В тестировании <BugsLink {...this.state.bugsBlock.BillyNotVerified} />
                </h4>
            </SiteHeader>
            {taskListGroups}
        </div>;
    }
});