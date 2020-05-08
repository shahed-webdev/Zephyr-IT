
//globar store
let storage = [];

//selectors
const cartForm = document.getElementById("cart-form");
const tbody = document.getElementById("tbody");

 //functions
const getStorage = function () {
    if (localStorage.getItem('cart-storage')) {
        storage = JSON.parse(localStorage.getItem('cart-storage'));
    };
}

const isProductExist = function (product) {
    return storage.some(elem => elem.ParentId === product.ParentId)
}

const updateStock = function (product, ProductCode) {
    storage.forEach(item => {
        if (item.ParentId === product.ParentId) {
            item.ProductStocks.push(ProductCode);
        }
    });
}

const createTableRow = function (item) {
    let tr = document.createElement("tr");

    //column 1
    let td1 = tr.insertCell(0);
    td1.appendChild(document.createTextNode(item.ParentId));

    //column 2
    let td2 = tr.insertCell(1);
    td2.appendChild(document.createTextNode(item.ProductName));

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

const showCartedProduct = function () {
    getStorage();

    tbody.innerHTML = '';
    const fragment = document.createDocumentFragment();

    storage.forEach(item => {
        const tr = createTableRow(item);
        fragment.appendChild(tr);
    });

    tbody.appendChild(fragment);
 }

const onAddtoCart = function (form) {
    form.preventDefault();
    const element = form.target;

    const ParentId = element.ParentId;
    const ProductName = element.inputProductName.value;
    const PurchasePrice = element.inputPurchasePrice.value;
    const SellingPrice = element.inputSellingPrice.value;
    const Warranty = element.inputWarranty.value;
    const Description = element.inputDescription.value;

    //stock
    const ProductCode = { ProductCode: element.inputProductCode.value };
    const product = { ParentId: ParentId.value, ProductName, PurchasePrice, SellingPrice, Warranty, Description, ProductStocks: [] };
    product.ProductStocks.push(ProductCode);


    const uniqueProduct = !isProductExist(product);
    if (uniqueProduct)
        storage.push(product);
    else
        updateStock(product, ProductCode);


    localStorage.setItem('cart-storage', JSON.stringify(storage));
    console.log(storage)
    showCartedProduct();
}

//event listners
cartForm.addEventListener('submit', onAddtoCart);


//call function
showCartedProduct();
console.log(storage)