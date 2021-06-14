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
            codesObj.codes.push(code.textContent);
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


    //**** CART BUTTON****
    formProductInfo.addEventListener("submit",
        function(evt) {
            evt.preventDefault();

            const { codes } = addedCodeList()
            console.log(codes)
            const options = {
                method: 'post',
                url: '/Purchase/IsPurchaseCodeExist',
                data: codes
            }

            axios(options)
                .then(response => {
                    console.log(response.data)
                })
                .catch(error => console.log(error.response));
        });
})(document);

