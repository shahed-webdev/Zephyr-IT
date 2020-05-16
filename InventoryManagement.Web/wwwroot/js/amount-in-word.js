function number2text(value) {
    const fraction = Math.round((value % 1) * 100);
    let fullText = "";

    if (fraction > 0) {
        fullText = ` AND ${convertNumber(fraction)} PAISA`;
    }

    return `${convertNumber(value)} TK${fullText} ONLY`;
}

function convertNumber(number) {
    if ((number < 0) || (number > 999999999)) return "NUMBER OUT OF RANGE!";

    let coreAmount = Math.floor(number / 10000000);
    number -= coreAmount * 10000000;

    let lakhAmount = Math.floor(number / 100000);
    number -= lakhAmount * 100000;

    let thousandAmount = Math.floor(number / 1000);
    number -= thousandAmount * 1000;

    let hundredAmount = Math.floor(number / 100);
    number = number % 100;

    let tn = Math.floor(number / 10);
    let one = Math.floor(number % 10);
    let res = "";

    if (coreAmount > 0) {
        res += (convertNumber(coreAmount) + " CRORE");
    }
    if (lakhAmount > 0) {
        res += (((res === "") ? "" : " ") + convertNumber(lakhAmount) + " LAKH");
    }
    if (thousandAmount > 0) {
        res += (((res === "") ? "" : " ") + convertNumber(thousandAmount) + " THOUSAND");
    }

    if (hundredAmount) {
        res += (((res === "") ? "" : " ") + convertNumber(hundredAmount) + " HUNDRED");
    }


    let ones = Array("","ONE","TWO","THREE","FOUR","FIVE", "SIX","SEVEN","EIGHT","NINE","TEN","ELEVEN","TWELVE","THIRTEEN","FOURTEEN","FIFTEEN","SIXTEEN","SEVENTEEN","EIGHTEEN","NINETEEN");
    let tens = Array("", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY");

    if (tn > 0 || one > 0) {
        if (!(res === "")) {
            res += " AND ";
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