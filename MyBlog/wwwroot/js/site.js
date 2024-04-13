document.addEventListener("DOMContentLoaded", function () {

    document.getElementById('menu-btn').addEventListener("click", function () {

        document.querySelector(".menu").classList.toggle("active");
        document.querySelector(".content").classList.toggle("active");
    })

})

