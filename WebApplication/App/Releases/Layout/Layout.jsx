import { PureComponent, PropTypes } from "react";
import { connect } from "react-redux";
import { getReleasesInfo } from "../selectors";

import Release from "../Release";

import styles from "./Layout.scss";

class Layout extends PureComponent {
    render() {
        return (
            <div className={styles.layout}>
                <div className={styles.header}>
                    <div className={styles.title}>Новости</div>
                    <div className={styles.logo} />
                    <div className={styles.title}>Биллинга</div>
                </div>
                {this.props.Releases.map(release => <div>{release.Title}</div>)}
            </div>
        );
    }
}

Layout.propTypes = {
    Releases: PropTypes.arrayOf(PropTypes.shape({
        ReleaseId: PropTypes.string
    }))
};

export default connect(getReleasesInfo)(Layout);