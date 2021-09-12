let cartStorage = [];
const purchaseUpdate = (function() {
    //global store
    const cartStorage = [...data];
    const formProductInfo = document.getElementById("formProductInfo");
    const formPayment = document.getElementById("formPayment");
    const codeContainer = document.getElementById("code-container");

    //on change category
    const selectCategory = document.getElementById("selectCategory");
    const selectProductId = document.getElementById("selectProductId");

    //on change category
    selectCategory.addEventListener('change',
        function() {
            const categoryId = +this.value
            if (!categoryId) return;

            const url = '/Product/GetProductByCategoryDropDown';
            const parameter = { params: { categoryId } }

            axios.get(url, parameter).then(res => {
                const fragment = document.createDocumentFragment();
                const items = { value: "", text: "Brand and Model", firstItem: true };

                fragment.appendChild(createOptions(items));

                if (res.data.length) {
                    res.data.forEach(item => {
                        const newItem = { value: item.ProductId, text: item.ProductName, firstItem: false };
                        fragment.appendChild(createOptions(newItem));
                    });
                }

                selectProductId.innerHTML = '';
                selectProductId.appendChild(fragment);
            })
        });

    //create select options
    function createOptions(items) {
        const option = document.createElement("option");

        option.value = items.value;
        option.text = items.text;

        if (items.firstItem) {
            option.setAttribute("disabled", "disabled");
            option.setAttribute("selected", true);
        }

        return option;
    }

    //on change product
    selectProductId.addEventListener('change',
        function() {
            const productId = +this.value
            if (!productId) return;

            //remove input value
            inputBinding({}, false);

            const url = '/Product/GetProductInfo'
            const parameter = { params: { productId } }

            axios.get(url, parameter).then(res => {
                inputBinding(res.data, false);
            })
        });

    //form elements binding
    function inputBinding(items, reset) {
        const elements = formProductInfo.elements;

        if (reset) codeContainer.innerHTML = "";

        for (let element of elements) {
            if ((element.type === "text" || element.type === "number") && !element.readOnly && !element.classList.contains("search")) {
                const label = element.nextElementSibling;
                if (!reset) {
                    element.value = items[element.name];
                    label && label.classList.add('active');
                } else {
                    element.value = "";
                    label && label.classList.remove('active');
                }
            }
        }
    }


    /**** ADD NEW CODE ****/
    //add new product code
    const formAddCode = document.getElementById("formAddCode");
    formAddCode.addEventListener("submit",
        function(evt) {
            evt.preventDefault();

            const code = this.inputProductCode.value.trim();
            if (!code) return;

            const { codes } = addedCodeList();

            if (codes.indexOf(code) !== -1) {
                $.notify(`'${code}' Already added!`, "error");
                return;
            }

            appendNewCode(code);
            this.reset();

            //show check btn
            btnCheckCodeAndCartDisabled(true);
        })

    //append code to DOM
    function appendNewCode(code) {
        const div = document.createElement("div");
        div.className = "unsold";
        div.innerHTML = `<span class="code">${code}</span><i class="fas fa-times code-remove"></i>`;
        codeContainer.appendChild(div);
    }

    //added code list
    function addedCodeList() {
        const codeSpan = document.querySelectorAll("#code-container .code");
        const codesObj = { codes: [], elements: [] };

        for (let code of codeSpan) {
            codesObj.codes.push(code.textContent.trim());
            codesObj.elements.push(code);
        }

        return codesObj;
    }

    //remove code
    codeContainer.addEventListener("click",
        function(evt) {
            const onRemove = evt.target.classList.contains("code-remove");

            if (!onRemove) return;
            evt.target.parentElement.remove();
        })

    // on Product code details
    codeContainer.addEventListener('click',
        evt => {
            const element = evt.target;
            const onCode = element.classList.contains("code-details");
            if (!onCode) return;

            const url = '/Product/FindProductDetailsByCode';
            const param = { params: { code: element.innerText } };

            axios.get(url, param).then(res => {
                appendData(res.data)
            }).catch(err => console.log(err))
        })

    //append Product code details
    function appendData(product) {
        document.getElementById('category').textContent = product.ProductCatalogName;
        document.getElementById('productCode').textContent = product.ProductCode;
        document.getElementById('productName').textContent = product.ProductName;
        document.getElementById('purchasePrice').textContent = product.PurchasePrice.toFixed(2);
        document.getElementById('sellingPrice').textContent = product.SellingPrice.toFixed(2);
        document.getElementById('warranty').textContent = product.Warranty;
        document.getElementById('description').textContent = product.Description;
        document.getElementById('note').textContent = product.Note;

        const bill = document.getElementById('receipt');
        const purchase = document.getElementById('purchase');

        product.SellingId ? bill.innerHTML =`<strong class="green-text">Bill No: </strong><a target="_blank" href="/Selling/SellingReceipt/${product.SellingId}">#${product.SellingSn}</a>`: bill.innerHTML = '';
        purchase.innerHTML = `<a target="_blank" href="/Purchase/PurchaseReceipt/${product.PurchaseId}">Purchase Details <i class="fal fa-long-arrow-right"></i></a>`;

        $("#codeDetailsModal").modal("show");
    }

    //**** CART BUTTON****
    const btnCheckProduct = document.getElementById("btnCheckProduct");
    const btnAddToList = document.getElementById("btnAddToList");

    const serializeForm = function(form) {
        const obj = {};
        const formData = new FormData(form);
        for (let key of formData.keys()) {
            obj[key] = formData.get(key);
        }
        return obj;
    };

    //submit product info
    const tbody = document.getElementById("tbody");
    formProductInfo.addEventListener("submit",
        function(evt) {
            evt.preventDefault();

            //get code list
            const { codes } = addedCodeList();

            if (!codes.length) {
                $.notify("Add Product To Purchase", "error");
                return;
            }

            //create object from array
            const postCodes = codes.map(item => {
                return { ProductCode: item }
            });

            //check product code from db
            $.ajax({
                url: '/Purchase/IsPurchaseCodeExist',
                type: "POST",
                data: JSON.stringify(postCodes),
                contentType: "application/json;charset=utf-8",
                success: onAddToCart,
                error: error => console.log(error)
            });
        });

    //add to cart event listen
    function onAddToCart(response) {
        btnCheckCodeAndCartDisabled(false);

        if (response) {
            const isUnsoldExist = matchExistingProductCode(response);
            if (isUnsoldExist) return;
        }

        //add to cart
        if (document.activeElement.id !== "btnAddToList") return;

        //get code list
        const { codes } = addedCodeList();

        const PurchaseList = serializeForm(formProductInfo);
        PurchaseList.ProductCatalogName = selectCategory.options[selectCategory.selectedIndex].text;
        PurchaseList.ProductName = selectProductId.options[selectProductId.selectedIndex].text;
        PurchaseList.ProductStocks = [];

        //check product is added
        const isAdded = cartStorage.some(item => +item.ProductId === +PurchaseList.ProductId);

        if (isAdded) {
            const index = cartStorage.findIndex(item => +item.ProductId === +PurchaseList.ProductId);
            PurchaseList.ProductStocks = cartStorage[index].ProductStocks;
            PurchaseList.AddedProductCodes = cartStorage[index].AddedProductCodes;
            PurchaseList.PurchaseListId = cartStorage[index].PurchaseListId;

            codes.forEach(code => {
                if (PurchaseList.ProductStocks.indexOf(code) === -1) {
                    //update new code
                    PurchaseList.ProductStocks.push({
                        ProductStockId: 0,
                        ProductCode: code,
                        IsSold: false,
                        isRemove: false,
                        isNew: true
                    });
                }
            });

            PurchaseList.Quantity = PurchaseList.ProductStocks.filter(item => !item.isRemove).length;
            cartStorage[index] = PurchaseList;

            renderTable();
            appendTotalPrice();
            updateCalculation();

            //reset
            inputBinding({}, true);

            btnCheckCodeAndCartDisabled(true);
            return;
        }

        //add to store 
        codes.forEach(code => {
            //update new code
            PurchaseList.ProductStocks.push({
                ProductStockId: 0,
                ProductCode: code,
                IsSold: false,
                isRemove: false,
                isNew: true
            });
        });

        PurchaseList.Quantity = PurchaseList.ProductStocks.length;
        PurchaseList.PurchaseListId = 0;

        cartStorage.push(PurchaseList);

        //append new row
        tbody.appendChild(createRow(PurchaseList));

        appendTotalPrice();
        updateCalculation();

        //reset
        inputBinding({}, true);

        btnCheckCodeAndCartDisabled(true);
    }
   
    //show hide check cart btn
    function btnCheckCodeAndCartDisabled(isChecking) {
        btnCheckProduct.style.display = isChecking ? "block" : "none";
        btnAddToList.style.display = isChecking ? "none" : "block";
    }


    //match Existing Product code
    function matchExistingProductCode(stocks) {
        //get code list
        const { elements } = addedCodeList();
        let isUnsoldExist = false;

        elements.forEach(added => {
            stocks.forEach(stock => {
                const span = added.parentElement.classList;

                if (added.textContent.trim() === stock.ProductCode) {
                    span.remove('unsold');

                    if (stock.IsSold) {
                        added.classList.add("code-details");
                        span.add('sold');
                    } else {
                        added.classList.add("code-details");
                        span.add('badge-danger');
                        isUnsoldExist = true;
                    }
                }
            })
        });

        return isUnsoldExist;
    }

    //render table
    function renderTable() {
        const fragment = document.createDocumentFragment();

        cartStorage.forEach(item => {
            fragment.appendChild(createRow(item));
        });
        tbody.innerHTML = "";
        tbody.appendChild(fragment);
    }

    //create new table row
    function createRow(item) {
        const tr = document.createElement("tr");
        tr.innerHTML = `<td>
                           <p class="m-0">${item.ProductCatalogName}</p>
                              <small class="text-muted">${item.Description ? item.Description:""}</small>
                           </td>
                           <td>
                               <p class="m-0">${item.ProductName}</p>
                               <small class="text-muted">${item.Note ? item.Note:""}</small>
                           </td>
                           <td>${item.PurchasePrice}</td>
                           <td>${item.SellingPrice}</td>
                           <td>${item.Warranty}</td>
                           <td>${item.Quantity}</td>
                           <td id="${item.ProductId}">${appendCodes(item.ProductStocks)}</td>`;
        return tr;
    }

    //append product code
    function appendCodes(codes) {
        let codeSpan = "";
        codes.forEach(code => {
            const remove = code.isRemove ? "code-removed" : "";
            const stockOut = code.IsSold ? "stock-out" : "";

            codeSpan += `<span class="m-1 stock ${remove} ${stockOut}">${code.ProductCode}</span>`;
        });

        return codeSpan;
    }


    // remove product code from table stock
    tbody.addEventListener('click',
        function(evt) {
            const element = evt.target;

            //remove code
            const codeClicked = element.classList.contains('stock');

            if (!codeClicked) return;
            const id = +element.parentElement.id;
            const code = element.textContent.trim();

            const index = cartStorage.findIndex(item => +item.ProductId === id);
            const codes = cartStorage[index].ProductStocks;

            codes.forEach((obj, i) => {
                if (obj.ProductCode === code) {
                    codes[i].isRemove = !codes[i].isRemove;
                    return;
                }
            });

            cartStorage[index].Quantity = codes.filter(item => !item.isRemove).length;

            renderTable();
            appendTotalPrice();
            updateCalculation();
        });


    //***** PAYMENT ****
    const totalPrice = document.getElementById("totalPurchaseAmount");
    const previousPaid = document.getElementById("previousPaid");
    const totalDue = document.getElementById("totalDue");

    const inputDiscount = document.getElementById("inputDiscount");
    const inputReturn = document.getElementById("inputReturn");
    const inputPaid = document.getElementById("inputPaid");
    const selectAccountId = document.getElementById("selectAccountId");
 
    //sum total product price
    function sumTotal() {
        return cartStorage.map(item => +item.PurchasePrice * +item.Quantity).reduce((prev, cur) => prev + cur, 0);
    }

    //append total price to DOM
    function appendTotalPrice() {
        const totalAmount = sumTotal();

        //set max discount limit
        inputDiscount.setAttribute("max", totalAmount);

        totalPrice.innerText = totalAmount.toFixed(2);

        //for reset paid amount and method
        if (inputPaid.value) {
            inputPaid.value = '';
        }

        if (selectAccountId.selectedIndex > 0) {
            selectAccountId.removeAttribute('required');
        }
    }

    //on discount
    inputDiscount.addEventListener("input", function() {
        updateCalculation();
    });

    //on return
    inputReturn.addEventListener("input", function () {
        setAccountRequired();
        updateCalculation();
    });

    //input paid amount
    inputPaid.addEventListener("input", function () {
        setAccountRequired();
    });

    //set account dropdown required
    function setAccountRequired() {
        const paid = +inputPaid.value;
        const returnAmount = +inputReturn.value;

        paid || returnAmount ? selectAccountId.setAttribute('required', true) : selectAccountId.removeAttribute('required');
    }

    //dom calculate paid due
    function updateCalculation() {
        const totalAmount = +totalPrice.textContent;
        const discount = +inputDiscount.value;
        const prevPaid = +previousPaid.textContent;
        const returnAmount = +inputReturn.value;

        const dueAmount = (totalAmount + returnAmount) - (discount + prevPaid);

        totalDue.innerText = dueAmount.toFixed(2);

        //input paid set attribute
        enabledDisablePaid(dueAmount);
    }

    //enabled Disable Paid input
    function enabledDisablePaid(dueAmount) {
        if (dueAmount > 0) {
            inputPaid.removeAttribute('disabled');
            selectAccountId.removeAttribute('disabled');
            inputPaid.setAttribute('max', dueAmount);
        } else {
            inputPaid.setAttribute('disabled', 'disabled');
            selectAccountId.setAttribute('disabled', 'disabled');

            const paid = +inputPaid.value;

            if (paid) inputPaid.value = '';

            paid ? selectAccountId.setAttribute('required', true) : selectAccountId.removeAttribute('required');
        }
    }

    //submit update bill
    formPayment.addEventListener("submit", function(evt) {
        evt.preventDefault();

        const due = +totalDue.textContent;

        if (due < 0) {
            $.notify("Due Amount must be more than or equal 0", "error");
            return;
        }

        this.btnUpdateBill.disabled = true;

        const model = serializeForm(this);
        model.PurchaseTotalPrice = sumTotal();
        model.RemovedProductStockIds = [];

        cartStorage.forEach(product => {
            const added = [];
            product.ProductStocks.forEach(item => {
                //remove list
                if (item.isRemove && !item.isNew && !item.IsSold) {
                    model.RemovedProductStockIds.push(item.ProductStockId);
                }

                //added list
                if (!item.isRemove && item.isNew) {
                    added.push(item.ProductCode);
                }
            });

            product.AddedProductCodes = added;
        });

        model.PurchaseList = cartStorage;
    
        $.ajax({
            url: '/Purchase/PostPurchaseBillChange',
            type: "POST",
            data: model,
            success: response=> {
                if (response.IsSuccess) {
                    location.href = `/Purchase/PurchaseRecords`;
                    return;
                }

                $.notify(response.Message, response.IsSuccess ? "success" : "error");
                this.btnUpdateBill.disabled = false;
            },
            error: error=> {
                this.btnUpdateBill.disabled = false;
                console.log(error);
            }
        });
    });
})(document);

