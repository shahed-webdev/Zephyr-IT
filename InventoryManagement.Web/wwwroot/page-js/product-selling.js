﻿
 // material select initialization
$('.mdb-select').materialSelect()

// global storage
let cartProducts = []


//*****SELECTORS*****/
// product code form
const formCode = document.getElementById('formCode')
const inputBarCode = formCode.inputBarCode
const productCount = formCode.querySelector('#productCount')
const codeExistError = formCode.querySelector('#codeExistError')
const btnFind = formCode.btnFind

// product table
const formTable = document.getElementById('formTable')
const tbody = document.getElementById('t-body')

//payment selectors
const formPayment = document.getElementById('formPayment')
const totalPrice = formPayment.querySelector('#totalPrice')
const inputDiscount = formPayment.inputDiscount
const totalPayable = formPayment.querySelector('#totalPayable')
const inputPaid = formPayment.inputPaid
const totalDue = formPayment.querySelector('#totalDue')
const selectPaymentMethod = formPayment.selectPaymentMethod

//customer
const hiddenCustomerId = formPayment.hiddenCustomerId
const customerError = formPayment.querySelector('#customer-error')

// get set storage object
const storage = {
    saveData: function (product) {
        codeExistError.textContent = ''
        if (!product) {
            codeExistError.textContent = `"${inputBarCode.value}" Not found!`
            return
        }
       
        const found = cartProducts.some(el => el.ProductCatalogId === product.ProductCatalogId && el.ProductName === product.ProductName);
        if (!found) {
            //save to global object
            product.codes = [product.ProductCode]
            product.sellingFixedValue = product.SellingPrice
            cartProducts.push(product)
        }
        else {
            const index = cartProducts.findIndex(item => item.ProductCatalogId === product.ProductCatalogId)
            const codes = cartProducts[index].codes

            if (codes.indexOf(product.ProductCode) === -1) {
                codes.push(product.ProductCode)
            }
            else {
                codeExistError.textContent = `"${inputBarCode.value}" code already added!`
            }
        }

        //clear input
        inputBarCode.value = ''

        //save to local-storage
        this.setData()

        //show table data
        tbody.innerHTML =''
        showProducts()
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
// scan barcode
onScan.attachTo(document, {
    suffixKeyCodes: [13],
    reactToPaste: true,
    onScan: (sCode, iQty) => {
        inputBarCode.value = sCode
        inputBarCode.nextElementSibling.classList.add('active')
        btnFind.click()
    }
})

//append code
const appendCode = function (codes) {
   let html = ''
    codes.forEach(code => html += `<span class="code">${code}</span>`)
    return html
}

// create table rows
const createTableRow = function (item) {
    const description = item.Description && `${item.Description},`
    const note = item.Note && `<span style="font-size: 12px;" class="badge badge-pill badge-secondary">${item.Note}</span>`

    return `<tr data-id="${item.ProductCatalogId}">
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
                <td>${item.SellingPrice * item.codes.length}</td>
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
    const id = +code.parentElement.parentElement.parentElement.getAttribute('data-id')
    const pCode = code.textContent

    const index = cartProducts.findIndex(item => item.ProductCatalogId === id)
    const codes = cartProducts[index].codes
    const pIndex = codes.indexOf(pCode)

    if (codes.length > 1) {
        codes.splice(pIndex, 1)

        //save to local-storage
        storage.setData()

        //remove code element
        code.remove()

        showProducts()
    }
}

// click remove or stock
const onRemoveClicked = function (evt) {
    const element = evt.target;
    const removeClicked = element.classList.contains('remove');
    const codeClicked = element.classList.contains('code');
    const row = element.parentElement.parentElement;
    const id = +row.getAttribute('data-id');

    if (codeClicked) removeProductCode(element)

    if (!removeClicked) return;

    //remove product from storage
    cartProducts = cartProducts.filter(item => item.ProductCatalogId !== id);

    //save to local storage
    storage.setData() 

    //delete row
    row.remove()

    //show added items count
    storage.countProduct()

    //append price
    appendTotalPrice()
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

//calculate purchase Total
const purchaseTotalPrice = function () {
    return cartProducts.map(item => item.SellingPrice * item.codes.length).reduce((prev, cur) => prev + cur, 0);
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

//selling price change
const onInputUnitPrice = function (evt) {
    const input = evt.target
    const onInput = input.classList.contains('inputUnitPrice')
    if (onInput) {
        const val = +input.value
       const min = +input.getAttribute('min')
        input.setAttribute('max', input.value)

        if (min > val) return

        const id = +input.parentElement.parentElement.getAttribute('data-id')
        const index = cartProducts.findIndex(item => item.ProductCatalogId === id)
        cartProducts[index].SellingPrice = +input.value

        const qty = +input.parentElement.previousElementSibling.innerText
        input.parentElement.nextElementSibling.innerText = val * qty

        //save to local-storage
        storage.setData()

        //append price
        appendTotalPrice()
    }
}

//selling price click
formTable.addEventListener('input', onInputUnitPrice)


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
tbody.addEventListener('click', onRemoveClicked)


//*****CALL FUNCTION*****//
storage.getData()
showProducts()


//****PAYMENT SECTION****/

//functions
//compare Validation
const validInput = function (total, inputted) {
    return (total < inputted) ? false : true;
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

    //check due limit 
    checkDueLimit();
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

    //check due limit 
    checkDueLimit();
}

//reset customer Id
hiddenCustomerId.value = '';

//validation info
const validation = function () {
    customerError.innerText = ''

    if (!cartProducts.length) {
        customerError.innerText = 'Add product to sell!'
        return false;
    }

    if (!hiddenCustomerId.value) {
        customerError.innerText = 'Select or add customer!'
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

    //disable button on submit
    const btnSubmit = formPayment.btnSelling
    btnSubmit.innerText = 'submitting..'
    btnSubmit.disabled = true

    const productCodes = []
    const productList = []

    cartProducts.forEach(product => {
        const { ProductId, SellingPrice, Description, Warranty, codes} = product
        productCodes.push(...codes)
        productList.push({ ProductId, SellingPrice, Description, Warranty})
    })

    if (!productCodes.length) return

    const body = {
        CustomerId: +hiddenCustomerId.value,
        SellingTotalPrice: +totalPrice.textContent,
        SellingDiscountAmount: +inputDiscount.value | 0,
        SellingPaidAmount: +inputPaid.value | 0,
        PaymentMethod: inputPaid.value ? selectPaymentMethod.value : '',
        SellingDate: new Date(),
        ProductCodes: productCodes,
        ProductList: productList
    }

    const url = '/Selling/Selling'
    const options = {
        method: 'post',
        url: url,
        data: body
    }

    axios(options)
        .then(response => {
            if (response.data.IsSuccess) {
                localStoreClear()
                location.href = `/Selling/SellingReceipt/${response.data.Data}`
            }
        })
        .catch(error => {
            if (error.response)
                customerError.textContent = error.response.data.Message
            else if (error.request)
                console.log(error.request)
            else
                console.log('Error', error.message)
        })
        .finally(() => {
            btnSubmit.innerText = 'Sell Product'
            btnSubmit.disabled = false
        });
}

//event listener
formPayment.addEventListener('submit', onCheckFormValid)
formTable.addEventListener('submit', onSellSubmitClicked)
inputDiscount.addEventListener('input', onInputDiscount)
inputPaid.addEventListener('input', onInputPaid)

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
        customerError.innerText = ''
        checkDueLimit()
        return item;
    }
})

function appendInfo(item) {
    const html = `<span class="badge badge-pill badge-success">${item.CustomerName}</span>
        <span class="badge badge-pill badge-danger">Previous Due: ৳<span id="prevDue">${item.Due}</span></span>
        <span class="badge badge-pill badge-info">Due Limit: ৳<span id="dueLimit">${item.DueLimit}</span></span>`;

    document.getElementById('customerInfo').innerHTML = html;
}

//check customer due limit
function checkDueLimit() {
    const infoContainer = formPayment.querySelector('#customerInfo')
    if (!infoContainer.innerHTML) return;

    const prevDue = +formPayment.querySelector('#prevDue').textContent || 0
    const currentDue = +totalDue.textContent
    const dueLimit = +formPayment.querySelector('#dueLimit').textContent || 0

    if (dueLimit === 0) return true;

    const due = prevDue + currentDue;

    customerError.innerText = ''
    if (due > dueLimit) {
        customerError.innerText = 'Current due greater than due limit!';
        return false
    }

    return true  
}

//remove localstorage
function localStoreClear() {
    localStorage.removeItem('selling-cart');
}
