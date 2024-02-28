let activeStockPage = 1, activeCompanyPage = 1;

document.addEventListener('DOMContentLoaded', (event) => {
    paginateStock(activeStockPage);
    paginateCompany(activeCompanyPage);
    searchStock();
    searchCompany();
    setupPaginationClickHandler();
});

function setupPaginationClickHandler() {
    $('#paginationStock').on('click', 'li', function () {
        var selectedPage = $(this).text();
        var currentPage = parseInt(selectedPage);
        $('#paginationStock li').removeClass('active');
        $(this).addClass('active');
        paginateStock(currentPage);
        activeStockPage = currentPage;
    });

    $('#paginationCompany').on('click', 'li', function () {
        var selectedCompanyPage = $(this).text();
        var currentCompanyPage = parseInt(selectedCompanyPage);
        $('#paginationCompany li').removeClass('active');
        $(this).addClass('active');
        paginateStock(currentCompanyPage);
        activeCompanyPage = currentCompanyPage;
    });
}

function paginateStock(currentPage) {
    if (currentPage === undefined) {
        currentPage = 1;
    }
    activeStockPage = currentPage;

    var dataCount = 5;

    $.ajax({
        url: '/Stocks/Paginate',
        type: 'GET',
        data: { page: currentPage, dataCount: dataCount },
        success: function (response) {
            if (response && response.data && response.pageCount) {
                var tableBody = $('#listContainerStock');
                tableBody.empty();
                updateStockPagination(response.pageCount, currentPage);
                document.getElementById('totalStockDataCount').innerText = `Toplam ${response.totalCount} Kayıt`;
                const startRecord = ((response.page - 1) * response.perPage) + 1;
                let endRecord = response.page * response.perPage;
                endRecord = endRecord > response.totalCount ? response.totalCount : endRecord;
                document.getElementById('listCountStock').innerText = `${response.totalCount} kayıt içinden ${startRecord} - ${endRecord} arası listeleniyor`;
                response.data.forEach(function (item) {
                    var row = `                            <tr>
<td><div class="image-container"><img id="${item.Guid + 1}" src="/Files/Products/${item.ImgName}" class="img" alt="${item.ImgName}" /></div></td>
                        <td>${item.BrandName} ${item.ProductName}</td>
                                <td>
                                    <label>
                    <input type="checkbox" id="${item.Guid}" onChange="updateSelectedStocks('${item.Guid}', '${item.BrandName}', '${item.ProductName}', '${item.ImgName}','${item.UnitName}','${item.SellPrice}','${item.Vat}','${item.Quantity}')" />
    <span></span>
</label>
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

function updateStockPagination(pageCount, currentPage) {
    var paginationUl = $('#paginationStock');
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

function searchStock() {
    var debouncedSearch = debounce(function () {
        var filter = $('#searchStock').val();

            $.ajax({
                url: '/Stocks/Filter',
                type: 'GET',
                data: { filter: filter },
                success: function (response) {
                    if (filter.length === 0) {
                        paginateStock(activeStockPage);
                    } else {
                        var tableBody = $('#listContainerStock');
                        tableBody.empty();
                        response.data.forEach(function (item) {
                            var row = `                            <tr>
<td><div class="image-container"><img id="${item.Guid + 1}" src="/Files/Products/${item.ImgName}" class="img" alt="${item.ImgName}" /></div></td>
                        <td>${item.BrandName} ${item.ProductName}</td>
                                <td>
                                    <label>
                    <input type="checkbox" id="${item.Guid}" onChange="updateSelectedStocks('${item.Guid}', '${item.BrandName}', '${item.ProductName}', '${item.ImgName}','${item.UnitName}','${item.SellPrice}','${item.Vat}','${item.Quantity}')" />
    <span></span>
</label>
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

    $('#searchStock').on('input', debouncedSearch);
}

function paginateCompany(currentPage) {
    if (currentPage === undefined) {
        currentPage = 1;
    }
    activeCompanyPage = currentPage;

    var dataCount = 5;

    $.ajax({
        url: '/Companies/Paginate',
        type: 'GET',
        data: { page: currentPage, dataCount: dataCount },
        success: function (response) {
            if (response && response.data && response.pageCount) {
                var tableBody = $('#listContainer');
                tableBody.empty();
                updateCompanyPagination(response.pageCount, currentPage);
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
                                <td>${item.TaxNumber}</td>
                                <td>${item.Address}</td>
                                <td class="icn-td">
                                    <a href="#add-tag" class="waves-effect waves-light blue btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Firmayı Düzenle" onclick="setValues('${item.Guid}')"><i class="icn ti-plus" aria-hidden="true"></i></a>
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

function updateCompanyPagination(pageCount, currentPage) {
    var paginationUl = $('#paginationCompany');
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

function searchCompany() {
    var debouncedSearch = debounce(function () {
        var filter = $('#search').val();

        $.ajax({
            url: '/Companies/Filter',
            type: 'GET',
            data: { filter: filter },
            success: function (response) {
                if (filter.length === 0) {
                    paginateCompany(activeCompanyPage);
                } else {
                    var tableBody = $('#listContainer');
                    tableBody.empty();
                    response.data.forEach(function (item) {
                        var row = `                            <tr>
                                <td>${item.CompanyCode}</td>
                                <td>${item.CompanyName}</td>
                                <td>${item.TaxNumber}</td>
                                <td>${item.Address}</td>
                                <td class="icn-td">
                                    <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Firmayı Düzenle" onclick="setValues('${item.Guid}')"><i class="icn ti-plus" aria-hidden="true"></i></a>
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

