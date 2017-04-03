import { PureComponent, PropTypes } from "react";
import { formatDate } from "billing-ui/libs/moment";

import Actions from "../Actions";
import styles from "./Release.scss";

class Release extends PureComponent {
    _renderContent = () => ({ __html: this.props.Content });

    render() {
        const { PresentationId, CreateDate, Title, ImageUrl } = this.props;

        return (
            <div className={styles.release}>
                <div className={styles.date}>
                    {formatDate(CreateDate)}
                </div>
                <div className={styles.content}>
                    <Actions PresentationId={PresentationId} />

                    <div className={styles.title}>{Title}</div>
                    <div className={styles.text} dangerouslySetInnerHTML={this._renderContent()} />

                    {ImageUrl && (
                        <div className={styles["image-wrapper"]}>
                            <img src={ImageUrl} className={styles.image} alt="Демонстрация функционала" />
                        </div>
                    )}
                </div>
            </div>
        );
    }
}

Release.propTypes = {
    PresentationId: PropTypes.string,
    CreateDate: PropTypes.string,
    Title: PropTypes.string,
    Content: PropTypes.string,
    ImageUrl: PropTypes.string,
    Comments: PropTypes.array
};

export default Release;