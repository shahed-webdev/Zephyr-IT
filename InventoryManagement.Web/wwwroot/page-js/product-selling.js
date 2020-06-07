
// date picker
$('.datepicker').pickadate().val(moment(new Date()).format('DD MMMM, YYYY'))
document.getElementById('label-date').classList.add('active')

 // material select initialization
$('.mdb-select').materialSelect()

// global storage
let cart = {
    products: [],
    codes:[]
}


//*****SELECTORS*****/
// product code form
const formCode = document.getElementById('formCode')
const inputBarCode = formCode.inputBarCode
const productCount = formCode.querySelector('#productCount')
const codeExistError = formCode.querySelector('#codeExistError')
const btnFind = formCode.btnFind

// product table
const tbody = document.getElementById('t-body')

//payment selectors
const formPayment = document.getElementById('formPayment')
const totalPrice = formPayment.querySelector('#totalPrice')
const inputDiscount = formPayment.inputDiscount
const totalPayable = formPayment.querySelector('#totalPayable')
const inputPaid = formPayment.inputPaid
const totalDue = formPayment.querySelector('#totalDue')
const selectPaymentMethod = formPayment.selectPaymentMethod
const inputPurchaseDate = formPayment.inputPurchaseDate
const customerError = formPayment.querySelector('#customer-error')


// get set storage object
const storage = {
    saveData: function (product) {
        codeExistError.textContent = ''
        if (!product) {
            codeExistError.textContent = `"${inputBarCode.value}" Not found!`
            return
        }

        console.log(cart.products)
        let unique = true;
        cart.products.forEach(item => {
            if (item.ProductCode === product.ProductCode) {
                unique = false
                return
            }
        })

        if (unique) {
            //save to global object
            cart.products.push(product)
            cart.codes.push(product.ProductCode)

            //append data to table
            tbody.appendChild(createTableRow(product))

            //show added items count
            this.countProduct()

            //append price
            appendTotalPrice()
        }
        else {
            codeExistError.textContent = `"${inputBarCode.value}" code already added!`
        }

        //clear input
        inputBarCode.value = ''
        //save to local-storage
        this.setData()
    },
    setData: function () {
        localStorage.setItem('selling-cart', JSON.stringify(cart))
    },
    getData: function () {
        const store = localStorage.getItem('selling-cart')
        if (!store) return

        cart = JSON.parse(store)
    },
    countProduct: function () {
        productCount.textContent = cart.codes.length
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

// create table rows
const createTableRow = function (item) {
    const tr = document.createElement("tr");
    tr.setAttribute('data-code', item.ProductCode);

    //column 1
    const span = document.createElement('span');
    span.classList.add('text-dark');
    span.appendChild(document.createTextNode(item.ProductName));

    const p = document.createElement('p');
    p.classList.add('text-muted', 'small', 'mb-0');
    p.appendChild(document.createTextNode(item.ProductCatalogName));

    const td1 = tr.insertCell(0);
    td1.appendChild(span);
    td1.appendChild(p);

    if (item.Warranty) {
        const warranty = document.createElement('span');
        warranty.classList.add('light-blue-text', 'small');
        warranty.textContent = `Warranty: ${item.Warranty}`;
        td1.appendChild(warranty);
    }

    //column 2
    const td2 = tr.insertCell(1);
    td2.appendChild(document.createTextNode(item.ProductCode));

    //column 3
    const td3 = tr.insertCell(2);
    td3.appendChild(document.createTextNode(item.SellingPrice));

    //column 4
    const i = document.createElement('i');
    i.classList.add('fal', 'fa-times', 'remove');
    const td4 = tr.insertCell(3);
    td4.classList.add('text-center');
    td4.appendChild(i);

    return tr;
}

// create Table on load
const showProducts = function () {
    const fragment = document.createDocumentFragment()

    cart.products.forEach(item => {
        fragment.appendChild(createTableRow(item))
    })

    tbody.appendChild(fragment)

    //show added items count
    storage.countProduct()

    //sub price
    appendTotalPrice()

}

// click remove or stock
const onRemoveClicked = function (evt) {
    const element = evt.target;
    const removeClicked = element.classList.contains('remove');
    const row = element.parentElement.parentElement;
    const code = row.getAttribute('data-code');

    if (!removeClicked) return

    //remove product from storage
    cart.products = cart.products.filter(item => item.ProductCode !== code);
    cart.codes = cart.codes.filter(item => item !== code);


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
    return cart.products.map(item => item.SellingPrice).reduce((prev, cur) => prev + cur, 0);
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


// event listners
formCode.addEventListener('submit', evt => {
    evt.preventDefault()
    const url = '/Product/FindProductByCode'
    const param = { params: { code: inputBarCode.value } };

    loading(btnFind,true)

    axios.get(url, param)
        .then(res => storage.saveData(res.data))
        .catch(err => console.log(err))
        .finally(() => loading(btnFind, false))
})

// remove click
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
    let isValid = true;

    if (!hiddenVendorId.value) {
        isValid = false;
        vendorInfo.innerHTML = '<li class="list-group-item list-group-item-danger text-center"><i class="fas fa-exclamation-triangle mr-1 red-text"></i>Select or add Vendor for Purchase!</li>';
    }

    if (!storage.length)
        isValid = false;

    vendorError.textContent = storage.length ? '' : 'Add product to purchase!';

    return isValid;
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
//formPayment.addEventListener('submit', onPurchaseSubmitClicked);
inputDiscount.addEventListener('input', onInputDiscount);
inputPaid.addEventListener('input', onInputPaid);



//****CUSTOMER****//
const inputCustomer = formPayment.inputCustomer
const hiddenCustomerId = formPayment.hiddenCustomerId

//customer autocomplete
$('#inputCustomer').typeahead({
    minLength: 1,
    displayText: function (item) {
        return `${item.CustomerName}`;
    },
    afterSelect: function (item) {
        this.$element[0].value = item.CustomerName
    },
    source: function (request, result) {
        $.ajax({
            url: "/Product/FindCustomerAsync",
            data: { prefix: request },
            success: function (response) { result(response); }
        });
    },
    updater: function (item) {
        //appendVendorInfo(item);
        console.log(item)
        return item;
    }
});