const extractTextPlugin = require("extract-text-webpack-plugin");

module.exports = {
    module: {
        rules: [{
            test: /\.scss$/,
            // todo: postcss
            loader: extractTextPlugin.extract({
                fallback: "style-loader",
                use: "css-loader?localIdentName=[name]-[local]-[hash:base64:8]&minimize"
            })
        },
            {
                test: /\.scss$/,
                loader: "sass-loader?outputStyle=expanded"
            }
        ]
    },
    plugins: [
        new extractTextPlugin({
            filename: "[name].css",
            allChunks: true
        })
    ]
};