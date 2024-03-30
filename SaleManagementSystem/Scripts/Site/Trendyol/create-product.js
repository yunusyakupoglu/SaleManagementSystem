
let categoryArray = [];
let brandArray = [];
let cargoArray = [];
let imgArray = [];
let selectedAttributes = [];

document.addEventListener('DOMContentLoaded', function () {
    $(document).ready(function () {
        $('.collapsible').collapsible();
    });
    getCargoCompanies();
    initializeCreateProductCategories();
    initializeCreateProductBrands();
    initializeCreateProductCargoCompanies();
    fileUploader();

    // Assuming your checkboxes and custom input fields are appended to the DOM as you showed
    // Bind event listeners to these elements for changes
    $(document).on('change', 'input[type="checkbox"]', function () {
        let attributeId = parseInt($(this).attr('attr-data-id'));
        let attributeValueId = parseInt($(this).attr('attrVal-data-id'));

        updateSelectedAttributes(attributeId, attributeValueId);
    });

    $(document).on('input', '.input-field input[data-custom-attribute="true"]', function () {
        let attributeId = parseInt($(this).attr('attr-data-id'));
        let customAttributeValue = $(this).val().trim();
        updateSelectedAttributes(attributeId, null, customAttributeValue);
    });
});

function fileUploader() {
    const fileInput = document.getElementById('file-input');
    const previewContainer = document.querySelector('.preview-img-container');
    let selectedFiles = []; // Seçilen dosyaları takip etmek için dizi oluştur

    fileInput.addEventListener('change', function () {
        const files = Array.from(this.files).slice(0, 8); // İlk 8 dosyayı sınırla
        previewContainer.innerHTML = ''; // Mevcut önizlemeleri temizle
        selectedFiles = []; // Seçilen dosyalar dizisini temizle
        files.forEach(file => {
            if (!file.type.startsWith('image/')) { return; }

            const reader = new FileReader();
            reader.onload = function (e) {
                const imgContainer = document.createElement('div');
                imgContainer.classList.add('preview-img');

                const image = new Image();
                image.src = e.target.result;
                imgContainer.appendChild(image);

                const removeBtn = document.createElement('span');
                removeBtn.classList.add('remove-img');
                removeBtn.innerHTML = 'X';
                removeBtn.onclick = function () {
                    imgContainer.remove();
                    // SeçilenDosyalardan karşılık gelen dosyayı kaldır
                    const index = selectedFiles.indexOf(file);
                    if (index !== -1) {
                        selectedFiles.splice(index, 1);
                        fileInput.files = new FileListFromArray(selectedFiles);
                    }
                };
                imgContainer.appendChild(removeBtn);

                previewContainer.appendChild(imgContainer);
                selectedFiles.push(file); // Dosyayı seçilen dosyalar dizisine ekle
            };
            reader.readAsDataURL(file);
        });
        imgArray = selectedFiles;
    });
}

// Diziyi FileList'e dönüştürmek için yardımcı işlev
function FileListFromArray(array) {
    const dataTransfer = new DataTransfer();
    array.forEach(file => dataTransfer.items.add(file));
    return dataTransfer.files;
}


function initializeCreateProductCategories() {
    const dropdownContent = document.getElementById('dropdownContent');
    const searchInput = document.getElementById('searchInput');

    let isDropdownOpen = false;
    let currentCategories = [];
    let breadcrumb = [];

    async function getCategories(parentId = null) {
        const response = await fetch('/Trendyol/GetCategories');
        const data = await response.json();
        const parsedData = JSON.parse(data);
        return parentId === null ? parsedData.categories : parsedData.categories.filter(c => c.parentId === parentId);
    }

    function renderCategories(categories) {
        dropdownContent.innerHTML = '';
        if (breadcrumb.length > 0) {
            const backButton = document.createElement('button');
            backButton.textContent = 'Back';
            backButton.className = 'backButton';
            backButton.onclick = (event) => {
                event.stopPropagation(); // Olayın üst seviyelere yayılmasını engelle
                breadcrumb.pop();
                renderCategories(breadcrumb.length ? breadcrumb[breadcrumb.length - 1].subCategories : currentCategories);
            };
            dropdownContent.appendChild(backButton);
        }

        categories.forEach(category => {
            const categoryElement = document.createElement('div');
            categoryElement.textContent = category.name;
            categoryElement.onclick = (event) => {
                event.stopPropagation(); // Olayın üst seviyelere yayılmasını engelle
                if (category.subCategories && category.subCategories.length > 0) {
                    breadcrumb.push(category);
                    renderCategories(category.subCategories);
                } else {
                    isDropdownOpen = false;
                    dropdownContent.style.display = 'none';
                    searchInput.value = category.name;
                    searchInput.setAttribute('data-category-id', category.id);
                    console.log('Seçilen kategori ID:', searchInput.getAttribute('data-category-id'));
                    var selectedCategoryId = searchInput.getAttribute('data-category-id');
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
                                                data: { categoryId: selectedCategoryId, SvcCredentials: data.Data.SvcCredentials, UserAgent: data.Data.UserAgent },
                                                success: function (resp) {
                                                    var arr = JSON.parse(resp.message);
                                                    var categoryAttributes = arr.categoryAttributes;
                                                    console.log(arr.categoryAttributes, "arr");
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
                                    <input id="${item.attribute.name}" data-custom-attribute="true" attr-data-id="${item.attribute.id}" type="text">
                                    <label for="${item.attribute.name}">${item.attribute.name}</label>
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
                                                    })
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
                }
            };
            dropdownContent.appendChild(categoryElement);
        });
    }

    function flattenCategories(categories, result = []) {
        categories.forEach(category => {
            result.push(category); // Add the current category
            if (category.subCategories && category.subCategories.length) {
                flattenCategories(category.subCategories, result); // Recurse into subcategories
            }
        });
        return result;
    }


    //searchInput.onkeyup = (e) => {
    //    const searchTerm = e.target.value.toLowerCase();
    //    const filteredCategories = currentCategories.filter(c => c.name.toLowerCase().includes(searchTerm));
    //    renderCategories(filteredCategories);
    //};

    searchInput.onkeyup = (e) => {
        const searchTerm = e.target.value.toLowerCase();
        const flatCategories = flattenCategories(currentCategories); // Flatten all categories
        const filteredCategories = flatCategories.filter(c => c.name.toLowerCase().includes(searchTerm));
        renderCategories(filteredCategories);
    };

    function toggleDropdown() {
        isDropdownOpen = !isDropdownOpen;
        dropdownContent.style.display = isDropdownOpen ? 'block' : 'none';
    }

    async function initDropdown() {
        currentCategories = await getCategories();
        renderCategories(currentCategories);
    }

    searchInput.onfocus = () => {
        if (!isDropdownOpen) toggleDropdown();
    };

    document.addEventListener('click', function (event) {
        if (!dropdownContent.contains(event.target) && !searchInput.contains(event.target) && isDropdownOpen) {
            toggleDropdown();
        }
    });

    initDropdown();
    console.log(brandArray, "arrayList");
}

function initializeCreateProductBrands() {
    const brandDropdownContent = document.getElementById('brandDropdownContent');
    const searchBrandInput = document.getElementById('searchBrandInput');
    let isBrandDropdownOpen = false;
    let brandArray = []; 
    function renderBrands(brands) {
        brandDropdownContent.innerHTML = '';
        brands.forEach(brand => {
            const brandElement = document.createElement('div');
            brandElement.textContent = brand.name;
            brandElement.onclick = (event) => {
                event.stopPropagation();
                isBrandDropdownOpen = false;
                brandDropdownContent.style.display = 'none';
                searchBrandInput.value = brand.name;
                searchBrandInput.setAttribute('data-brand-id', brand.id);
                console.log('Seçilen brand ID:', searchBrandInput.getAttribute('data-brand-id'));
            };
            brandDropdownContent.appendChild(brandElement);
        });
    }

    function toggleBrandDropdown() {
        isBrandDropdownOpen = !isBrandDropdownOpen;
        brandDropdownContent.style.display = isBrandDropdownOpen ? 'block' : 'none';
    }


    function initBrandDropdown() {
        // Initially render all brands or perhaps a subset
        getBrands('').then(renderBrands); // Assuming getBrands is now promise-based
    }

    // Debounce function to optimize performance
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    const debouncedGetBrands = debounce((searchTerm) => {
        getBrands(searchTerm).then(renderBrands); // Adjust getBrands to return a promise
    }, 250); // Adjust debounce time as needed

    searchBrandInput.onkeyup = (e) => {
        const searchTerm = e.target.value;
        debouncedGetBrands(searchTerm);
    };

    searchBrandInput.onfocus = () => {
        if (!isBrandDropdownOpen) toggleBrandDropdown();
    };

    document.addEventListener('click', function (event) {
        if (!brandDropdownContent.contains(event.target) && !searchBrandInput.contains(event.target) && isBrandDropdownOpen) {
            toggleBrandDropdown();
        }
    });

    initBrandDropdown();
}

function initializeCreateProductCargoCompanies() {
    const cargoDropdownContent = document.getElementById('cargoDropdownContent');
    const searchCargoInput = document.getElementById('searchCargoInput');
    let isCargoDropdownOpen = false;
    //let cargoArray = [];
    function renderCargos(cargos) {
        cargoDropdownContent.innerHTML = '';
        cargos.data.forEach(cargo => {
            const cargoElement = document.createElement('div');
            cargoElement.textContent = cargo.Name;
            cargoElement.onclick = (event) => {
                event.stopPropagation();
                isCargoDropdownOpen = false;
                cargoDropdownContent.style.display = 'none';
                searchCargoInput.value = cargo.Name;
                searchCargoInput.setAttribute('data-cargo-id', cargo.CargoId);
                console.log('Seçilen cargo ID:', searchCargoInput.getAttribute('data-cargo-id'));
            };
            cargoDropdownContent.appendChild(cargoElement);
        });
    }

    function toggleCargoDropdown() {
        isCargoDropdownOpen = !isCargoDropdownOpen;
        cargoDropdownContent.style.display = isCargoDropdownOpen ? 'block' : 'none';
    }

    async function getCargos(cargoName) {
        try {
            const filterResult = await fetch(`/Cargo/Filter?filter=${cargoName}`);
            if (!filterResult.ok) {
                throw new Error('Failed to fetch cargo data');
            }
            return filterResult.json();
        } catch (error) {
            console.error('Error fetching cargo data:', error);
            return []; // Return empty array or handle error gracefully
        }
    }


    function initCargoDropdown() {
        // Initially render all brands or perhaps a subset
        getCargos('').then(renderCargos); // Assuming getBrands is now promise-based
    }

    // Debounce function to optimize performance
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    const debouncedGetCargos = debounce((searchTerm) => {
        getCargos(searchTerm).then(renderCargos); // Adjust getBrands to return a promise
    }, 250); // Adjust debounce time as needed

    searchCargoInput.onkeyup = (e) => {
        const searchTerm = e.target.value;
        debouncedGetCargos(searchTerm);
    };

    searchCargoInput.onfocus = () => {
        if (!isCargoDropdownOpen) toggleCargoDropdown();
    };

    document.addEventListener('click', function (event) {
        if (!cargoDropdownContent.contains(event.target) && !searchCargoInput.contains(event.target) && isCargoDropdownOpen) {
            toggleCargoDropdown();
        }
    });

    initCargoDropdown();
}

function getBrands(brandname) {
    return new Promise((resolve, reject) => { // Make getBrands promise-based
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
                                    url: 'http://localhost:64238/trendyol/marka-adina-gore-getir',
                                    type: 'GET',
                                    data: { brandName: brandname, SvcCredentials: data.Data.SvcCredentials, UserAgent: data.Data.UserAgent, MerchantId: data.Data.MerchantId  },
                                    contentType: 'application/json',
                                    success: function (resp) {
                                        var arr = JSON.parse(resp.message);
                                        resolve(arr); // Resolve with the parsed brand array
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
    });
}

async function getCategories() {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/Trendyol/GetCategories',
            type: 'GET',
            contentType: 'application/json',
            success: function (resp) {
                resolve(resp);
            },
            error: function (xhr, status, error) {
                reject(error);
            }
        });
    });
}


async function fetchCategories() {
    try {
        const mainCategoryArray = await getCategories();
        categoryArray = JSON.parse(mainCategoryArray);
    } catch (error) {
        console.error("Kategorileri alırken bir hata oluştu:", error);
    }
}

async function createProducts() {
    console.log(imgArray, "imgArray");
    const searchBrandInput = document.getElementById('searchBrandInput');
    const searchInput = document.getElementById('searchInput');
    const productMainId = document.getElementById('productMainId');
    const currencyType = document.getElementById('currencyType');
    const cargoCompanyId = searchCargoInput.getAttribute('data-cargo-id');
    console.log(cargoCompanyId, "yeni seçtim ulan!");
    const shipmentAddressId = document.getElementById('shipmentAddressId');
    const returningAddressId = document.getElementById('returningAddressId');
    const barcode = document.getElementById('barcode');
    const title = document.getElementById('title');
    const quantity = document.getElementById('quantity');
    const stockCode = document.getElementById('stockCode');
    const dimensionalWeight = document.getElementById('dimensionalWeight');
    const listPrice = document.getElementById('listPrice');
    const salePrice = document.getElementById('salePrice');
    //const deliveryDuration = document.getElementById('deliveryDuration');
    const deliveryOption = document.getElementById('deliveryOption');
    const vatRate = document.getElementById('vatRate');
    const description = document.getElementById('description');



    // Bu kısmı attribute ve image verilerini doldurmak için kullanabilirsin. Örnek olarak boş bıraktım.
    var formData = new FormData();

    // imgArray içindeki her dosyayı tek tek ekleyin.
    $.each(imgArray, function (index, file) {
        formData.append('files[' + index + ']', file); // 'files' adını kullanın
    });

    const uploadedImagesData = await uploadedImages(formData);
    console.log("Uploaded images:", uploadedImagesData);

    const postRequest = {
        items: [
            {
                barcode: barcode.value,
                title: title.value,
                productMainId: productMainId.value,
                brandId: parseInt(searchBrandInput.getAttribute('data-brand-id')),
                categoryId: parseInt(searchInput.getAttribute('data-category-id')),
                quantity: parseInt(quantity.value),
                stockCode: stockCode.value,
                dimensionalWeight: parseFloat(dimensionalWeight.value),
                description: description.value,
                currencyType: currencyType.value,
                listPrice: parseFloat(listPrice.value),
                salePrice: parseFloat(salePrice.value),
                cargoCompanyId: cargoCompanyId.value,
                vatRate: parseInt(vatRate.value),
                deliveryOption: {
                    //deliveryDuration: deliveryDuration.value ? parseInt(deliveryDuration.value) : null,
                    deliveryDuration: 1,
                    fastDeliveryType: deliveryOption.value
                },
                images: uploadedImagesData,
                attributes: selectedAttributes
            }
        ]
    };

    console.log(postRequest.items[0].images, "postRequest.items[0].images");

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
                                url: 'http://localhost:64238/trendyol/urun-ekle',
                                type: 'POST',
                                data: JSON.stringify(postRequest), // JSON olarak gönderilecek data,
                                contentType: 'application/json',
                                headers: {
                                    'SvcCredentials': data.Data.SvcCredentials,
                                    'UserAgent': data.Data.UserAgent,
                                    'MerchantId': data.Data.MerchantId
                                },
                                success: function (result) {
                                    alert(result.message);
                                    $.ajax({
                                        url: 'http://localhost:64238/trendyol/get-batch-request-result',
                                        type: 'GET',
                                        headers: {
                                            'batchRequestId': result.message,
                                            'SvcCredentials': data.Data.SvcCredentials,
                                            'UserAgent': data.Data.UserAgent,
                                            'MerchantId': data.Data.MerchantId
                                        },
                                        success: function (batchResult) {
                                            //alert(batchResult);
                                        }
                                    });
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

async function uploadedImages(formData) {
    try {
        // Use the Fetch API to upload the form data
        const response = await fetch('/File/TrendyolFileUpload', {
            method: 'POST',
            body: formData,
            // Note: Fetch API does not require contentType and processData to be set; it handles FormData correctly
        });

        if (!response.ok) {
            // If the server response is not OK, throw an error
            throw new Error(`Server responded with a status of ${response.status}`);
        }

        const result = await response.json(); // Parse the JSON result
        console.log(result.data, "result'tran gelen data");
        console.log(result.data, "images array");

        return result.data; // Return the images data for further processing

    } catch (error) {
        console.error("Error uploading images:", error);
        swal("Hata!", "Resimler sisteme yüklenirken bir hata oluştu: " + error.message, "error");
        return []; // Return an empty array or any suitable default in case of error
    }
}

function getCargoCompanies() {
    $.ajax({
        url:'/Cargo/GetCargos',
        type: 'GET',
        success: function (result) {
            cargoArray = result.data;
        }
    })
}



function parseJWT(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

// Function to update the selectedAttributes array
function updateSelectedAttributes(attributeId, attributeValueId = null, customAttributeValue = null) {
    // Check if the attribute already exists in the array
    let existingAttributeIndex = selectedAttributes.findIndex(attr => attr.attributeId === attributeId);

    if (existingAttributeIndex !== -1) {
        // Update existing attribute
        if (attributeValueId) {
            selectedAttributes[existingAttributeIndex].attributeValueId = attributeValueId;
        } else if (customAttributeValue !== null) {
            selectedAttributes[existingAttributeIndex].customAttributeValue = customAttributeValue;
        }
    } else {
        // Add new attribute
        let newAttribute = { attributeId };
        if (attributeValueId) {
            newAttribute.attributeValueId = attributeValueId;
        } else if (customAttributeValue !== null) {
            newAttribute.customAttributeValue = customAttributeValue;
        }
        selectedAttributes.push(newAttribute);
    }

    console.log(selectedAttributes, "selectedAttributes");
}

function generateRandomBarcode() {
    // Barkodun uzunluğunu belirle
    const barcodeLength = 12;
    let barcode = '';

    // Barkod için rastgele sayılar üret ve string'e ekle
    for (let i = 0; i < barcodeLength; i++) {
        const randomNum = Math.floor(Math.random() * 10); // 0 ile 9 arasında rastgele bir sayı üret
        barcode += randomNum.toString();
    }

    // Barkodu bir input elementine yazdır
    document.getElementById('barcode').value = barcode;
}
