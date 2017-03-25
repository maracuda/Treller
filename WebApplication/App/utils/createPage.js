import { Provider } from "react-redux";
import { configureStore } from "billing-ui/helpers/reduxHelpers/configureStore";

const createPageComponent = (Component, rootReducer, rootSaga) => (props) => {
    const store = configureStore(props, rootReducer, rootSaga);

    return (
        <Provider store={store}>
            <Component />
        </Provider>
    );
};

export default createPageComponent;