import { handleActions } from "redux-actions";
import omit from "lodash/omit";
import { arrayReduceHelper } from "billing-ui/helpers/ArrayHelper";
import * as types from "../actionTypes";

export default handleActions({
    [types.SUBMIT_COMMENT_SUCCESS]: (state, action) => {
        return arrayReduceHelper(
            release => release.ReleaseId === action.payload.releaseId,
            (release, { payload }) => ({
                ...release,
                Comments: [omit(payload, ["releaseId"])].concat(release.Comments || [])
            }),
            state,
            action
        );
    }
}, {});
