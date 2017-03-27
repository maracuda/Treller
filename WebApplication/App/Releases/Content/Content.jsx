import { PureComponent, PropTypes } from "react";
import { connect } from "react-redux";
import { getReleasesInfo } from "../selectors";

import Release from "../Release";
import styles from "./Content.scss";

class Content extends PureComponent {
    render() {
        const { Releases } = this.props;

        return (
            <div className={styles.content}>
                {Releases.map(release => <Release key={release.ReleaseId} { ...release } />)}
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