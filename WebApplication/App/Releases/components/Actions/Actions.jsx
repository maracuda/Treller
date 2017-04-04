import { PureComponent, PropTypes } from "react";
import { connect } from "react-redux";
import Icon, { IconTypes } from "billing-ui/components/Icon";

import { getActionsInfo } from "../../selectors";
import { openComments } from "../../actions";
import styles from "./Actions.scss";

class Actions extends PureComponent {
    _handleOpenComments = () => {
        const { openComments, PresentationId } = this.props;

        openComments({ PresentationId });
    };

    render() {
        const { commentsCount } = this.props;

        return (
            <div className={styles.wrapper}>
                <div className={styles.action} onClick={this._handleOpenComments}>
                    <Icon type={IconTypes.CommentLite} className={styles.bubble} />
                    <div>{commentsCount}</div>
                </div>
            </div>
        );
    }
}

Actions.propTypes = {
    PresentationId: PropTypes.string,
    commentsCount: PropTypes.number,

    openComments: PropTypes.func
};

export default connect(getActionsInfo, { openComments })(Actions);