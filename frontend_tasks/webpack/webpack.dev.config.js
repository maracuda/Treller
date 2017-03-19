const webpack = require("webpack");

module.exports = {
    module: {
        rules: [
            {
                test: /\.scss$/,
                // todo: postcss
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
                    }
                ]
            }
        ]
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