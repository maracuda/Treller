import createPageComponent from "../utils/createPage";
import Layout from "./components/Layout";
import { releasesRootSaga } from "./saga";
import rootReducer from "./reducer";

const Page = createPageComponent(Layout, rootReducer, releasesRootSaga);

export default Page;
