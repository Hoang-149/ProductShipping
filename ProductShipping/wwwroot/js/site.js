$(document).ready(function () {
    let products = [];

    // Load products
    $.get('/api/orders/products', function (data) {
        products = data;
        data.forEach(product => {
            $('#productList').append(
                `<li>
                            <input type='checkbox' value='${product.id}' data-name='${product.name}' data-price='${product.price}' data-weight='${product.weight}' />
                        ${product.name} - $${product.price} - ${product.weight}g
                    </li>`
            );
        });
    });

    // Place Order
    $('#placeOrder').click(function () {
        let selected = [];
        $('#productList input:checked').each(function () {
            let id = parseInt($(this).val());
            let name = $(this).data('name').toString();
            let price = parseFloat($(this).data('price'));
            let weight = parseInt($(this).data('weight'));
            selected.push({ id, name, price, weight });
        });

        if (selected.length === 0) {
            alert("Please select at least one product.");
            return;
        }

        $.ajax({
            url: '/api/orders/placeorder',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(selected),
            success: function (packages) {
                $('#result').empty();
                if (packages.length === 0) {
                    $('#result').append("<p>No packages generated. Please check the selected products.</p>");
                } else {
                    packages.forEach((pkg, index) => {
                        $('#result').append(
                            `<div>
                                <h4>Package ${index + 1}</h4>
                                <p>Items: ${pkg.items.map(i => i.name).join(', ')}</p>
                                <p>Total Weight: ${pkg.totalWeight}g</p>
                                <p>Total Price: $${pkg.totalPrice}</p>
                                <p>Courier Price: $${pkg.courierPrice}</p>
                            </div>`
                        );
                    });
                }
            },
            error: function (xhr, status, error) {
                alert("An error occurred while placing the order. Please try again.");
                console.error("Error:", status, error);
            }
        });
    });
});