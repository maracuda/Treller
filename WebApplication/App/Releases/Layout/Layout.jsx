import { PureComponent, PropTypes } from "react";
import { connect } from "react-redux";
import { getReleasesInfo } from "../selectors";

import styles from "./Layout.scss";

class Layout extends PureComponent {
    render() {
        return (
            <div className={styles.wrapper}>
                {this.props.Releases.map(release => <div>{release.Title}</div>)}
            </div>
        );
    }
}

Layout.propTypes = {
    Releases: PropTypes.array
};

export default connect(getReleasesInfo)(Layout);