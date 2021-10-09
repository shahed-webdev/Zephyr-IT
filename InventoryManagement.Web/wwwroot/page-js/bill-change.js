
$(function() {
    // material select initialization
    $('.mdb-select').materialSelect();

    $('.datepicker').pickadate({
        format: 'd-mmmm-yyyy'
        //min: new Date()
    });
});

// global storage
 let cartProducts = [];


//*****SELECTORS*****/
// product code form
const formCode = document.getElementById('formCode');
const inputBarCode = formCode.inputBarCode;
const codeExistError = formCode.querySelector('#codeExistError');
const btnFind = formCode.btnFind;

// product table
const formTable = document.getElementById('formTable');
const tbody = document.getElementById('t-body');
const productTotalPrice = document.getElementById('productTotalPrice')

//payment selectors
const formPayment = document.getElementById('formPayment');
const prevTransactionCost = formPayment.querySelector('#prevTransactionCost');
const totalPrice = formPayment.querySelector('#totalPrice');
const inputDiscount = formPayment.inputDiscount;
const inputReturnAmount = formPayment.inputReturnAmount;
const totalPrevPaid = formPayment.querySelector('#totalPrevPaid');
const totalDue = formPayment.querySelector('#totalDue');
const inputPaid = formPayment.inputPaid;
const selectPaymentMethod = formPayment.selectPaymentMethod;
const inputAccountTransactionCharge = formPayment.inputAccountTransactionCharge;
const inputPromisedDate = formPayment.inputPromisedDate;

//service charge
const inputServiceCharge = formPayment.inputServiceCharge
const inputServiceChargeDescription = formPayment.inputServiceChargeDescription
const inputServiceCost = formPayment.inputServiceCost

//customer
 const hiddenSellingId = formPayment.hiddenSellingId;
 const customerError = formPayment.querySelector('#customer-error');

//purchase from customer
 const findPurchaseBill = formPayment.findPurchaseBill
 const hiddenPurchaseId = formPayment.hiddenPurchaseId
 const inputPurchaseBillNo = formPayment.inputPurchaseBillNo
 const inputPurchaseAmount = formPayment.inputPurchaseAmount
 const inputPurchaseDescription = formPayment.inputPurchaseDescription

//formReceiptDelete
formReceiptDelete = document.getElementById('formReceiptDelete');

// get set storage object
const storage = {
    saveData: function (product) {
        codeExistError.textContent = ''
        if (!product) {
            codeExistError.textContent = `"${inputBarCode.value}" Not found!`
            return
        }

        //check product added or not
        const found = cartProducts.some(function(obj) {
            return (obj.ProductCatalogId === product.ProductCatalogId && obj.ProductName === product.ProductName && obj.PurchasePrice === product.PurchasePrice);
        });

        if (!found) {
            product.codes = [{ code: product.ProductCode, isRemove: false }];
            product.RemainCodes = [];
            product.sellingFixedValue = product.SellingPrice;
            product.sellingQuantity = 1;
            product.SellingListId = 0;

            cartProducts.unshift(product);
        }
        else {
            const index = cartProducts.findIndex(item => item.ProductCatalogId === product.ProductCatalogId && item.ProductName === product.ProductName && item.PurchasePrice === product.PurchasePrice);
            const codes = cartProducts[index].codes

            //check code added or not
            const codeFound = codes.some(obj => obj.code === product.ProductCode);

            if (codeFound) {
                codeExistError.textContent = `"${inputBarCode.value}" code already added!`;
                return;
            }

            codes.push({ code: product.ProductCode, isRemove: false });
            cartProducts[index].sellingQuantity = codes.filter(itm => !itm.isRemove).length;
        }


        //clear input
        inputBarCode.value = '';

        //show table data
        tbody.innerHTML = '';
        showProducts();
    }
}


//****FUNCTIONS****//
//append code
const appendCode = function(codes) {
    let html = ''
    codes.forEach(code => {
        const remove = code.isRemove ? "code-removed" : "";
        html += `<span class="code ${remove}">${code.code}</span>`
    })

    return html;
}

//dropdown selected index 0
const clearMDBdropDownList = function(mainSelector) {
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

// create table rows
const createTableRow = function (item) {
    const description = item.Description? `${item.Description},`:''
    const note = item.Note ? `<span style="font-size: 12px;" class="badge badge-pill badge-secondary">${item.Note}</span>` : "";
    const disable = item.sellingQuantity ? "" : "disable-row";

    return `<tr data-id="${item.ProductCatalogId}" data-name="${item.ProductName}" data-pprice="${item.PurchasePrice}" class="${disable}">
                <td>${item.ProductCatalogName}</td>
                <td>
                    ${item.ProductName},
                    ${description}
                    ${note}
                    <span class="codeSpan">${appendCode(item.codes)}</span>
                </td>
                <td>${item.Warranty}</td>
                <td>${item.sellingQuantity}</td>
                <td><input type="number" required class="form-control inputUnitPrice" step="0.01" min="${item.sellingFixedValue}" max="${item.SellingPrice}" value="${item.SellingPrice.toFixed(2)}" /></td>
                <td>${(item.SellingPrice * item.sellingQuantity).toFixed(2)}</td>
            </tr>`
}

// create Table on load
const showProducts = function () {
    let table = ''
    cartProducts.forEach(item => {
        table += createTableRow(item);
    });

    tbody.innerHTML = table;

    //append price
    appendTotalPrice();
}


//show loading
const loading = function (element, isLoading) {
    element.children[0].style.display = isLoading ? "none" : "inline-block";
    element.children[1].style.display = isLoading ? "inline-block" : "none";
    element.disabled = isLoading ? true : false;
}

//calculate selling Total
const sellingTotalPrice = function () {
    return cartProducts.map(item => item.SellingPrice * item.sellingQuantity).reduce((prev, cur) => prev + cur, 0);
}

//append total price to DOM
const appendTotalPrice = function () {
    const productPrice = sellingTotalPrice();
    const serviceCharge = +inputServiceCharge.value;

    const prevTransactionAmount = prevTransactionCost? +prevTransactionCost.textContent:0;
    const accountTransactionCharge = +inputAccountTransactionCharge.value;

    const totalAmount = productPrice + serviceCharge + accountTransactionCharge + prevTransactionAmount;


    //set max discount limit
    inputDiscount.setAttribute("max", totalAmount);

    productTotalPrice.innerText = productPrice ? `Total: ${productPrice.toFixed(2)}` : "";
    totalPrice.innerText = totalAmount.toFixed(2);

    const purchaseAmount = +inputPurchaseAmount.value;
    const discount = +inputDiscount.value;
    const prevPaid = +totalPrevPaid.textContent;
    const returnAmount = +inputReturnAmount.value;

    const sum = totalAmount - (discount + prevPaid + returnAmount + purchaseAmount);

    totalDue.textContent = sum.toFixed(2);

    disablePaid(sum);

    //for reset paid amount and method
    if (inputPaid.value) {
        inputPaid.value = '';
        inputPaid.previousElementSibling.classList.remove('active');
    }

    if (selectPaymentMethod.selectedIndex > 0) {
        clearMDBdropDownList(formPayment);
        selectPaymentMethod.removeAttribute('required');
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
        cartProducts[index].SellingPrice = +input.value

        const qty = +input.parentElement.previousElementSibling.innerText;
        input.parentElement.nextElementSibling.innerText = val * qty;

        //append price
        appendTotalPrice();
    }
}

//selling price click
formTable.addEventListener('input', onInputUnitPrice);


// on Product code submit
formCode.addEventListener('submit', evt => {
    evt.preventDefault()
    const url = '/Selling/FindProductByCode'
    const param = { params: { code: inputBarCode.value } };

    loading(btnFind,true)

    axios.get(url, param)
        .then(res => storage.saveData(res.data))
        .catch(err => console.log(err))
        .finally(() => loading(btnFind, false))
})


// remove product code click
tbody.addEventListener('click', function (evt) {
    const element = evt.target;
    const row = element.parentElement.parentElement.parentElement;
    const id = +row.getAttribute('data-id');
    const name = row.getAttribute('data-name');
    const purchasePrice = +row.getAttribute('data-pprice');

    //remove code
    const codeClicked = element.classList.contains('code');

    if (!codeClicked) return;
    const pCode = element.textContent;

    $.ajax({
        url: '/Selling/ProductIsInStock',
        type: "POST",
        data: { code: pCode },
        success: function (response) {
            if (response) {
                $.notify("Product Already In Stock", "error");
                return;
            }

            const index = cartProducts.findIndex(item => item.ProductCatalogId === id && item.ProductName === name && item.PurchasePrice === purchasePrice);
            const codes = cartProducts[index].codes;

            codes.forEach((obj, i) => {
                if (obj.code === pCode) {
                    codes[i].isRemove = !codes[i].isRemove;
                    return;
                }
            });

            cartProducts[index].sellingQuantity = codes.filter(itm => !itm.isRemove).length;
            showProducts();
        },
        error: function (error) {
            console.log(error);
        }
    });
});


//**** PAYMENT SECTION ****/

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
                //calculatePayableAmount(response.Data.PurchaseAdjustedAmount);

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
    //calculatePayableAmount();
    appendTotalPrice();
})
//*** End Purchase from customer ****


//compare Validation
const validInput = function (total, inputted) {
    return (total < inputted) ? false : true;
}


//input discount amount
const onInputDiscount = function () {
    const totalAmount = +totalPrice.textContent;
    const discount = +this.value;
    const prevPaid = +totalPrevPaid.textContent;
    const returnAmount = +inputReturnAmount.value;

    const sum = (totalAmount + returnAmount) - (discount + prevPaid);

    totalDue.innerText = sum;

    disablePaid(sum);
}

//input return amount
const onInputReturnAmount = function () {
    const totalAmount = +totalPrice.textContent;
    const discount = +inputDiscount.value;
    const prevPaid = +totalPrevPaid.textContent;
    const returnAmount = +this.value;

    const sum = (totalAmount + returnAmount) - (discount + prevPaid);

    totalDue.innerText = sum;

    disablePaid(sum);
    setAccountRequired();
}

//input paid amount
const onInputPaid = function () {
    setAccountRequired();
}


//enabled/disabled paid and account
function disablePaid(dueAmount) {
    if (dueAmount > 0) {
        inputPaid.removeAttribute('disabled');
        selectPaymentMethod.removeAttribute('disabled');
        inputPaid.setAttribute('max', dueAmount);
    } else {
        inputPaid.setAttribute('disabled', 'disabled');
        selectPaymentMethod.setAttribute('disabled', 'disabled');

        const paid = +inputPaid.value;

        if (paid) {
            inputPaid.value = '';
            inputPaid.previousElementSibling.classList.remove('active');
        }
    }
}


//set account dropdown required
function setAccountRequired() {
    const paid = +inputPaid.value;
    const returnAmount = +inputReturnAmount.value;

    paid || returnAmount ? selectPaymentMethod.setAttribute('required', true) : selectPaymentMethod.removeAttribute('required');
}


//Account Transaction Charge
const onAccountTransactionCharge = function () {
    appendTotalPrice();
}

//input Service Charge
inputServiceCharge.addEventListener("input", function () {
    appendTotalPrice();
});

//validation info
const validation = function () {
    customerError.innerText = ''

    if (cartProducts.length || inputServiceCharge.value) {

    } else {
        customerError.innerText = 'Add product or add service!';
        return false;
    }

    const due = +totalDue.textContent;
    if (due < 0) {
        customerError.innerText = "Due Amount must be more than or equal 0"
        return false;
    }

    return true;
}

const onCheckFormValid = function (evt) {
    evt.preventDefault()
    formTable.btnProduct.click()
}

//submit on server
const onSellSubmitClicked = function(evt) {
    evt.preventDefault()

    const valid = validation()
    if (!valid) return;


    //get remove and new code
    const addedProductCodes = [];
    const removedProductCodes = [];

    cartProducts.forEach(obj => {
        obj.codes.forEach(itm => {
            if (!itm.isRemove) {
                if (obj.RemainCodes.indexOf(itm.code) === -1) {
                    obj.RemainCodes.push(itm.code);
                }
            }

            if (obj.hasOwnProperty("ProductCodes")) {
                if (obj.ProductCodes.indexOf(itm.code) !== -1) {
                    if (itm.isRemove) {
                        removedProductCodes.push(itm.code);
                    }
                } else {
                    if (!itm.isRemove) {
                        addedProductCodes.push(itm.code);
                    }
                }
            } else {
                if (!itm.isRemove) {
                    addedProductCodes.push(itm.code);
                }
            }
        })
    })

    //added product without quantity
    const products = cartProducts.filter(item => item.sellingQuantity);

    const body = {
        SellingId: +hiddenSellingId.value,
        SellingTotalPrice: sellingTotalPrice(),
        SellingDiscountAmount: +inputDiscount.value || 0,
        SellingReturnAmount: +inputReturnAmount.value || 0,
        PaidAmount: +inputPaid.value || 0,
        AddedProductCodes: addedProductCodes,
        RemovedProductCodes: removedProductCodes,
        Products: products,

        AccountId: inputPaid.value ? selectPaymentMethod.value : '',
        AccountTransactionCharge: +inputAccountTransactionCharge.value,
        PromisedPaymentDate: inputPromisedDate.value,
        ServiceCharge: inputServiceCharge.value,
        ServiceChargeDescription: inputServiceChargeDescription.value,
        ServiceCost: inputServiceCost.value,

        PurchaseAdjustedAmount: inputPurchaseAmount.value,
        PurchaseDescription: inputPurchaseDescription.value,
        PurchaseId: hiddenPurchaseId.value
    }

    //disable button on submit
    const btnSubmit = formPayment.btnSelling;
    btnSubmit.innerText = 'submitting..';
    btnSubmit.disabled = true;

    $.ajax({
        url: '/Selling/BillChange',
        type: "POST",
        data: body,
        success: function(response) {
            if (response.IsSuccess) {
                location.href = `/Selling/SellingReceipt/${response.Data}`;
                return;
            }

            $.notify(response.Message, response.IsSuccess ? "success" : "error");
        },
        error: function(error) {
            console.log(error);
            btnSubmit.innerText = 'Update Bill';
            btnSubmit.disabled = false;
        }
    });
}

//event listener
formPayment.addEventListener('submit', onCheckFormValid)
formTable.addEventListener('submit', onSellSubmitClicked)
inputDiscount.addEventListener('input', onInputDiscount)
inputPaid.addEventListener('input', onInputPaid)
inputReturnAmount.addEventListener('input', onInputReturnAmount)
inputAccountTransactionCharge.addEventListener("input", onAccountTransactionCharge);

//delete receipt
formReceiptDelete.addEventListener('submit', function(evt) {
    evt.preventDefault();
    const error = document.getElementById('delete-error');
    const id = +hiddenSellingId.value;

    error.textContent = '';

    if (confirm('This receipt will be deleted permanently')) {
        $.ajax({
            type: "POST",
            url: `/Selling/DeleteBill/${id}`,
            success: function () {
                location.href = '/Selling/BillList';
            },
            error: function (response) {
                console.log(response);
                error.textContent = response.responseText;
            }
        });  
    }
});