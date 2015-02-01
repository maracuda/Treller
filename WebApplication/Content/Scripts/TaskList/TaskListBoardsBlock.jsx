var TaskListBoardsBlock = React.createClass({
    render(){
        var boards = this.props.Boards.map(board => <a className="boardsBlock-link" target="_blank" href={board.Url} key={board.Name}>{board.Name}</a>);
        var releaseBranches = this.props.BranchesInCandidateRelease.map(branch => "{0}".format(branch.Name));

        return <div className="taskList-boardsBlock">
            <div className="boardsBlock-item">
                Доступные доски: {boards}            
            </div>
            <div className="boardsBlock-item">
                В релиз кандидате: {releaseBranches.join(", ") || "пусто"}
            </div>            
        </div>
    }
});