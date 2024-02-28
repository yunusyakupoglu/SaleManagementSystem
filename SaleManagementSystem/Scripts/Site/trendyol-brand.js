let activePage = 1;
let currentPage = 1; // Şu anki sayfa
let pageSize = 20; // Her sayfada gösterilecek marka sayısı
let pageNumber = 1;

document.addEventListener('DOMContentLoaded', (event) => {
    paginate();
});

function getPageData(page, pageSize, data) {
    return data.slice((page - 1) * pageSize, page * pageSize);
}

function setPageNumber(){
    pageNumber = document.getElementById('pageNumber').value;
    if (pageNumber.length === 0) {
        pageNumber = 1;
    }
}

function paginate() {
    pageSize = document.getElementById('paginator-select').value;
    setPageNumber();
    currentPage = 1; // Sayfa büyüklüğü değiştiğinde tekrar ilk sayfaya dön
    getBrandData(); // Verileri yeniden yüklemek için getBrandData fonksiyonunu çağır
}


function getBrandData() {
    $.ajax({
        url: 'http://localhost:64238/trendyol/markalar?page=' + pageNumber,
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
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}
