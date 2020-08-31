
$(function() {
    // material select initialization
    $('.mdb-select').materialSelect();
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

//payment selectors
 const formPayment = document.getElementById('formPayment');
const totalPrice = formPayment.querySelector('#totalPrice');
const inputDiscount = formPayment.inputDiscount;
const inputReturnAmount = formPayment.inputReturnAmount;
const totalPrevPaid = formPayment.querySelector('#totalPrevPaid');
const totalDue = formPayment.querySelector('#totalDue');
const inputPaid = formPayment.inputPaid;
const selectPaymentMethod = formPayment.selectPaymentMethod;

//customer
 const hiddenSellingId = formPayment.hiddenSellingId;
 const customerError = formPayment.querySelector('#customer-error');

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
            return (obj.ProductCatalogId === product.ProductCatalogId && obj.ProductName === product.ProductName);
        });

        if (!found) {
            product.codes = [{ code: product.ProductCode, isRemove: false }];
            product.sellingFixedValue = product.SellingPrice;
            product.sellingQuantity = 1;
            product.SellingListId = 0;
            cartProducts.push(product);
        }
        else {
            const index = cartProducts.findIndex(item => item.ProductCatalogId === product.ProductCatalogId && item.ProductName === product.ProductName);
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

    return `<tr data-id="${item.ProductCatalogId}" data-name="${item.ProductName}" class="${disable}">
                <td>${item.ProductCatalogName}</td>
                <td>
                    ${item.ProductName},
                    ${description}
                    ${note}
                    <span class="codeSpan">${appendCode(item.codes)}</span>
                </td>
                <td>${item.Warranty}</td>
                <td>${item.sellingQuantity}</td>
                <td><input type="number" required class="form-control inputUnitPrice" step="0.01" min="${item.sellingFixedValue}" max="${item.SellingPrice}" value="${item.SellingPrice}" /></td>
                <td>${item.SellingPrice * item.sellingQuantity}</td>
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

// click remove or stock
const onRemoveClicked = function(evt) {
    const element = evt.target;
    const row = element.parentElement.parentElement.parentElement;
    const id = +row.getAttribute('data-id');
    const name = row.getAttribute('data-name');

    //remove code
    const codeClicked = element.classList.contains('code');

    if (!codeClicked) return;

    const pCode = element.textContent;
    const index = cartProducts.findIndex(item => item.ProductCatalogId === id && item.ProductName === name);
    const codes = cartProducts[index].codes;

    codes.forEach((obj, i) => {
        if (obj.code === pCode) {
            codes[i].isRemove = !codes[i].isRemove;
            return;
        }
    });

    cartProducts[index].sellingQuantity = codes.filter(itm => !itm.isRemove).length;
    showProducts();
}

//show loading
const loading = function (element, isLoading) {
    element.children[0].style.display = isLoading ? "none" : "inline-block";
    element.children[1].style.display = isLoading ? "inline-block" : "none";
    element.disabled = isLoading ? true : false;
}


//append total price to DOM
const appendTotalPrice = function () {
    const totalAmount = cartProducts.map(item => {
        return item.SellingPrice * item.sellingQuantity;
    }).reduce((prev, cur) => prev + cur, 0);

    totalPrice.innerText = totalAmount;

    const discount = +inputDiscount.value;
    const prevPaid = +totalPrevPaid.textContent;
    const returnAmount = +inputReturnAmount.value;

    const sum = totalAmount - (discount + prevPaid + returnAmount);

    totalDue.textContent = sum;

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

        const index = cartProducts.findIndex(item => item.ProductCatalogId === id && item.ProductName === name);
        cartProducts[index].SellingPrice = +input.value

        const qty = +input.parentElement.previousElementSibling.innerText;
        input.parentElement.nextElementSibling.innerText = val * qty;

        //append price
        appendTotalPrice();
    }
}

//selling price click
formTable.addEventListener('input', onInputUnitPrice);


// onProduct code submit
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

// remove product click
tbody.addEventListener('click', onRemoveClicked);


//****PAYMENT SECTION****/

//functions
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
}

//input paid amount
const onInputPaid = function () {
    const paid = +this.value;
    paid ? selectPaymentMethod.setAttribute('required', true) : selectPaymentMethod.removeAttribute('required');
}


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

        paid ? selectPaymentMethod.setAttribute('required', true) : selectPaymentMethod.removeAttribute('required');
    }
}

//validation info
const validation = function () {
    customerError.innerText = ''

    if (!cartProducts.length) {
        customerError.innerText = 'Product Not found!'
        return false;
    }

    const due = +totalDue.textContent;
    if (due < 0) {
        customerError.innerText = "Due Amount must be more than or equal 0!"
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

    //disable button on submit
    const btnSubmit = formPayment.btnSelling
    btnSubmit.innerText = 'submitting..'
    btnSubmit.disabled = true

    //added product without quantity
    const products = cartProducts.filter(item => item.sellingQuantity);

    //get remove and new code
    const addedProductCodes = [];
    const removedProductCodes = [];

    cartProducts.forEach(obj => {
        obj.codes.forEach(itm => {
            if (obj.hasOwnProperty("ProductCodes")) {
                if (obj.ProductCodes.indexOf(itm.code) !== -1) {
                    if (itm.isRemove)
                        removedProductCodes.push(itm.code);
                } else {
                    if (!itm.isRemove)
                        addedProductCodes.push(itm.code);
                }
            } else {
                if (!itm.isRemove)
                    addedProductCodes.push(itm.code);
            }
        })
    })

    const body = {
        SellingId: +hiddenSellingId.value,
        SellingTotalPrice: +totalPrice.textContent,
        SellingDiscountAmount: +inputDiscount.value || 0,
        SellingReturnAmount: +inputReturnAmount.value || 0,
        PaidAmount: +inputPaid.value,
        PaymentMethod: inputPaid.value ? selectPaymentMethod.value : '',
        AddedProductCodes: addedProductCodes,
        RemovedProductCodes: removedProductCodes,
        PaidDate: new Date(),
        Products: products
    }


    const url = '/Selling/BillChange'
    const options = {
        method: 'post',
        url: url,
        data: body
    }

    axios(options).then(response => {
        location.href = `/Selling/SellingReceipt/${response.data}`
    }).catch(error => {
        if (error.response)
            customerError.textContent = error.response.data.Message
        else if (error.request)
            console.log(error.request)
        else
            console.log('Error', error.message)
    }).finally(() => {
        btnSubmit.innerText = 'Sell Product'
        btnSubmit.disabled = false
    });
}

//event listener
formPayment.addEventListener('submit', onCheckFormValid)
formTable.addEventListener('submit', onSellSubmitClicked)
inputDiscount.addEventListener('input', onInputDiscount)
inputPaid.addEventListener('input', onInputPaid)
inputReturnAmount.addEventListener('input', onInputReturnAmount)

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