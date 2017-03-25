import { PureComponent, PropTypes } from "react";

import styles from "./Releases.scss";

class Releases extends PureComponent {
    render() {
        return (
            <div className={styles.wrapper}>
                hello react world!
                <br />
                <br />
                {this.props.TaskNews.map(news => <div>{news.Content.Title}</div>)}
            </div>
        );
    }
}

Releases.propTypes = {
    TaskNews: PropTypes.array
};

export default Releases;