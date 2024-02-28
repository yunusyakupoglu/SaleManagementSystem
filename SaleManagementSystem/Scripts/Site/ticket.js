var selectedStocks = [];
let company;
document.addEventListener('DOMContentLoaded', (event) => {
    setupEventListeners();
});

//--------------------------------------------------------------------------------------------------------------------------
function setupEventListeners() {
    selectedStocks.forEach(function (stock) {
        $(`#quantity-${stock.Stock}, #sellPrice-${stock.Stock}, #vat-${stock.Stock}, #discount-${stock.Stock}`).on('input', function () {
            calculateTotals();
        });
    });
}

function calculateTotals() {
    var total = 0, totalDiscount = 0, totalVAT = 0, netTotal = 0, subTotal = 0;

    $.each(selectedStocks, function (i, stock) {
        var quantity = parseFloat($('#quantity-' + stock.Stock).val()) || 0;
        var sellPrice = parseFloat($('#sellPrice-' + stock.Stock).val()) || 0;
        var vat = parseFloat($('#vat-' + stock.Stock).val()) || 0;
        var discount = parseFloat($('#discount-' + stock.Stock).val()) || 0;

        var totalSellPrice = quantity * sellPrice;
        var discountAmount = (totalSellPrice * discount) / 100;
        var vatAmount = (totalSellPrice - discountAmount) * (vat / 100);

        total += totalSellPrice;
        totalDiscount += discountAmount;
        subTotal += totalSellPrice - discountAmount;
        totalVAT += vatAmount;
        netTotal += totalSellPrice - discountAmount + vatAmount;
    });

    $('#total').text(total.toFixed(2));
    $('#totalDiscount').text(totalDiscount.toFixed(2));
    $('#subTotalValue').text(subTotal.toFixed(2));
    $('#totalVAT').text(totalVAT.toFixed(2));
    $('#netTotal').text(netTotal.toFixed(2));
}

function updateSelectedStocks(guid, brandName, productName, imgName, unitName, sellPrice,vat, quantity) {
    var checkBox = document.getElementById(guid);
    if (checkBox.checked) {
        selectedStocks.push({ Stock: guid, BrandName: brandName, ProductName: productName, ImgName: imgName, UnitName: unitName, SellPrice: sellPrice, Vat: vat, Quantity: quantity });
    } else {
        selectedStocks = selectedStocks.filter(function (stock) {
            return stock.Stock !== guid || stock.BrandName !== brandName || stock.ProductName !== productName || stock.ImgName !== imgName || stock.UnitName !== unitName || stock.SellPrice !== sellPrice || stock.Vat !== vat || stock.Quantity !== quantity;
        });
    }
}

function isDecimal(evt, element) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (
        (charCode != 46 || $(element).val().indexOf('.') != -1) && // virgül ',' tek olmalı
        (charCode < 48 || charCode > 57) // sadece rakamlara izin ver
    )
        return false;

    return true;
}

function preventExceedingMax(event, element, max) {
    var attemptedValue = element.value + event.key;
    if (Number.isInteger(max)) {
        max = max + '.00';
    }
    var floatAttemptedValue = parseFloat(attemptedValue);
    var floatMax = parseFloat(max);
    return floatAttemptedValue <= floatMax;
}

function setValues(guid) {
    $.ajax({
        url: '/Companies/GetByGuid',
        type: 'GET',
        data: { guid: guid },
        success: function (response) {
            if (response && response.data) {

                document.getElementById('companyName').value = response.data.CompanyName;
                document.getElementById('taxNumber').value = response.data.TaxNumber;
                document.getElementById('taxOffice').value = response.data.TaxOffice;
                document.getElementById('email').value = response.data.Email;
                document.getElementById('phone').value = response.data.Phone;
                document.getElementById('address').value = response.data.Address;
                company = response.data.Guid;
                var instance = M.Modal.getInstance(document.getElementById('add-company'));
                instance.close();



            } else {
                console.error("Textboxlar doldurulamadı.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Textboxlar doldurulamadı: " + error);
        }
    });

}

function Confirm() {
    var tableBody = $('#selectedStockList');
    tableBody.empty(); // Mevcut içeriği temizle
    selectedStocks.forEach(function (stock) {
        if (!$(`#row-${stock.Guid}`).length) {
            var row = `<tr id="row-${stock.Stock}">
                                    <td><div class="image-container"><img id="img-c-${stock.Stock}" src="/Files/Products/${stock.ImgName}" class="img" alt="${stock.ImgName}" /></div></td>
                        <td>${stock.BrandName} ${stock.ProductName}</td>
                        <td>
                                <input id="quantity-${stock.Stock}" placeholder="Miktar (MAX:${stock.Quantity})" type="text">
</td>
                        <td>${stock.UnitName}</td>
                        <td>
                                <input id="sellPrice-${stock.Stock}" placeholder="Satış Fiyatı" type="text">
</td>
                        <td>
                                <input id="vat-${stock.Stock}" placeholder="KDV" type="text">
</td>
                        <td>
                                <input id="discount-${stock.Stock}" placeholder="İndirim" type="text">
</td>
                        <td><p id="singleStockTotal-${stock.Stock}"></p></td>
<td>
<div class="icn-td">
                            <button type="button" onclick="removeList('${stock.Stock}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                        </div></td>
                    </tr>`;
            tableBody.append(row); // Yeni satırı ekle
        }
        var discountValue = $(`#discount-${stock.Stock}`).val() || '0'; // Eğer değer yoksa 0 olarak ayarla
        stock.Discount = discountValue;
        document.getElementById(`sellPrice-${stock.Stock}`).value = `${stock.SellPrice}`;
        document.getElementById(`vat-${stock.Stock}`).value = `${stock.Vat}`;
        setupEventListeners();
    });
}

function removeList(guid) {
    const index = selectedStocks.findIndex(stock => stock.Stock === guid);
    if (index > -1) {
        // Silinecek stock'ın değerlerini hesapla
        var stock = selectedStocks[index];
        var quantity = parseFloat($('#quantity-' + stock.Stock).val()) || 0;
        var sellPrice = parseFloat($('#sellPrice-' + stock.Stock).val()) || 0;
        var vat = parseFloat($('#vat-' + stock.Stock).val()) || 0;
        var discount = parseFloat($('#discount-' + stock.Stock).val()) || 0;

        var totalSellPrice = quantity * sellPrice;
        var discountAmount = (totalSellPrice * discount) / 100;
        var vatAmount = (totalSellPrice - discountAmount) * (vat / 100);

        // Toplamlardan bu değerleri çıkar
        $('#total').text((parseFloat($('#total').text()) - totalSellPrice).toFixed(2));
        $('#totalDiscount').text((parseFloat($('#totalDiscount').text()) - discountAmount).toFixed(2));
        $('#subTotalValue').text((parseFloat($('#subTotalValue').text()) - (totalSellPrice - discountAmount)).toFixed(2));
        $('#totalVAT').text((parseFloat($('#totalVAT').text()) - vatAmount).toFixed(2));
        $('#netTotal').text((parseFloat($('#netTotal').text()) - (totalSellPrice - discountAmount + vatAmount)).toFixed(2));

        // Stock'ı listeden sil
        selectedStocks.splice(index, 1);
    }
    var checkBox = document.getElementById(guid);
    checkBox.checked = false;
    var tableBody = $('#selectedStockList');
    tableBody.empty(); // Mevcut içeriği temizle
    selectedStocks.forEach(function (stock) {
        var row = `<tr>
                                    <td><div class="image-container"><img id="img-r-${stock.Stock}" src="/Files/Products/${stock.ImgName}" class="img" alt="${stock.ImgName}" /></div></td>
                        <td>${stock.BrandName} ${stock.ProductName}</td>
                        <td>
                                <input id="quantity-${stock.Stock}" placeholder="Miktar (MAX:${stock.Quantity})" type="text">
</td>
                        <td>${stock.UnitName}</td>
                        <td>
                                <input id="sellPrice-${stock.Stock}" placeholder="Satış Fiyatı" type="text">
</td>
                        <td>
                                <input id="vat-${stock.Stock}" placeholder="KDV" type="text">
</td>
                        <td>
                                <input id="discount-${stock.Stock}" placeholder="İndirim" type="text">
</td>
                        <td><p id="singleStockTotal-${stock.Stock}"></p></td>
<td>
<div class="icn-td">
                            <button type="button" onclick="removeList('${stock.Stock}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                        </div></td>
                    </tr>`;
        tableBody.append(row); // Yeni satırı ekle
        document.getElementById(`sellPrice-${stock.Stock}`).value = `${stock.SellPrice}`;
        document.getElementById(`vat-${stock.Stock}`).value = `${stock.Vat}`;
        setupEventListeners();
    });
}

function insertTicket() {
    var ticket = {
        Company: company,
        SumSellPrice: formatDecimal(document.getElementById('total').textContent),
        SumVat: formatDecimal(document.getElementById('totalVAT').textContent),
        SumDiscount: formatDecimal(document.getElementById('totalDiscount').textContent),
        SubSellPrice: formatDecimal(document.getElementById('subTotalValue').textContent),
        NetTotalPrice: formatDecimal(document.getElementById('netTotal').textContent)
    }

    $.ajax({
        url: '/Ticket/Insert',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(ticket),
        success: function (response) {
            if (response.success) {
                insertTicketProducts(response.data);
            } else {
            }
        },
        error: function (xhr, status, error) {
            console.error("Textboxlar doldurulamadı: " + error);
        }
    });

}

function insertTicketProducts(guid) {
    updateStock(guid);

    $.ajax({
        url: '/TicketProduct/Insert',
        type: 'POST',
        data: JSON.stringify(selectedStocks),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                updateDatabaseStock(response.data);
                swal({
                    title: "Başarılı!",
                    text: response.data,
                    type: "success", // 'icon' yerine 'type' kullanılıyor
                    timer: 3000,
                });
            } else {
            }
        },
        error: function (xhr, status, error) {
            console.error("Textboxlar doldurulamadı: " + error);
        }
    });
}

function updateDatabaseStock(ticketProductResponse) {
    $.ajax({
        url: '/Stocks/UpdateRange',
        type: 'POST',
        data: JSON.stringify(ticketProductResponse),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                swal({
                    title: "Başarılı!",
                    text: "Sipariş alındı. Sipariş Fişleri'ne yönlendiriliyorsunuz.",
                    icon: "success",
                    timer: 3000,
                    button: false // Kullanıcıya buton göstermemek için
                });
                setTimeout(function () {
                    window.location.href = '/Ticket/Index'; // Timer sona erdiğinde yönlendirme
                }, 3000);
            } else {
                swal({
                    title: "Başarısız İşlem!",
                    text: "Sipariş alınamadı. Lütfen daha sonra tekrar deneyiniz.",
                    type: "danger", // 'icon' yerine 'type' kullanılıyor
                    timer: 3000,
                });
            }
        },
        error: function (xhr, status, error) {
            console.error("Textboxlar doldurulamadı: " + error);
        }
    });
}

function formatDecimal(value) {
    return Number(value.replace(',', '.'));
}

function updateStock(ticket) {
    selectedStocks.forEach((item) => {
        const quantityInput = document.getElementById(`quantity-${item.Stock}`);
        const sellPriceInput = document.getElementById(`sellPrice-${item.Stock}`);
        const vatInput = document.getElementById(`vat-${item.Stock}`);
        const discountInput = document.getElementById(`discount-${item.Stock}`);

        if (quantityInput || sellPriceInput || vatInput || discountInput) {
            const newQuantity = parseFloat(quantityInput.value);
            const newSellPrice = parseFloat(sellPriceInput.value);
            const newVat = parseFloat(vatInput.value);
            const newDiscount = discountInput.value ? parseInt(discountInput.value) : 0;

            if (!isNaN(newQuantity)) {
                item.Quantity = newQuantity;
            }

            if (!isNaN(newSellPrice)) {
                item.SellPrice = newSellPrice;
            }

            if (!isNaN(newVat)) {
                item.Vat = newVat;
            }

            item.Discount = newDiscount;
            item.Ticket = ticket;

        }
    });
}
//--------------------------------------------------------------------------------------------------------------------------




