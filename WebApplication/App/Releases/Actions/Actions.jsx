import { PureComponent, PropTypes } from "react";
import { connect } from "react-redux";

import Icon, { IconTypes } from "billing-ui/components/Icon";
import { getActionsInfo } from "../selectors";
import styles from "./Actions.scss";

class Actions extends PureComponent {
    render() {
        const { commentsCount } = this.props;

        return (
            <div className={styles.wrapper}>
                <div className={styles.action}>
                    <Icon type={IconTypes.CommentLite} />
                    <div>{commentsCount}</div>
                </div>
            </div>
        );
    }
}

Actions.propTypes = {
    ReleaseId: PropTypes.string,
    commentsCount: PropTypes.number
};

export default connect(getActionsInfo)(Actions);