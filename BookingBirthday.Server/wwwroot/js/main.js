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


    //// Fixed Navbar
    //$(window).scroll(function () {
    //    if ($(window).width() < 992) {
    //        if ($(this).scrollTop() > 45) {
    //            $('.fixed-top').addClass('bg-white shadow');
    //        } else {
    //            $('.fixed-top').removeClass('bg-white shadow');
    //        }
    //    } else {
    //        if ($(this).scrollTop() > 45) {
    //            $('.fixed-top').addClass('bg-white shadow').css('top', -45);
    //        } else {
    //            $('.fixed-top').removeClass('bg-white shadow').css('top', 0);
    //        }
    //    }
    //});


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
function detailOrder(Id) {
    $.getJSON("/Booking/ViewBooking?Id=" + Id, function (data) {
        var html = '';
        var n = 1;
        if (data != null && data.length > 0) {
            html += '<div class="table-responsive card mt-2">'
            html += '<table class="table table-hover">'
            html += '<tr>'
            html += '<th>#</th>'
            html += '<th>Mã đơn hàng</th>'
            html += '<th>Tên sản phẩm</th>'
            html += '<th>Đơn giá</th>'
            html += '</tr>'
            $.each(data, function (key, value) {
                console.log(data)
                console.log(value)
                html += '<tr>'
                html += '<td><label style="width: auto">' + n + '</label></td>'
                html += '<td><label style="width: auto">' + (value.booking_id || '') + '</label></td>'
                html += '<td><label style="width: auto">' + (value.package_name || '') + '</label></td>'
                html += '<td><label style="width: auto">' + (value.price || '') + '</label></td>'
                html += '</tr>'
                n += 1;
            })
            html += '</table>'
            html += '</div>'
        } else {
            html += "Không có dữ liệu chi tiết đơn hàng"
        }
        $(".modal-body").html(html);
        $('#orderModal').modal('show');
    })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.error("Lỗi: " + textStatus, errorThrown);
        });
}
function changeStatusOrder(order_id) {
    if (confirm("Bạn muốn thay đổi trạng thái đơn hàng?")) {
        $.ajax({
            url: "/HostBooking/Edit",
            type: "POST",
            data: {
                Id: order_id,
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
function xoaProduct(Id) {
    if (confirm("Bạn muốn xóa bữa tiệc?")) {
        $.ajax({
            url: "/HostPackage/Delete",
            type: "POST",
            data: {
                Id: Id
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
            url: "/GuestRequest/Delete",
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

function xoaReport(category_request_id) {
    if (confirm("Bạn muốn xóa yêu cầu thêm mới danh mục?")) {
        $.ajax({
            url: "/GuestReport/Delete",
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
function huydonhang(order_id) {
    if (confirm("Nếu bạn hủy sau thời gian quy định sẽ mất cọc, Bạn có chắc muốn hủy đơn?")) {
        $.ajax({
            url: "/Booking/HuyDon",
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

function dathanhtoan(Id) {
    if (confirm("Bạn muốn chuyển sang trạng thái 'đã thanh toán'?")) {
        $.ajax({
            url: "/HostBooking/dathanhtoan",
            type: "POST",
            data: {
                Id: Id
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

function formatDate(dateString) {
    var date = new Date(dateString);

    var day = date.getDate();
    var month = date.getMonth() + 1; // Lưu ý: Tháng bắt đầu từ 0
    var year = date.getFullYear();

    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();

    var period = "AM";
    if (hours >= 12) {
        period = "PM";
        hours = hours % 12;
    }
    if (hours === 0) {
        hours = 12;
    }

    var formattedDate = `${day}-${month}-${year} ${hours}:${minutes}:${seconds} ${period}`;
    return formattedDate;
}


function changeSelectRequest(type) {
    var selectElement = document.getElementById("selectRequest");

    var selectedValue = selectElement.value;
    $.getJSON('/' + type + '/getRequest?is_approved=' + selectedValue, function (data) {
        var html = '';
        if (data != null) {
            $.each(data, function (index, value) { // Sử dụng biến index thay vì item
                console.log(value);
                html += '<tr>';
                html += '<td>' + (index + 1) + '</td>'; // Sử dụng biến index để đếm
                html += '<td><label style="width: auto">' + value.category_name + '</label></td>';
                html += '<td><label style="width: auto">' + formatDate(value.created_at) + '</label></td>';
                html += '<td><label style="width: auto">' + value.rejection_reason + '</label></td>';
                html += '<td><label style="width: auto">' + value.host_name + '</label></td>';
                html += '<td><label style="width: auto">' + value.mail + '</label></td>';
                html += '<td>';
                if (value.is_approved == -1) {
                    html += '<label style="width: auto">Yêu cầu bị từ chối</label>';
                } else if (value.is_approved == 1) {
                    html += '<label style="width: auto">Yêu cầu được duyệt</label>';
                } else {
                    html += '<label style="width: auto">Yêu cầu chờ duyệt</label>';
                }
                html += '</td>';
                html += '<td>';
                if (value.is_approved == 0) {
                    if (type == "HostRequest") {
                        html += '<a class="btn btn-primary" onclick="Duyet(' + value.category_request_id + ')">Duyệt</a>';
                        html += '<a class="btn btn-success" onclick="TuChoi(' + value.category_request_id + ')">Từ chối</a>';
                    } else {
                        html += '<a class="btn btn-primary" onclick="xoaRequest(' + value.category_request_id + ')">Xóa</a>';
                        html += '<a class="btn btn-success" onclick="openUpdate(' + value.category_request_id + ')">Sửa</a>';
                    }
                }
                html += '</td>';
                html += '</tr>';
            });
        } else {
            html += '<p class="alert alert-warning">Danh sách yêu cầu mới</p>';
        }
        $("#bodyRequest").html(html);

        // Tạo modal ở đây, bên ngoài vòng lặp
        $.each(data, function (index, value) {
            var modalHtml = '<div class="modal fade" id="update' + value.category_request_id + '" tabindex="-1" role="dialog" aria-labelledby="productModalLabel" aria-hidden="true">';
            modalHtml += '<div class="modal-dialog modal-dialog-centered" role="document">';
            modalHtml += '<div class="modal-content">';
            modalHtml += '<form method="post" action="/GuestRequest/Edit">';
            modalHtml += '<div class="modal-header">';
            modalHtml += '<h5 class="modal-title" id="productModalLabel">Chỉnh sửa gói</h5>';
            modalHtml += '</div>';
            modalHtml += '<div class="modal-body">';
            modalHtml += '<div class="form-group">';
            modalHtml += '<label>Yêu cầu</label>';
            modalHtml += '<input type="hidden" value="' + value.category_request_id + '" name="category_request_id" />';
            modalHtml += '<input type="text" class="form-control" name="category_name" value="' + value.category_name + '" required placeholder="Nhập yêu cầu">';
            modalHtml += '</div>';
            modalHtml += '</div>';
            modalHtml += '<div class="modal-footer">';
            modalHtml += '<button type="button" onclick="closeUpdate(' + value.category_request_id + ')" class="btn btn-secondary">Đóng</button>';
            modalHtml += '<button type="submit" class="btn btn-success">Sửa</button>';
            modalHtml += '</div>';
            modalHtml += '</form>';
            modalHtml += '</div>';
            modalHtml += '</div>';
            modalHtml += '</div>';

            $("#bodyRequest").append(modalHtml);
        });
    });
}

function changeSelectRequestHost(type) {
    var selectElement = document.getElementById("selectRequest");

    var selectedValue = selectElement.value;
    $.getJSON('/' + type + '/getRequest?is_approved=' + selectedValue, function (data) {
        var html = '';
        if (data != null) {
            $.each(data, function (index, value) { // Sử dụng biến index thay vì item
                console.log(value);
                html += '<tr>';
                html += '<td>' + (index + 1) + '</td>'; // Sử dụng biến index để đếm
                html += '<td><label style="width: auto">' + value.category_name + '</label></td>';
                html += '<td><label style="width: auto">' + formatDate(value.created_at) + '</label></td>';
                html += '<td><label style="width: auto">' + value.rejection_reason + '</label></td>';
                html += '<td>';
                if (value.is_approved == -1) {
                    html += '<label style="width: auto">Yêu cầu bị từ chối</label>';
                } else if (value.is_approved == 1) {
                    html += '<label style="width: auto">Yêu cầu được duyệt</label>';
                } else {
                    html += '<label style="width: auto">Yêu cầu chờ duyệt</label>';
                }
                html += '</td>';
                html += '<td>';
                if (value.is_approved == 0) {
                    if (type == "HostRequest") {
                        html += '<a class="btn btn-primary" onclick="Duyet(' + value.category_request_id + ')">Duyệt</a>';
                        html += '<a class="btn btn-success" onclick="TuChoi(' + value.category_request_id + ')">Từ chối</a>';
                    } else {
                        html += '<a class="btn btn-primary" onclick="xoaRequest(' + value.category_request_id + ')">Xóa</a>';
                        html += '<a class="btn btn-success" onclick="openUpdate(' + value.category_request_id + ')">Sửa</a>';
                    }
                }
                html += '</td>';
                html += '</tr>';
            });
        } else {
            html += '<p class="alert alert-warning">Danh sách yêu cầu mới</p>';
        }
        $("#bodyRequest").html(html);

        // Tạo modal ở đây, bên ngoài vòng lặp
        $.each(data, function (index, value) {
            var modalHtml = '<div class="modal fade" id="update' + value.category_request_id + '" tabindex="-1" role="dialog" aria-labelledby="productModalLabel" aria-hidden="true">';
            modalHtml += '<div class="modal-dialog modal-dialog-centered" role="document">';
            modalHtml += '<div class="modal-content">';
            modalHtml += '<form method="post" action="/GuestRequest/Edit">';
            modalHtml += '<div class="modal-header">';
            modalHtml += '<h5 class="modal-title" id="productModalLabel">Chỉnh sửa gói</h5>';
            modalHtml += '</div>';
            modalHtml += '<div class="modal-body">';
            modalHtml += '<div class="form-group">';
            modalHtml += '<label>Yêu cầu</label>';
            modalHtml += '<input type="hidden" value="' + value.category_request_id + '" name="category_request_id" />';
            modalHtml += '<input type="text" class="form-control" name="category_name" value="' + value.category_name + '" required placeholder="Nhập yêu cầu">';
            modalHtml += '</div>';
            modalHtml += '</div>';
            modalHtml += '<div class="modal-footer">';
            modalHtml += '<button type="button" onclick="closeUpdate(' + value.category_request_id + ')" class="btn btn-secondary">Đóng</button>';
            modalHtml += '<button type="submit" class="btn btn-success">Sửa</button>';
            modalHtml += '</div>';
            modalHtml += '</form>';
            modalHtml += '</div>';
            modalHtml += '</div>';
            modalHtml += '</div>';

            $("#bodyRequest").append(modalHtml);
        });
    });
}




function changeSelectReport(type) {
    var selectElement = document.getElementById("selectReport");

    var selectedValue = selectElement.value;
    $.getJSON('/' + type + '/getReport?is_approved=' + selectedValue, function (data) {
        var html = '';
        if (data != null) {
            $.each(data, function (index, value) {
                console.log(value);
                html += '<tr>';
                html += '<td>' + (index + 1) + '</td>';
                html += '<td><label style="width: auto">' + value.report + '</label></td>';
                html += '<td><label style="width: auto">' + value.host_name + '</label></td>';
                html += '<td><label style="width: auto">' + value.mail + '</label></td>';
                html += '<td><label style="width: auto">' + formatDate(value.created_at) + '</label></td>';
                html += '<td><label style="width: auto">' + value.rejection_reason + '</label></td>';
                html += '<td>';
                if (value.is_approved == -1) {
                    html += '<label style="width: auto">Khiếu nại bị từ chối</label>';
                } else if (value.is_approved == 1) {
                    html += '<label style="width: auto">Khiếu nại được duyệt</label>';
                } else {
                    html += '<label style="width: auto">Khiếu nại chờ duyệt</label>';
                }
                html += '</td>';
                html += '<td>';
                if (value.is_approved == 0) {
                    if (type == "AdminReport") {
                        html += '<a class="btn btn-primary" onclick="Duyet(' + value.category_request_id + ')">Duyệt</a>';
                        html += '<a class="btn btn-success" onclick="TuChoi(' + value.category_request_id + ')">Từ chối</a>';
                    } else {
                        html += '<a class="btn btn-primary" onclick="xoaRequest(' + value.category_request_id + ')">Xóa</a>';
                        html += '<a class="btn btn-success" onclick="openUpdate(' + value.category_request_id + ')">Sửa</a>';
                    }
                }
                html += '</td>';
                html += '</tr>';
            });

            // Tạo modal ở đây, bên ngoài vòng lặp
            $.each(data, function (index, value) {
                var modalHtml = '<div class="modal fade" id="update' + value.category_request_id + '" tabindex="-1" role="dialog" aria-labelledby="productModalLabel" aria-hidden="true">';
                modalHtml += '<div class="modal-dialog modal-dialog-centered" role="document">';
                modalHtml += '<div class="modal-content">';
                modalHtml += '<form method="post" action="/GuestRequest/Edit">';
                modalHtml += '<div class="modal-header">';
                modalHtml += '<h5 class="modal-title" id="productModalLabel">Chỉnh sửa khiếu nại</h5>';
                modalHtml += '</div>';
                modalHtml += '<div class="modal-body">';
                modalHtml += '<div class="form-group">';
                modalHtml += '<label>Khiếu nại</label>';
                modalHtml += '<input type="hidden" value="' + value.category_request_id + '" name="category_request_id" />';
                modalHtml += '<input type="text" class="form-control" name="category_name" value="' + value.category_name + '" required placeholder="Nhập khiếu nại">';
                /*modalHtml += '<input type="text" class="form-control" name="host_name" value="' + value.host_name + '" required placeholder="Nhập tên chủ tiệc">';*/
                modalHtml += '</div>';
                modalHtml += '</div>';
                modalHtml += '<div class="modal-footer">';
                modalHtml += '<button type="button" onclick="closeUpdate(' + value.category_request_id + ')" class="btn btn-secondary">Đóng</button>';
                modalHtml += '<button type="submit" class="btn btn-success">Sửa</button>';
                modalHtml += '</div>';
                modalHtml += '</form>';
                modalHtml += '</div>';
                modalHtml += '</div>';
                modalHtml += '</div>';

                $("#bodyRequest").append(modalHtml);
            });
        } else {
            html += '<p class="alert alert-danger">Danh sách khiếu nại mới</p>';
        }
        $("#bodyRequest").html(html);
    });
}


function Duyet(category_request_id) {
    if (confirm("Bạn muốn duyệt yêu cầu mới?")) {
        $.ajax({
            url: "/HostRequest/Approved",
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
        url: "/HostRequest/Approved",
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

function DuyetDon(Id) {
    if (confirm("Bạn muốn nhận đơn?")) {
        $.ajax({
            url: "/HostBooking/DuyetDon",
            type: "POST",
            data: {
                Id: Id,
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

function TuChoiDon(Id) {
    var lyDo = prompt("Nhập lý do từ chối:");

    if (lyDo === null || lyDo.trim() === "") {
        return;
    }

    $.ajax({
        url: "/HostBooking/TuChoiDon",
        type: "POST",
        data: {
            Id: Id,
            Reason: lyDo
        },
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            window.location.reload();
        }
    });
}

function DuyetReport(category_request_id) {
    if (confirm("Bạn muốn duyệt yêu cầu mới?")) {
        $.ajax({
            url: "/AdminReport/Approved",
            type: "POST",
            data: {
                category_request_id: category_request_id,
                is_approved: 1,
            },
            success: function (response) {
                window.location.reload();
            },
            error: function (xhr, status, error) {
                window.location.reload();
            }
        });
    }
}


function TuChoiReport(category_request_id) {
    var lyDo = prompt("Nhập lý do từ chối:");

    if (lyDo === null || lyDo.trim() === "") {
        return;
    }

    $.ajax({
        url: "/AdminReport/Approved",
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


document.addEventListener("DOMContentLoaded", function () {
    // Code JavaScript của bạn ở đây
    document.getElementById('selectArea').onchange = function () {
        var areaId = this.value;
        document.getElementById("checkboxOptions").innerHTML = '';
        fetch(`/HostPackage/GetLocationsByAreaId?areaId=${areaId}`)
            .then(response => response.json())
            .then(data => {
                var select = document.getElementById("selectLocation");
                select.innerHTML = '<option value="">Chọn địa điểm</option>';
                data.forEach(function (item) {
                    var option = new Option(item.name, item.name); // Sử dụng Name làm value để truy vấn sau này
                    select.add(option);
                });
            }).catch(error => console.error('Error:', error));
    };

});

document.addEventListener("DOMContentLoaded", function () {
    // Code JavaScript của bạn ở đây
    document.getElementById('selectLocation').onchange = function () {
        var locationName = this.value;
        var areaId = document.getElementById('selectArea').value; // Lấy AreaId từ dropdown selectArea
        fetch(`/HostPackage/GetAddressesByLocationNameAndAreaId?locationName=${encodeURIComponent(locationName)}&areaId=${areaId}`)
            .then(response => response.json())
            .then(data => {
                var checkboxesDiv = document.getElementById("checkboxOptions");
                checkboxesDiv.innerHTML = ''; // Xóa các checkbox cũ
                data.forEach(function (item) {
                    var checkbox = document.createElement('input');
                    checkbox.type = 'checkbox';
                    checkbox.id = 'address-' + item.id;
                    checkbox.value = item.address;
                    checkbox.name = 'selectedAddresses[]';

                    var label = document.createElement('label');
                    label.htmlFor = 'address-' + item.id;
                    label.textContent = item.address;

                    checkboxesDiv.appendChild(checkbox);
                    checkboxesDiv.appendChild(label);
                    checkboxesDiv.appendChild(document.createElement('br'));
                });
            }).catch(error => console.error('Error:', error));
    };

});

document.addEventListener("DOMContentLoaded", function () {
    // Code JavaScript của bạn ở đây
    document.getElementById('selectAreaEdit').onchange = function () {
        var areaId = this.value;
        document.getElementById("checkboxOptionsEdit").innerHTML = '';
        fetch(`/HostPackage/GetLocationsByAreaId?areaId=${areaId}`)
            .then(response => response.json())
            .then(data => {
                var select = document.getElementById("selectLocationEdit");
                select.innerHTML = '<option value="">Chọn địa điểm</option>';
                data.forEach(function (item) {
                    var option = new Option(item.name, item.name); // Sử dụng Name làm value để truy vấn sau này
                    select.add(option);
                });
            }).catch(error => console.error('Error:', error));
    };

});

document.addEventListener("DOMContentLoaded", function () {
    // Code JavaScript của bạn ở đây
    document.getElementById('selectLocationEdit').onchange = function () {
        var locationName = this.value;
        var areaId = document.getElementById('selectAreaEdit').value; // Lấy AreaId từ dropdown selectArea
        fetch(`/HostPackage/GetAddressesByLocationNameAndAreaId?locationName=${encodeURIComponent(locationName)}&areaId=${areaId}`)
            .then(response => response.json())
            .then(data => {
                var checkboxesDiv = document.getElementById("checkboxOptionsEdit");
                checkboxesDiv.innerHTML = ''; // Xóa các checkbox cũ
                data.forEach(function (item) {
                    var checkbox = document.createElement('input');
                    checkbox.type = 'checkbox';
                    checkbox.id = 'address-' + item.id;
                    checkbox.value = item.address;
                    checkbox.name = 'selectedAddresses[]';

                    var label = document.createElement('label');
                    label.htmlFor = 'address-' + item.id;
                    label.textContent = item.address;

                    checkboxesDiv.appendChild(checkbox);
                    checkboxesDiv.appendChild(label);
                    checkboxesDiv.appendChild(document.createElement('br'));
                });
            }).catch(error => console.error('Error:', error));
    };

});


document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('selectAreaCreate').onchange = function () {
        var areaId = this.value;
        fetch(`/HostPackage/GetLocationsByAreaId?areaId=${areaId}`)
            .then(response => response.json())
            .then(data => {
                var locationOptions = document.getElementById("locationOptionsCreate");
                locationOptions.innerHTML = ''; // Xóa các checkbox cũ
                data.forEach(function (item) {
                    var checkbox = document.createElement('input');
                    checkbox.type = 'checkbox';
                    checkbox.id = 'location-' + item.name.replace(/\s+/g, ''); // Đảm bảo id là duy nhất
                    checkbox.value = item.name;
                    checkbox.name = 'selectedLocationsCreate[]';

                    var label = document.createElement('label');
                    label.htmlFor = 'location-' + item.name.replace(/\s+/g, ''); // Đảm bảo id là duy nhất
                    label.textContent = item.name;

                    locationOptions.appendChild(checkbox);
                    locationOptions.appendChild(label);
                    locationOptions.appendChild(document.createElement('br'));
                });
                // Hiển thị Checkbox 2 sau khi đã có dữ liệu
                document.getElementById('checkboxLocationCreate').style.display = 'block';
            }).catch(error => console.error('Error:', error));
    };
});

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('locationOptionsCreate').onclick = function () {
        var selectedLocations = Array.from(document.querySelectorAll('input[name="selectedLocationsCreate[]"]:checked'))
            .map(checkbox => checkbox.value);
        if (selectedLocations.length > 0) {
            var areaId = document.getElementById('selectAreaCreate').value;
            fetch(`/HostPackage/GetAddressesByLocationsAndAreaId`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ locationNames: selectedLocations, areaId: areaId })
            })
                .then(response => response.json())
                .then(data => {
                    var addressOptions = document.getElementById("addressOptionsCreate");
                    addressOptions.innerHTML = ''; // Xóa các checkbox cũ
                    data.forEach(function (item) {
                        var checkbox = document.createElement('input');
                        checkbox.type = 'checkbox';
                        checkbox.id = 'address-' + item.id;
                        checkbox.value = item.address;
                        checkbox.name = 'selectedAddresses[]';

                        var label = document.createElement('label');
                        label.htmlFor = 'address-' + item.id;
                        label.textContent = item.address;

                        addressOptions.appendChild(checkbox);
                        addressOptions.appendChild(label);
                        addressOptions.appendChild(document.createElement('br'));
                    });
                    // Hiển thị Checkbox Group sau khi đã có dữ liệu
                    document.getElementById('checkboxAddressCreate').style.display = 'block';
                }).catch(error => console.error('Error:', error));
        }
    };
});



