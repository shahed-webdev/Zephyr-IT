
//globar store
let storage = [];
let tempStorage = null;
let codeStorage = [];

//selectors
const cartForm = document.getElementById("cart-form");
const formAddCode = document.getElementById("formAddCode");
const codeExistError = formAddCode.querySelector("#code-exist-error");
const selectCategory = cartForm.querySelector("#ParentId");
const tbody = document.getElementById("tbody");
const modalInsetCode = $('#modalInsetCode');
const showAddedCode = document.getElementById('showAddedCode');


//selectors object
const productFormSelectors = function(){
    let productName = cartForm.inputProductName;
    let purchasePrice = cartForm.inputPurchasePrice;
    let sellingPrice = cartForm.inputSellingPrice;
    let warranty = cartForm.inputWarranty;
    let description = cartForm.inputDescription;

    return [productName, purchasePrice, sellingPrice, warranty, description];
}

//****Global product Code Storage****//
const productCode = {
    isExistServer: function (codeArray) {
        const url = '/Product/PurchaseCodeIsExist';
        const options = {
            method: 'post',
            url: url,
            data: codeArray
        }

        axios(options)
            .then(function (response) {
                console.log(response)
            })
            .catch(function (error) {
                console.log(error.response);
            });
    },
    isExist: function (newCode) {
        if (!codeStorage.length) return false;

        const codeExis = codeStorage.some(code => code === newCode);
        codeExistError.innerText = codeExis ? `${newCode}: Already Added!` : '';

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

//append un-added data to form
const setTempDataToForm = function () {
    getTempStorage();

    if (!tempStorage) return;

    cartForm.ParentId.value = tempStorage.ParentId;
    const elements = { ...productFormSelectors() };

    elements['0'].value = tempStorage.ProductName
    elements['0'].nextElementSibling.classList.add('active');

    elements['1'].value = tempStorage.PurchasePrice
    elements['1'].nextElementSibling.classList.add('active');

    elements['2'].value = tempStorage.SellingPrice
    elements['2'].nextElementSibling.classList.add('active');

    if (tempStorage.Warranty) {
        elements['3'].value = tempStorage.Warranty
        elements['3'].nextElementSibling.classList.add('active');
    }

    if (tempStorage.Description) {
        elements['4'].value = tempStorage.Description
        elements['4'].nextElementSibling.classList.add('active');
    }
}

//calculate purchase Total
const purchaseTotalPrice = function () {
    const multi = storage.map(item => item.PurchasePrice * item.ProductStocks.length);
    return multi.reduce((prev, curr) => prev + curr, 0);
}

//append total price to DOM
const appendTotalPrice = function () {
    const totalPrice = document.getElementById('totalPrice');
    const totalPayable = document.getElementById('totalPayable');
    const totalDue = document.getElementById('totalDue');
    const totalAmount = purchaseTotalPrice();

    totalPrice.innerText = totalAmount
    totalPayable.innerText = totalAmount;
    totalDue.innerText = totalAmount;
}

//create table rows
const createTableRow = function (item) {
    let tr = document.createElement("tr");
    tr.setAttribute('data-sn', item.SN);

    //column 1
    let td1 = tr.insertCell(0);
    td1.appendChild(document.createTextNode(item.Category));

    //column 2
    let td2 = tr.insertCell(1);
    td2.appendChild(document.createTextNode(item.ProductName));
    td2.setAttribute('title', item.Description);

    //column 3
    let td3 = tr.insertCell(2);
    td3.appendChild(document.createTextNode(item.PurchasePrice));

    //column 4
    let td4 = tr.insertCell(3);
    td4.appendChild(document.createTextNode(item.SellingPrice));

    //column 5
    let td5 = tr.insertCell(4);
    td5.appendChild(document.createTextNode(item.Warranty));

    //column 6
    let td6 = tr.insertCell(5);
    let strong = document.createElement('strong');
    strong.appendChild(document.createTextNode(item.ProductStocks.length));
    strong.classList.add('badge-pill', 'badge-success', 'stock');
    td6.appendChild(strong);

    //column 6
    let td7 = tr.insertCell(6);
    let removeIcon = document.createElement('i');
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

    //save to local storage
    localStorage.setItem('cart-storage', JSON.stringify(storage));

    //remove the row
    row.remove();

    //re calculate total amount
    appendTotalPrice();
}

//clear textbox field
const clearTextinput = function () {
    const elements = productFormSelectors();
    elements.forEach(element => {
        if (element.value) {
            element.value = '';
            element.nextElementSibling.classList.remove('active');
        }
    });
}

//category drodown change
const onCategoryChanged = function () {
    //clear bind text field
    clearTextinput();
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

//show product code on popup
const showAddedProductCode = function () {
    showAddedCode.innerHTML = '';
    if (!tempStorage) return;

    const stocks = tempStorage.ProductStocks;
    const fragment = document.createDocumentFragment();

    if (stocks.length > 0) {
        stocks.forEach(stock => {
            let iCode = document.createElement('span');
            iCode.classList.add('badge-pill', 'badge-success','code-span');
            iCode.appendChild(document.createTextNode(stock.ProductCode));

            fragment.appendChild(iCode);
        });
       
        showAddedCode.appendChild(fragment);
    }
}

//set temp Storage value
const setProductTempObject = function (element) {
    const SN = tbody.querySelectorAll('tr').length + 1;
    const ParentId = element.ParentId;
    const ProductName = element.inputProductName.value;
    const PurchasePrice = +element.inputPurchasePrice.value;
    const SellingPrice = +element.inputSellingPrice.value;
    const Warranty = element.inputWarranty.value;
    const Description = element.inputDescription.value;

    if (!tempStorage) {
        tempStorage = {
            SN,
            ParentId: ParentId.value,
            Category: ParentId.options[ParentId.selectedIndex].text,
            ProductName,
            PurchasePrice,
            SellingPrice,
            Warranty,
            Description,
            ProductStocks: []
        }
    }
    else {
        tempStorage.ParentId = ParentId.value;
        tempStorage.Category = ParentId.options[ParentId.selectedIndex].text;
        tempStorage.ProductName = ProductName;
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

        //save code to global hub
        productCode.setStorage(stock.ProductCode);

        //show code on modal
        showAddedProductCode();

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

    //show code on modal
    showAddedProductCode();

    //save to local storage
    localStorage.setItem('temp-storage', JSON.stringify(tempStorage));
}

//add product to list
const onAddProductToList = function (evt) {
    const ParentId = cartForm.ParentId.value;
    const ProductName = cartForm.inputProductName.value;
    const PurchasePrice = cartForm.inputPurchasePrice.value;
    const SellingPrice = cartForm.inputSellingPrice.value;

    if (!ParentId && !ProductName && !PurchasePrice && !SellingPrice) {
        console.log('form value can not be empty');
        return;
    }

    //set the last value of input
    setProductTempObject(cartForm);

    if (tempStorage.ProductStocks.length) {

        //check product code on server
        productCode.isExistServer(tempStorage.ProductStocks);

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
    }
    else {
        modalInsetCode.modal('show');
    }
}


//event listners
cartForm.addEventListener('submit', onOpenProductCodeModal);
cartForm.btnAddToList.addEventListener('click', onAddProductToList);

//add product code form
formAddCode.addEventListener('submit', onSubmitProductCode);
showAddedCode.addEventListener('click', onProductCodeClicked);

selectCategory.addEventListener('change', onCategoryChanged);
tbody.addEventListener('click', ontableRowElementClicked);

//call function
showCartedProduct();
setTempDataToForm();
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
        .then(response => {
            insertModal.html(response.data).modal('show');
        })
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
    console.log(response.Data)
    if (response.Status) {
        insertModal.modal('hide');
        inputFindVendor.value = '';

        appendVendorInfo(response.Data);
    }
    else {
        insertModal.html(response);
    }
}

//vendor autocomplete
$('#inputFindVendor').typeahead({
    minLength: 3,
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
            success: function (response) { result(response); }
        });
    },
    updater: function (item) {
        appendVendorInfo(item);
        return item;
    }
});

//event listner
vendorAddClick.addEventListener('click', onVendorAddClicked);