const path = require("path");
const extractTextPlugin = require("extract-text-webpack-plugin");

module.exports = function(webAppRoot) {
    return {
        module: {
            rules: [{
                test: /\.scss$/,
                loader: extractTextPlugin.extract({
                    fallback: "style-loader",
                    use: "css-loader?localIdentName=[name]-[local]-[hash:base64:8]&minimize"
                })
            },
                {
                    test: /\.scss$/,
                    use: [
                        {
                            loader: "sass-loader",
                            options: {
                                outputStyle: "expanded"
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
            new extractTextPlugin({
                filename: "[name].css",
                allChunks: true
            })
        ]
    };
};