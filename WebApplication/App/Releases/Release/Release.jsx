import { PureComponent, PropTypes } from "react";
import { formatDate } from "billing-ui/libs/moment";

import styles from "./Release.scss";

class Release extends PureComponent {
    render() {
        const { CreateDate, Title, Content, ImageUrl } = this.props;

        return (
            <div>
                <div className={styles.date}>
                    {formatDate(CreateDate)}
                </div>
                <div className={styles.content}>
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
    CreateDate: PropTypes.string,
    Title: PropTypes.string,
    Content: PropTypes.string,
    ImageUrl: PropTypes.string
};

export default Release;