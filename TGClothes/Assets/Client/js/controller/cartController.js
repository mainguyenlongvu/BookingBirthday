var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/san-pham";
        });

        $('#btnPayment').off('click').on('click', function () {
            window.location.href = "/thanh-toan";
        });

        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(this).val(),
                    Product: {
                        Id: $(item).data('product-id')
                    },
                    Size: {
                        Id: $(item).data('size-id')
                    }
                })
            });

            $.ajax({
                url: 'Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang"
                    }
                }
            });
        });

        $('#btnDeleteAll').off('click').on('click', function () {
            if (confirm("Bạn có chắc chắn muốn xóa toàn bộ sản phẩm?")) {
                $.ajax({
                    url: 'Cart/DeleteAll',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/gio-hang"
                        }
                    }
                });
            }
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();

            if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này?")) {
                $.ajax({
                    url: 'Cart/Delete',
                    data: { productId: $(this).data('product-id'), sizeId: $(this).data('size-id') },
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/gio-hang"
                        }
                    }
                });
            }
        });
    }
}
cart.init();