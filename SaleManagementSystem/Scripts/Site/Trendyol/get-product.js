let activePage = 1;
let currentPage = 1; // Şu anki sayfa
const pageSize = 10; // Her sayfada gösterilecek marka sayısı
let productArray = [];
let selectedAttributes = [];

document.addEventListener('DOMContentLoaded', (event) => {
    getProductData();
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
    let filter = document.getElementById("filter-select").value;
    let size = document.getElementById("paginator-select").value;
    $.ajax({
        url: 'http://localhost:57183/user/login',
        type: 'POST',
        data: { Email: 'yunusyakupoglu@outlook.com', Password: '6248624aA*' },
        success: function (data) {
            if (data.StatusCode == 200) {
                var decodedJWT = parseJWT(data.Data);
                $.ajax({
                    url: 'http://localhost:57183/integrator/get-by-user-guid-and-integrator-name',
                    type: 'GET',
                    data: { userGuid: decodedJWT.guid, integratorName: 'Trendyol' },
                    success: function (data) {
                        if (data.StatusCode == 200) {
                            $.ajax({
                                url: 'http://localhost:64238/trendyol/urunler',
                                type: 'GET',
                                data: { parameter: filter, size: size, SvcCredentials: data.Data.SvcCredentials, UserAgent: data.Data.UserAgent, MerchantId: data.Data.MerchantId },
                                success: function (data) {
                                    var tableBody = $('#listContainer');
                                    tableBody.empty();
                                    document.getElementById('totalDataCount').innerText = `Toplam ${data.totalElements} Kayıt`;
                                    productArray = data;
                                    console.log(productArray, "productArrayproductArrayproductArray");
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
    `).join('')}</table>`;
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
                            <a href="#add-tag" class="waves-effect waves-light btn btn-extra-small modal-trigger tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Düzenle" onclick="getProduct('${product.Id}')"><i class="icn ti-pencil-alt" aria-hidden="true"></i></a>
                            <button type="button" onclick="deleteProductData('${product.Barcode}')" class="btn btn-extra-small tooltipped" data-position="top" data-delay="50" data-tooltip="Markayı Sil"><i class="icn ti-close" aria-hidden="true"></i></button>
                        </div></td>
                    </tr>`;

                                        tableBody.append(row);


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
                                                popoverattr.show();
                                                var buttonOffset = $(this).offset();
                                                var buttonHeight = $(this).outerHeight();
                                                var popoverWidth = popoverattr.outerWidth();
                                                var buttonWidth = $(this).outerWidth();
                                                var topPosition = buttonOffset.top + buttonHeight;
                                                var leftPosition = buttonOffset.left + buttonWidth - popoverWidth;
                                                if (leftPosition < 0) {
                                                    leftPosition = buttonOffset.left;
                                                }
                                                popoverattr.css({
                                                    'top': topPosition,
                                                    'left': leftPosition
                                                });
                                            } else {
                                                popoverattr.hide();
                                            }
                                        });
                                    });
                                },
                                error: function (xhr, status, error) {
                                    swal("Hata!", "Ürün bilgileri yüklenirken bir hata oluştu: " + error, "error");
                                }
                            });

                        } else {
                            alert("Entegrator'e giriş yapılamadı.");
                        }
                    }
                });
            } else if (data.StatusCode == 404) {
                alert("kullanıcı bulunamadı");
            } else if (data.StatusCode == 401) {
                alert("Kullanıcı girişi başarısız");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}


function deleteProductData(barcode) {
    $.ajax({
        url: 'http://localhost:57183/user/login',
        type: 'POST',
        data: { Email: 'yunusyakupoglu@outlook.com', Password: '6248624aA*' },
        success: function (data) {
            if (data.StatusCode == 200) {
                var decodedJWT = parseJWT(data.Data);
                $.ajax({
                    url: 'http://localhost:57183/integrator/get-by-user-guid-and-integrator-name',
                    type: 'GET',
                    data: { userGuid: decodedJWT.guid, integratorName: 'Trendyol' },
                    success: function (data) {
                        if (data.StatusCode == 200) {
                            $.ajax({
                                url: 'http://localhost:64238/trendyol/urun-sil',
                                type: 'DELETE',
                                headers: {
                                    'Barcode': barcode,
                                    'SvcCredentials': data.Data.SvcCredentials,
                                    'UserAgent': data.Data.UserAgent,
                                    'MerchantId': data.Data.MerchantId
                                },
                                success: function (response) {
                                    var jsonResponse = JSON.parse(response.message);
                                    if (response.statusCode === 200) {
                                        $.ajax({
                                            url: 'http://localhost:64238/trendyol/get-batch-request-result',
                                            type: 'GET',
                                            headers: {
                                                'batchRequestId': jsonResponse.batchRequestId,
                                                'SvcCredentials': data.Data.SvcCredentials,
                                                'UserAgent': data.Data.UserAgent,
                                                'MerchantId': data.Data.MerchantId
                                            },
                                            success: function (data) {
                                                if (data.statusCode === 200) {
                                                    swal("Başarılı!", data.message, "success");
                                                } else if (data.statusCode === 401) {
                                                    swal("Yetkisiz Erişim!", data.message, "error");
                                                } else {
                                                    swal("Bilgi!", data.message, "info");
                                                }
                                            },
                                            error: function (xhr) {
                                                var errorMessage = "Bir hata oluştu.";
                                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                                    errorMessage = xhr.responseJSON.message;
                                                } else if (xhr.status) {
                                                    errorMessage = `Hata Kodu: ${xhr.status}`;
                                                }
                                                swal("Hata!", errorMessage, "error");
                                            }
                                        });

                                    } else {
                                    }
                                },
                                error: function (xhr) {

                                    var errorMessage = "Bir hata oluştu.";
                                    if (xhr.responseJSON && xhr.responseJSON.message) {
                                        errorMessage = xhr.responseJSON.message;
                                    } else if (xhr.status) {
                                        errorMessage = `Hata Kodu: ${xhr.status}`;
                                    }
                                    swal("Hata!", errorMessage, "error");
                                }
                            });
                        } else {
                            alert("Entegrator'e giriş yapılamadı.");
                        }
                    }
                });
            } else if (data.StatusCode == 404) {
                console.log(404);
                alert("kullanıcı bulunamadı");
            } else if (data.StatusCode == 401) {
                console.log(401);
                alert("Kullanıcı girişi başarısız");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });
}

async function getProduct(id) {
    let foundData = productArray.content.find(x => x.Id === id);
    console.log(foundData, "foundData");

    document.getElementById('description').value = foundData.Description;
    document.getElementById('productMainId').value = foundData.ProductMainId;
    //document.getElementById('currencyType').value = foundData.CurrencyType;
    document.getElementById('barcode').value = foundData.Barcode;
    document.getElementById('title').value = foundData.Title;
    document.getElementById('quantity').value = foundData.Quantity;
    document.getElementById('stockCode').value = foundData.StockCode;
    document.getElementById('dimensionalWeight').value = foundData.DimensionalWeight;
    document.getElementById('listPrice').value = foundData.ListPrice;
    document.getElementById('salePrice').value = foundData.SalePrice;
    document.getElementById('vatRate').value = foundData.VatRate;
    document.getElementById('searchBrandInput').value = foundData.Brand;
    document.getElementById('searchBrandInput').setAttribute('data-brand-Id', foundData.BrandId);
    document.getElementById('deliveryOption').value = foundData.DeliveryOptions.FastDeliveryType.toString();
    var sCategory = await getCategory(foundData.CategoryName);
    document.getElementById('searchInput').value = sCategory.name;
    document.getElementById('searchInput').setAttribute('data-category-id', sCategory.id);
    selectedAttributes = foundData.Attributes;
    console.log(selectedAttributes, "selectedAttributes in foundData");
    $.ajax({
        url: 'http://localhost:57183/user/login',
        type: 'POST',
        data: { Email: 'yunusyakupoglu@outlook.com', Password: '6248624aA*' },
        success: function (data) {
            if (data.StatusCode == 200) {
                var decodedJWT = parseJWT(data.Data);
                $.ajax({
                    url: 'http://localhost:57183/integrator/get-by-user-guid-and-integrator-name',
                    type: 'GET',
                    data: { userGuid: decodedJWT.guid, integratorName: 'Trendyol' },
                    success: function (data) {
                        if (data.StatusCode == 200) {

                            $.ajax({
                                url: 'http://localhost:64238/trendyol/kategori-ozellik-listesi',
                                type: 'GET',
                                data: { categoryId: document.getElementById('searchInput').getAttribute('data-category-id'), SvcCredentials: data.Data.SvcCredentials, UserAgent: data.Data.UserAgent },
                                success: function (resp) {
                                    var arr = JSON.parse(resp.message);
                                    var categoryAttributes = arr.categoryAttributes;
                                    var attributeBody = $('#collapsible-category-attributes');
                                    attributeBody.empty();
                                    categoryAttributes.forEach(function (item) {
                                        var attributeClass = item.required ? 'required-attribute' : '';
                                        var attributeBodyClass = item.required ? 'required-attribute-body' : '';
                                        var attributeText = item.required ? `<span style="color: red; font-size:10px;">${item.attribute.name} seçimi zorunludur.*</span>` : ``;
                                        var attributeBodyText = item.required ? `<span style="color: red; font-size:10px;">${item.attribute.name} alanı doldurulmalıdır.*</span>` : ``;
                                        var valuesListId = 'values-' + item.attribute.id;
                                        var attribute = `                    
                    <li>
                        <div class="collapsible-header my-collapse-features" id="${item.attribute.id}"><p>${item.attribute.name} ${attributeText}</p></div>
                        <div class="collapsible-body ${attributeBodyClass}"><span><ul class="my-custom-ul" id="${valuesListId}"></ul></span></div>
                    </li>`;

                                        var customAttribute = `                    
                    <li>
                        <div class="collapsible-header my-collapse-features" id="${item.attribute.id}"><p>${item.attribute.name} ${attributeBodyText}</p></div>
                        <div class="collapsible-body ${attributeBodyClass}">
                            <span>
                                <div class="input-field">
                                    <input id="${item.attribute.name}" data-custom-attribute="true" placeholder="${item.attribute.name}" attr-data-id="${item.attribute.id}" type="text">
                                </div>
                            </span>
                        </div>
                    </li>`;

                                        if (item.attributeValues.length != 0 || item.allowCustom) {
                                            if (item.allowCustom) {
                                                attributeBody.append(customAttribute);
                                            } else {
                                                attributeBody.append(attribute);
                                            }
                                        }
                                        var attrValBody = $('#' + valuesListId);
                                        item.attributeValues.forEach(function (attrVal) {
                                            var val = `<li>
    <p>
        <label>
            <input type="checkbox" class="filled-in" id="${attrVal.id}" attrVal-data-id="${attrVal.id}" attr-data-id="${item.attribute.id}" />
            <span>${attrVal.name}</span>
        </label>
    </p>
</li>`; // Burada attrVal değerine göre değişiklik yapabilirsin
                                            attrValBody.append(val);
                                        });
                                    });
                                    populateAttributes(selectedAttributes);
                                },
                                error: function (xhr, status, error) {
                                    reject(error);
                                }
                            });

                        } else {
                            alert("Entegrator'e giriş yapılamadı.");
                        }
                    }
                });
            } else if (data.StatusCode == 404) {
                console.log(404);
                alert("kullanıcı bulunamadı");
            } else if (data.StatusCode == 401) {
                console.log(401);
                alert("Kullanıcı girişi başarısız");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });

    //
    let event = new Event('change');
    document.getElementById('vatRate').dispatchEvent(event);

    let categoryEvent = new Event('change');
    document.getElementById('searchInput').dispatchEvent(categoryEvent);
}

function populateAttributes(selectedAttributes) {
    selectedAttributes.forEach(attr => {
        console.log(attr, "attrattrattr");
        if (attr.AttributeValueId === 0) { // Assuming custom attributes have an AttributeValueId of 0
            // Set the value of the input field for custom attributes
            $(`input[attr-data-id="${attr.AttributeId}"]`).val(attr.AttributeValue);
        } else {
            // Check the checkbox for predefined attributes
            $(`input[attrVal-data-id="${attr.AttributeValueId}"]`).prop('checked', true);
        }
    });
}

function parseJWT(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

async function getCategory(categoryName) {
    const response = await fetch(`/Trendyol/GetCategory?categoryName=${encodeURIComponent(categoryName)}`);
    const data = await response.json();
    return data;
}
