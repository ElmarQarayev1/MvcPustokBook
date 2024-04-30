$(document).ready(function () {

    $(".book-modal").click(function (e) {
        e.preventDefault();
        let url = this.getAttribute("href");

        fetch(url)
        .then(response => response.text())
        .then(data => {
                $("#quickModal .modal-dialog").html(data)
            })

        $("#quickModal").modal('show');
    })
})

//document.querySelector(".book-modal").addEventListener("click", function (e) {
//    e.preventDefault();
//    alert("slaa");
    //$("#quickModal").modal('show');

//})