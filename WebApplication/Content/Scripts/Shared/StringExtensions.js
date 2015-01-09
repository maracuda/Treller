if (String.prototype.format === undefined) {
    String.prototype.format = function () {
        var args = Array.prototype.slice.call(arguments);
        return this.toString().replace(/{(\d+)}/g, function (match, number) {
            return args[number] !== undefined ? args[number] : match;

        });
    }
}