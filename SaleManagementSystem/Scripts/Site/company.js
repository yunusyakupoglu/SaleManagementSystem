﻿let activePage = 1;
document.addEventListener('DOMContentLoaded', (event) => {
    paginate(activePage);
    search();
    setupPaginationClickHandler();
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
        url: '/Companies/Paginate',
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
                                <td>${item.CompanyCode}</td>
                                <td>${item.CompanyName}</td>
                                <td>${item.TaxOffice}</td>
                                <td>${item.TaxNumber}</td>
                                <td>${item.Email}</td>
                                <td>${item.Phone}</td>
                                <td>${item.Address}</td>
                                <td>
                                    <div class="switch">
                                        <label>
                                            <input id="checkbox" type="checkbox" data-guid="${item.Guid}" ${checkedAttribute} onchange="handleCheckboxChange(this)">
                                            <span class="lever"></span>
                                        </label>
                                    </div>
                                </td>
                                <td class="icn-td">
                                    <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Firmayı Düzenle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-pencil-alt" aria-hidden="true"></i></a>
                                    <button type="button" onclick="remove('${item.Guid}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Firmayı Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
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
        if (filter.length === 0) {
            paginate(activePage);
        } else {
        $.ajax({
            url: '/Companies/Filter',
            type: 'GET',
            data: { filter: filter },
            success: function (response) {
                if (response && response.data) {
                    var tableBody = $('#listContainer');
                    tableBody.empty(); // Mevcut içeriği temizle

                    response.data.forEach(function (item) {
                        var row = `<tr>
                            <td>${item.CompanyCode}</td>
                            <td>${item.CompanyName}</td>
                            <td>${item.TaxOffice}</td>
                            <td>${item.TaxNumber}</td>
                            <td>${item.Email}</td>
                            <td>${item.Phone}</td>
                            <td>${item.Address}</td>
                            <td>
                                <div class="switch">
                                    <label>
                                        <input type="checkbox" data-guid="${item.Guid}" ${item.IsActive ? "checked" : ""} onchange="handleCheckboxChange(this)">
                                        <span class="lever"></span>
                                    </label>
                                </div>
                            </td>
                            <td>
<div class="icn-td">
                            <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Düzenle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-pencil-alt" aria-hidden="true"></i></a>
                            <button type="button" onclick="remove('${item.Guid}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                        </div>
                            </td>
                        </tr>`;
                        tableBody.append(row); // Yeni satırı ekle
                    });
                } else {
                    console.error("Liste yenilenirken veri alınamadı.");
                }
            },
            error: function (xhr, status, error) {
                console.error("Arama sırasında hata oluştu: " + error);
            }
        });

        }
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
function handleCheckboxChange(checkboxElement) {
    var checkboxValue = checkboxElement.checked; // true veya false değerini alır
    var itemGuid = checkboxElement.getAttribute('data-guid'); // GUID değerini alır

    $.ajax({
        url: '/Companies/UpdateStatus', // AJAX isteğinin gönderileceği URL
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
    var companyName = document.getElementById('companyName').value;
    var taxNumber = document.getElementById('taxNumber').value;
    var taxOffice = document.getElementById('taxOffice').value;
    var email = document.getElementById('email').value;
    var phone = document.getElementById('phone').value;
    var address = document.getElementById('address').value;
    //var companyName = document.getElementById('companyName').value;

    $.ajax({
        url: '/Companies/Insert',
        type: 'POST',
        data: { companyName: companyName, taxNumber: taxNumber, taxOffice: taxOffice, email: email, phone: phone, address: address },
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
                url: '/Companies/Delete',
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
