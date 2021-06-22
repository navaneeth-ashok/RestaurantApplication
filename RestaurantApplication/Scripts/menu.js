window.onload = function () {
    add_buttons = document.getElementsByClassName("food_add");
    for (element of add_buttons) {
        element.onclick = addFood;
    }
    remove_buttons = document.getElementsByClassName("food_remove");
    for (element of remove_buttons) {
        element.onclick = removeFood;
    }
    // show the navigator
    if (window.innerWidth < 992) {
        document.getElementById('orderNavigator').style.display = 'block';
    }
    calculateAmount();
}

// function to show/hide the helper button
document.onscroll = function () {
    if (window.innerHeight + window.scrollY > document.body.clientHeight) {
        document.getElementById('orderNavigator').style.display = 'none';
    } else if (window.innerWidth < 992) {
        document.getElementById('orderNavigator').style.display = 'block';
    }
}

function addFood() {
    if (document.getElementById("orderWarning").style.display == "block") {
        document.getElementById("orderWarning").style.display = "none";
    }
    var order_amt = this.parentElement.querySelector(".foodQ").value;
    order_amt++;
    this.parentElement.querySelector(".foodQ").value = order_amt;
    calculateAmount();
    return false;
}
function removeFood() {
    var order_amt = this.parentElement.querySelector(".foodQ").value;
    if (order_amt > 0) {
        order_amt--;
        this.parentElement.querySelector(".foodQ").value = order_amt;
    }
    calculateAmount();
    return false;

}

// Calculate the total and preview the order Summary
function calculateAmount() {
    //renderOrderList();
    let orderList = document.getElementsByClassName("orderList");
    let previewTable = document.getElementById("orderPreviewTable");
    // remove exisitng summary
    previewTable.querySelector("tbody").innerHTML = "";
    let totalAmount = 0;
    for (order of orderList) {
        //console.log(order.querySelector(".foodPrice").textContent)
        //console.log(order.querySelector(".foodQ").value)
        totalAmount += parseFloat(order.querySelector(".foodPrice").textContent.replace('$', '')) * parseInt(order.querySelector(".foodQ").value);
        //console.log(totalAmount);
        orderQuantity = parseInt(order.querySelector(".foodQ").value);
        if (orderQuantity > 0) {
            let row = previewTable.querySelector("tbody").insertRow();
            row.insertCell(0).innerHTML = order.querySelector(".foodName").textContent;
            row.insertCell(1).innerHTML = orderQuantity;
        }
    }
    document.getElementById("totalAmt").innerHTML = totalAmount;
}

function renderOrderList() {
    let orderList = document.getElementsByClassName("orderList");
    let previewTable = document.getElementById("orderPreviewTable");
    // removing existing content
    previewTable.querySelector("tbody").innerHTML = "";
    for (order of orderList) {
        //console.log(order.querySelector(".foodName").textContent);
        //console.log(order.querySelector(".foodQ").value);
        orderQuantity = parseInt(order.querySelector(".foodQ").value);
        if (orderQuantity > 0) {
            let row = previewTable.insertRow();
            row.insertCell(0).innerHTML = order.querySelector(".foodName").textContent;
            row.insertCell(1).innerHTML = orderQuantity;
        }
    }
    return false;
}

function validateEmptyOrder() {
    let orderValues = document.getElementsByClassName("foodQ");
    let flag = 0;
    for (orderValue of orderValues) {
        if (orderValue.value != 0) {
            return true;
        }
    }
    event.preventDefault();
    document.getElementById("orderWarning").style.display = "block";
    return false;
}

// menu filtering

function filterMenu(className) {
    if (className === 'All') {
        $('.orderList').show();
    } else {
        $('.orderList').hide();
        $("." + className).toggle();
    }

}