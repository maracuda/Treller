import { createAction } from "redux-actions";
import * as types from "./actionTypes";

export const openComments = createAction(types.OPEN_COMMENTS);
export const closeComments = createAction(types.CLOSE_COMMENTS);