import commentsPanelReducer from "./reducers/commentsPanelReducer";
import releasesReducer from "./reducers/releasesReducer";

export default (state, action) => ({
    ...state,
    commentsPanel: commentsPanelReducer(state.commentsPanel, action),
    Releases: releasesReducer(state.Releases, action)
});