var TaskGroupComponent = React.createClass({
    toggleTaskGroupBody(){
        $(this.refs.taskGroupBody.getDOMNode()).slideToggle();
    },
    render(){
        return <div className="task-block-group">
            <div className="task-block-group-header" onClick={this.toggleTaskGroupBody}>
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

            <div className="task-block-group-body" ref="taskGroupBody">
                { this.props.Cards.map(card => <TaskListCardComponent {...card} key={card.CardId} />) }
            </div>
        </div>
    }
});