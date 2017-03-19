import { PureComponent, PropTypes } from "react";

import styles from "./News.scss";

class News extends PureComponent {
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

News.propTypes = {
    TaskNews: PropTypes.array
};

export default News;