let activePage = 1;
let pageSize = 20; // Her sayfada gösterilecek marka sayısı

document.addEventListener('DOMContentLoaded', (event) => {
    getProductData();
    setupPaginationClickHandler();
});

function setupPaginationClickHandler() {
    $('.pagination').on('click', 'li', function () {
        var selectedPage = $(this).text();
        var currentPage = parseInt(selectedPage);
        $('.pagination li').removeClass('active');
        $(this).addClass('active');
        getProductData(currentPage); // Sayfa değiştiğinde verileri yeniden yükle
        activePage = currentPage;
    });
}

function updatePagination(pageCount, currentPage) {
    var paginationUl = $('.pagination');
    paginationUl.empty();
    var prevDisabled;
    var nextDisabled;
    paginationUl.append(`<li class="waves-effect" ${prevDisabled} id="prev-page"><a href="#!"><i class="material-icons">chevron_left</i></a></li>`);

    for (var i = 1; i <= pageCount; i++) {
        var activeClass = i === currentPage ? 'active' : '';
        paginationUl.append(`<li class="waves-effect ${activeClass}" id="page-${i}"><a href="#!">${i}</a></li>`);
        if (i == 1) {
            prevDisabled = 'disabled';
        } else if (i == pageCount) {
            nextDisabled = 'disabled';
        }
    }

    paginationUl.append(`<li class="waves-effect" ${nextDisabled} id="next-page"><a href="#!"><i class="material-icons">chevron_right</i></a></li>`);
    prevPreviousButtons(pageCount, currentPage);
}

function prevPreviousButtons(pageCount, currentPage) {
    // Bu fonksiyonu güncelle, pageCount ve currentPage'i parametre olarak al
    document.getElementById('prev-page').addEventListener('click', function () {
        if (currentPage > 1) {
            currentPage--;
            updatePagination(pageCount, currentPage);
            getProductData(currentPage); // Sayfayı güncelle
        }
    });

    document.getElementById('next-page').addEventListener('click', function () {
        if (currentPage < pageCount) {
            currentPage++;
            updatePagination(pageCount, currentPage);
            getProductData(currentPage); // Sayfayı güncelle
        }
    });
}

function getPageData(page, pageSize, data) {
    return data.slice((page - 1) * pageSize, page * pageSize);
}


function getProductData(currentPage) {
    if (currentPage === undefined) {
        currentPage = 1;
    }
    activePage = currentPage;
    $.ajax({
        url: 'http://localhost:64238/fake-store/urunler',
        type: 'GET',
        success: function (data) {
            var jsonData = JSON.parse(data); // Veriyi JSON nesnesine dönüştür
            pageSize = document.getElementById('paginator-select').value;
            var pagedData = getPageData(currentPage, pageSize, jsonData);
            document.getElementById('totalDataCount').innerText = `Toplam ${jsonData.length} Kayıt`;
            var pageCount = jsonData.length / pageSize;
            if (pageCount < 1) {
                pageCount = 1;
            }
            var tableBody = $('#listContainer');
            tableBody.empty();
            updatePagination(pageCount, currentPage);
            pagedData.forEach(function (item) {
                var row = `<tr>
                        <td>${item.id}</td>
<td><div class="image-container"><img id="${item.id}" src="${item.image}" class="img" alt="${item.image}" /></div></td>
                        <td>${item.title}</td>
                        <td>${item.description}</td>
                        <td>${item.price}</td>
                        <td>${item.category}</td>
                        <td>Rate: ${item.rating.rate}</td>
                    </tr>`;

                tableBody.append(row); // Yeni satırı ekle
            });
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}

function addProduct() {
    var model = { id:30, title: "Deneme", price: 2.50, description: "Deneme içeriği", category: "deneme kategorisi", image: "image"};

    $.ajax({
        url: 'http://localhost:64238/fake-store/urunler',
        type: 'POST',
        data: model,
        success: function (response) {
            if (response) {
                swal({
                    title: "Başarılı!",
                    text: response,
                    type: "success", // 'icon' yerine 'type' kullanılıyor
                    timer: 3000,
                    onClose: () => { // 'willClose' yerine 'onClose' kullanılıyor
                        getProductData();
                    }
                });
            } else {
                swal("Hata!", "Bir hata oluştu: " + response, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Kategori güncellenirken bir hata oluştu: " + error, "error");
        }
    });
}