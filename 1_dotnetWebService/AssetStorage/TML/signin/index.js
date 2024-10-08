var swiper = new Swiper(".swiper-container", {
  pagination: ".swiper-pagination",
  paginationClickable: true,
  parallax: true,
  speed: 600,
  autoplay: 3500,
  loop: true,
  grabCursor: true,
});

sessionStorage.removeItem("username");
sessionStorage.removeItem("role");
sessionStorage.removeItem("userID");
sessionStorage.removeItem("User_role");
sessionStorage.removeItem("page");

$(document).ready(function () {
  var token = window.localStorage.getItem("token");
  if (token) {
    var landing_page = window.localStorage.getItem("landing_page");
    window.location.href = `../${landing_page}/index.html`;
  } else {
    window.localStorage.removeItem("token");
  }

  ValidateUser = function () {
    console.log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
    var userObj = {
      userId: document.getElementById("username").value,
      password: document.getElementById("pwd").value,
    };

    $.ajax({
      url: "/login",
      data: JSON.stringify(userObj),
      type: "POST",
      contentType: "application/json;charset=utf-8",
      dataType: "json",
      success: function (result) {
        console.log("result", result.user);
        if (result.token != null &&  result.landing_page != null) {
          window.localStorage.setItem("token", result.token);
          window.localStorage.setItem("landing_page", result.landing_page);
          window.location.href = `../${result.landing_page}/index.html`;
          //}
        } 
        else
        {
          alert("Landing page is not defined please contact admin")
          window.location.href = `../signin/index.html`;
        }
        $("#username").val("");
      },
      error: function (errormessage) {
        alert("Username or Password incorrect");
      },
    });
  };
});
