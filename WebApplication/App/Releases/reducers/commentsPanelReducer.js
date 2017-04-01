import { handleActions } from "redux-actions";
import * as types from "../actionTypes";

const initialState = {
    isOpened: false
};

export default handleActions({
    [types.OPEN_COMMENTS]: (state, { payload }) => ({
        ...state,
        ReleaseId: payload.ReleaseId,
        isOpened: true
    }),
    [types.CLOSE_COMMENTS]: () => ({
        ...initialState
    })
}, initialState);
