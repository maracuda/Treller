var TaskListCardComponent = React.createClass({
    render() {
        var avatars = this.props.Avatars ? this.props.Avatars.map(avatar => <AvatarItem {...avatar} key={avatar.UserFullName} />) : null;
        var labels = this.props.Labels ? this.props.Labels.map(label => <TaskListCardLabel {...label} key={label.Name} />) : null;

        return <section className="task-list-block">
            <TaskListCardProggressInfo {...this.props.StageInfo.StageParrots.ProgressInfo} />
            <header className="task-block-header">
                <a href={this.props.CardUrl} className="fa fa-trello" title="посмотреть в Trello" target="_blank"></a>&nbsp;                
                
                { this.props.AnalyticLink &&
                    <a href={this.props.AnalyticLink} className="fa fa-info-circle text-success"></a>                   
                }

                &nbsp;

                <a href={ "/TaskInfo/TaskInfo/?cardId=" + this.props.CardId } className="colorbox-link">{this.props.CardName}</a>

                { this.props.BranchName &&
                    <CardBranchName title={this.props.BranchName} inRelease={this.props.IsInCandidateRelease} />
                }
            </header>

            <div className="task-block-content">
                <div className="parrots-info-block">
                    { this.props.IsNewCard && <span className="fa fa-lightbulb-o"></span> }

                    <TaskListCardParrots {...this.props.StageInfo.StageParrots} />

                    { this.props.Bugs && this.props.Bugs.OverallBugsCount > 0 &&
                        <TaskGroupCounter count={ "({0}/{1})".format(this.props.Bugs.FixedBugsCount, this.props.Bugs.OverallBugsCount) } title="Незакрытых багов" iconClass="fa-bug" />
                    }

                    <span style={{marginLeft: 10}}>{"с " + this.props.StageInfo.StageParrots.BeginDateFormat}</span>
                </div>

                { avatars && <TaskListCardAvatarList>{avatars}</TaskListCardAvatarList> }                
            </div>

            {labels && <div className="task-block-label-list">{labels}</div>}
        </section>;
    }
});