
$(function() {
    //date picker
    $('.datepicker').pickadate().val(moment(new Date()).format('DD MMMM, YYYY'));

    // Material Select Initialization
    $('.mdb-select').materialSelect();
});

//global store
let storage = [];
let tempStorage = null;
let codeStorage = [];

//selectors
//product info form
const formCart = document.getElementById("cart-form")
const selectCategory = formCart.ParentId
const selectProductId = formCart.selectProductId

const inputPurchasePrice = formCart.inputPurchasePrice;
const inputSellingPrice = formCart.inputSellingPrice;
const inputWarranty = formCart.inputWarranty;
const inputDescription = formCart.inputDescription;
const inputNote = formCart.inputNote;
const productError = document.querySelector("#product-error")

//payment selectors
const formPayment = document.getElementById('formPayment');
const totalPrice = formPayment.querySelector('#totalPrice');
const inputDiscount = formPayment.inputDiscount;
const totalPayable = formPayment.querySelector('#totalPayable');
const inputPaid = formPayment.inputPaid;
const totalDue = formPayment.querySelector('#totalDue');
const selectPaymentMethod = formPayment.selectPaymentMethod;
const inputMemoNumber = formPayment.inputMemoNumber;
const inputPurchaseDate = formPayment.inputPurchaseDate;
const vendorError = formPayment.querySelector('#vendor-error');

//form add code
const formAddCode = document.getElementById("formAddCode");
const codeExistError = formAddCode.querySelector("#code-exist-error");

const tbody = document.getElementById("tbody");
const modalInsetCode = $('#modalInsetCode');
const showAddedCode = document.getElementById('showAddedCode');
const btnCheckProduct = document.getElementById('btnCheckProduct');
const btnAddToList = document.getElementById('btnAddToList');
const buzzAudio = document.getElementById('audio');


//****Global product Code Storage****//
const productCode = {
    isExistServer: async function (codeArray) {
        const url = '/Purchase/IsPurchaseCodeExist';
        const options = {
            method: 'post',
            url: url,
            data: codeArray
        }

        return await axios(options).then(response => response.data).catch(error => console.log(error.response));
    },
    isExist: function (newCode) {
        if (!codeStorage.length) return false;

        const codeExist = codeStorage.some(code => code === newCode);
        codeExistError.innerText = codeExist ? `${newCode}: Already Added!` : '';

        if (codeExist) buzzAudio.play();

        return codeExist;
    },
    setStorage: function (code) {
        if (codeStorage.indexOf(code) === -1) {
            codeStorage.push(code);
            localStorage.setItem('code-storage', JSON.stringify(codeStorage));
        }
    },
    getStorage: function () {
        if (localStorage.getItem('code-storage'))
            codeStorage = JSON.parse(localStorage.getItem('code-storage'));
    },
    updateStorage: function (code) {
        const index = codeStorage.indexOf(code);
        if (index > -1) {
            codeStorage.splice(index, 1);
            localStorage.setItem('code-storage', JSON.stringify(codeStorage));
        }
    }
}

//functions
const getStorage = function () {
    if (localStorage.getItem('cart-storage')) {
        storage = JSON.parse(localStorage.getItem('cart-storage'));
    };
}

//product stock temp storage
const getTempStorage = function () {
    if (localStorage.getItem('temp-storage')) {
        tempStorage = JSON.parse(localStorage.getItem('temp-storage'));
    }
}

//dropdown selected index 0
//const clearMDBdropDownList = function (mainSelector) {
//    const content = mainSelector.querySelectorAll('.select-dropdown li');
//    content.forEach(li => {
//        content[0].classList.add('active','selected');
//
//        if (li.classList.contains('selected')) {
//            li.classList.remove(['active', 'selected']);
//            li.click();
//            return;
//        }
//    });
//}

//calculate purchase Total
const purchaseTotalPrice = function () {
    const multi = storage.map(item => item.PurchasePrice * item.ProductStocks.length);
    return multi.reduce((prev, curr) => prev + curr, 0);
}

//append total price to DOM
const appendTotalPrice = function () {
    const totalAmount = purchaseTotalPrice();

    totalPrice.innerText = totalAmount
    totalPayable.innerText = totalAmount;
    totalDue.innerText = totalAmount;

    if (inputDiscount.value)
        inputDiscount.value = '';

    if (inputPaid.value)
        inputPaid.value = '';

    if (selectPaymentMethod.selectedIndex > 0) {
        //clearMDBdropDownList(formPayment);
        selectPaymentMethod.removeAttribute('required');
    }
}

//create table rows
const createTableRow = function (item) {
    const tr = document.createElement("tr");
    tr.setAttribute('data-sn', item.SN);

    //column 1
    const td1 = tr.insertCell(0);
    td1.appendChild(document.createTextNode(item.Category));

    const p = document.createElement('p');
    p.textContent = item.Description;
    td1.appendChild(p);

    //column 2
    const td2 = tr.insertCell(1);
    td2.appendChild(document.createTextNode(item.ProductName));

    const p2 = document.createElement('p');
    p2.textContent = item.Note;
    td2.appendChild(p2);

    //column 3
    const td3 = tr.insertCell(2);
    td3.appendChild(document.createTextNode(item.PurchasePrice));

    //column 4
    const td4 = tr.insertCell(3);
    td4.appendChild(document.createTextNode(item.SellingPrice));

    //column 5
    const td5 = tr.insertCell(4);
    td5.appendChild(document.createTextNode(item.Warranty));

    //column 6
    const td6 = tr.insertCell(5);
    const strong = document.createElement('strong');
    strong.appendChild(document.createTextNode(item.ProductStocks.length));
    strong.classList.add('badge-pill', 'badge-success', 'stock');
    td6.appendChild(strong);

    //column 6
    const td7 = tr.insertCell(6);
    const removeIcon = document.createElement('i');
    removeIcon.classList.add('fal', 'fa-trash-alt', 'remove');
    td7.appendChild(removeIcon);
    td7.classList.add('text-center');

    return tr;
}

//show product on table
const showCartedProduct = function () {
    getStorage();
    appendTotalPrice();

    const fragment = document.createDocumentFragment();

    storage.forEach((item ,SN) => {
        const tr = createTableRow(item, SN+1);
        fragment.appendChild(tr);
    });

    tbody.appendChild(fragment);
 }

//remove product from list
const removeProduct = function (row, SN) {
    //remove product code from code storage
    const removedObj = storage.find(item => item.SN === SN).ProductStocks;
    removedObj.forEach(stock => {
        productCode.updateStorage(stock.ProductCode);
    });

    //remove product from storage
    storage = storage.filter(item => item.SN !== SN);

    //re-serial SN
    storage = storage.map((item, i) => ({ ...item, SN : i+1 }));

    //save to local storage
    localStorage.setItem('cart-storage', JSON.stringify(storage));

    //show product on table
    tbody.innerHTML = '';
    showCartedProduct();
}

//empty text input
const clearInput = function () {
    inputPurchasePrice.value = ''
    inputPurchasePrice.nextElementSibling.classList.remove('active')

    inputSellingPrice.value = ''
    inputSellingPrice.nextElementSibling.classList.remove('active')

    inputWarranty.value = ''
    inputWarranty.nextElementSibling.classList.remove('active')

    inputDescription.value = ''
    inputDescription.nextElementSibling.classList.remove('active')

    inputNote.value = ''
    inputNote.nextElementSibling.classList.remove('active')
}

//category dropdown change
const onCategoryChanged = function() {
    const categoryId = +this.value

    if (!categoryId) return;

    const url = '/Product/GetProductByCategoryDropDown'
    const parameter = { params: { categoryId } }

    axios.get(url, parameter).then(res => {
        const fragment = document.createDocumentFragment();
        const option1 = document.createElement("option");

        option1.value = "";
        option1.text = "Brand and Model";
        option1.setAttribute("disabled", "disabled");
        option1.setAttribute("selected", true);
        fragment.appendChild(option1);

        if (res.data.length) {
            res.data.forEach(item => {
                const option = document.createElement("option");
                option.value = item.ProductId;
                option.text = item.ProductName;
                fragment.appendChild(option);
            })
        }

        selectProductId.innerHTML = '';
        selectProductId.appendChild(fragment);
    })
}

//product dropdown change
const onProductChanged = function() {
    const productId = +this.value
    if (!productId) return;

    const url = '/Product/GetProductInfo'
    const parameter = { params: { productId } }

    //clear input value
    clearInput()

    axios.get(url, parameter).then(res => {
        const { SellingPrice, Warranty, Description, Note } = res.data

        if (SellingPrice) {
            inputSellingPrice.value = SellingPrice
            inputSellingPrice.nextElementSibling.classList.add('active')
        }

        if (Warranty) {
            inputWarranty.value = Warranty
            inputWarranty.nextElementSibling.classList.add('active')
        }

        if (Description) {
            inputDescription.value = Description
            inputDescription.nextElementSibling.classList.add('active')
        }

        if (Note) {
            inputNote.value = Note
            inputNote.nextElementSibling.classList.add('active')
        }
    })
}


//click remove or stock
const onTableRowElementClicked = function (evt) {
    const element = evt.target;
    const removeClicked = element.classList.contains('remove');
    const row = element.parentElement.parentElement;
    const SN = +row.getAttribute('data-sn');

    if (removeClicked)
        removeProduct(row, SN);
}

//create product code span on modal
const createCodeSpan = function (newCode) {
    const content = document.createElement('div');
    content.classList.add('badge-pill', 'badge-success','d-flex',"m-2");

    const iCode = document.createElement('span');
    iCode.classList.add("code-span");
    iCode.textContent = newCode;

    const deleteIcon = document.createElement('i');
    deleteIcon.classList.add('code-delete', "fas", "fa-trash-alt");
    deleteIcon.id = newCode;

    content.appendChild(iCode);
    content.appendChild(deleteIcon);

    return content;
}

//show product code on popup
const showAddedProductCode = function () {
    showAddedCode.innerHTML = '';

    if (codeExistError.innerText)
        codeExistError.innerText = '';

    if (!tempStorage) return;

    const stocks = tempStorage.ProductStocks;
    const fragment = document.createDocumentFragment();

    if (stocks.length > 0) {
        stocks.forEach(stock => fragment.appendChild(createCodeSpan(stock.ProductCode)));    
        showAddedCode.appendChild(fragment);
    }
}

//set temp Storage value
const setProductTempObject = function (element) {
    const SN = tbody.querySelectorAll('tr').length + 1;
    const ParentId = element.ParentId;
    const ProductId = element.selectProductId;
    const PurchasePrice = +element.inputPurchasePrice.value;
    const SellingPrice = +element.inputSellingPrice.value;
    const Warranty = element.inputWarranty.value;
    const Description = element.inputDescription.value;
    const Note = element.inputNote.value;

    if (tempStorage === null) {
        tempStorage = {
            SN,
            ProductCatalogId: +ParentId.value,
            Category: ParentId.options[ParentId.selectedIndex].text,
            ProductId: +ProductId.value,
            ProductName: ProductId.options[ProductId.selectedIndex].text,
            PurchasePrice,
            SellingPrice,
            Warranty,
            Description,
            Note,
            ProductStocks: []
        }
    }
    else {
        tempStorage.ProductCatalogId = +ParentId.value;
        tempStorage.Category = ParentId.options[ParentId.selectedIndex].text;
        tempStorage.ProductId = +ProductId.value;
        tempStorage.ProductName = ProductId.options[ProductId.selectedIndex].text;
        tempStorage.PurchasePrice = PurchasePrice;
        tempStorage.SellingPrice = SellingPrice;
        tempStorage.Warranty = Warranty;
        tempStorage.Description = Description;
        tempStorage.Note = Note;
    }

    localStorage.setItem('temp-storage', JSON.stringify(tempStorage));
}

//match Existing Product code
const matchExistingProductCode = function (stocks) {
    const addedCode = showAddedCode.querySelectorAll('.code-span');
    let isUnsoldExist = false;
    
    addedCode.forEach(added => {
        stocks.forEach(stock => {
            if (added.textContent.trim() === stock.ProductCode) {
                added.parentElement.classList.remove('badge-success');

                if (stock.IsSold) {
                    added.classList.add("code");
                    added.parentElement.classList.add('badge-warning');
                } else {
                    added.classList.add("code");
                    added.parentElement.classList.add('badge-danger');
                    isUnsoldExist = true;
                }
            }
        })
    });

    return isUnsoldExist;
}

//open code modal event listeners
formCart.addEventListener('submit', function (form) {
    form.preventDefault();
    productError.textContent = ''

    //get temp obj
    getTempStorage()

    //save the last value of input
    setProductTempObject(form.target);

    const ProductId = +formCart.selectProductId.value;

    //check product already added
    let isAdded = false
    storage.forEach(product => {
        if (product.ProductId === ProductId) {
            isAdded = true
            return
        }
    })

    if (isAdded) {
        productError.textContent = 'This product already added!';
        return;
    }

    //show product code on modal
    showAddedProductCode();

    //open modal popup
    modalInsetCode.modal('show');

    //show check btn
    btnCheckCodeAndCartDisabled(true);
});

//show hide check cart btn
function btnCheckCodeAndCartDisabled(isChecking) {
    btnCheckProduct.style.display = isChecking ? "block" : "none";
    btnAddToList.style.display = isChecking ? "none" : "block";
}

//btn check product
btnCheckProduct.addEventListener('click', function (evt) {
    productError.textContent = '';

    const ParentId = selectCategory.value;
    const ProductId = +selectProductId.value;
    const PurchasePrice = +inputPurchasePrice.value;
    const SellingPrice = +inputSellingPrice.value;

    if (!ParentId || !ProductId || !PurchasePrice || !SellingPrice) {
        productError.textContent = 'Provide product info!'
        return;
    }

    //set the last value of input
    setProductTempObject(formCart);

    if (tempStorage.ProductStocks.length) {
        //start loading spinner
        this.disabled = true;

        //check product code on server
        const serverCode = productCode.isExistServer(tempStorage.ProductStocks);
        serverCode.then(res => {
            if (res.length) {
                //show matched code
                const isUnsoldExist = matchExistingProductCode(res);

                if (isUnsoldExist) {
                    //play buzzer
                    buzzAudio.play();
                    return;
                }
            }
        }).finally(() => {
            this.disabled = false;
            btnCheckCodeAndCartDisabled(false);
        });
    }
});

//add product to cart
btnAddToList.addEventListener('click', function (evt) {
    productError.textContent = '';

    const ParentId = selectCategory.value;
    const ProductId = +selectProductId.value;
    const PurchasePrice = +inputPurchasePrice.value;
    const SellingPrice = +inputSellingPrice.value;

    if (!ParentId || !ProductId || !PurchasePrice || !SellingPrice) {
        productError.textContent = 'Provide product info!'
        return;
    }

    //set the last value of input
    setProductTempObject(formCart);

    if (tempStorage.ProductStocks.length) {
        //start loading spinner
        this.disabled = true;

        //check product code on server
        const serverCode = productCode.isExistServer(tempStorage.ProductStocks);
        serverCode.then(res => {
            if (res.length) {
                //show matched code
                const isUnsoldExist = matchExistingProductCode(res);

                if (isUnsoldExist) {
                    //play buzzer
                    buzzAudio.play();
                    return;
                }
            }

            //add value to cart storage
            storage.push(tempStorage);

            //show cart on table
            tbody.appendChild(createTableRow(tempStorage));

            //calculate and show price
            appendTotalPrice();

            //save to local storage
            localStorage.setItem('cart-storage', JSON.stringify(storage));

            //remove the stock temp storage
            tempStorage = null;
            localStorage.removeItem('temp-storage');

            //clear the text input
            clearInput()

            //hide modal
            modalInsetCode.modal('hide');

        }).finally(() => {
            this.disabled = false;
        });
    }
});


//input product code submit
formAddCode.addEventListener('submit', function (form) {
    form.preventDefault();

    const element = form.target.inputProductCode;
    const stock = { ProductCode: element.value };
    const codeExist = productCode.isExist(stock.ProductCode);

    if (!codeExist) {
        //add code to temp storage
        tempStorage.ProductStocks.push(stock);

        //save code to global code hub
        productCode.setStorage(stock.ProductCode);

        //append code on modal
        showAddedCode.appendChild(createCodeSpan(stock.ProductCode));

        //save to local storage
        localStorage.setItem('temp-storage', JSON.stringify(tempStorage));

        //show check btn
        btnCheckCodeAndCartDisabled(true);
    }

    //clear the code input field
    element.value = '';
});

//remove product code
showAddedCode.addEventListener('click', function (evt) {
    const codeClicked = evt.target.classList.contains('code-delete');
    if (!codeClicked) return;

    const code = evt.target.id;

    tempStorage.ProductStocks = tempStorage.ProductStocks.filter(stock => stock.ProductCode !== code);

    productCode.updateStorage(code);

    //remove code on modal
    evt.target.parentElement.remove();

    //save to local storage
    localStorage.setItem('temp-storage', JSON.stringify(tempStorage));
});

//on change select
selectCategory.addEventListener('change', onCategoryChanged);
selectProductId.addEventListener('change', onProductChanged);
tbody.addEventListener('click', onTableRowElementClicked);

//call function
showCartedProduct();
productCode.getStorage();



//****VENDORS****//

//selectors
const vendorAddClick = document.getElementById('vendorAddClick');
const inputFindVendor = document.getElementById('inputFindVendor');
const vendorInfo = document.getElementById('vendor-info');
const hiddenVendorId = document.getElementById('vendorId');
const insertModal = $('#InsertModal');

//functions

//get vendor insert modal
const onVendorAddClicked = function () {
    const url = this.getAttribute('data-url');

    axios.get(url)
        .then(response => insertModal.html(response.data).modal('show'))
        .catch(err => console.log(err))
}

//append vendor info to DOM
const appendVendorInfo = function (data) {
    hiddenVendorId.value = data.VendorId;
    vendorInfo.innerHTML = '';

    const html = `
        <li class="list-group-item"><i class="fas fa-building"></i> ${data.VendorCompanyName}</li>
        <li class="list-group-item"><i class="fas fa-user-tie"></i> ${data.VendorName}</li>
        <li class="list-group-item"><i class="fas fa-phone"></i> ${data.VendorPhone}</li>
        <li class="list-group-item"><i class="fas fa-map-marker-alt"></i> ${data.VendorAddress}</li>`;

    vendorInfo.innerHTML= html;
}

//vendor create success
function onCreateSuccess(response) {
    if (response.Status) {
        insertModal.modal('hide');
        inputFindVendor.value = '';

        appendVendorInfo(response.Data);
    }
    else {
        insertModal.html(response);
    }
}

//reset vendorId
hiddenVendorId.value = ''

//vendor autocomplete
$('#inputFindVendor').typeahead({
    minLength: 1,
    displayText: function (item) {
        return `${item.VendorCompanyName} (${item.VendorName}, ${item.VendorPhone})`;
    },
    afterSelect: function (item) {
        this.$element[0].value = item.VendorCompanyName
    },
    source: function (request, result) {
        $.ajax({
            url: "/Purchase/FindVendor",
            data: { prefix: request },
            success: function (response) { result(response); },
            error: function (err) { console.log(err) }
        });
    },
    updater: function (item) {
        appendVendorInfo(item);
        return item;
    }
});

//add event listener
vendorAddClick.addEventListener('click', onVendorAddClicked);

//customer phone auto complete as vendor
$(document).on("paste, keydown", "#VendorPhone", function () {
    $(this).typeahead({
        minLength: 1,
        displayText: function (item) {
            return `${item.CustomerName} ${item.PhonePrimary ? item.PhonePrimary : ''} ${item.OrganizationName ? item.OrganizationName : ''}`;
        },
        afterSelect: function (item) {
            this.$element[0].value = item.PhonePrimary
        },
        source: function (request, result) {
            setTimeout(() => {
                $.ajax({
                    url: "/Selling/FindCustomers",
                    data: { prefix: request },
                    success: function (response) { result(response); },
                    error: function (err) { console.log(err) }
                });
            }, 1000);
        },
        updater: function (item) {
            autoFillInput(item);
            return item;
        }
    })
});

//fill customer info to input
function autoFillInput(customer) {
    document.querySelector("#VendorCompanyName").value = customer.OrganizationName ? customer.OrganizationName : customer.CustomerName;
    document.querySelector("#VendorName").value = customer.CustomerName;
    document.querySelector("#VendorAddress").value = customer.CustomerAddress;
    document.querySelector("#Description").value = customer.Description;

    const errorMessage = document.querySelector("#errorMessage");
    $.ajax({
        url: '/Purchase/IsCustomerPhoneExist',
        type: "POST",
        data: { phone: customer.PhonePrimary },
        success: function (response) {
            errorMessage.style.display = response ? "block" : "none";
        },
        error: function (error) {
            console.log(error);
        }
    });
}

//****PAYMENT SECTION****/

//functions
//compare Validation
const validInput = function(total, inputted) {
    return (total < inputted) ? false: true;
}

//input discount amount
const onInputDiscount = function () {
    const total = +totalPrice.textContent;
    const discount = +this.value;
    const isValid = validInput(total, discount);
    const grandTotal = (total - discount);

    this.setAttribute('max', total);
    
    totalPayable.innerText = isValid ? grandTotal.toFixed() : total;
    totalDue.innerText = isValid ? grandTotal.toFixed() : total;

    if (inputPaid.value)
        inputPaid.value = '';
}

//input paid amount
const onInputPaid = function () {
    const payable = +totalPayable.textContent;
    const paid = +this.value;
    const isValid = validInput(payable, paid);
    const due = (payable - paid);

    paid ? selectPaymentMethod.setAttribute('required', true) : selectPaymentMethod.removeAttribute('required');

    this.setAttribute('max', payable);

    totalDue.innerText = isValid ? due.toFixed() : payable;
}

//validation info
const validation = function () {
    vendorError.textContent = ''

    if (!hiddenVendorId.value) {
        vendorInfo.innerHTML = '<li class="list-group-item list-group-item-danger text-center"><i class="fas fa-exclamation-triangle mr-1 red-text"></i>Select or add Vendor for Purchase!</li>';
        return false;
    }

    if (!storage.length) {
        vendorError.textContent = 'Add product to purchase!';
        return false;
    }
    return true;
}

//remove localstorage
const localStoreClear = function () {
    localStorage.removeItem('cart-storage');
    localStorage.removeItem('code-storage');
}

//submit on server
const onPurchaseSubmitClicked = function(evt) {
    evt.preventDefault();

    const valid = validation();
    if (!valid) return;

    //disable button on submit
    const btnSubmit = evt.target.btnPurchase;
    btnSubmit.innerText = 'submitting..';
    btnSubmit.disabled = true;

    const body = {
        VendorId: +hiddenVendorId.value,
        PurchaseTotalPrice: +totalPrice.textContent,
        PurchaseDiscountAmount: +inputDiscount.value || 0,
        PurchasePaidAmount: +inputPaid.value || 0,
        AccountId: inputPaid.value ? selectPaymentMethod.value : '',
        MemoNumber: inputMemoNumber.value,
        PurchaseDate: inputPurchaseDate.value,
        Products: storage
    }

    $.ajax({
        url: '/Purchase/Purchase',
        type: "POST",
        data: body,
        success: function (response) {
            $.notify(response.Message, response.IsSuccess ? "success" : "error");

            if (response.IsSuccess) {
                localStoreClear();
                location.href = `/Purchase/PurchaseReceipt/${response.Data}`;
            }

            btnSubmit.innerText = 'PURCHASE';
            btnSubmit.disabled = false;
        },
        error: function (error) {
            console.log(error);
            btnSubmit.innerText = 'PURCHASE';
            btnSubmit.disabled = false;
        }
    });
}

//event listener
formPayment.addEventListener('submit', onPurchaseSubmitClicked);
inputDiscount.addEventListener('input', onInputDiscount);
inputPaid.addEventListener('input', onInputPaid);

