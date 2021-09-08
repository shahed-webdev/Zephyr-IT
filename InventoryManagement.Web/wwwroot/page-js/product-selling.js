
$(function() {
    // material select initialization
    $(".mdb-select").materialSelect();

    $(".datepicker").pickadate({
        format: "d-mmmm-yyyy",
        min: new Date()
    });
});

// global storage
let cartProducts = [];


//*****SELECTORS*****/
// product code form
const formCode = document.getElementById('formCode');
const inputBarCode = formCode.inputBarCode;
const productCount = formCode.querySelector('#productCount');
const codeExistError = formCode.querySelector('#codeExistError');
const btnFind = formCode.btnFind;

// product table
const formTable = document.getElementById('formTable');
const tbody = document.getElementById('t-body');
const productTotalPrice = document.getElementById('productTotalPrice');

//payment selectors
const formPayment = document.getElementById('formPayment');
const totalPrice = formPayment.querySelector('#totalPrice');
const inputDiscount = formPayment.inputDiscount;
const totalPayable = formPayment.querySelector('#totalPayable');
const inputPaid = formPayment.inputPaid
const totalDue = formPayment.querySelector('#totalDue');
const selectPaymentMethod = formPayment.selectPaymentMethod;
const inputAccountTransactionCharge = formPayment.inputAccountTransactionCharge;
const inputPromisedDate = formPayment.inputPromisedDate;

//expense
const inputExpense = formPayment.inputExpense;
const inputExpenseDescription = formPayment.inputExpenseDescription;

//service charge
const inputServiceCharge = formPayment.inputServiceCharge;
const inputServiceChargeDescription = formPayment.inputServiceChargeDescription;
const inputServiceCost = formPayment.inputServiceCost;

//purchase from customer
const findPurchaseBill = formPayment.findPurchaseBill;
const hiddenPurchaseId = formPayment.hiddenPurchaseId;
const inputPurchaseBillNo = formPayment.inputPurchaseBillNo;
const inputPurchaseAmount = formPayment.inputPurchaseAmount;
const inputPurchaseDescription = formPayment.inputPurchaseDescription;

//customer
const hiddenCustomerId = formPayment.hiddenCustomerId;
const customerError = formPayment.querySelector('#customer-error');

// get set storage object
const storage = {
    saveData: function (product) {
        codeExistError.textContent = '';
        if (!product) {
            codeExistError.textContent = `"${inputBarCode.value}" Not found!`;
            return;
        }

        //unique by name, purchase price
        const found = cartProducts.some(el => el.ProductCatalogId === product.ProductCatalogId && el.ProductName === product.ProductName && el.PurchasePrice === product.PurchasePrice);
        if (!found) {
            //save to global object
            product.codes = [product.ProductCode];
            product.sellingFixedValue = product.SellingPrice;
            cartProducts.unshift(product);
        }
        else {
            const index = cartProducts.findIndex(item => item.ProductCatalogId === product.ProductCatalogId && item.ProductName === product.ProductName && item.PurchasePrice === product.PurchasePrice);
            const codes = cartProducts[index].codes;

            if (codes.indexOf(product.ProductCode) === -1) {
                codes.push(product.ProductCode);
            }
            else {
                codeExistError.textContent = `"${inputBarCode.value}" code already added!`;
            }
        }

        //clear input
        inputBarCode.value = '';

        //save to local-storage
        this.setData();

        //show table data
        tbody.innerHTML = '';
        showProducts();
    },
    setData: function () {
        localStorage.setItem('selling-cart', JSON.stringify(cartProducts))
    },
    getData: function () {
        const store = localStorage.getItem('selling-cart')
        if (!store) return;

        cartProducts = JSON.parse(store)
    },
    countProduct: function () {
        productCount.textContent = cartProducts.length
    }
}


//****FUNCTIONS****//
//append code
const appendCode = function (codes) {
   let html = ''
    codes.forEach(code => html += `<span class="code">${code}</span>`)
    return html
}

// create table rows
const createTableRow = function (item) {
    const description = item.Description ? `${item.Description},` :'';
    const note = item.Note ? `<span style="font-size: 12px;" class="badge badge-pill badge-secondary">${item.Note}</span>` : "";
   
    return `<tr data-id="${item.ProductCatalogId}" data-name="${item.ProductName}" data-pprice="${item.PurchasePrice}">
                <td>${item.ProductCatalogName}</td>
                <td>
                    ${item.ProductName},
                    ${description}
                    ${note}
                    <span class="codeSpan">${appendCode(item.codes)}</span>
                </td>
                <td>${item.Warranty}</td>
                <td>${item.codes.length}</td>
                <td><input type="number" required class="form-control inputUnitPrice" step="0.01" min="${item.sellingFixedValue}" max="${item.SellingPrice}" value="${item.SellingPrice}" /></td>
                <td class="text-right">${item.SellingPrice * item.codes.length}</td>
                <td class="text-center"><i class="fal fa-times remove"></i></td>
            </tr>`
}

// create Table on load
const showProducts = function () {
    let table = ''
    cartProducts.forEach(item => {
        table += createTableRow(item)
    });
    tbody.innerHTML = table

    //show added items count
    storage.countProduct()

    //append price
    appendTotalPrice()
}

//remove product code
const removeProductCode = function (code) {
    const id = +code.parentElement.parentElement.parentElement.getAttribute('data-id');
    const name = code.parentElement.parentElement.parentElement.getAttribute('data-name');
    const purchasePrice = +code.parentElement.parentElement.parentElement.getAttribute('data-pprice');
    const pCode = code.textContent;

    const index = cartProducts.findIndex(item => item.ProductCatalogId === id && item.ProductName === name && item.PurchasePrice === purchasePrice);
    const codes = cartProducts[index].codes;
    const pIndex = codes.indexOf(pCode);

    if (codes.length > 1) {
        codes.splice(pIndex, 1);

        //save to local-storage
        storage.setData();

        //remove code element
        code.remove();

        showProducts();
    }
}

//on click remove or stock
const onRemoveClicked = function (evt) {
    const element = evt.target;
    const removeClicked = element.classList.contains('remove');
    const codeClicked = element.classList.contains('code');
    const row = element.parentElement.parentElement;

    const id = +row.getAttribute('data-id');
    const name = row.getAttribute('data-name');
    const purchasePrice = +row.getAttribute('data-pprice');

    if (codeClicked) removeProductCode(element);

    if (!removeClicked) return;

    //remove product from storage
    cartProducts = cartProducts.filter(item => item.ProductCatalogId !== id && item.ProductName !== name && item.PurchasePrice !== purchasePrice);

    //save to local storage
    storage.setData();

    //delete row
    row.remove();

    //show added items count
    storage.countProduct();

    //append price
    appendTotalPrice();
}

//show loading
const loading = function (element, isLoading) {
    element.children[0].style.display = isLoading ? "none" : "inline-block";
    element.children[1].style.display = isLoading ? "inline-block" : "none";
    element.disabled = isLoading ? true : false;
}

//dropdown selected index 0
const clearMDBdropDownList = function (mainSelector) {
    const content = mainSelector.querySelectorAll('.select-dropdown li');
    content.forEach(li => {
        content[0].classList.add('active', 'selected');

        if (li.classList.contains('selected')) {
            li.classList.remove(['active', 'selected']);
            li.click();
            return;
        }
    });
}

//calculate selling Total
const sellingTotalPrice = function () {
    return cartProducts.map(item => item.SellingPrice * item.codes.length).reduce((prev, cur) => prev + cur, 0);
}

//append total price to DOM
const appendTotalPrice = function () {
    const productPrice = sellingTotalPrice();
    const serviceCharge = +inputServiceCharge.value;
    const accountTransactionCharge = +inputAccountTransactionCharge.value;

    const totalAmount = productPrice + serviceCharge + accountTransactionCharge;

    productTotalPrice.innerText = productPrice ? `Total: ${productPrice}` : "";
    totalPrice.innerText = totalAmount
    totalPayable.innerText = totalAmount;
    totalDue.innerText = totalAmount;

    if (inputDiscount.value)
        inputDiscount.value = "";

    if (inputPaid.value)
        inputPaid.value = "";

    if (selectPaymentMethod.selectedIndex > 0) {
        clearMDBdropDownList(formPayment);
        selectPaymentMethod.removeAttribute("required");
    }
}

//selling price change
const onInputUnitPrice = function (evt) {
    const input = evt.target;
    const onInput = input.classList.contains('inputUnitPrice');

    if (onInput) {
        const val = +input.value;
        const min = +input.getAttribute('min');
        input.setAttribute('max', input.value);

        if (min > val) return;

        const id = +input.parentElement.parentElement.getAttribute('data-id');
        const name = input.parentElement.parentElement.getAttribute('data-name');
        const purchasePrice = +input.parentElement.parentElement.getAttribute('data-pprice');

        const index = cartProducts.findIndex(item => item.ProductCatalogId === id && item.ProductName === name && item.PurchasePrice === purchasePrice);
        cartProducts[index].SellingPrice = +input.value;

        const qty = +input.parentElement.previousElementSibling.innerText;
        input.parentElement.nextElementSibling.innerText = val * qty;

        //save to local-storage
        storage.setData();

        //append price
        appendTotalPrice();
    }
}

//selling price click
formTable.addEventListener("input", onInputUnitPrice)


/**SCAN QR CODE*/
function btnScan(btn,isScan) {
    btn.disabled = !isScan;
    btn.textContent = isScan ? "Scanning.." : "Scan QR Code";

    if (isScan) {
        btn.classList.remove("btn-outline-success");
        btn.classList.add("btn-outline-danger");
    } else {
        btn.classList.add("btn-outline-success");
        btn.classList.remove("btn-outline-danger");
    }
}

//const btnScanQrCode = document.getElementById("btnScanQrCode");
//let scanner;
//btnScanQrCode.addEventListener("click", function () {
//    if (scanner) return;

//    const btn = this;
//    scanner = new Instascan.Scanner({ video: document.getElementById('preview') });
//    scanner.addListener('scan', onScanQRCode);

//    Instascan.Camera.getCameras().then(function (cameras) {
//        if (cameras.length > 0) {
//            scanner.start(cameras[0]);

//            btnScan(btn,true);

//        } else {
//            console.error('No cameras found.');
//            btnScan(btn,false);
//        }
//    }).catch(function (e) {
//        console.log(e);
//    });
//});

//scanned value
//function onScanQRCode(content) {
//    if (!content) return;
//    getProductByCode(content.trim());
//}


//get product from server
function getProductByCode(code) {
    const url = '/Selling/FindProductByCode'
    const param = { params: { code } };

    axios.get(url, param)
        .then(res => storage.saveData(res.data))
        .catch(err => console.log(err))
}

//add Product to cart
formCode.addEventListener('submit', evt => {
    evt.preventDefault();

    getProductByCode(inputBarCode.value.trim())
})

// remove product click
tbody.addEventListener("click", onRemoveClicked);


//*****CALL FUNCTION*****//
storage.getData()
showProducts()


//****PAYMENT SECTION****/

//*** Purchase from customer ****
//purchase bill no input
findPurchaseBill.addEventListener("click", function () {
    hiddenPurchaseId.value = "";
    inputPurchaseAmount.value = "";
    inputPurchaseDescription.value = "";

    $.ajax({
        url: "/Selling/GetPurchasePaidAmount",
        data: { billNo: inputPurchaseBillNo.value },
        type: "POST",
        success: function (response) {
      
            if (response.IsSuccess) {
                calculatePayableAmount(response.Data.PurchaseAdjustedAmount);

                hiddenPurchaseId.value = response.Data.PurchaseId;
                inputPurchaseAmount.value = response.Data.PurchaseAdjustedAmount;
                inputPurchaseAmount.previousElementSibling.classList.add('active');

                inputPurchaseDescription.value = response.Data.PurchaseDescription;
                inputPurchaseDescription.previousElementSibling.classList.add('active');
                return;
            }

            $.notify(response.Message, response.IsSuccess ? "success" : "error");
        },
        error: function (error) {
            console.log(error);
        }
    });
})

//purchase paid amount change input
inputPurchaseAmount.addEventListener("input", function () {
    calculatePayableAmount();
})

//re calculate payable amount after input purchase paid amount
function calculatePayableAmount(paidAmount) {
    const total = +totalPrice.textContent;
    const purchasePaid = paidAmount ? paidAmount: +inputPurchaseAmount.value;

    const grandTotal = (total - purchasePaid);

    totalPayable.innerText = grandTotal.toFixed();
    totalDue.innerText = grandTotal.toFixed();

    inputDiscount.value = '';
    inputPaid.value = '';
}
//*** End Purchase from customer ****


//compare Validation
const validInput = function (total, inputted) {
    return (total < inputted) ? false : true;
}

//input discount amount
const onInputDiscount = function () {
    const total = +totalPrice.textContent;
    const purchaseAmount = +inputPurchaseAmount.value;
    const discount = +this.value;
    const isValid = validInput(total, discount);
    const grandTotal = (total - purchaseAmount - discount);

    this.setAttribute('max', total);

    totalPayable.innerText = isValid ? grandTotal.toFixed() : total;
    totalDue.innerText = isValid ? grandTotal.toFixed() : total;

    if (inputPaid.value) {
        inputPaid.value = '';
        inputPaid.previousElementSibling.classList.remove('active');
    }

    //check due limit 
    checkDueLimit();
}

//input paid amount
const onInputPaid = function () {
    const payable = +totalPayable.textContent;
    const paid = +this.value;
    const isValid = validInput(payable, paid);
    const due = (payable - paid);

    paid ? selectPaymentMethod.setAttribute("required", true) : selectPaymentMethod.removeAttribute("required");

    this.setAttribute("max", payable);

    totalDue.innerText = isValid ? due.toFixed() : payable;

    if (due < 1) {
        inputPromisedDate.value = "";
        inputPromisedDate.disabled = true;
    } else {
        inputPromisedDate.disabled = false;
    }

    //check due limit 
    checkDueLimit();
}


//Account Transaction Charge
const onAccountTransactionCharge = function() {
    appendTotalPrice();
}

//input Service Charge
inputServiceCharge.addEventListener("input", function() {
    appendTotalPrice();
});

//reset customer Id
hiddenCustomerId.value = "";

//validation info
const validation = function () {
    customerError.innerText = '';

    if (cartProducts.length || inputServiceCharge.value) {
       
    } else {
        customerError.innerText = 'Add product or add service!';
        return false;
    }

    if (!hiddenCustomerId.value) {
        customerError.innerText = 'Select or add customer!';
        return false;
    }

    const due = +totalDue.innerText;
    if (due < 0) {
        customerError.innerText = 'Due Amount No More Than 0!';
        return false;
    }

    if (!checkDueLimit()) {
        return false
    }

    return true;
}

const onCheckFormValid = function (evt) {
    evt.preventDefault()
    formTable.btnProduct.click()
}

//submit on server
const onSellSubmitClicked = function (evt) {
    evt.preventDefault()

    const valid = validation()
    if (!valid) return;

    const productList = [];

    cartProducts.forEach(product => {
        const { ProductId, SellingPrice, PurchasePrice,Description, Warranty, codes } = product;
        productList.push({ ProductId, SellingPrice, PurchasePrice, Description, Warranty, ProductCodes: codes });
    })

    const body = {
        CustomerId: +hiddenCustomerId.value,
        SellingTotalPrice: sellingTotalPrice(),
        SellingDiscountAmount: +inputDiscount.value,
        SellingPaidAmount: +inputPaid.value,

        PromisedPaymentDate: inputPromisedDate.value,
        ServiceCharge: +inputServiceCharge.value,
        ServiceChargeDescription: inputServiceChargeDescription.value,
        ServiceCost: +inputServiceCost.value,
        Expense: +inputExpense.value,
        ExpenseDescription: inputExpenseDescription.value,
        AccountId: inputPaid.value ? selectPaymentMethod.value : "",
        AccountTransactionCharge: +inputAccountTransactionCharge.value,
        ProductList: productList,

        PurchaseAdjustedAmount: +inputPurchaseAmount.value,
        PurchaseDescription: inputPurchaseDescription.value,
        PurchaseId: hiddenPurchaseId.value
    }

    //disable button on submit
    const btnSubmit = formPayment.btnSelling
    btnSubmit.innerText = "submitting.."
    btnSubmit.disabled = true

    $.ajax({
        url: "/Selling/Selling",
        type: "POST",
        data: body,
        success: function (response) {
            if (response.IsSuccess) {
                localStoreClear();
                location.href = `/Selling/SellingReceipt/${response.Data}`;
                return;
            }

            $.notify(response.Message, response.IsSuccess ? "success" : "error");
            btnSubmit.innerText = "Sell Product";
            btnSubmit.disabled = false;
        },
        error: function (error) {
            console.log(error);
            btnSubmit.innerText = "Sell Product";
            btnSubmit.disabled = false;
        }
    });
}

//event listener
formPayment.addEventListener("submit", onCheckFormValid);
formTable.addEventListener("submit", onSellSubmitClicked);
inputDiscount.addEventListener("input", onInputDiscount);
inputPaid.addEventListener("input", onInputPaid);
inputAccountTransactionCharge.addEventListener("input", onAccountTransactionCharge);

//****CUSTOMER****//
//customer autocomplete
$('#inputCustomer').typeahead({
    minLength: 1,
    displayText: function (item) {
        return `${item.CustomerName} ${item.PhonePrimary ? item.PhonePrimary: ''} ${item.OrganizationName ? item.OrganizationName: ''}`;
    },
    afterSelect: function (item) {
        this.$element[0].value = item.CustomerName
    },
    source: function (request, result) {
        $.ajax({
            url: "/Selling/FindCustomers",
            data: { prefix: request },
            success: function (response) { result(response); },
            error: function (err) { console.log(err) }
        });
    },
    updater: function (item) {
        appendInfo(item);

        hiddenCustomerId.value = item.CustomerId;
        customerError.innerText = '';

        checkDueLimit()
        return item;
    }
})

function appendInfo(item) {
    const name = item.IsIndividual ? item.CustomerName : item.OrganizationName;

    const html = `<h5 class="badge badge-pill badge-success">${name}</h5>
        <span class="badge badge-pill badge-danger">Previous Due: ৳<span id="prevDue">${item.Due}</span></span>
        <span class="badge badge-pill badge-info">Due Limit: ৳<span id="dueLimit">${item.DueLimit}</span></span>`;

    document.getElementById('customerInfo').innerHTML = html;
}

//check customer due limit
function checkDueLimit() {
    const infoContainer = formPayment.querySelector('#customerInfo');
    if (!infoContainer.innerHTML) return;

    const prevDue = +formPayment.querySelector('#prevDue').textContent || 0;
    const currentDue = +totalDue.textContent
    const due = prevDue + currentDue;

    customerError.innerText = '';
    const dueLimit = +formPayment.querySelector('#dueLimit').textContent || 0;

    if (due > dueLimit) {
        customerError.innerText = `Customer Due limit ${dueLimit}/-`;
        return false;
    }

    return true;
}

//remove localstorage
function localStoreClear() {
    localStorage.removeItem("selling-cart");
}
