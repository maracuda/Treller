import { handleActions } from "redux-actions";
import * as types from "../actionTypes";

const defaultValidationResult = { isValid: false, error: "" };

const initialState = {
    isOpened: false,
    commentForm: {
        name: "",
        text: ""
    },
    validationResult: {
        name: defaultValidationResult,
        text: defaultValidationResult
    }
};

export default handleActions({
    [types.OPEN_COMMENTS]: (state, { payload }) => ({
        ...state,
        ReleaseId: payload.ReleaseId,
        isOpened: true
    }),
    [types.CLOSE_COMMENTS]: () => ({
        ...initialState
    }),

    [types.CHANGE_COMMENT_FIELD]: (state, { payload }) => ({
        ...state,
        commentForm: {
            ...state.commentForm,
            [payload.field]: payload.value
        },
        validationResult: {
            ...state.validationResult,
            [payload.field]: { isValid: true, error: "" }
        }
    }),
    [types.CHECK_VALIDITY]: (state, { payload }) => ({
        ...state,
        validationResult: {
            ...state.validationResult,
            [payload.field]: payload.validationResult || defaultValidationResult
        }
    })
}, initialState);
