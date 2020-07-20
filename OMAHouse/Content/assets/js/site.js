
function DisplayTextbox(type)
{
    console.log(type);
    if (type == 1) {
        $("#CreditTextBox").show();
        $("#DebitTextBox").hide();
    }
    else {
        $("#DebitTextBox").show();
        $("#CreditTextBox").hide();
    }
}

$('.radioAmountType').on('change', function () {

    alert($('radioAmountType:checked').closest('label').text());

});