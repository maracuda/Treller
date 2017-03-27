const path = require("path");
const webpack = require("webpack");
const _ = require("lodash");
const webpackMerge = require("webpack-merge");

const prodConfig = require("./frontend_tasks/webpack/webpack.prod.config");
const devConfig = require("./frontend_tasks/webpack/webpack.dev.config");

module.exports = function(env) {
    let resultConfig = {};

    const webAppRoot = path.join(__dirname, "WebApplication");
    const appRoot = path.join(webAppRoot, "App");
    const isProduction = _.some(process.argv, _.partial(_.includes, ["-p", "--optimize-minimize", "--optimize-occurence-order"])) || env.prod;

    const createEntry = (partialPath, entryNameToExpose) => {
        const expose = entryNameToExpose ? `expose-loader?${entryNameToExpose}!` : null;
        const fullPath = path.join(appRoot, partialPath);

        return `${expose}${fullPath}`;
    };

    const baseConfig = {
        entry: {
            vendor: ["react", "react-dom", "redux", "react-redux", "redux-actions", "reselect"],
            releases: createEntry("Releases", "Releases")
        },
        module: {
            rules: [
                {
                    test: /\.jsx?$/,
                    use: "babel-loader",
                    include: [
                        appRoot
                    ],
                    exclude: /node_modules/
                },
                { test: require.resolve("react"), use: "expose-loader?React" },
                { test: require.resolve("react-dom"), use: "expose-loader?ReactDOM" },

                { test: /\.(jpe?g|png|gif|svg)$/i, loader: "url-loader?name=[name].[hash].[ext]&limit=10000" },
                { test: /\.ttf$/, loader: "file-loader?prefix=font/" }
            ]
        },
        output: {
            // publicPath: "/assets/",
            filename: "[name].js",
            chunkFilename: "chunks/[name].[chunkhash].js"
        },
        plugins: [
            new webpack.ProvidePlugin({
                React: "react",
                ReactDom: "react-dom"
            }),
            new webpack.optimize.CommonsChunkPlugin({
                names: ["vendor", "manifest"],
                minChunks: Infinity
            })
        ],
        resolve: {
            modules: [
                "node_modules"
            ],
            extensions: [".js", ".jsx"]
        }
    };

    if (isProduction) {
        resultConfig = webpackMerge(baseConfig, prodConfig(webAppRoot));
    } else {
        resultConfig = webpackMerge(baseConfig, devConfig("C:\\Treller"));
    }

    return resultConfig;
};