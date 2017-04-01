import { PureComponent, PropTypes } from "react";
import TextInput from "billing-ui/components/TextInput";
import TextArea from "billing-ui/components/TextArea";
import Button, { ButtonSize } from "billing-ui/components/Button";

import styles from "./Comment.scss";

class CommentForm extends PureComponent {
    render() {
        return (
            <div className={styles.comment}>
                <TextInput width="100%"
                           placeholder="Кто ты?"
                           placeholderClassName={styles.label}
                           inputClassName={styles.input}
                           wrapperClassName={styles["input-wrapper"]} />
                <TextArea width="100%"
                          placeholder="Умные мысли, троллинг, сарказм здесь"
                          placeholderClassName={styles.label}
                          inputClassName={styles.input}
                          wrapperClassName={styles["input-wrapper"]} />
                <Button size={ButtonSize.small} className={styles.button}>Погнали!</Button>
            </div>
        );
    }
}

CommentForm.propTypes = {};

export default CommentForm;