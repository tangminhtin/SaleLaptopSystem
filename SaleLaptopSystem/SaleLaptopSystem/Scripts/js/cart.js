let decrease = document.getElementById('decrease');
let increase = document.getElementById('increase');

function tang(clicked) {
    let prodId = clicked.value;
    let quantity = document.getElementById('quantity-' + clicked.value);
    if (quantity.value >= 9) {
        alert("Mỗi sản phẩm chỉ cho phép mua tối đa số lượng 9");
        return;
    }
    quantity.value++

    $.ajax(
        {
            type: "GET", //HTTP POST Method  
            url: "Cart", // Controller/View   
            data: { //Passing data  
                prodID: prodId, //Reading text box values using Jquery   
                msg: "add"
            }

        });
    reload();

}

function giam(clicked) {
    let prodId = clicked.value;
    let quantity = document.getElementById('quantity-' + clicked.value);
    console.log(quantity.value);

    if (quantity.value <= 1) {
        return;
    }
    console.log(clicked.value)
    quantity.value--

    $.ajax(
        {
            type: "GET", //HTTP POST Method  
            url: "Cart", // Controller/View   
            data: { //Passing data  
                prodID: prodId, //Reading text box values using Jquery   
                msg: "minus"
            }

        });
    reload();
}
function reload() {
    $.ajax({
        url: $('#result').data('remote-url'),
        type: 'POST',
        beforeSend: function () {
            // TODO: you could show an AJAX loading spinner
            // to indicate to the user that there is an ongoing
            // operation so that he doesn't run out of patience
        },
        complete: function () {
            // this will be executed no matter whether the AJAX request
            // succeeds or fails => you could hide the spinner here
        },
        success: function (result) {
            // In case of success update the corresponding div with
            // the results returned by the controller action
            $('#result').html(result);
        },
        error: function () {
            // something went wrong => inform the user 
            // in the gentler possible manner and remember
            // that he spent some of his precious time waiting 
            // for those results
        }
    });
}