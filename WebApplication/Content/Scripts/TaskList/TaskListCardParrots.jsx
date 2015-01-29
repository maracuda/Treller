var TaskListCardParrots = React.createClass({
    render(){
        return <span>
            <TaskGroupCounter count={this.props.PastDays ? this.props.PastDays : 1} title="Дней прошло" iconClass="fa-flag-o" />
            
            {this.props.AverageDaysRemind > 0
                && <TaskGroupCounter count={this.props.AverageDaysRemind} title="Дней осталось" iconClass="fa-flag-checkered" />
            }

            <TaskGroupCounter count={this.props.AverageSpeedInDay > 0.005 ? this.props.AverageSpeedInDay.toFixed(2) : 0} title="Попугаев в день" iconClass="fa-tachometer" />

            <TaskGroupCounter count={this.props.ProgressInfo.Progress} title="Готово на">
                { this.props.ProgressInfo.Progress }<span className="text-success">%</span>
            </TaskGroupCounter>
        </span>
    }
});