
 // material select initialization
$('.mdb-select').materialSelect()

// global storage
let cartProducts = []


//*****SELECTORS*****/
// product code form
const formCode = document.getElementById('formCode')
const inputBarCode = formCode.inputBarCode
const codeExistError = formCode.querySelector('#codeExistError')
const btnFind = formCode.btnFind

// product table
const formTable = document.getElementById('formTable')
const tbody = document.getElementById('t-body')

//payment selectors
const formPayment = document.getElementById('formPayment')
const totalPrice = formPayment.querySelector('#totalPrice')
const inputDiscount = formPayment.inputDiscount
const inputReturnAmount = formPayment.inputReturnAmount
const totalPayable = formPayment.querySelector('#totalPayable')
const totalDue = formPayment.querySelector('#totalDue')

//customer
 const hiddenSellingId = formPayment.hiddenSellingId;
const customerError = formPayment.querySelector('#customer-error')

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
            product.sellingFixedValue = product.SellingPrice
            cartProducts.push(product)
        }
        else {
            const index = cartProducts.findIndex(item => item.ProductCatalogId === product.ProductCatalogId)
            const codes = cartProducts[index].codes

            //check code added or not
            const codeFound = codes.some(obj => obj.code === product.ProductCode);

            if (codeFound) {
                codeExistError.textContent = `"${inputBarCode.value}" code already added!`;
                return;
            }

            codes.push({ code: product.ProductCode, isRemove: false })
        }


        //clear input
        inputBarCode.value = ''

        //show table data
        tbody.innerHTML =''
        showProducts()
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

// create table rows
const createTableRow = function (item) {
    const description = item.Description && `${item.Description},`
    const note = item.Note ? `<span style="font-size: 12px;" class="badge badge-pill badge-secondary">${item.Note}</span>` : "";
    const quantity = item.codes.filter(itm => !itm.isRemove).length;

    return `<tr data-id="${item.ProductCatalogId}">
                <td>${item.ProductCatalogName}</td>
                <td>
                    ${item.ProductName},
                    ${description}
                    ${note}
                    <span class="codeSpan">${appendCode(item.codes)}</span>
                </td>
                <td>${item.Warranty}</td>
                <td>${quantity}</td>
                <td><input type="number" required class="form-control inputUnitPrice" step="0.01" min="${item.sellingFixedValue}" max="${item.SellingPrice}" value="${item.SellingPrice}" /></td>
                <td>${item.SellingPrice * quantity}</td>
                <td class="text-center"><i class="fal fa-times remove"></i></td>
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

    codeValue()
}

//remove product code
const removeProductCode = function(code) {
    const id = +code.parentElement.parentElement.parentElement.getAttribute('data-id')
    const pCode = code.textContent;

    const index = cartProducts.findIndex(item => item.ProductCatalogId === id)
    const codes = cartProducts[index].codes;

    codes.forEach((obj, i) => {
        if (obj.code === pCode) {
            codes[i].isRemove = !codes[i].isRemove;
            return;
        }
    })

    showProducts()
}


// click remove or stock
const onRemoveClicked = function (evt) {
    const element = evt.target;
    const row = element.parentElement.parentElement;
    const id = +row.getAttribute('data-id');

    //remove code
    const codeClicked = element.classList.contains('code');
    if (codeClicked) removeProductCode(element);

    //remove product
    const removeClicked = element.classList.contains('remove');
    if (!removeClicked) return;

    //remove product from storage
    cartProducts = cartProducts.filter(item => item.ProductCatalogId !== id);

    //delete row
    row.remove()

    //append price
    appendTotalPrice()
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
        return item.SellingPrice * item.codes.filter(itm => !itm.isRemove).length;
    }).reduce((prev, cur) => prev + cur, 0);

    totalPrice.innerText = totalAmount
    totalPayable.innerText = totalAmount;
    totalDue.innerText = totalAmount;

    if (inputDiscount.value)
        inputDiscount.value = '';

    if (inputReturnAmount.value)
        inputReturnAmount.value = '';
}

//selling price change
const onInputUnitPrice = function (evt) {
    const input = evt.target
    const onInput = input.classList.contains('inputUnitPrice')

    if (onInput) {
        const val = +input.value
        const min = +input.getAttribute('min')
        input.setAttribute('max', input.value)

        if (min > val) return;

        const id = +input.parentElement.parentElement.getAttribute('data-id')
        const index = cartProducts.findIndex(item => item.ProductCatalogId === id)
        cartProducts[index].SellingPrice = +input.value

        const qty = +input.parentElement.previousElementSibling.innerText
        input.parentElement.nextElementSibling.innerText = val * qty

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
}


//validation info
const validation = function () {
    customerError.innerText = ''

    if (!cartProducts.length) {
        customerError.innerText = 'Product Not found!'
        return false;
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

    if (!productCodes.length) return;

    const body = {
        SellingId: +hiddenSellingId.value,
        SellingTotalPrice: +totalPrice.textContent,
        SellingDiscountAmount: +inputDiscount.value | 0,
        ProductCodes: productCodes,
        ProductList: productList
    }

    const url = '/Selling/BillChange'
    const options = {
        method: 'post',
        url: url,
        data: body
    }

    axios(options)
        .then(response => {
            if (response.data.IsSuccess) {
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

function codeValue() {
    const newCode = []
    const removeCode = []

    cartProducts.forEach(obj => {
        obj.codes.forEach(itm => {
            if (obj.hasOwnProperty("ProductCodes")) {
                if (obj.ProductCodes.indexOf(itm.code) !== -1) {
                    if (itm.isRemove)
                        removeCode.push(itm.code);
                }
            } else {
                if (!itm.isRemove)
                    newCode.push(itm.code);
            }
        })
    })

    console.log(`new ${newCode}`)
    console.log(`removed ${removeCode}`)
}