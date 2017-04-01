const path = require("path");
const webpack = require("webpack");
const extractTextPlugin = require("extract-text-webpack-plugin");

module.exports = function(webAppRoot) {
    return {
        cache: true,
        devtool: "inline-source-map",
        module: {
            rules: [
                {
                    test: /\.scss$/,
                    loader: extractTextPlugin.extract({
                        fallback: "style-loader",
                        use: "css-loader?sourceMap&localIdentName=[name]-[local]-[hash:base64:8]"
                    })
                },
                {
                    test: /\.scss$/,
                    use: [
                        {
                            loader: "sass-loader",
                            options: {
                                outputStyle: "expanded",
                                sourceMap: true,
                                sourceMapContents: true
                            }
                        },
                        {
                            loader: "postcss-loader",
                            options: {
                                plugins: () => ([
                                    require("autoprefixer")
                                ])
                            }
                        }
                    ]
                }
            ]
        },
        output: {
            path: path.join(webAppRoot, "Content", "bundle")
        },
        plugins: [
            // new webpack.HotModuleReplacementPlugin(),
            new webpack.NoEmitOnErrorsPlugin(),
            new extractTextPlugin({
                filename: "[name].css",
                allChunks: true
            }),
            new webpack.LoaderOptionsPlugin({
                debug: true
            })
            // new webpack.DefinePlugin({
            //     "process.env": {
            //         BUILD_TARGET: JSON.stringify("client")
            //     }
            // })
        ]
    };
};