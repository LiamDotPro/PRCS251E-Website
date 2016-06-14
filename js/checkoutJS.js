function updatePrice() {

    var y = document.getElementsByClassName("classy-Input");
    var x = document.getElementsByClassName("eventPrice");

    var totalPrice = 0;

    for (var i = 0; i < x.length; i++) {
        var result1 = parseFloat(x[i].innerHTML);
        var result2 = parseFloat(x[i].innerHTML);

        totalPrice += result1 * result2;
    }

    document.getElementById("UpdatedPrice").innerHTML = "£" + totalPrice.toString();

}