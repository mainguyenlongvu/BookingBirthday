var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $("#txtSearch").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/ListName",
                    dataType: "json",
                    data: {
                        term: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                //$("#project-id").val(ui.item.value);
                //$("#project-description").html(ui.item.desc);
                //$("#project-icon").attr("src", "images/" + ui.item.icon);

                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                $(ul).css({
                    "max-height": "200px",
                    "overflow": "auto"
                });
                return $("<li class='p-all-8'>")
                    .append("<div class='stext-106'>" + item.label + "</div>")
                    .appendTo(ul);
            };
    }
}
common.init();