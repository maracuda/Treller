import { PureComponent } from "react";
import Content from "../Content";
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
                <Content />
            </div>
        );
    }
}

export default Layout;