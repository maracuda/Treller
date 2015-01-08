var TaskGroupCounter = React.createClass({
    render: function() {
        return <span className="parrots-info-action"  title={ this.props.title }>
            { this.props.count } <span className={ "fa text-success " + this.props.iconClass}></span>
        </span>;
    }
});

var CardBranchName = React.createClass({
    render: function() {
        return <div>
            <small className="text-muted">in branch { this.props.BranchName }</small>
            { this.props.inRelease && <span>(RC)</span> }
        </div>;
    }
});

var TaskListCardComponent = React.createClass({
    render: function() {
        return <section className="task-list-block">            
            <header className="task-block-header">
                <a href={this.props.CardUrl} className="fa fa-trello" title="посмотреть в Trello" target="_blank"></a>&nbsp;                
                <a href={ "/TaskInfo/TaskInfo/?cardId=" + this.props.CardId } className="colorbox-link">{this.props.CardName}</a>

                { this.props.BranchName &&
                    <CardBranchName title={this.props.BranchName} inRelease={this.props.IsInCandidateRelease} />
                }
            </header>
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