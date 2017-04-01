import { select, take } from "redux-saga/effects";
import { fetchData } from "./utils/sagaHelpers";

import { getCommentSubmitData, getUrls } from "./selectors";
import * as actionTypes from "./actionTypes";
import * as actions from "./actions";

function* saveComment() {
    while (true) {
        yield take(actionTypes.SUBMIT_COMMENT);

        const data = yield select(state => getCommentSubmitData(state));
        const url = yield select(state => getUrls(state).saveComment);

        yield fetchData({
            url,
            data,
            onBegin: actions.submitCommentBegin,
            onSuccess: actions.submitCommentSuccess,
            onError: actions.submitCommentError,
            additionalResponseData: { releaseId: data.releaseId }
        });
    }
}

export function* releasesRootSaga() {
    yield [
        saveComment()
    ];
}
