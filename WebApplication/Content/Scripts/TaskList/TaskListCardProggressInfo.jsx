var TaskListCardProggressInfo = React.createClass({
    render(){
        return <section className="task-progress-block" title={ "Общий прогресс: {0}/{1}".format(this.props.CurrentCount, this.props.TotalCount) }>
            <div className="task-progress-legend">{ "{0}% ({1}/{2})".format(this.props.Progress, this.props.CurrentCount, this.props.TotalCount) }</div>
            <div className="task-progress-line total"></div>
            <div className="task-progress-line accomplished progress-bar-striped" style={{ width: "{0}%".format(this.props.Progress) }}></div>
        </section>
    }
});