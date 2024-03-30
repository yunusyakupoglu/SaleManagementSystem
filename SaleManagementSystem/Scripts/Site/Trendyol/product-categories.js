document.addEventListener('DOMContentLoaded', (event) => {
    getProductCategories();
    $(document).ready(function () {
        // Popover butonuna tıklama işlevselliği
        $('#listContainer').on('click', '.popoverButton', function (event) {
            event.stopPropagation(); // Bu olayın üst elementlere yayılmasını önle
            var id = this.id.replace('popoverButton', '');
            var popover = $(`#popoverContent${id}`);

            // Eğer şu anda gösteriliyorsa gizle, değilse göster
            if (popover.css('display') === 'block') {
                popover.css('display', 'none');
            } else {
                // Diğer tüm popover'ları gizle
                $('.popover').css('display', 'none');
                // İlgili popover'ı göster
                popover.css({
                    'display': 'block',
                    'position': 'absolute',
                    'right': 'auto'
                });
            }
        });

        // Dökümanın herhangi bir yerine tıklandığında tüm popover'ları gizle
        $(document).click(function () {
            $('.popover').css('display', 'none');
        });

        // Popover içeriğine tıklama. Bu, popover'ın içine tıklanınca kapanmasını önler.
        $(document).on('click', '.popover', function (event) {
            event.stopPropagation();
        });



        $('#listContainer').on('click', '.popoverButtonScChild', function (event) {
            event.stopPropagation(); // Bu olayın üst elementlere yayılmasını önle
            var id = this.id.replace('popoverButtonScChild', '');
            var popover = $(`#popoverContentScChild${id}`);

            // Eğer şu anda gösteriliyorsa gizle, değilse göster
            if (popover.css('display') === 'block') {
                popover.css('display', 'none');
            } else {
                // Diğer tüm popover'ları gizle
                $('.popoverScChild').css('display', 'none');
                // İlgili popover'ı göster
                popover.css({
                    'display': 'block',
                    'position': 'absolute',
                    'right': 'auto'
                });
            }
        });

        // Dökümanın herhangi bir yerine tıklandığında tüm popover'ları gizle
        $(document).click(function () {
            $('.popoverScChild').css('display', 'none');
        });

        // Popover içeriğine tıklama. Bu, popover'ın içine tıklanınca kapanmasını önler.
        $(document).on('click', '.popoverScChild', function (event) {
            event.stopPropagation();
        });
    });
});

function getProductCategories() {
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
                                url: 'http://localhost:64238/trendyol/urun-kategorileri',
                                type: 'GET',
                                data: { SvcCredentials: data.Data.SvcCredentials, UserAgent: data.Data.UserAgent },
                                success: function (data) {
                                    var jsonData = JSON.parse(data.message);
                                    var categories = jsonData.categories;

                                    var tableBody = $('#listContainer');
                                    tableBody.empty();
                                    console.log(categories[0].subCategories);
                                    categories.forEach(function (item) {
                                        var firstLayerSubCategoriesHtml = item.subCategories.map(function (firstLayer) {
                                            var secondLayerSubCategoriesHtml = firstLayer.subCategories.map(function (secondLayer) {
                                                var thirdLayerSubCategoriesHtml = secondLayer.subCategories.map(function (thirdLayer) {
                                                    //var scChildSubCategories
                                                    return `<li>id: ${thirdLayer.id} - name: ${thirdLayer.name}</li>`;
                                                }).join('');
                                                return `<li>
                                    <div style="display: inline-block;">
                                        <a href="#add-tag" class="popoverButtonScChild" id="popoverButtonScChild${secondLayer.id}">
                                        id: ${secondLayer.id} - name: ${secondLayer.name}
                                        </a>

                                        <div id="popoverContentScChild${secondLayer.id}" class="popoverScChild" style="display:none;">
                                            <table>
                                                <tr><th>${secondLayer.name} Alt Kategorileri</th></tr>
                                                <tr><td><ul>${thirdLayerSubCategoriesHtml}</ul></td></tr>
                                            </table>
                                        </div>
                                    </div>
                                </li>`;
                                            }).join('');
                                            return `<li>
  <div style="display: inline-block;">
    <a href="#add-tag" class="popoverButton" id="popoverButton${firstLayer.id}">
    id:${firstLayer.id} - name:${firstLayer.name}
    </a>

                        <div id="popoverContent${firstLayer.id}" class="popover" style="display:none;">
                            <table>
<tr><th>${firstLayer.name} Alt Kategorileri</th></tr>
<tr><td><ul>${secondLayerSubCategoriesHtml}</ul></td></tr>
</table>
                        </div>
  </div>
</li>`;
                                        }).join('');
                                        var row = `<tr>
                    <td>${item.id}</td>
                    <td>${item.name}</td>
                    <td><ul>${firstLayerSubCategoriesHtml}</ul></td>
                </tr>`;

                                        tableBody.append(row); // Yeni satırı ekle
                                    });
                                },
                                error: function (xhr, status, error) {
                                    swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
                                }
                            });
                        } else {
                            alert("Entegrator'e giriş yapılamadı.");
                        }
                    }
                });
            } else if (data.StatusCode == 404) {
                console.log(404); // kullanıcı bulunamadı
                alert("kullanıcı bulunamadı");
            } else if (data.StatusCode == 401) {
                console.log(401); // Kullanıcı girişi başarısız
                alert("Kullanıcı girişi başarısız");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
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

function saveProductCategories() {
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
                                url: 'http://localhost:64238/trendyol/urun-kategorileri',
                                type: 'GET',
                                data: { SvcCredentials: data.Data.SvcCredentials, UserAgent: data.Data.UserAgent },
                                success: function (data) {
                                    var jsonData = JSON.parse(data.message);
                                    $.ajax({
                                        url: 'http://localhost:53097/Trendyol/SaveCategory',
                                        type: 'POST',
                                        contentType: 'application/json', // Bu satırı ekleyin.
                                        data: JSON.stringify({ jsonData: data.message }), // Burası önemli, doğru formatta veri gönderilmeli.
                                        success: function (result) {
                                            alert(result.message);
                                        }
                                    });
                                },
                                error: function (xhr, status, error) {
                                    swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
                                }
                            });
                        } else {
                            alert("Entegrator'e giriş yapılamadı.");
                        }
                    }
                });
            } else if (data.StatusCode == 404) {
                console.log(404); // kullanıcı bulunamadı
                alert("kullanıcı bulunamadı");
            } else if (data.StatusCode == 401) {
                console.log(401); // Kullanıcı girişi başarısız
                alert("Kullanıcı girişi başarısız");
            }
        },
        error: function (xhr, status, error) {
            swal("Hata!", "Stok bilgileri yüklenirken bir hata oluştu: " + error, "error");
        }
    });

}
