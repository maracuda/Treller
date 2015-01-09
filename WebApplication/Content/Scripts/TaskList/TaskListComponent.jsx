if (String.prototype.format === undefined) {
    String.prototype.format = function () {
        var args = Array.prototype.slice.call(arguments);
        return this.toString().replace(/{(\d+)}/g, function (match, number) {
            return args[number] !== undefined ? args[number] : match;
        });
    }
}

var TaskGroupCounter = React.createClass({
    render: function() {
        return <span className="parrots-info-action"  title={ this.props.title }>
            { this.props.children
                ? this.props.children
                : <span>{ this.props.count } <span className={ "fa text-success " + this.props.iconClass}></span></span>
            }
        </span>;
    }
});

var CardBranchName = React.createClass({
    render() {
        return <span style={{ marginLeft: 10 }}>
            <small className="text-muted">in branch { this.props.BranchName }</small>
            { this.props.inRelease && <span>(RC)</span> }
        </span>;
    }
});

var TaskListCardProggressInfo = React.createClass({
    render(){
        return <section className="task-progress-block" title={ "Общий прогресс: {0}/{1}".format(this.props.CurrentCount, this.props.TotalCount) }>
            <div className="task-progress-legend">{ "{0}% ({1}/{2})".format(this.props.Progress, this.props.CurrentCount, this.props.TotalCount) }</div>
            <div className="task-progress-line total"></div>
            <div className="task-progress-line accomplished progress-bar-striped" style={{ width: "{0}%".format(this.props.Progress) }}></div>
        </section>
    }
});

var AvatarItem = React.createClass({
    render(){
        return <div className="user-avatar-block" title={this.props.UserFullName} style={{ backgroundImage: this.props.AvatarSrc ? "url('{0}')".format(this.props.AvatarSrc) : "none" }}>
            {!this.props.AvatarSrc && this.props.UserFullName
                && <span className="user-avatar-initial">{this.props.UserFullName.charAt(0)}</span>
            }
        </div>
    }
});

var TaskListCardAvatarList = React.createClass({
    render(){
        return <div className="clearfix task-block-members-list">
            <span></span>
            {this.props.children}
        </div>
    }
});

var TaskListCardLabel = React.createClass({
    render(){
        return <span className={"task-block-label card-label " + this.props.ColorText} title={this.props.Name}></span>
    }
});

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

var TaskListCardComponent = React.createClass({
    render() {
        var avatars = this.props.Avatars ? this.props.Avatars.map(avatar => <AvatarItem {...avatar} key={avatar.UserFullName} />) : null;
        var labels = this.props.Labels ? this.props.Labels.map(label => <TaskListCardLabel {...label} key={label.Name} />) : null;

        return <section className="task-list-block">
            <TaskListCardProggressInfo {...this.props.StageInfo.StageParrots.ProgressInfo} />
            <header className="task-block-header">
                <a href={this.props.CardUrl} className="fa fa-trello" title="посмотреть в Trello" target="_blank"></a>&nbsp;                
                <a href={ "/TaskInfo/TaskInfo/?cardId=" + this.props.CardId } className="colorbox-link">{this.props.CardName}</a>

                { this.props.BranchName &&
                    <CardBranchName title={this.props.BranchName} inRelease={this.props.IsInCandidateRelease} />
                }
            </header>

            <div className="task-block-content">
                <div className="parrots-info-block">
                    { this.props.IsNewCard && <span className="fa fa-lightbulb-o"></span> }

                    <TaskListCardParrots {...this.props.StageInfo.StageParrots} />

                    { this.props.StageInfo.Bugs.NotFixedBugsCount > 0 &&
                        <TaskGroupCounter count={this.props.StageInfo.Bugs.NotFixedBugsCount} title="Незакрытых багов" iconClass="fa-bug" />
                    }

                    <span style={{marginLeft: 10}}>{"с " + this.props.StageInfo.StageParrots.BeginDateFormat}</span>
                </div>

                { avatars && <TaskListCardAvatarList>{avatars}</TaskListCardAvatarList> }                
            </div>

            {labels && <div className="task-block-label-list">{labels}</div>}
        </section>;
    }
});

var TaskGroupComponent = React.createClass({
    render: function(){
        return <div className="task-block-group">
            <div className="task-block-group-header">
                <h3 className="task-block-group-title">{ this.props.Title }</h3>

                <div className="task-block-group-legend">
                    <TaskGroupCounter count={ this.props.TotalCardsCount } iconClass="fa-tasks" title="Количество" />

                    { this.props.NewCardsCount > 0 &&
                        <TaskGroupCounter count={ this.props.NewCardsCount } iconClass="fa-lightbulb-o" title="Новые карточки за сегодня" />
                    }

                    { this.props.FinishingCardsCount > 0 &&
                        <TaskGroupCounter count={ this.props.FinishingCardsCount } iconClass="fa-check-square-o" title="Близки к завершению" />
                    }
                </div>
            </div>

            <div className="task-block-group-body">
                { this.props.Cards.map(card => <TaskListCardComponent {...card} key={card.CardId} />) }
            </div>
        </div>
    }
});

var TaskListComponent = React.createClass({
    render: function () {
        return <div>{ this.props.data.OverallStateCards.map(group => <TaskGroupComponent {...group} key={group.State} />) }</div>;
    }
});