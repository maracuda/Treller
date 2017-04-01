import { PureComponent, PropTypes } from "react";
import { formatDate } from "billing-ui/libs/moment";

import Actions from "../Actions";
import styles from "./Release.scss";

class Release extends PureComponent {
    render() {
        const { ReleaseId, CreateDate, Title, Content, ImageUrl } = this.props;

        return (
            <div className={styles.release}>
                <div className={styles.date}>
                    {formatDate(CreateDate)}
                </div>
                <div className={styles.content}>
                    <Actions ReleaseId={ReleaseId} />

                    <div className={styles.title}>{Title}</div>
                    <div className={styles.text}>{Content}</div>

                    {ImageUrl && (
                        <div className={styles["image-wrapper"]}>
                            <img src={ImageUrl} alt="Демонстрация функционала" />
                        </div>
                    )}
                </div>
            </div>
        );
    }
}

Release.propTypes = {
    ReleaseId: PropTypes.string,
    CreateDate: PropTypes.string,
    Title: PropTypes.string,
    Content: PropTypes.string,
    ImageUrl: PropTypes.string,
    Comments: PropTypes.array
};

export default Release;