import { createSelector } from "reselect";
import { isFieldValid } from "billing-ui/helpers/ValidationHelpers";

export const getReleases = store => store.Releases;
export const getCommentsPanel = store => store.commentsPanel;

export const getReleasesInfo = createSelector(
    getReleases,
    releases => ({
        Releases: releases
    })
);

export const findRelease = (releases, releaseId) => releases.find(release => release.ReleaseId === releaseId);

export const getCurrentRelease = createSelector(
    getReleases,
    (state, { ReleaseId }) => ReleaseId,
    (releases, releaseId) => findRelease(releases, releaseId)
);

export const getCommentsCount = comments => comments ? comments.length : 0;

export const getActionsInfo = createSelector(
    getCurrentRelease,
    release => ({
        commentsCount: getCommentsCount(release.Comments)
    })
);

export const getCommentsPanelInfo = createSelector(
    getCommentsPanel,
    getReleases,
    (panel, releases) => {
        const release = findRelease(releases, panel.ReleaseId);
        const comments = release ? release.Comments : null;
        return {
            isOpened: panel.isOpened,
            Comments: comments ? comments : [],
            commentsCount: getCommentsCount(comments)
        };
    }
);

export const getCommentForm = createSelector(
    getCommentsPanel,
    panel => {
        const validationResult = panel.validationResult;

        return {
            ...panel.commentForm,
            validationResult,
            canSubmit: isFieldValid(validationResult)
        };
    }
);