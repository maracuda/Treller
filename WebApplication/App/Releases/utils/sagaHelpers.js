import { call, put } from "redux-saga/effects";
import axios from "billing-ui/libs/axios";

export const httpMethod = {
    get: "get",
    post: "post",
    put: "put",
    delete: "delete",
    patch: "patch",
    trace: "trace",
    connect: "connect",
    options: "options",
    head: "head"
};

function* createPutEffects(actionCreators, payload) {
    if (Array.isArray(actionCreators)) {
        yield actionCreators.map(action => put(action(payload)));
    }

    if (typeof actionCreators === "function") {
        yield put(actionCreators(payload));
    }
}

export function* fetchData({
                               url,
                               data = null,
                               onBegin = null,
                               onSuccess,
                               onError = null,
                               requestMethod = httpMethod.post,
                               additionalResponseData = null
                           }) {
    if (onBegin) {
        yield* createPutEffects(onBegin, additionalResponseData);
    }

    try {
        const params = requestMethod === httpMethod.get ? { params: data } : data;
        const response = yield call(axios[requestMethod], url, params);
        const responseData = additionalResponseData ? {
            ...response.data,
            ...additionalResponseData
        } : response.data;

        yield* createPutEffects(onSuccess, responseData);
    } catch (xhr) {
        if (onError) {
            const errorData = additionalResponseData ? { ...additionalResponseData, error: xhr } : xhr;
            yield* createPutEffects(onError, errorData);
        }
    }
}
