function convertFirstLetterToUpperCase(text) {
    return text.charAt(0).toUpperCase() + text.slice(1);
}

function convertToShortDate(dateString) {
    const shortDate = new Date(dateString).toLocaleDateString('en-US')
    return shortDate;
}

function selam(dateString) {
    //formatter = new Intl.DateTimeFormat(undefined, { year: 'numeric', month: 'short', day: 'numeric' })
    return "";
}


