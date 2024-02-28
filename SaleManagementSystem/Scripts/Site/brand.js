let activePage = 1;

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
        url: '/Brands/Paginate',
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
                    var row = `<tr>
                                    <td><div class="image-container"><img id="${item.Guid}" src="/Files/Brands/${item.ImgName}" class="img" alt="${item.ImgName}" /></div></td>
                        <td>${item.BrandName}</td>
                        <td><div class="switch">
                            <label>
                                <input type="checkbox" data-guid="${item.Guid}" ${checkedAttribute} onchange="handleCheckboxChange(this)">
                                <span class="lever"></span>
                            </label>
                         </div></td>
                        <td><div class="icn-td">
                            <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Düzenle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-pencil-alt" aria-hidden="true"></i></a>
                            <button type="button" onclick="remove('${item.Guid}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                        </div></td>
                    </tr>`;

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
            url: '/Brands/Filter',
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
                        var row = `<tr>
                                    <td><div class="image-container"><img id="${item.Guid}" src="/Files/Brands/${item.ImgName}" class="img" alt="${item.ImgName}" /></div></td>
                        <td>${item.BrandName}</td>
                        <td><div class="switch">
                            <label>
                                <input type="checkbox" data-guid="${item.Guid}" ${checkedAttribute} onchange="handleCheckboxChange(this)">
                                <span class="lever"></span>
                            </label>
                         </div></td>
                        <td><div class="icn-td">
                            <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Düzenle" onclick="openModalWithGuid('${item.Guid}')"><i class="icn ti-pencil-alt" aria-hidden="true"></i></a>
                            <button type="button" onclick="remove('${item.Guid}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                        </div></td>
                    </tr>`;

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

function openModalWithGuid(guid) {
    // AJAX isteği ile kategori bilgilerini al
    $.ajax({
        url: 'Brands/GetBrand',
        type: 'GET',
        data: { guid: guid },
        success: function (brand) {
            // Form alanlarını doldur
            document.getElementById('editBrandName').value = brand.data.BrandName;
            document.getElementById('editBrandGuid').value = brand.data.Guid;
            document.getElementById('fileInput').value = brand.data.ImgName;
            document.getElementById('editBrandImgUrl').value = brand.data.ImgUrl;
            document.getElementById('editBrandImgName').value = brand.data.ImgName;

            // Label'i aktif hale getir (Materialize için)
            M.updateTextFields();

            // Modalı aç
            $('#edit-brand-modal').modal('open');
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Kategori bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}

function updateBrand() {
    var fileInput = document.getElementById('file');
    var file = fileInput.files[0];
    var brandName = document.getElementById('editBrandName').value;
    var guid = document.getElementById('editBrandGuid').value;
    var imgUrl = document.getElementById('editBrandImgUrl').value;
    var imgName = document.getElementById('editBrandImgName').value;
    var formData = new FormData();
    formData.append('guid', guid);
    formData.append('file', file);
    formData.append('brandName', brandName);
    formData.append('imgUrl', imgUrl);
    formData.append('imgName', imgName);
    // AJAX isteği ile kategoriyi güncelle
    $.ajax({
        url: '/Brands/Update',
        type: 'POST',
        data: formData,
        contentType: false, // AJAX'ın varsayılan içerik tipini kullanmaması için
        processData: false, // FormData'nın işlenmemesi için
        success: function (response) {
            if (response.success) {
                $('#edit-brand-modal').modal('close');
                swal("Başarılı!", "Marka başarıyla güncellendi.", "success");
                paginate(activePage);
            } else {
                swal("Hata!", "Bir hata oluştu: " + response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Kategori güncellenirken bir hata oluştu: " + error, "error");
        }
    });
}

function handleCheckboxChange(checkboxElement) {
    var checkboxValue = checkboxElement.checked; // true veya false değerini alır
    var itemGuid = checkboxElement.getAttribute('data-guid'); // GUID değerini alır

    $.ajax({
        url: '/Brands/UpdateStatus', // AJAX isteğinin gönderileceği URL
        type: 'POST',
        data: { guid: itemGuid, isActive: checkboxValue },
        success: function (response) {
            if (response.success) {
                paginate(active);
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
    var brandName = document.getElementById('brandName').value;
    var formData = new FormData();
    formData.append('file', file);
    formData.append('brandName', brandName);

    $.ajax({
        url: '/Brands/Insert',
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
                url: '/Brands/Delete',
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

function thumb(id, img) {
    document.addEventListener('DOMContentLoaded', function () {
        const specialImage = document.querySelector(`#${id}`);

        // Thumbnail ekleme
        const thumbnail = document.createElement('img');
        thumbnail.src = `${img}`; // Thumbnail resminin yolu
        thumbnail.style.position = 'absolute';
        thumbnail.style.left = '0';
        thumbnail.style.top = '0';
        thumbnail.style.opacity = '0';
        thumbnail.style.transition = 'opacity 0.3s';
        specialImage.parentNode.insertBefore(thumbnail, specialImage);

        // Resim üzerine gelindiğinde büyütme ve thumbnail gösterimi
        specialImage.addEventListener('mouseover', function () {
            specialImage.style.transform = 'scale(1.1)';
            specialImage.style.transition = 'transform 0.3s';
            thumbnail.style.opacity = '1';
        });

        // Mouse çekildiğinde eski haline dönme
        specialImage.addEventListener('mouseout', function () {
            specialImage.style.transform = 'scale(1)';
            thumbnail.style.opacity = '0';
        });
    });
}
