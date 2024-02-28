let activePage = 1;
document.addEventListener('DOMContentLoaded', (event) => {
    paginate(activePage);
    search();
    setupPaginationClickHandler();
    GetCategories('#categorySelect');
    GetBrands('#brandSelect');
});
function setupPaginationClickHandler() {
    $('.pagination').on('click', 'li', function () {
        var selectedPage = $(this).text();
        var currentPage = parseInt(selectedPage);
        $('.pagination li').removeClass('active');
        $(this).addClass('active');
        paginate(currentPage);
        activePage = currentPage;
    });
}
function paginate(currentPage) {
    if (currentPage === undefined) {
        currentPage = 1;
    }
    activePage = currentPage;

    var dataCount = document.getElementById('paginator-select').value;

    $.ajax({
        url: '/Products/Paginate',
        type: 'GET',
        data: { page: currentPage, dataCount: dataCount },
        success: function (response) {
            if (response && response.data && response.pageCount) {
                var tableBody = $('#listContainer');
                tableBody.empty();
                updatePagination(response.pageCount, currentPage);
                document.getElementById('totalDataCount').innerText = `Toplam ${response.totalCount} Kayıt`;
                const startRecord = ((response.page - 1) * response.perPage) + 1;
                let endRecord = response.page * response.perPage;
                endRecord = endRecord > response.totalCount ? response.totalCount : endRecord;
                document.getElementById('listCount').innerText = `${response.totalCount} kayıt içinden ${startRecord} - ${endRecord} arası listeleniyor`;
                response.data.forEach(function (item) {
                    var checkedAttribute = item.IsActive ? "checked='checked'" : "";
                    var row = `                            <tr>
                                    <td><div class="image-container"><img id="${item.Guid}" src="/Files/Products/${item.ImgName}" class="img" alt="${item.ImgName}" /></div></td>
                                    <td>${item.BrandName} ${item.ProductName}</td>
                                    <td>${item.CategoryName}</td>
                                    <td>${item.PurchasePrice} ₺</td>
                                    <td>${item.SellPrice} ₺</td>
                                    <td>${createLinksFromTags(item.Tags)}</td>
                                <td>
                                    <div class="switch">
                                        <label>
                                            <input id="checkbox" type="checkbox" data-guid="${item.Guid}" ${checkedAttribute} onchange="handleCheckboxChange(this)">
                                            <span class="lever"></span>
                                        </label>
                                    </div>
                                </td>
                                <td class="icn-td">
                                        <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Etiket Ekle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-bookmark" aria-hidden="true"></i></a>
                                        <a href="#edit-product-modal" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Ürünü Düzenle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-pencil-alt" aria-hidden="true"></i></a>
                                        <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Ürünü Görüntüle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-eye" aria-hidden="true"></i></a>
                                        <button type="button" onclick="remove('${item.Guid}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Ürünü Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                                </td>
                            </tr>
`;

                    tableBody.append(row); // Yeni satırı ekle
                });
            } else {
                console.error("Liste yenilenirken veri alınamadı.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Liste yenilenirken hata oluştu: " + error);
        }
    });
}
function updatePagination(pageCount, currentPage) {
    var paginationUl = $('.pagination');
    paginationUl.empty();

    // İlk sayfa ve önceki butonu
    paginationUl.append('<li class="waves-effect"><a href="#!"><i class="material-icons">chevron_left</i></a></li>');

    for (var i = 1; i <= pageCount; i++) {
        var activeClass = i === currentPage ? 'active' : '';
        paginationUl.append(`<li class="waves-effect ${activeClass}" id="page-${i}"><a href="#!">${i}</a></li>`);
    }

    // Son sayfa ve sonraki butonu
    paginationUl.append('<li class="waves-effect"><a href="#!"><i class="material-icons">chevron_right</i></a></li>');
}
function search() {
    var debouncedSearch = debounce(function () {
        var filter = $('#search').val();

        $.ajax({
            url: '/Products/Filter',
            type: 'GET',
            data: { filter: filter },
            success: function (response) {
                if (filter.length === 0) {
                    paginate(activePage);
                } else {
                    var tableBody = $('#listContainer');
                    tableBody.empty();
                    response.data.forEach(function (item) {
                        var checkedAttribute = item.IsActive ? "checked='checked'" : "";
                        var row = `                            <tr>
                                <td><div class="image-container"><img id="${item.Guid}" src="/Files/Products/${item.ImgName}" class="img" alt="${item.ImgName}" /></div></td>
                                    <td>${item.BrandName} ${item.ProductName}</td>
                                    <td>${item.CategoryName}</td>
                                    <td>${item.PurchasePrice} ₺</td>
                                    <td>${item.SellPrice} ₺</td>
                                    <td>${createLinksFromTags(item.Tags)}</td>
                                <td>
                                    <div class="switch">
                                        <label>
                                            <input id="checkbox" type="checkbox" data-guid="${item.Guid}" ${checkedAttribute} onchange="handleCheckboxChange(this)">
                                            <span class="lever"></span>
                                        </label>
                                    </div>
                                </td>
                                <td class="icn-td">
                                        <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Etiket Ekle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-bookmark" aria-hidden="true"></i></a>
                                        <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Ürünü Düzenle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-pencil-alt" aria-hidden="true"></i></a>
                                        <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Ürünü Görüntüle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-eye" aria-hidden="true"></i></a>
                                        <button type="button" onclick="remove('${item.Guid}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Ürünü Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                                </td>
                            </tr>
`;

                        tableBody.append(row); // Yeni satırı ekle
                    });
                }


            },
            error: function (xhr, status, error) {
                console.error("Arama sırasında hata oluştu: " + error);
            }
        });
    }, 250); // 250 milisaniye gecikme

    $('#search').on('input', debouncedSearch);
}
function debounce(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};
function insertTag() {
    var tagName = document.getElementById('tagName').value;
    var tagColor = document.getElementById('tagColor').value;
    var guid = document.getElementById('itemGuid').value;

    $.ajax({
        url: '/Tags/Insert',
        type: 'POST',
        data: { tagName: tagName, tagColor: tagColor, product: guid },
        success: function (response) {
            if (response.success) {
                swal({
                    title: "Başarılı!",
                    text: response.message,
                    type: "success", // 'icon' yerine 'type' kullanılıyor
                    timer: 3000,
                    onClose: () => { // 'willClose' yerine 'onClose' kullanılıyor
                        paginate(activePage);
                    }
                });
            } else {
                swal("Hata!", "Bir hata oluştu: " + response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Sunucu ile iletişim kurulamadı: " + error, "error");
        }
    });
}
function openModalWithGuid(guid) {
    document.getElementById('itemGuid').value = guid;

    $.ajax({
        url: '/Products/GetProduct',
        type: 'GET',
        data: { guid: guid },
        success: function (product) {
            // Form alanlarını doldur
            //document.getElementById('editUnitName').value = stock.data.UnitName;
            //document.getElementById('editQuantity').value = stock.data.Quantity;
            //document.getElementById('editStockGuid').value = stock.data.Guid;

            var brandGuid = product.data.Brand; // Stok verisinden ürün GUID'ını al
            var categoryGuid = product.data.Category; // Stok verisinden ürün GUID'ını al
            GetCategories('#editCategorySelect', categoryGuid);
            GetBrands('#editBrandSelect', brandGuid);
            // Ürün GUID'ını string türüne dönüştür

            // Label'i aktif hale getir (Materialize için)
            M.updateTextFields();

            // Modalı aç
            $('#edit-stock-modal').modal('open');
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}
function isDecimal(evt, element) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (
        (charCode != 44 || $(element).val().indexOf(',') != -1) && // virgül ',' tek olmalı
        (charCode < 48 || charCode > 57) // sadece rakamlara izin ver
    )
        return false;

    return true;
}
function handleCheckboxChange(checkboxElement) {
    var checkboxValue = checkboxElement.checked; // true veya false değerini alır
    var itemGuid = checkboxElement.getAttribute('data-guid'); // GUID değerini alır

    $.ajax({
        url: '/Products/UpdateStatus', // AJAX isteğinin gönderileceği URL
        type: 'POST',
        data: { guid: itemGuid, isActive: checkboxValue },
        success: function (response) {
            if (response.success) {
                paginate(activePage);
            } else {
                swal("Hata!", "Bir hata oluştu: " + response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Sunucu ile iletişim kurulamadı: " + error, "error");
        }
    });
}
function insert() {
    var fileInput = document.getElementById('file');
    var file = fileInput.files[0];
    var brandSelect = document.getElementById('brandSelect');
    var categorySelect = document.getElementById('categorySelect');
    var productName = document.getElementById('productName').value;
    var purchasePrice = document.getElementById('purchasePrice').value;
    var sellPrice = document.getElementById('sellPrice').value;
    var vat = document.getElementById('vat').value;

    var formData = new FormData();
    formData.append('file', file);
    formData.append('brand', brandSelect.value);
    formData.append('category', categorySelect.value);
    formData.append('productName', productName);
    formData.append('purchasePrice', purchasePrice);
    formData.append('sellPrice', sellPrice);
    formData.append('vat', vat);

    $.ajax({
        url: '/Products/Insert',
        type: 'POST',
        data: formData,
        contentType: false, // AJAX'ın varsayılan içerik tipini kullanmaması için
        processData: false, // FormData'nın işlenmemesi için
        success: function (response) {
            if (response.success) {
                swal({
                    title: "Başarılı!",
                    text: response.message,
                    type: "success", // 'icon' yerine 'type' kullanılıyor
                    timer: 3000,
                    onClose: () => { // 'willClose' yerine 'onClose' kullanılıyor
                        paginate(activePage);
                    }
                });
            } else {
                swal("Hata!", "Bir hata oluştu: " + response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Sunucu ile iletişim kurulamadı: " + error, "error");
        }
    });
}
function remove(guid) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu öğeyi silmek istediğinize emin misiniz?",
        showCancelButton: true,
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'İptal',
        reverseButtons: true
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: '/Products/Delete',
                type: 'POST',
                data: { guid: guid },
                success: function (response) {
                    if (response.success) {
                        Swal.fire('Silindi!', response.message, 'success');
                    } else {
                        Swal.fire('Hata!', 'Bir hata oluştu: ' + response.message, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire('Hata!', 'Sunucu ile iletişim kurulamadı: ' + error, 'error');
                }
            }).always(() => {
                paginate(activePage);
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            Swal.fire('İptal Edildi', 'Öğe silinmedi', 'error');
        }
    });
}
function createLinksFromTags(tagsString) {
    // Etiketleri ayır
    const tags = tagsString.split(', ');

    // Her etiket için HTML linklerini oluştur
    return tags.map(tag => {
        // Etiket adı ve rengini ayrıştır
        const match = tag.match(/(.*) \[([^\]]+)\]/);
        if (match) {
            const tagName = match[1];
            const tagColor = match[2];

            // Link HTML'ini oluştur
            return `<a href="#!" class="collection-item tooltipped" data-position="top" data-delay="50" data-tooltip="Etiketi Düzenle"><span class="new badge ${tagColor}" data-badge-caption="${tagName}"></span></a>`;
        }
    }).join('');
}

function GetCategories(id, selectedCategoryGuid = null) {
    $.ajax({
        url: '/Categories/GetCategories',
        type: 'GET',
        success: function (response) {
            var categories = response.data;
            if (!Array.isArray(categories)) {
                console.error('Kategoriler alınamadı. Veri beklenen formatta değil.');
                return;
            }
            categories.forEach(function (category) {
                var isSelected = category.Guid === selectedCategoryGuid;
                $(`${id}`).append('<option value="' + category.Guid + '" ' + (isSelected ? 'selected' : '') + '>' + category.CategoryName + '</option>');
            });
            $('select').formSelect();
        },
        error: function (xhr, status, error) {
            console.error('Kategoriler alınamadı. Hata: ' + error);
        }
    });
}

function GetBrands(id, selectedBrandGuid = null) {
    $.ajax({
        url: '/Brands/GetBrands',
        type: 'GET',
        success: function (response) {
            var brands = response.data;
            console.log(brands, "br");
            if (!Array.isArray(brands)) {
                console.error('Markalar alınamadı. Veri beklenen formatta değil.');
                return;
            }
            brands.forEach(function (brand) {
                var isSelected = brand.Guid === selectedBrandGuid;
                $(`${id}`).append('<option value="' + brand.Guid + '" ' + (isSelected ? 'selected' : '') + ' data-icon="/Files/Brands/' + brand.ImgName + '">' + brand.BrandName + '</option>');
            });
            $('select').formSelect();
        },
        error: function (xhr, status, error) {
            console.error('Ürünler alınamadı. Hata: ' + error);
        }
    });
}
