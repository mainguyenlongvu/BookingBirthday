(function ($) {
    "use strict";

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner();


    // Initiate the wowjs
    new WOW().init();


    // Fixed Navbar
    $(window).scroll(function () {
        if ($(window).width() < 992) {
            if ($(this).scrollTop() > 45) {
                $('.fixed-top').addClass('bg-white shadow');
            } else {
                $('.fixed-top').removeClass('bg-white shadow');
            }
        } else {
            if ($(this).scrollTop() > 45) {
                $('.fixed-top').addClass('bg-white shadow').css('top', -45);
            } else {
                $('.fixed-top').removeClass('bg-white shadow').css('top', 0);
            }
        }
    });


    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });


    // Testimonials carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        margin: 25,
        loop: true,
        center: true,
        dots: false,
        nav: true,
        navText: [
            '<i class="bi bi-chevron-left"></i>',
            '<i class="bi bi-chevron-right"></i>'
        ],
        responsive: {
            0: {
                items: 1
            },
            768: {
                items: 2
            },
            992: {
                items: 3
            }
        }
    });


})(jQuery);
function FillterByCategory(category_id) {
    $.ajax({
        url: "/Home/FilterProducts",
        data: { category_id: category_id },
        success: function (result) {
            $("#productListContainer").html(result);
        }
    });

}
function detailOrder(orderId) {
    $.getJSON("/Order/ViewOrder?orderId=" + orderId, function (data) {
        var html = '';
        var n = 1;
        if (data != null) {
            html += '<div class="table-responsive card mt-2">'
            html += '<table class="table table-hover">'
            html += '<tr>'
            html += '<th>#</th>'
            html += '<th>Mã đơn hàng</th>'
            html += '<th>Tên sản phẩm</th>'
            html += '<th>Số lượng</th>'
            html += '<th>Đơn giá</th>'
            html += '</tr>'
            $.each(data, function (key, value) {
                console.log(data)
                console.log(value)
                html += '<tr>'
                html += '<td><label style="width: auto">' + n + '</label></td>'
                html += '<td><label style="width: auto">' + value.order_id + '</label></td>'
                html += '<td><label style="width: auto">' + value.product_name + '</label></td>'
                html += '<td><label style="width: auto">' + value.quantity + '</label></td>'
                html += '<td><label style="width: auto">' + value.price + '</label></td>'
                html += '</tr>'
                n += 1;
            })
            html += '</table>'
            html += '</div>'
        }
        else {
            html += "Không có dữ liệu chi tiết đơn hàng"
        }
        $(".modal-body").html(html);
        $('#orderModal').modal('show');
    })
        .fail(function (jqXHR, textStatus, errorThrown) {
            // Xử lý khi có lỗi xảy ra
            console.error("Lỗi: " + textStatus, errorThrown);
        });

}
function changeStatusOrder(order_id) {
    if (confirm("Bạn muốn thay đổi trạng thái đơn hàng?")) {
        $.ajax({
            url: "/OwnerOrder/Edit",
            type: "POST",
            data: {
                orderId: order_id,
                status: $("#status" + order_id).val()
            },
            success: function (response) {
                window.location.reload() = true;
            },
            error: function (xhr, status, error) {
                window.location.reload() = true;
            }
        });
    }
}
function xoaProduct(product_id) {
    if (confirm("Bạn muốn xóa sản phẩm?")) {
        $.ajax({
            url: "/OwnerOrder/Delete",
            type: "POST",
            data: {
                productId: product_id
            },
            success: function (response) {
                window.location.reload() = true;
            },
            error: function (xhr, status, error) {
                window.location.reload() = true;
            }
        });
    }
}
function xoaUser(user_id) {
    if (confirm("Bạn muốn xóa người dùng?")) {
        $.ajax({
            url: "/AdminUser/Delete",
            type: "POST",
            data: {
                userId: user_id
            },
            success: function (response) {
                window.location.reload() = true;
            },
            error: function (xhr, status, error) {
                window.location.reload() = true;
            }
        });
    }
}
function xoaRequest(category_request_id) {
    if (confirm("Bạn muốn xóa yêu cầu thêm mới danh mục?")) {
        $.ajax({
            url: "/OwnerRequest/Delete",
            type: "POST",
            data: {
                category_request_id: category_request_id
            },
            success: function (response) {
                window.location.reload() = true;
            },
            error: function (xhr, status, error) {
                window.location.reload() = true;
            }
        });
    }
}
function xoaDanhMuc(category_id) {
    if (confirm("Bạn muốn xóa danh mục?")) {
        $.ajax({
            url: "/AdminCategory/Delete",
            type: "POST",
            data: {
                category_id: category_id
            },
            success: function (response) {
                window.location.reload() = true;
            },
            error: function (xhr, status, error) {
                window.location.reload() = true;
            }
        });
    }
}
function openAdd() {
    $('#add').modal('show');

}
function closeAdd() {
    $('#add').modal('hide');

}
function closeUpdate(id) {
    $('#update' + id).modal('hide');

}
function closeModal() {
    $('#orderModal').modal('hide');
}
function openUpdate(id) {
    $('#update' + id).modal('show');
}
function huyDon(order_id) {
    if (confirm("Bạn muốn hủy đơn hàng?")) {
        $.ajax({
            url: "/Order/HuyDon",
            type: "POST",
            data: {
                orderId: order_id
            },
            success: function (response) {
                window.location.reload() = true;
            },
            error: function (xhr, status, error) {
                window.location.reload() = true;
            }
        });
    }
}
function changeSelectRequest(type) {
    var selectElement = document.getElementById("selectRequest");

    var selectedValue = selectElement.value;
    $.getJSON('/' + type + '/getRequest?is_approved=' + selectedValue, function (data) {
        var html = '';
        if (data != null) {
            $.each(data, function (item, value) {
                console.log(value)
                html += '<tr>'
                html += '<td>' + (item + 1) + '</td>'
                html += '<td>'
                html += '<label style="width: auto">' + value.category_name + '</label>'
                html += '</td>'
                html += '<td>'
                html += '<label style="width: auto">' + value.created_at + '</label>'
                html += '</td>'
                html += '<td>'
                html += '<label style="width: auto">' + value.rejection_reason + '</label>'
                html += '</td>'
                html += '<td>'
                if (value.is_approved == -1) {
                    html += '<label style="width: auto">Yêu cầu bị từ chối</label>'
                }
                else if (value.is_approved == 1) {
                    html += '<label style="width: auto">Yêu cầu được duyệt</label>'
                }
                else {
                    html += '<label style="width: auto">Yêu cầu chờ duyệt</label>'
                }
                html += '</td>'
                html += '<td>'
                if (value.is_approved == 0) {
                    if (type == "AdminRequest") {
                        html += '<a class="btn btn-primary" onclick="Duyet(' + item.category_request_id + ')">Duyệt</a>'
                        html += '<a class="btn btn-success" onclick="TuChoi(' + item.category_request_id + ')">Từ chối</a>'
                    } else {
                        html += '<a class="btn btn-primary" onclick="xoaRequest(' + value.category_request_id + ')">Xóa</a>'
                        html += '<a class="btn btn-success" onclick="openUpdate(' + value.category_request_id + ')">Sửa</a>'
                    }
                }

                html += '<div class="modal fade" id="update' + value.category_request_id + '" tabindex="-1" role="dialog" aria-labelledby="productModalLabel" aria-hidden="true">'
                html += '<div class="modal-dialog modal-dialog-centered" role="document">'
                html += '<div class="modal-content">'
                html += '<form method="post" action="/OwnerRequest/Edit">'
                html += '<div class="modal-header">'
                html += '<h5 class="modal-title" id="productModalLabel">Chỉnh sửa sản phẩm</h5>'
                html += '</div>'
                html += '<div class="modal-body">'
                html += '<div class="form-group">'
                html += '<label>Tên sản phẩm</label>'
                html += '<input type="hidden" value="' + value.category_request_id + '" name="category_request_id" />'
                html += '<input type="text" class="form-control" name="category_name" value="' + value.category_name + '" required placeholder="Nhập tên sản phẩm">'
                html += '</div>'
                html += '</div>'
                html += '<div class="modal-footer">'
                html += '<button type="button" onclick="closeUpdate(' + value.category_request_id + ')" class="btn btn-secondary">Đóng</button>'
                html += '<button type="submit" class="btn btn-success">Sửa</button>'
                html += '</div>'
                html += '</form>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</td>'
                html += '</tr>'

            });
        }
        else {
            html += '<p class="alert alert-danger">Danh sách yêu cầu thêm mới danh mục trống</p>';
        }
        $("#bodyRequest").html(html);
    });
}
function Duyet(category_request_id) {
    if (confirm("Bạn muốn duyệt yêu cầu thêm mới danh mục?")) {
        $.ajax({
            url: "/AdminRequest/Approved",
            type: "POST",
            data: {
                category_request_id: category_request_id,
                is_approved: 1
            },
            success: function (response) {
                window.location.reload() = true;
            },
            error: function (xhr, status, error) {
                window.location.reload() = true;
            }
        });
    }
}
function TuChoi(category_request_id) {
    var lyDo = prompt("Nhập lý do từ chối:");

    if (lyDo === null || lyDo.trim() === "") {
        return;
    }

    $.ajax({
        url: "/AdminRequest/Approved",
        type: "POST",
        data: {
            category_request_id: category_request_id,
            is_approved: -1,
            rejection_reason: lyDo
        },
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            window.location.reload();
        }
    });
}

setTimeout(function () {
    $("#msgAlert").fadeOut("slow");
}, 2000);