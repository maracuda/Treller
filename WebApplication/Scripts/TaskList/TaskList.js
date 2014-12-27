(function ($, undefined) {
    "use strict";

    $("body").on("click.taskInfoBlockToggle", ".task-block-group-title", function (evt) {
        $(this).next(".task-block-group-body").slideToggle();
    });
})(jQuery, undefined);