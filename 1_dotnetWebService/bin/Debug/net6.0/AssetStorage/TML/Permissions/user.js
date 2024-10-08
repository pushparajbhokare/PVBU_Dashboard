var users = [];
//**************** Datatable Generator for user Details table *********************************//
var dataTable = $("#char-data-table").DataTable({
    paging: true,

    searching: true,
    info: false,

    columnDefs: [
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex px-3 py-1">
                                     <div class="d-flex flex-column justify-content-center">
                                         <h6 class="mb-0 text-sm">${meta.row + 1}</h6>
                                      </div>
                                 </div>`;
            },
            targets: 0,
        },
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex px-3 py-1">
                                     <div class="d-flex flex-column justify-content-center">
                                         <h6 class="mb-0 text-sm">${full.username}</h6>
                                      </div>
                                 </div>`;
            },
            targets: 1,
        },
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex px-3 py-1">
                                     <div class="d-flex flex-column justify-content-center">
                                         <h6 class="mb-0 text-sm">${full.full_name}</h6>
                                      </div>
                                 </div>`;
            },
            targets: 2,
        },
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex ">  <h6 class="mb-0 text-sm">${full.role_name}</h6></div>`;
            },
            targets: 3,
        },
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex ">  <h6 class="mb-0 text-sm">${full.plant_location ? full.plant_location : `Non Selected`}</h6></div>`;
            },
            targets: 4,
        },
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex ">  <h6 class="mb-0 text-sm">${full.area ? full.area : `Non Selected`}</h6></div>`;
            },
            targets: 5,
        },
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex justify-content-center">${full.role_id == 1 ? ` <span  class="badge bg-gradient-danger">Inactive</span>` : `<span class="badge bg-gradient-success">Active</span>`} </div>`;
            },
            targets: 6,
        },

        {
            render: function (data, type, full, meta) {
                return (
                    ` 
        <div class="d-flex px-3 py-1 justify-content-center">
             <div class="d-flex justify-content-around">
                 
                        <img ${full.role_id == 2 ? `style="opacity:0.5;cursor: not-allowed! important;" disbled` : `onclick='updateUser(${JSON.stringify(full)})'`}  class='editData' src='./pen-to-square-solid.svg' style='width: 15px; margin:0 10px'  />
                        <img ${full.role_id == 2 ? `style="opacity:0.5; cursor: not-allowed! important;" disbled` : `onclick='DeleteUser(${JSON.stringify(full.username)})'`}  class='editData' src='./delete.svg' style='width: 20px;'  />
                 
               </div>
         </div>`
                );
            },
            targets: 7,
        },
    ],
    language: {
        paginate: {
            next: "&#8594;", // or '→'
            previous: "&#8592;", // or '←'
        },
    },
});

//************************ Document Ready Function  *********************************//

$(document).ready(function () {

    $("#areaDropDown", "#plantDropDown").multiselect({
        allSelectedText: "All",
        buttonWidth: '100%'
        // includeSelectAllOption: true,
    });
    sessionStorage.setItem("page", "User List");

    var getCall = new ApiGet(`/userDetails`);
    getCall.onSuccess = function onSuccess(result) {
        addHead("User List", result);
    };

    getCall.call();

    GetUserDetails();
    showSidebar();

    GetUserData();

    $("#roleDropDown").on("change", function () {
        updateUser({});
    });

    $("#cancelData").click(function () {
        // $("#hiddenForm").css({ display: "none" });
        $("#hidden-form").modal("hide")


    });

    $("#saveData").click(function () {
        // $("#hiddenForm").css("display", "none");

        id = $("#Id").val();
        role_id = $("#roleDropDown").val();
        plant_location = ($("#plantDropDown").val() || []).join(',');
        area = ($("#areaDropDown").val() || []).join(',');
        $('#areaDropDown').multiselect("refresh");

        data = {
            id,
            role_id,
            plant_location,
            area,
        };
        if ($("#roleDropDown").val() == 1) {
            updateUserData(data);
            $("#hidden-form").modal("hide")

        } else {
            validate()

        }



    });
    function validate() {
        var plant = $("#plantDropDown :checked");
        var area = $("#areaDropDown :checked");
        if (plant.length == 0 || area.length == 0) {
            alert("Please select atleast one area or one plant");
        }
        else {
            updateUserData(data);
            $("#hidden-form").modal("hide")
        }

    }

    addRoleNames();
    addAreaNames();
    addPlantNames();


    $('#AddUser').click(function () {

        var adminId = $('#AdminId').val()
        var admpass = $('#AdminPass').val()
        var userId = $('#UserId').val()
        var obj = {
            AdminPassword: admpass,
            AdminUserId: adminId,
            UserId: userId,
        }
  
        var postCall = new ApiPost(`/addUserByAdmin`, obj);
        postCall.onSuccess = function onSuccess(result) {
            DisplayUserInfo(result)
            $('#modal-notification').modal("show");


        };
        postCall.onError = function onError(errormessage) {
            $('#modal-notification').modal("show");

            $(`#modal_body`).html('')
            var html = `<div class="py-3 text-center">
    <img src="../../img/notifications_active.svg" alt="">
    <h4 class="text-gradient text-danger mt-4">You should read this!</h4>
    <p>"User does not exist" OR  "Invalid credentials" </p>
    </div>`
            $(`#modal_body`).append(html)
        }
        postCall.call();


    })

    $('#userInfo').click(function () {
        $('#modal-notification').modal("hide");

    })

});

//************************ Document Ready Function  *********************************//

function DisplayUserInfo(data) {
    $('#AdminId').val('')
    $('#AdminPass').val('')
    $('#UserId').val('')
    $('#modal-form').modal("hide");

    $(`#modal_body`).html(``)
    var html = data ? ` <div class="row  d-flex align-items-center" >
  <h6 class="text-gradient mb-0 text-dark col-sm-4">User Name: </h6><p class="mb-0 col-sm-8" id="userName">${data.name} </p>
</div>
<div class=" row  d-flex align-items-center">
  <h6 class="text-gradient mb-0 text-dark col-sm-4">User Id: </h6><p id="userId" class=" col-sm-8 mb-0">${data.userId}</p>
</div>
<div class=" row  d-flex align-items-center">
  <h6 class="text-gradient mb-0 text-dark col-sm-4">Email: </h6><p id="mail" class="mb-0 col-sm-8">${data.mail}</p>
</div>`
        :
        `<div class="py-3 text-center">
<img src="../../img/notifications_active.svg" alt="">
<h4 class="text-gradient text-danger mt-4">You should read this!</h4>
<p>A small river named Duden flows by their place and supplies it with the necessary regelialia.</p>
</div>`

    $(`#modal_body`).append(html)
}

//************************ updateUser Function  *********************************//

function updateUser(user) {

    $("#hidden-form").modal("show")

    $("#areaDropDown").multiselect('enable')
    $("#plantDropDown").multiselect('enable')
    if (user.role_id) {
        $("#roleDropDown").val(user.role_id);
    }
   
    if ($("#roleDropDown").val() == 2 || $("#roleDropDown").val() == 14) {


        $('#plantDropDown option').prop('selected', true);
        $('#areaDropDown option').prop('selected', true);
        $('#areaDropDown').multiselect('disable')
        $("#plantDropDown").multiselect("disable");
        $('#areaDropDown').multiselect("refresh");
        $('#plantDropDown').multiselect("refresh");


    }
    else if ($("#roleDropDown").val() == 1) {
        $("#areaDropDown").val([]).multiselect('refresh')
        $("#plantDropDown").val([]).multiselect('refresh')

        $('#areaDropDown').multiselect('disable')
        $("#plantDropDown").multiselect("disable");
        // $('#areaDropDown').multiselect("refresh");


    }
    else if ($("#roleDropDown").val() == 4) {



        $("#areaDropDown").val((user.area || '').split(',')).attr({ selected: true })
        $("#plantDropDown").val((user.plant_location || '').split(',')).attr({ selected: true })

        $('#areaDropDown').multiselect("refresh");
        $('#plantDropDown').multiselect("refresh");
        OnchangePlant();
        jQuery('#plantDropDown').change(function () {
            OnchangePlant()
        });


    }

    else {
        $("#plantDropDown").multiselect('enable')
        $('#areaDropDown option').prop('selected', true);
        $("#areaDropDown").multiselect('disable')
         $("#plantDropDown").val((user.plant_location || '').split(',')).attr({ selected: true })//:$("#plantDropDown").val("0").attr("disabled", true);

        $('#areaDropDown').multiselect("refresh");
        $('#plantDropDown').multiselect("refresh");
    }

    if (user.id) {
        $("#Id").val(user.id);
    }


}

//************************OnchangePlant Function  *********************************//

function OnchangePlant() {

    // Get selected options.
    var selectedOptions = jQuery('#plantDropDown option:selected');

    if ($("#roleDropDown").val() == 4) {
  
        if (selectedOptions.length >= 1) {
            // Disable all other checkboxes.
            var nonSelectedOptions = jQuery('#plantDropDown option').filter(function () {
                return !jQuery(this).is(':selected');
            });

            nonSelectedOptions.each(function () {
                var input = jQuery('input[value="' + jQuery(this).val() + '"]');
                input.prop('disabled', true);
                input.parent('li').addClass('disabled');
            });
        }
        else {
            // Enable all checkboxes.
            jQuery('#plantDropDown option').each(function () {
                var input = jQuery('input[value="' + jQuery(this).val() + '"]');
                input.prop('disabled', false);
                input.parent('li').addClass('disabled');
            });
        }
    }


}

//************************ DeleteUser Function  *********************************//

function DeleteUser(user) {
    var getCall = new ApiPost(`/DeleteUser?User_id=${user}`);
    getCall.onSuccess = function onSuccess(result) {
        alert("User Deleted successfully!!!");
        GetUserData();
    };
    getCall.call();
}

//************************ addPlantNames Function  *********************************//

function addPlantNames() {
    var getCall = new ApiGet(`/GetPlantList`);
    getCall.onSuccess = function onSuccess(result) {
        $("#plantDropDown").html("");

        result.forEach(function (ele) {
            $("#plantDropDown").append(`<option value="${ele}">${ele}</option>`);
        });
        $("#plantDropDown").multiselect("rebuild");
        $("#plantDropDown").multiselect({
            allSelectedText: "All",
            // includeSelectAllOption: true,
        }).multiselect("updateButtonText");;
        // }
    };
    getCall.call();
}

//************************ addAreaNames Function  *********************************//

function addAreaNames() {
    var getCall = new ApiGet(`/GetAreaList`);
    getCall.onSuccess = function onSuccess(result) {
        $("#areaDropDown").html("");
        // $("#areaDropDown").append(
        //   `<option value="0" selected >All</option>`
        // );
        result.forEach(function (ele) {
            $("#areaDropDown").append(`<option value="${ele}">${ele}</option>`);
            $("#areaDropDown").multiselect("rebuild");

        });
        $("#areaDropDown").multiselect({
            allSelectedText: "All",
            // includeSelectAllOption: true,
        }).multiselect("updateButtonText");;


    };
    getCall.call();
}

//************************ addRoleNames Function  *********************************//

function addRoleNames() {
    var getCall = new ApiGet(`/GetRoleList`);
    getCall.onSuccess = function onSuccess(result) {
      
        $("#roleDropDown").html("");
        result.forEach(function (ele) {
            $("#roleDropDown").append(
                `<option value="${ele.role_id}">${ele.role_name}</option>`
            );
        });
    };
    getCall.call();
}

//************************ updateUserData Function  *********************************//

function updateUserData(data) {
  

    var getCall = new ApiPost(`/UpdateUser`, data);
    getCall.onSuccess = function onSuccess(result) {
        alert("User updated successfully!!!");
        GetUserData()
    };
    getCall.call();
}

//************************ GetUserData Function  *********************************//

function GetUserData() {
    var getCall = new ApiGet(`/GetUserList`);
    getCall.onSuccess = function onSuccess(result) {
  
        users = result;
        dataTable.clear().draw();
        dataTable.rows.add(result);
        dataTable.columns.adjust().draw();
    };
    getCall.call();
}

//************************ addHead Function  *********************************//

function addHead(...data) {
    $("#shownav").html("");
    var addNav = `
<nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl" id="navbarBlur" data-scroll="true">
<div class="container-fluid py-1 px-3">
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
            <li class="breadcrumb-item text-sm"><a class="opacity-5 text-dark" href="javascript:;">Pages</a></li>
            <li class="breadcrumb-item text-sm text-dark active" aria-current="page">Dashboard</li>
        </ol>
        <h6 class="font-weight-bolder mb-0">${data[0]}</h6>
    </nav>
    <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">

        
        <div class="ms-md-auto pe-md-3 d-flex align-items-center">

            <!-- <div class="input-group input-group-outline">
              <label class="form-label">Type here...</label>
              <input type="text" class="form-control">
            </div> -->
        </div>
        <ul class="navbar-nav  justify-content-end">
            <ref>
                <li class="nav-item d-flex align-items-center">
                    <a class="btn btn-outline-primary btn-sm mb-0 me-3" target="_blank" href="https://www.creative-tim.com/builder/material?ref=navbar-dashboard">Online Builder</a>
                </li>
            </ref>
            <li class="nav-item d-flex align-items-center">
                <a href="#" class="nav-link text-body font-weight-bold px-0">
                    <i class="fa fa-user me-sm-1"></i>
                    <span class="d-sm-inline d-none" id="roleName">${data[1].role_name} : ${data[1].full_name} </span>
                </a>
            </li>
            <li class="nav-item d-xl-none ps-3 d-flex align-items-center">
                <a href="javascript:;" class="nav-link text-body p-0" id="iconNavbarSidenav">
                    <div class="sidenav-toggler-inner">
                        <i class="sidenav-toggler-line"></i>
                        <i class="sidenav-toggler-line"></i>
                        <i class="sidenav-toggler-line"></i>
                    </div>
                </a>
            </li>
            <li class="nav-item px-3 d-flex align-items-center" >
                <a  class="nav-link text-body p-0">
                    <i class="fa-solid fa-arrow-right-from-bracket" ><img src="../../img/icons/gear-solid.svg" id="showHiddenMenu" style="width:20px"></i>
                </a>
            </li>

        </ul>
    </div>
</div>
</nav>
<div class="settingDropDown">
<ul>
    <a href="../Permissions/manageUser.html"><li  >Configure Users</li></a>
    <a href="../Permissions/manageRole.html"><li  >Configure Roles</li></a>
    <a onclick="LogOut()"><li>Log out</li></a>
</ul>
</div>`;
    $("#shownav").append(addNav);

    $("#showHiddenMenu").click(function () {
        $(".settingDropDown").toggleClass("active");
    });
}

//************************ LogOut Function  *********************************//

function LogOut() {
    // alert(".............!");
    window.localStorage.removeItem("token");
    window.localStorage.removeItem("landing_page");

    window.location.href = "../signin/index.html";
}

//************************ showSidebar Function  *********************************//

function showSidebar() {
    $("#sideMenuBar").html("");
    var html = `
    <aside class="sidenav navbar navbar-vertical navbar-expand-xs border-0 border-radius-xl my-3 fixed-start ms-3   bg-gradient-dark" id="sidenav-main">
        <div class="sidenav-header">
            <i class="fas fa-times p-3 cursor-pointer text-white opacity-5 position-absolute end-0 top-0 d-none d-xl-none" aria-hidden="true" id="iconSidenav"></i>
            <a class="navbar-brand m-0" href="../CVBU/index.html" target="_blank">
                <img src="../../assets/img/logo-ct.png" class="navbar-brand-img h-100" alt="main_logo">
                <span class="ms-1 font-weight-bold text-white">Q-DAS Quality System</span>
            </a>
        </div>
        <hr class="horizontal light mt-0 mb-2">
        <div class="collapse navbar-collapse  w-auto " id="sidenav-collapse-main">
            <ul class="navbar-nav" id="ListItem">
                
            </ul>
        </div>
        <div class="sidenav-footer position-absolute w-100 bottom-0 ">
            <div class="mx-3">
                <img src="../../assets/img/logos/tata_motors_logo.png" style="height:auto; width:100%" />
                <ref>
                    <a class="btn bg-gradient-primary mt-4 w-100" href="https://www.creative-tim.com/product/material-dashboard-pro?ref=sidebarfree" type="button">Upgrade to pro</a>
                </ref>
            </div>
        </div>
    </aside>`;

    $("#sideMenuBar").append(html);

    navData.forEach((ele) => {
        html = ` 
<li class="nav-item" PERMISSION_CODE=${ele.permission_code}>
<a class="nav-link text-white " href="../${ele.navPath}/index.html">
<div class="text-white text-center me-2 d-flex align-items-center justify-content-center">
   <img src="../../img/icons/${ele.imgPath}"    width="20" alt="">

</div>
<span class="nav-link-text ms-1">${ele.title}</span>
</a>
</li>

`;
        $("#ListItem").append(html);
    });

    for (i = 0; i < navData.length; i++) {
        if (sessionStorage.getItem("page") == navData[i].title) {
          
            $(`#ListItem li:nth-child(${i + 1}) a`).addClass(
                "active bg-gradient-primary"
            );
        }
    }
}

//************************ updateUserDetails Function  *********************************//


function updateUserDetails() {
    var userObj = {
        username: "",
        role_name: "",
        plant_location: "",
    };

    var getCall = new ApiPost(`/UpdateUser`, userObj);
    getCall.onSuccess = function onSuccess(result) {
        if (result > 0) {
            alert("Data is updated successfully!!!");
        } else {
            alert("Error in updation!!!");
        }
    };
    getCall.call();
}
