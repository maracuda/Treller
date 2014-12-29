(function ($, undefined) {
    "use strict";

    $("body").on("click.taskInfoBlockToggle", ".task-block-group-header", function (evt) {
        $(this).next(".task-block-group-body").slideToggle();
    });
})(jQuery, undefined);