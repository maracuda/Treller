const path = require("path");
const webpack = require("webpack");

module.exports = {
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    "style-loader",
                    {
                        loader: "css-loader",
                        options: {
                            sourceMap: true,
                            modules: true,
                            importLoaders: 1,
                            localIdentName: "[name]-[local]-[hash:base64:8]"
                        }
                    },
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
        path: path.join("C:\\Treller", "Content", "bundle")
    },
    plugins: [
        // new webpack.HotModuleReplacementPlugin(),
        new webpack.NoEmitOnErrorsPlugin()
        // new webpack.DefinePlugin({
        //     "process.env": {
        //         BUILD_TARGET: JSON.stringify("client")
        //     }
        // })
    ]
};