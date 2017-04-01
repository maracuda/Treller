import { PureComponent, PropTypes } from "react";
import { formatDate } from "billing-ui/libs/moment";

import { getRandomAvatar } from "./avatar";
import styles from "./Comment.scss";

class Comment extends PureComponent {
    render() {
        const { Name, Text, AvatarUrl, CreateDate } = this.props;
        const avatar = AvatarUrl ? AvatarUrl : getRandomAvatar();

        return (
            <div className={styles.comment}>
                <div className={styles.head}>
                    <div className={styles.name}>
                        <img className={styles.avatar} src={avatar} alt={`Аватар для ${Name}`} />
                        {Name}
                    </div>
                    <div className={styles.date}>{formatDate(CreateDate)}</div>
                </div>

                <div className={styles.text}>
                    {Text}
                </div>
            </div>
        );
    }
}

Comment.propTypes = {
    Name: PropTypes.string,
    Text: PropTypes.string,
    AvatarUrl: PropTypes.string,
    CreateDate: PropTypes.string
};

export default Comment;