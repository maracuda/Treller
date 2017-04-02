import { handleActions } from "redux-actions";
import { isFieldValid } from "billing-ui/helpers/ValidationHelpers";
import * as types from "../actionTypes";

const defaultValidationResult = { isValid: true, error: "" };

const initialState = {
    isOpened: false,
    isLoading: false,
    canSubmit: false,
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
        PresentationId: payload.PresentationId,
        isOpened: true
    }),
    [types.CLOSE_COMMENTS]: () => ({
        ...initialState
    }),

    [types.CHANGE_COMMENT_FIELD]: (state, { payload }) => {
        const validationResult = {
            ...state.validationResult,
            [payload.field]: { isValid: true, error: "" }
        };
        return {
            ...state,
            commentForm: {
                ...state.commentForm,
                [payload.field]: payload.value
            },
            validationResult,
            canSubmit: isFieldValid(validationResult)
        };
    },
    [types.CHECK_VALIDITY]: (state, { payload }) => ({
        ...state,
        validationResult: {
            ...state.validationResult,
            [payload.field]: payload.validationResult || defaultValidationResult
        }
    }),

    [types.SUBMIT_COMMENT_BEGIN]: state => ({
        ...state,
        isLoading: true
    }),
    [types.SUBMIT_COMMENT_ERROR]: state => ({
        ...state,
        isLoading: false
    }),
    [types.SUBMIT_COMMENT_SUCCESS]: (state, { payload }) => ({
        ...state,
        isLoading: false,
        canSubmit: false,
        commentForm: initialState.commentForm,
        validationResult: initialState.validationResult
    })
}, initialState);
