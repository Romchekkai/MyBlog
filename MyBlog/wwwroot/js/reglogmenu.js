
document.addEventListener("DOMContentLoaded", function () {

    document.querySelector('.register-link').addEventListener("click", function () {

        document.querySelector(".loginwr").classList.add("active");
    })

})

document.addEventListener("DOMContentLoaded", function () {

    document.querySelector('.login-link').addEventListener("click", function () {

        document.querySelector(".loginwr").classList.remove("active");
    })

})