let activePage = 1;
let currentPage = 1; // Şu anki sayfa
const pageSize = 10; // Her sayfada gösterilecek marka sayısı

document.addEventListener('DOMContentLoaded', (event) => {
    getProductData();
    //paginate(activePage);
    //search();
    //setupPaginationClickHandler();
});

function getPageData(page, pageSize, data) {
    return data.slice((page - 1) * pageSize, page * pageSize);
}


function getBrandData() {
    $.ajax({
        url: 'http://localhost:64238/trendyol/markalar',
        type: 'GET',
        success: function (data) {
            var jsonData = JSON.parse(data); // Veriyi JSON nesnesine dönüştür
            var brands = jsonData.brands; // brands dizisine eriş
            var pagedData = getPageData(currentPage, pageSize, brands);
            document.getElementById('totalDataCount').innerText = `Toplam ${brands.length} Kayıt`;
            var tableBody = $('#listContainer');
            tableBody.empty();
            pagedData.forEach(function (item) {
                var row = `<tr>
                        <td>${item.id}</td>
                        <td>${item.name}</td>
                    </tr>`;

                tableBody.append(row); // Yeni satırı ekle
            });




            // brands üzerinde istediğin işlemleri burada yapabilirsin.
            //brands.forEach(function (brand) {
            //    console.log('Brand ID:', brand.id, 'Brand Name:', brand.name);
            //});
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}

function getCategoryData() {
    $.ajax({
        url: 'http://localhost:64238/trendyol/kategoriler',
        type: 'GET',
        success: function (data) {
            document.getElementById('jsonData').innerText = data;
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}

function getProductData() {
    $.ajax({
        url: 'http://localhost:64238/trendyol/urunler',
        type: 'GET',
        success: function (data) {
            var tableBody = $('#listContainer');
            tableBody.empty();
            //updatePagination(response.pageCount, currentPage);
            document.getElementById('totalDataCount').innerText = `Toplam ${data.totalElements} Kayıt`;
            //const startRecord = ((response.page - 1) * response.perPage) + 1;
            //let endRecord = response.page * response.perPage;
            //endRecord = endRecord > response.totalCount ? response.totalCount : endRecord;
            //document.getElementById('listCount').innerText = `${response.totalCount} kayıt içinden ${startRecord} - ${endRecord} arası listeleniyor`;
            console.log(data, "dt");
            let htmlContent = `
                <div>Page: ${data.page}</div>
                <div>Size: ${data.size}</div>
                <div>Total Pages: ${data.totalPages}</div>
                <div>Total Elements: ${data.totalElements}</div>
                <div>Products:</div>
                <ul>`;
            data.content.forEach(function (product) {
                let attributesTable = `<table>${product.Attributes.map(attr => `
        <tr>
            <td>${attr.AttributeName}</td>
            <td>${attr.AttributeValue}</td>
        </tr>
    `).join('')}</table>`; // Özellikler için HTML tablosu
                var row = `<tr>
   <td class="sticky-column">${
                    product.Images.length > 0
                        ? `<div class="image-container"><img src="${product.Images[0].Url}" class="img" alt="Ürün Resmi" /></div>`
                        : "YOK"
    }</td>
                        <td class="sticky-column">${product.Title}</td>
                    <td>

<a href="#add-tag" class="waves-effect waves-light btn btn-extra-small popoverButton" id="popoverButton${product.Id}"><i class="icn ti-eye" aria-hidden="true"></i></a>

                        <div id="popoverContent${product.Id}" class="popover" style="display:none;">
                            ${product.Description}
                        </div>
                    </td>
                        <td>${product.Barcode}</td>
                        <td>${product.Brand}</td>
                        <td>${product.CategoryName}</td>
                        <td>${product.DimensionalWeight}</td>
                        <td>${product.ListPrice}</td>
                        <td>${product.Quantity}</td>
                        <td>${product.SalePrice}</td>
                        <td>${product.StockCode}</td>
                        <td>${product.Version}</td>
                        <td>${product.VatRate}</td>
    <td>
<a href="#add-tag" class="waves-effect waves-light btn btn-extra-small popoverButtonattr" id="popoverButtonattr${product.Id}"><i class="icn ti-eye" aria-hidden="true"></i></a>

                        <div id="popoverContentattr${product.Id}" class="popoverattr" style="display:none;">
                            ${attributesTable}
                        </div>
</td>
                        <td class="sticky-column-right"><div class="icn-td">
                            <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Düzenle" onclick="openModalWithGuid('${product.Id}')"><i class="icn ti-pencil-alt" aria-hidden="true"></i></a>
                            <button type="button" onclick="deleteProductData('${product.Barcode}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                        </div></td>
                    </tr>`;

                tableBody.append(row); // Yeni satırı ekle


                $(`#popoverButton${product.Id}`).on('click', function () {
                    var popover = $(`#popoverContent${product.Id}`);
                    if (popover.css('display') === 'none') {
                        popover.css('display', 'block');
                    } else {
                        popover.css('display', 'none');
                    }
                });

                $(`#popoverButtonattr${product.Id}`).on('click', function () {
                    var popoverattr = $(`#popoverContentattr${product.Id}`);

                    if (popoverattr.css('display') === 'none') {
                        popoverattr.show(); // Popover'ı göster

                        var buttonOffset = $(this).offset();
                        var buttonHeight = $(this).outerHeight();
                        var popoverWidth = popoverattr.outerWidth();
                        var buttonWidth = $(this).outerWidth();

                        // Popover'ı butonun altında ve sol hizalı konumlandır
                        var topPosition = buttonOffset.top + buttonHeight; // Butonun altı
                        var leftPosition = buttonOffset.left + buttonWidth - popoverWidth; // Butonun solu

                        // Eğer popover ekranın sol sınırının dışına taşarsa, onu butonun sağ tarafına hizala
                        if (leftPosition < 0) {
                            leftPosition = buttonOffset.left; // Butonun sol kenarına hizala
                        }

                        popoverattr.css({
                            'top': topPosition,
                            'left': leftPosition
                        });
                    } else {
                        popoverattr.hide(); // Popover'ı gizle
                    }
                });



            });

        },
        error: function (xhr, status, error) {
            swal("Hata!", "Ürün bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}


function deleteProductData(barcode) {
    $.ajax({
        url: 'http://localhost:64238/trendyol/urun-sil',
        type: 'DELETE',
        headers: {
            'Barcode': barcode // Barcode değerini burada header olarak ekliyoruz
        },
        success: function (resp) {
            alert(resp);
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Ürün silinirken bir hata oluştu: " + error, "error");
        }
    });
}