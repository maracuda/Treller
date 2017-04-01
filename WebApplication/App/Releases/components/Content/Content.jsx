import { PureComponent, PropTypes } from "react";
import { connect } from "react-redux";
import { getReleasesInfo } from "../../selectors";

import Release from "../Release";
import Comments from "../Comments";
import styles from "./Content.scss";

class Content extends PureComponent {
    render() {
        const { Releases } = this.props;

        return (
            <div className={styles.content}>
                <div className={styles.releases}>{Releases.map(release => <Release key={release.ReleaseId} { ...release } />)}</div>
                <Comments />
            </div>
        );
    }
}

Content.propTypes = {
    Releases: PropTypes.arrayOf(PropTypes.shape({
        ReleaseId: PropTypes.string
    }))
};

export default connect(getReleasesInfo)(Content);