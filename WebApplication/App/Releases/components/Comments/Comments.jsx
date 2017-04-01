import { PureComponent, PropTypes } from "react";
import { connect } from "react-redux";
import { resolveString } from "billing-ui/helpers/PluralStringResolver";
import Icon, { IconTypes } from "billing-ui/components/Icon";

import Comment from "./Comment";
import CommentForm from "./CommentForm";
import { getCommentsPanelInfo } from "../../selectors";
import { closeComments } from "../../actions";
import styles from "./Comments.scss";
import cx from "classnames";

class Comments extends PureComponent {
    _getCommentsCountString = () => {
        const { commentsCount } = this.props;

        if (!commentsCount) {
            return "Комментариев нет";
        }

        return `${commentsCount} ${resolveString(commentsCount, ["комментарий", "комментария", "комментариев"])}`;
    };

    render() {
        const { isOpened, closeComments, Comments } = this.props;

        return (
            <div className={cx(styles.comments, { [styles["is-opened"]]: isOpened })}>
                <div className={styles.head}>
                    <div>
                        {this._getCommentsCountString()}
                    </div>
                    <div className={styles.close} onClick={closeComments}>
                        <Icon type={IconTypes.Delete} className={styles.cross} />
                        Закрыть
                    </div>
                </div>

                <div>
                    <CommentForm />
                    {Comments.map(comment => <Comment key={comment.CommentId} { ...comment } />)}
                </div>
            </div>
        );
    }
}

Comments.propTypes = {
    isOpened: PropTypes.bool,
    Comments: PropTypes.arrayOf(PropTypes.shape({
        CommentId: PropTypes.string
    })),
    commentsCount: PropTypes.number,

    closeComments: PropTypes.func
};

export default connect(getCommentsPanelInfo, { closeComments })(Comments);