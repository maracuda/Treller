import commentsPanelReducer from "./reducers/commentsPanelReducer";

export default (state, action) => ({
    ...state,
    commentsPanel: commentsPanelReducer(state.commentsPanel, action)
});