import { PureComponent, PropTypes } from "react";

import styles from "./Releases.scss";

class Releases extends PureComponent {
    render() {
        return (
            <div className={styles.wrapper}>
                {this.props.Releases.map(release => <div>{release.Title}</div>)}
            </div>
        );
    }
}

Releases.propTypes = {
    Releases: PropTypes.array
};

export default Releases;