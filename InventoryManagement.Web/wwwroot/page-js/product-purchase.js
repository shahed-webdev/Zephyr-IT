
//date picker
 $('.datepicker').pickadate().val(moment(new Date()).format('DD MMMM, YYYY'));

// Material Select Initialization
$('.mdb-select').materialSelect();


//global store
let storage = [];
let tempStorage = null;
let codeStorage = [];

//selectors
//product info form
const formCart = document.getElementById("cart-form")
const selectCategory = formCart.ParentId
const selectProductId = formCart.selectProductId

//payment selectors
const formPayment = document.getElementById('formPayment');
const totalPrice = formPayment.querySelector('#totalPrice');
const inputDiscount = formPayment.inputDiscount;
const totalPayable = formPayment.querySelector('#totalPayable');
const inputPaid = formPayment.inputPaid;
const totalDue = formPayment.querySelector('#totalDue');
const selectPaymentMethod = formPayment.selectPaymentMethod;
const inputPurchaseDate = formPayment.inputPurchaseDate;
const vendorError = formPayment.querySelector('#vendor-error');

//form add code
const formAddCode = document.getElementById("formAddCode");
const codeExistError = formAddCode.querySelector("#code-exist-error");

const tbody = document.getElementById("tbody");
const modalInsetCode = $('#modalInsetCode');
const showAddedCode = document.getElementById('showAddedCode');
const btnAddTolist = document.getElementById('btnAddToList');
const buzzAudio = document.getElementById('audio');


//****Global product Code Storage****//
const productCode = {
    isExistServer: function (codeArray) {
        const url = '/Product/PurchaseCodeIsExist';
        const options = {
            method: 'post',
            url: url,
            data: codeArray
        }

        return axios(options).then(response => response.data).catch(error => console.log(error.response));
    },
    isExist: function (newCode) {
        if (!codeStorage.length) return false;

        const codeExis = codeStorage.some(code => code === newCode);
        codeExistError.innerText = codeExis ? `${newCode}: Already Added!` : '';

        if (codeExis) buzzAudio.play();

        return codeExis;
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
const clearMDBdropDownList = function (mainSelector) {
    const content = mainSelector.querySelectorAll('.select-dropdown li');
    content.forEach(li => {
        content[0].classList.add('active','selected');

        if (li.classList.contains('selected')) {
            li.classList.remove(['active', 'selected']);
            li.click();
            return;
        }
    });
}

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
        clearMDBdropDownList(formPayment);
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

    //column 2
    const td2 = tr.insertCell(1);
    td2.appendChild(document.createTextNode(item.ProductName));
    td2.setAttribute('title', item.Description);

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

//category drodown change
const onCategoryChanged = function () {
    const categoryId = +this.value
    if (!categoryId) return

    const url = '/Product/GetProductByCategory'
    const parameter = { params: { categoryId } }

    axios.get(url, parameter)
        .then(res => {
            if (res.data.length) {
                const fragment = document.createDocumentFragment()
                let option1 = document.createElement("option");
                option1.value = ""
                option1.text = "Product Name"
                option1.setAttribute("disabled", "disabled")
                option1.setAttribute("selected", true)
                fragment.appendChild(option1)

                res.data.forEach(item => {
                    let option = document.createElement("option");
                    option.value = item.ProductId
                    option.text = item.ProductName
                    fragment.appendChild(option)
                })

                $('.product-select').materialSelect("destroy");

                selectProductId.innerHTML = ''
                selectProductId.appendChild(fragment)

                // Material Select Initialization
                $('.product-select').materialSelect();
            }
        })
}

//product drodown change
const onProductChanged = function () {
    const productId = +this.value
    if (!productId) return

    const url = '/Product/GetProductInfo'
    const parameter = { params: { productId } }

    const purchasePrice = formCart.inputPurchasePrice;
    const sellingPrice = formCart.inputSellingPrice;
    const warranty = formCart.inputWarranty;
    const description = formCart.inputDescription;

    purchasePrice.value = ''
    sellingPrice.value = ''
    warranty.value = ''
    description.value = ''

    axios.get(url, parameter)
        .then(res => {
            sellingPrice.value = res.data.SellingPrice
            sellingPrice.nextElementSibling.classList.add('active')

            warranty.value = res.data.Warranty
            warranty.nextElementSibling.classList.add('active')

            description.value = res.data.Description
            description.nextElementSibling.classList.add('active')
        })
}



//click remove or stock
const ontableRowElementClicked = function (evt) {
    const element = evt.target;
    const removeClicked = element.classList.contains('remove');
    const row = element.parentElement.parentElement;
    const SN = +row.getAttribute('data-sn');

    if (removeClicked)
        removeProduct(row, SN);
}

//create product code span on modal
const createCodeSpan = function (newCode) {
    const iCode = document.createElement('span');
    iCode.classList.add('badge-pill', 'badge-success', 'code-span');
    iCode.appendChild(document.createTextNode(newCode));

    return iCode;
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

    if (!tempStorage) {
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
    }

    localStorage.setItem('temp-storage', JSON.stringify(tempStorage));
}

//open product code modal submit
const onOpenProductCodeModal = function (form) {
    form.preventDefault();

    //save the last value of input
    setProductTempObject(form.target);

    //show product code on modal
    showAddedProductCode();

    //auto focus code input
    document.getElementById('inputProductCode').focus();

    //open modal popup
    modalInsetCode.modal('show');
}

//add product code to temp storage
const onSubmitProductCode = function (form) {
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
    }

    //clear the code input field
    element.value = '';
}

//remove product code
const onProductCodeClicked = function (evt) {
    const codeClicked = evt.target.classList.contains('code-span');
    if (!codeClicked) return;

    const code = evt.target.innerText;

    tempStorage.ProductStocks = tempStorage.ProductStocks.filter(stock => stock.ProductCode !== code);

    productCode.updateStorage(code);
    
    //remove code on modal
    evt.target.remove();

    //save to local storage
    localStorage.setItem('temp-storage', JSON.stringify(tempStorage));
}

//match Existing Product code
const matchExistingProductCode = function (stocks) {
    const addedCode = showAddedCode.querySelectorAll('.code-span');
    addedCode.forEach(added => {
        stocks.forEach(stock => {
            if (added.textContent === stock.ProductCode) {
                added.classList.remove('badge-success');
                added.classList.add('badge-danger');
            }
        })
    });
}

//add product to list
const onAddProductToList = function (){
    const ParentId = formCart.ParentId.value;
    const ProductId = formCart.selectProductId.value;
    const PurchasePrice = +formCart.inputPurchasePrice.value;
    const SellingPrice = +formCart.inputSellingPrice.value;

    if (!ParentId || !ProductId || !PurchasePrice || !SellingPrice) {
        alert('Provide product info!');
        return;
    }

    //set the last value of input
    setProductTempObject(formCart);

    if (tempStorage.ProductStocks.length) {
        //start loading spnner
        this.children[0].style.display = "none";
        this.children[1].style.display = "inline-block";
        this.disabled = true;

        //check product code on server
        const serverCode = productCode.isExistServer(tempStorage.ProductStocks);
        serverCode.then(res => {
            if (res.length) {
                //play buzzer
                buzzAudio.play();

                //show product code on modal
                showAddedProductCode();

                //show matched code
                matchExistingProductCode(res);
               
                //show modal
                modalInsetCode.modal('show');
                return;
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
            clearTextinput();

            //clear cart-form select
            clearMDBdropDownList(formCart);

        }).finally(() => {
            this.children[0].style.display = "inline-block";
            this.children[1].style.display = "none";
            this.disabled = false;
        });
    }
    else {
        modalInsetCode.modal('show');
    }
}


//event listners
formCart.addEventListener('submit', onOpenProductCodeModal);
btnAddTolist.addEventListener('click', onAddProductToList);

//add product code form
formAddCode.addEventListener('submit', onSubmitProductCode);
showAddedCode.addEventListener('click', onProductCodeClicked);

selectCategory.addEventListener('change', onCategoryChanged);
selectProductId.addEventListener('change', onProductChanged);
tbody.addEventListener('click', ontableRowElementClicked);

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
const appendVendorInfo = function (Data) {
    hiddenVendorId.value = Data.VendorId;
    vendorInfo.innerHTML = '';

    const html = `
        <li class="list-group-item"><i class="fas fa-building"></i> ${Data.VendorCompanyName}</li>
        <li class="list-group-item"><i class="fas fa-user-tie"></i> ${Data.VendorName}</li>
        <li class="list-group-item"><i class="fas fa-phone"></i> ${Data.VendorPhone}</li>
        <li class="list-group-item"><i class="fas fa-map-marker-alt"></i> ${Data.VendorAddress}</li>`;

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
            url: "/Product/FindVendor",
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

//event listner
vendorAddClick.addEventListener('click', onVendorAddClicked);


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

//remove localstore
const localstoreClear = function () {
    localStorage.removeItem('cart-storage');
    localStorage.removeItem('code-storage');
}

//submit on server
const onPurchaseSubmitClicked = function (evt) {
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
        PurchaseDiscountAmount: +inputDiscount.value | 0,
        PurchasePaidAmount: +inputPaid.value | 0,
        PaymentMethod: inputPaid.value ? selectPaymentMethod.value : '',
        PurchaseDate: new Date(inputPurchaseDate.value),
        Products: storage
    }

    const url = '/Product/Purchase';
    const options = {
        method: 'post',
        url: url,
        data: body
    }

    axios(options)
        .then(response => {
            if (response.data.IsSuccess) {
                localstoreClear();
                location.href = `/Product/PurchaseReceipt/${response.data.Data}`;
            }
        })
        .catch(error => {
            console.log('error:', error.response);

            if (error.response)
                vendorError.textContent = error.response.Message;

        })
        .finally(() => {
            btnSubmit.innerText = 'PURCHASE';
            btnSubmit.disabled = false;
        });
}

//event listner
formPayment.addEventListener('submit', onPurchaseSubmitClicked);
inputDiscount.addEventListener('input', onInputDiscount);
inputPaid.addEventListener('input', onInputPaid);

