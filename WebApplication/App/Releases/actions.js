import { createAction } from "redux-actions";
import * as types from "./actionTypes";

export const openComments = createAction(types.OPEN_COMMENTS);
export const closeComments = createAction(types.CLOSE_COMMENTS);

export const changeCommentField = createAction(types.CHANGE_COMMENT_FIELD);
export const checkValidity = createAction(types.CHECK_VALIDITY);

export const submitComment = createAction(types.SUBMIT_COMMENT);
export const submitCommentBegin = createAction(types.SUBMIT_COMMENT_BEGIN);
export const submitCommentSuccess = createAction(types.SUBMIT_COMMENT_SUCCESS);
export const submitCommentError = createAction(types.SUBMIT_COMMENT_ERROR);