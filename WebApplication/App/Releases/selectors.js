import { createSelector } from "reselect";

export const getReleases = store => store.Releases;

export const getReleasesInfo = createSelector(
    getReleases,
    releases => ({
        Releases: releases
    })
);