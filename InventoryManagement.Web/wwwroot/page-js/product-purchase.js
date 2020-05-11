
//globar store
let storage = [];

//selectors
const cartForm = document.getElementById("cart-form");
const selectCategory = cartForm.querySelector("#ParentId");
const codeExistError = cartForm.querySelector("#code-exist-error");
const tbody = document.getElementById("tbody");
const productCodeBody = document.getElementById('product-code-body');

//functions
const getStorage = function () {
    if (localStorage.getItem('cart-storage')) {
        storage = JSON.parse(localStorage.getItem('cart-storage'));
    };
}

//added product code count
const productCodeCount = function (value = null) {
    let codeCount = cartForm.querySelector('#codeCount');
    codeCount.innerText = value ? value : 0;
}

//increment the stock value
const increaseStockValue = function (ParentId, value) {
    const tRows = tbody.querySelectorAll('tr');
    productCodeCount();

    tRows.forEach(row => {
        const rowSn= row.getAttribute('SN');

        if (rowSn === SN) {
            row.cells[5].querySelector('strong').textContent = value;

            //count product on text field 
            productCodeCount(value);
            return;
        }
    });
}

//check product already added
const isProductExist = function (product) {
    return storage.some(elem => (elem.ProductName === product.ProductName) && (elem.ParentId === product.ParentId));
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

//update stock on table
const updateStock = function (product, newCode) {
    codeExistError.innerText = '';

    storage.forEach(item => {
        if (item.SN === product.SN) {
            const codeExist = item.ProductStocks.some(code => code.ProductCode === newCode.ProductCode);
            if (!codeExist) {
                //update storage
                item.ProductStocks.push(newCode);
                //update on DOM
                increaseStockValue(item.SN, item.ProductStocks.length);
            } else
                codeExistError.innerText = `${newCode.ProductCode}: Already Added!`;
            return;
        }
    });
}

//create table rows
const createTableRow = function (item, SN) {
    let tr = document.createElement("tr");
    tr.setAttribute('SN', SN);

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

//clear textbox field
const clearTextinput = function () {
    let productName = cartForm.inputProductName;
    let purchasePrice = cartForm.inputPurchasePrice;
    let sellingPrice = cartForm.inputSellingPrice;
    let warranty = cartForm.inputWarranty;
    let description = cartForm.inputDescription;
    let productCode = cartForm.inputProductCode;

    const elements = [productName, purchasePrice, sellingPrice, warranty, description, productCode];
    elements.forEach((element) => {
        element.value = '';
        element.nextElementSibling.classList.remove('active');
    });
}

//add product submit
const onAddtoCart = function (form) {
    form.preventDefault();
    const element = form.target;

    const ParentId = element.ParentId;
    const ProductName = element.inputProductName.value;
    const PurchasePrice = +element.inputPurchasePrice.value;
    const SellingPrice = +element.inputSellingPrice.value;
    const Warranty = element.inputWarranty.value;
    const Description = element.inputDescription.value;
    const SN = tbody.querySelectorAll('tr').length +1;

    //stock
    const ProductCode = { ProductCode: element.inputProductCode.value };
    const product = {
        SN,
        ParentId: ParentId.value,
        Category: ParentId.options[ParentId.selectedIndex].text,
        ProductName,
        PurchasePrice,
        SellingPrice,
        Warranty,
        Description,
        ProductStocks: []
    };

    product.ProductStocks.push(ProductCode);

    const uniqueProduct = !isProductExist(product);
    if (uniqueProduct) {
        //add product to global store
        storage.push(product);

        //append the new product
        const tr = createTableRow(product, SN);
        tbody.appendChild(tr);

        //count added product
        productCodeCount(1);
    }
    else {
        //update the stock
        updateStock(product, ProductCode);
    }

    //product code clear
    cartForm.inputProductCode.value = '';

    //sum total purchase amount
    appendTotalPrice();

    //save to local storage
    localStorage.setItem('cart-storage', JSON.stringify(storage));
}

//category drodown change
const onCategoryChanged = function () {
    //clear bind text field
    clearTextinput();
}

//product code input
const onProductCodeInput = function () {
    if (codeExistError.innerText)
        codeExistError.innerText = '';

    storage.forEach(item => {
        const codeExist = item.ProductStocks.some(code => code.ProductCode === this.value);

        if (codeExist) {
            codeExistError.innerText = `${this.value}: Already Added!`;
            return;
        }
    });
}

//show product code
const displayProductCode = function (SN) {
    const fragment = document.createDocumentFragment();
    let codes = [];

    storage.forEach(item => {
        if (item.SN === SN) {
            codes = item.ProductStocks;
            return;
        }
    });

    codes.forEach(code => {
        let span = document.createElement('span');
        let i = document.createElement('i');

        i.classList.add('fal', 'fa-times','ml-1');
        span.classList.add('badge-pill', 'badge-primary','m-1');
        span.appendChild(document.createTextNode(code.ProductCode));
        span.appendChild(i);

        fragment.appendChild(span);
    });

    productCodeBody.innerHTML = '';
    productCodeBody.appendChild(fragment);
    $('#modalProductCode').modal('show');
}

//remove product from list
const removeProduct = function (row, SN) {
    //remove product from storage
    storage = storage.filter(item => item.SN !== SN);

    //save to local storage
    localStorage.setItem('cart-storage', JSON.stringify(storage));

    //remove the row
    row.remove();

    //re calculate total amount
    appendTotalPrice();
}

//click remove or stock
const ontableRowElementClicked = function (evt) {
    const element = evt.target;
    const removeClicked = element.classList.contains('remove');
    const stockClicked = element.classList.contains('stock');
    const row = element.parentElement.parentElement;
    const SN = +row.getAttribute('SN');

    if (removeClicked)
        removeProduct(row, SN);

    if (stockClicked)
        displayProductCode(SN);

}

//event listners
cartForm.addEventListener('submit', onAddtoCart);
selectCategory.addEventListener('change', onCategoryChanged);
cartForm.inputProductCode.addEventListener('input', onProductCodeInput);
tbody.addEventListener('click', ontableRowElementClicked);

//call function
showCartedProduct();


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