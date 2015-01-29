var BugsLink = React.createClass({
    render(){
        return <a href={this.props.Link}>{this.props.Count}</a>
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
                <h3>
                    Информация по багам:
                    <br/>
                    Боевые инциденты: всего <BugsLink {...this.state.bugsBlock.BattleAssigned} />, неназначенных: <BugsLink {...this.state.bugsBlock.BattleUnassigned} />
                    <br/>
                    Billy баги всего: <BugsLink {...this.state.bugsBlock.BillyAll} />, на текущей версии: <BugsLink {...this.state.bugsBlock.BillyCurrent} />
                    <br/>
                    CS баги всего: <BugsLink {...this.state.bugsBlock.CsCurrent} />
                </h3>
            </SiteHeader>
            {taskListGroups}
        </div>;
    }
});