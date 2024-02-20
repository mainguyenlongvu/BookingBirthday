var slide = {
    init: function () {
        slide.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id')
            $.ajax({
                url: "/Admin/Slide/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('Kích hoạt').removeClass('text-danger').addClass('text-success');
                    }
                    else {
                        btn.text('Khóa').removeClass('text-success').addClass('text-danger');
                    }
                }
            });
        });
        $('.myActionLink').prepend("<i class='fa-solid fa-trash-can'></i> ");
    }
}
slide.init();