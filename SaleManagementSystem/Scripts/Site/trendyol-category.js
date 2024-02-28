let activePage = 1;
let currentPage = 1; // Şu anki sayfa
const pageSize = 10; // Her sayfada gösterilecek marka sayısı

document.addEventListener('DOMContentLoaded', (event) => {
    getBrandData();
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
            document.getElementById('jsonData').innerText = data;
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}