import PropTypes from 'prop-types';
import { PureComponent } from "react";
import { connect } from "react-redux";
import TextInput from "billing-ui/components/TextInput";
import TextArea from "billing-ui/components/TextArea";
import Button, { ButtonSize } from "billing-ui/components/Button";

import ValidationFields from "../../utils/ValidationFields";
import { changeCommentField, checkValidity, submitComment } from "../../actions";
import { getCommentForm } from "../../selectors";
import styles from "./Comment.scss";

class CommentForm extends PureComponent {
    _handleChange = (value, evt) => {
        const { changeCommentField } = this.props;

        changeCommentField({ value, field: evt.target.name });
    };

    _handleBlur = (evt, data) => {
        const { checkValidity } = this.props;

        checkValidity({
            field: evt.target.name,
            validationResult: data.validationResult
        });
    };

    render() {
        const { name, text, validationResult, canSubmit, submitComment, isLoading } = this.props;

        return (
            <div className={styles.comment}>
                <TextInput width="100%"
                           placeholder="Кто ты?"
                           value={name}
                           name="name"
                           onChange={this._handleChange}
                           onBlur={this._handleBlur}
                           isValid={validationResult.name.isValid}
                           validateFunction={ValidationFields.name}
                           placeholderClassName={styles.label}
                           inputClassName={styles.input}
                           wrapperClassName={styles["input-wrapper"]} />

                <TextArea width="100%"
                          placeholder="Умные мысли, троллинг, сарказм здесь"
                          value={text}
                          name="text"
                          onChange={this._handleChange}
                          onBlur={this._handleBlur}
                          isValid={validationResult.text.isValid}
                          validateFunction={ValidationFields.text}
                          placeholderClassName={styles.label}
                          inputClassName={styles.input}
                          wrapperClassName={styles["input-wrapper"]} />

                <Button size={ButtonSize.small} className={styles.button} disabled={!canSubmit || isLoading} onClick={submitComment}>
                    Сохранить
                </Button>
            </div>
        );
    }
}

CommentForm.propTypes = {
    name: PropTypes.string,
    text: PropTypes.string,
    validationResult: PropTypes.object,
    canSubmit: PropTypes.bool,
    isLoading: PropTypes.bool,

    changeCommentField: PropTypes.func,
    checkValidity: PropTypes.func,
    submitComment: PropTypes.func
};

export default connect(getCommentForm, { changeCommentField, checkValidity, submitComment })(CommentForm);