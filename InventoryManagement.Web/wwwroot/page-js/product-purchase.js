
//globar store
let storage = [];

//selectors
const cartForm = document.getElementById("cart-form");
const selectCategory = cartForm.querySelector("#ParentId");
const codeExistError = cartForm.querySelector("#code-exist-error");
const tbody = document.getElementById("tbody");


//functions
const getStorage = function () {
    if (localStorage.getItem('cart-storage')) {
        storage = JSON.parse(localStorage.getItem('cart-storage'));
    };
}

//set row to selected
const selectedRow = function (row, isFound) {
    const rStyle = row.classList;
    const isAdded = rStyle.contains('active-row');

    if (isFound) {
        if (!isAdded) rStyle.add('active-row');
    }
    else {
        if (isAdded) rStyle.remove('active-row');
    }

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
        const id = row.getAttribute('data-id');

        if (id === ParentId) {
            row.cells[row.cells.length - 1].querySelector('strong').textContent = value;

            //count product on text field 
            productCodeCount(value);

            //row selected
            selectedRow(row, true);
            return;
        }
        else selectedRow(row, false);
    });
}

//check product already added
const isProductExist = function (product) {
    return storage.some(elem => elem.ParentId === product.ParentId)
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
        if (item.ParentId === product.ParentId) {

            const codeExist = item.ProductStocks.some(code => code.ProductCode === newCode.ProductCode);
            if (!codeExist) {
                //update storage
                item.ProductStocks.push(newCode);
                //update on DOM
                increaseStockValue(item.ParentId, item.ProductStocks.length);
            } else
                codeExistError.innerText = `${newCode.ProductCode}: Already Added!`;
            return;
        }
    });
}

//create table rows
const createTableRow = function (item) {
    let tr = document.createElement("tr");
    tr.setAttribute('data-id', item.ParentId);

    //column 1
    let td1 = tr.insertCell(0);
    td1.appendChild(document.createTextNode(item.ParentId));

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
    let span = document.createElement('strong');
    span.appendChild(document.createTextNode(item.ProductStocks.length));
    span.classList.add('badge-pill', 'badge-success');
    td6.appendChild(span);

    return tr;
}

//show product on table
const showCartedProduct = function () {
    getStorage();
    appendTotalPrice();

    const fragment = document.createDocumentFragment();

    storage.forEach(item => {
        const tr = createTableRow(item);
        fragment.appendChild(tr);
    });

    tbody.appendChild(fragment);
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

    //stock
    const ProductCode = { ProductCode: element.inputProductCode.value };
    const product = { ParentId: ParentId.value, ProductName, PurchasePrice, SellingPrice, Warranty, Description, ProductStocks: [] };
    product.ProductStocks.push(ProductCode);

    const uniqueProduct = !isProductExist(product);
    if (uniqueProduct) {
        storage.push(product);

        //append the new product
        const tr = createTableRow(product);
        tr.classList.add('active-row');
        tbody.appendChild(tr);

        productCodeCount(1);
    }
    else {
        //update the stock
        updateStock(product, ProductCode);
    }

    localStorage.setItem('cart-storage', JSON.stringify(storage));
}

//bind textbox field
const bindTextinput = function (cells = null) {
    let productName = cartForm.inputProductName;
    let purchasePrice = cartForm.inputPurchasePrice;
    let sellingPrice = cartForm.inputSellingPrice;
    let warranty = cartForm.inputWarranty;
    let description = cartForm.inputDescription;

   //count added product code on taxt field
    if (cells) productCodeCount(cells[5].querySelector('strong').textContent);
    else productCodeCount();

    const elements = [productName, purchasePrice, sellingPrice, warranty, description];
    elements.forEach((element, index) => {
        if (cells) {
            index === 4 ? elements[index].value = cells[1].getAttribute('title') : element.value = cells[index + 1].textContent;
            element.nextElementSibling.classList.add('active');
            element.disabled = true;
        }
        else {
            element.value = '';
            element.nextElementSibling.classList.remove('active');
            element.disabled = false;
        }
    });
}

//category drodown change
const onCategoryChanged = function () {
    const id = this.value;
    const tRows = tbody.querySelectorAll('tr');

    //clear bind text field
    bindTextinput();

    tRows.forEach(row => {
        const rowId = row.getAttribute('data-id');
        if (id === rowId) {
            //selected table row
            selectedRow(row, true);
            //bind text field
            bindTextinput(row.cells);
            return;
        }
        else {
            //un-select table row
            selectedRow(row, false);
        }
    });
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



//event listners
cartForm.addEventListener('submit', onAddtoCart);
selectCategory.addEventListener('change', onCategoryChanged);
cartForm.inputProductCode.addEventListener('input', onProductCodeInput);

//call function
showCartedProduct();