import { createSelector } from "reselect";

export const getReleases = store => store.Releases;

export const getReleasesInfo = createSelector(
    getReleases,
    releases => ({
        Releases: releases
    })
);

export const getCurrentRelease = createSelector(
    getReleases,
    (state, { ReleaseId }) => ReleaseId,
    (releases, releaseId) => releases.find(release => release.ReleaseId === releaseId)
);

export const getActionsInfo = createSelector(
    getCurrentRelease,
    release => ({
        commentsCount: release.Comments ? release.Comments.length : 0
    })
);