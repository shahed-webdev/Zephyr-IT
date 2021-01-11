function number2text(value) {
    const fraction = Math.round((value % 1) * 100);
    let fullText = "";

    if (fraction > 0) {
        fullText = ` And ${convertNumber(fraction)} Paisa`;
    }

    return `${convertNumber(value)} Tk${fullText} Only`;
}

function convertNumber(number) {
    if ((number < 0) || (number > 999999999)) return "NUMBER OUT OF RANGE!";

    const coreAmount = Math.floor(number / 10000000);
    number -= coreAmount * 10000000;

    const lakhAmount = Math.floor(number / 100000);
    number -= lakhAmount * 100000;

    const thousandAmount = Math.floor(number / 1000);
    number -= thousandAmount * 1000;

    const hundredAmount = Math.floor(number / 100);
    number = number % 100;

    const tn = Math.floor(number / 10);
    const one = Math.floor(number % 10);
    let res = "";

    if (coreAmount > 0) {
        res += (convertNumber(coreAmount) + " Crore");
    }
    if (lakhAmount > 0) {
        res += (((res === "") ? "" : " ") + convertNumber(lakhAmount) + " Lakh");
    }
    if (thousandAmount > 0) {
        res += (((res === "") ? "" : " ") + convertNumber(thousandAmount) + " Thousand");
    }
    if (hundredAmount) {
        res += (((res === "") ? "" : " ") + convertNumber(hundredAmount) + " Hundred");
    }

    const ones = Array("", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen","Nineteen");
    const tens = Array("", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety");

    if (tn > 0 || one > 0) {
        if (!(res === "")) {
            res += " And ";
        }

        if (tn < 2) {
            res += ones[tn * 10 + one];
        }
        else {
            res += tens[tn];
            if (one > 0) {
                res += (`-${ones[one]}`);
            }
        }
    }

    if (res === "")
        res = "zero";

    return res;
}
