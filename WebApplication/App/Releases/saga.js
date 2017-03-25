function* fetchDataWatcher() {
}

export function* releasesRootSaga() {
    yield [
        fetchDataWatcher()
    ];
}
