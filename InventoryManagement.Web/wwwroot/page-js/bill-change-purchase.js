let cartStorage = [];
const purchaseUpdate = (function() {
    //global store
    let cartStorage = [...data];
    const formProductInfo = document.getElementById("formProductInfo");

    //on change category
    const selectCategory = document.getElementById("selectCategory");
    const selectProductId = document.getElementById("selectProductId");

    //on change category
    selectCategory.addEventListener('change', function() {
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
    selectProductId.addEventListener('change', function () {
        const productId = +this.value
        if (!productId) return;

        //remove input value
        inputBinding({}, false);

        const url = '/Product/GetProductInfo'
        const parameter = { params: { productId } }

        axios.get(url, parameter).then(res => {
            inputBinding(res.data,false);
        })
    });

    //form elements binding
    function inputBinding(items,reset) {
        const elements = formProductInfo.elements;
 
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




    /**** ADD CODE ****/
    //add product code
    const formAddCode = document.getElementById("formAddCode");
    formAddCode.addEventListener("submit", function(evt) {
        evt.preventDefault();

        const code = this.inputProductCode.value;
        if (!code) return;

        const { codes } = addedCodeList();
       
        if (codes.indexOf(code) !== -1) {
            $.notify(`'${code}' Already added!`, "error");
            return;
        }

        appendNewCode(code);
        this.reset();
    });

    ////append code to DOM
    const codeContainer = document.getElementById("code-container");
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
    codeContainer.addEventListener("click", function(evt) {
        const onRemove = evt.target.classList.contains("code-remove");

        if (!onRemove) return;
        evt.target.parentElement.remove();
    });

    // on Product code details
    codeContainer.addEventListener('click', evt => {
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

        product.SellingId ? bill.innerHTML = `<strong class="green-text">Bill No: </strong><a target="_blank" href="/Selling/SellingReceipt/${product.SellingId}">#${product.SellingSn}</a>` : bill.innerHTML = '';
        purchase.innerHTML = `<a target="_blank" href="/Purchase/PurchaseReceipt/${product.PurchaseId}">Purchase Details <i class="fal fa-long-arrow-right"></i></a>`;

        $("#codeDetailsModal").modal("show");
    }

    //**** CART BUTTON****
    const btnCheckProduct = document.getElementById("btnCheckProduct");
    const btnAddToList = document.getElementById("btnAddToList");

    const serializeForm = function (form) {
        const obj = {};
        const formData = new FormData(form);
        for (let key of formData.keys()) {
            obj[key] = formData.get(key);
        }
        return obj;
    };

    //submit product info
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

            const options = {
                method: 'post',
                url: '/Purchase/IsPurchaseCodeExist',
                data: postCodes
            }

            axios(options)
                .then(response => {
                    if (response.data.length) {
                        const isUnsoldExist = matchExistingProductCode(response.data);
                        if (isUnsoldExist) return;
                    }

                    btnCheckCodeAndCartDisabled(false);

                    if (document.activeElement.id !== "btnAddToList") return;

                    const model = serializeForm(this);
                    model.ProductCatalogName = selectCategory.options[selectCategory.selectedIndex].text;
                    model.ProductName = selectProductId.options[selectProductId.selectedIndex].text;
                    model.ProductStocks = codes

                    console.log(model)
                })
                .catch(error => console.log(error.response))
        });

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


    //check product already in listed
    function isProductAddedInCart(product) {
        return cartStorage.some(item => item.ProductId === product.ProductId);
    }

    //create new table row
    function createRow(item) {
        const tr = document.createElement("tr");
        tr.id = "";
        tr.innerHTML = `<td>
                           <p class="m-0">Processor</p>
                           <small class="text-muted">3.10 Ghz, 12MB Cache, 6 Cores &amp; 12 Threads</small>
                           </td>
                           <td>
                               <p class="m-0">Intel i5-10500</p>
                               <small class="text-muted"></small>
                           </td>
                           <td>18200.00</td>
                           <td>18200.00</td>
                           <td>1095days</td>
                           <td>
                               ${appendCodes(codes)}
                           </td>`
    }

    //append product code
    function appendCodes(codes) {
        let codeSpan = "";
        codes.forEach(code => {
            codeSpan += `<span class="m-1 stock">${code}</span>`;
        });

        return codeSpan;
    }
})(document);

