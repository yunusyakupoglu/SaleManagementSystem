document.addEventListener('DOMContentLoaded', (event) => {
    getSuplierAddresses();
});

function getSuplierAddresses() {
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
                                url: 'http://localhost:64238/trendyol/iade-ve-sevkiyat-adres-bilgileri',
                                type: 'GET',
                                data: { SvcCredentials: data.Data.SvcCredentials, UserAgent: data.Data.UserAgent, MerchantId: data.Data.MerchantId },
                                success: function (data) {
                                    console.log(data, "supplier addresses")
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

function parseJWT(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}