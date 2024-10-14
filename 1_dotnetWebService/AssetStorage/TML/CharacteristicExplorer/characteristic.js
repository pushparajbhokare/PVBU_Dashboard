ShowPageDetails("page", "Characteristic Explorer");

//************************************* DataTable for Characteristics Table ***************************************8//

var dataTable = $("#char-data-table").DataTable({
    paging: true,
    searching: true,
    info: true,
    dom: '<"button-container"B>lfrtip',

    buttons: {
        dom: {
            button: {
                className: 'btn btn-sm btn-secondary'
            }
        },
        buttons: [
            {
                extend: 'copy',
                text: 'COPY',
                exportOptions: {
                    columns: [3, 4, 5]
                }
            },
            {
                extend: 'csv',
                text: 'CSV',
                exportOptions: {
                    columns: [3, 4, 5]
                }
            },
            {
                extend: 'print',
                text: 'PRINT',
                exportOptions: {
                    columns: [3, 4, 5]
                }
            }
        ]
    },
    columnDefs: [
        {
            render: function (data, type, full, meta) {
                return ` <div class="d-flex py-1">
                                     <div class="d-flex flex-column justify-content-center">
                                         <h6 class="mb-0 text-sm">${full.parameter_desc},</h6>
                                             <p class="text-sm font-weight-normal text-secondary mb-0"><span class="" style="color:#000"> ${full.component}, </span> <span class="text-dark fst-italic"> ${full.model},</span></p>
                                             <p class="text-sm font-weight-normal text-secondary mb-0"><span class="text-dark fst-italic"> ${full.part_number}, </span> <span class="text-dark fst-italic"> ${full.part_desc}</span></p>
                                      </div>
                                 </div>`;
            },
            targets: 0,
        },
        //{
        //    render: function (data, type, full, meta) {
        //        return ` <p class="text-sm font-weight-normal mb-0 ">${full.parameter_desc}</p>`;
        //    },
        //    targets: 1,
        //},
        //{
        //    render: function (data, type, full, meta) {
        //        return ` <p class="text-sm font-weight-normal mb-0 ">${full.component}</p>`;
        //    },
        //    targets: 2,
        //},
        //{
        //    render: function (data, type, full, meta) {
        //        return ` <p class="text-sm font-weight-normal mb-0 ">${full.model}</p>`;
        //    },
        //    targets: 3,
        //},
        //{
        //    render: function (data, type, full, meta) {
        //        return ` <p class="text-sm font-weight-normal mb-0 ">${full.part_number}</p>`;
        //    },
        //    targets: 4,
        //},
        //{
        //    render: function (data, type, full, meta) {
        //        return ` <p class="text-sm font-weight-normal mb-0 ">${full.part_desc}</p>`;
        //    },
        //    targets: 5,
        //},

        {
            render: function (data, type, full, meta) {
                return ` <p class="text-sm font-weight-normal mb-0 ">${full.operation}</p>`;
            },
            targets: 1,
        },
        {
            render: function (data, type, full, meta) {
                return ` ${getClassification(full.mclass)}`;
            },

            targets: 2,
        },

        {
            render: function (data, type, full, meta) {
                return `<p class="text-sm font-weight-normal mb-0 d-flex py-1 justify-content-center align-items-center">${full.xBar}</p>`;
            },
            targets: 3,
        },
        {
            render: function (data, type, full, meta) {
                return `<p class="text-sm font-weight-normal mb-0 d-flex  py-1 justify-content-center align-items-center">${full.range}</p>`;
            },
            targets: 4,
        },

        {
            render: function (data, type, full, meta) {
                return `  <p class="text-sm font-weight-normal mb-0 d-flex  py-1 justify-content-center align-items-center">${full.stdDev}</p>`;
            },
            targets: 5,
        },
        {
            render: function (data, type, full, meta) {
                if (full.quadrant == Objquadrants.QuadI) {
                    return `<div class="d-flex  py-1 justify-content-center align-items-center">
                              <p class="text-sm font-weight-normal mb-0 p-2">cp = ${full.potIndex}</p>
                              <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#43A047"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>
                      </div>`;
                } else if (full.quadrant == Objquadrants.QuadII) {
                    return `<div class="d-flex py-1 justify-content-center align-items-center">
                                   <p class="text-sm font-weight-normal mb-0 p-2">cp = ${full.potIndex}</p>
                                   <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#1A73E8"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>
                  </div>`;
                } else if (full.quadrant == Objquadrants.QuadIII) {
                    return `<div class="d-flex py-1 justify-content-center align-items-center">
                       <p class="text-sm font-weight-normal mb-0 p-2">cp = ${full.potIndex}</p>
                       <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#EF5350"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>
                   </div>`;
                } else if (full.quadrant == Objquadrants.QuadIV) {
                    return `<div class="d-flex py-1 justify-content-center align-items-center">
                       <p class="text-sm font-weight-normal mb-0 p-2">cp = ${full.potIndex}</p>
                        <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#FFA726"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>
                  </div>`;
                } else {
                    return `<div class="d-flex py-1 justify-content-center align-items-center">
                  <p class="text-sm font-weight-normal mb-0 p-2">cp = ${full.potIndex}</p>
                  <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#808080"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>
                  </div>`;
                }
            },
            targets: 6,
        },
        {
            render: function (data, type, full, meta) {
                if (full.quadrant == Objquadrants.QuadI) {
                    return `<div class="d-flex  py-1 justify-content-center align-items-center">
                                         <p class="text-sm font-weight-normal mb-0 p-2">cpk = ${full.criticalIndex}</p>
                                         <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#43A047"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>


                                  </div>`;
                } else if (full.quadrant == Objquadrants.QuadII) {
                    return `<div class="d-flex py-1 justify-content-center align-items-center">
                                         <p class="text-sm font-weight-normal mb-0 p-2">cpk = ${full.criticalIndex}</p>
                                         <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#1A73E8"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>

                                  </div>`;
                } else if (full.quadrant == Objquadrants.QuadIII) {
                    return `<div class="d-flex py-1 justify-content-center align-items-center">
                                         <p class="text-sm font-weight-normal mb-0 p-2">cpk = ${full.criticalIndex}</p>
                                         <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#EF5350"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>

                                  </div>`;
                } else if (full.quadrant == Objquadrants.QuadIV) {
                    return `<div class="d-flex py-1 justify-content-center align-items-center">
                                         <p class="text-sm font-weight-normal mb-0 p-2">cpk = ${full.criticalIndex}</p>
                                         <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#FFA726"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>

                                  </div>`;
                } else {
                    return `<div class="d-flex py-1 justify-content-center align-items-center">
          <p class="text-sm font-weight-normal mb-0 p-2">cpk = ${full.criticalIndex}</p>
          <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#808080"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>


   </div>`;
                }
            },
            targets: 7,
        },
        {
            render: function (data, type, full, meta) {
                return `<img style="position:relative;height:30px;width:30px;display:flex;justify-content:center;margin:auto;" src="../../assets/img/Ppk.PNG" class="cursor-pointer" alt="image"  onclick="GetChar_Chart(${full.part_id}, ${full.char_id}, '${full.plant_name}')"  data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg" >`;
            },
            targets: 8,
        },
    ],
    //Buttons: [
    //    //{
    //    //    Extends: 'copy',
    //    //    exportOptions: {
    //    //        columns: [3, 4, 5]
    //    //    }
    //    //},
    //    {
    //        Extends: 'csv',
    //        exportOptions: {
    //            columns: [3,4,5]
    //        }
    //    },
    //    //{
    //    //    Extends: 'print',
    //    //    exportOptions: {
    //    //        columns: [3, 4, 5]
    //    //    }
    //    //},
    //    'colvis'
    //],
    language: {
        paginate: {
            next: "&#8594;", // or '→'
            previous: "&#8592;", // or '←'
        },
    },

});
dataTable.buttons().container().appendTo('#export-buttons');

// Toggle button visibility when the "Show Export Options" button is clicked
$('#toggle-buttons').on('click', function () {
    $('#export-buttons').toggle();
});

// Optional: Styling for buttons
$(".button-wrapper").css({
    marginBottom: '10px'
});

//************************************* Document Ready Function ***************************************8//

$(document).ready(function () {
    var getCall = new ApiGet(`/userDetails`);
    getCall.onSuccess = function onSuccess(result) {
        PageRefresh(result);
    };
    getCall.call();
});

//************************************* Page Refresh Function ***************************************8//

function PageRefresh(userData) {
    var plantName = GetPlants().responseText;

    var getCall = new ApiGet(`/Test_char_info`);
    getCall.onSuccess = function onSuccess(result) {
        console.log("/Test_char_info", result);
        var area = (userData.area || "").split(",");
        var plant = (userData.plant_location || "").split(",");
        var filterData;
        if (userData.role_id == 2 || userData.role_id == 14) {
            filterData = result;
        } else if (userData.role_id == 4) {
            filterData = result.filter(
                (ele) => area.includes(ele.area) && plant.includes(ele.plant_name)
            );
        } else {
            filterData = result.filter((ele) => plant.includes(ele.plant_name));
        }
        populateDropdown(filterData);
        $("#BtnRefresh").on("click", function () {
            LoadingFun(filterData);
        });

        $("#Btnclear").on("click", function () {
            $("#plant_id").val("");
            $("#Area_id").val("").trigger('change');
            $("#model_id").val("");
            $("#component_id").val("");
            $("#oper_id").val("");
            $("#part_no_id").val("");
            $("#part_desc_id").val("");
            $("#Quad_id").val("");
            $("#Type_ctq").val("");
            LoadingFun(filterData);
        });

        LoadingFun(filterData);

        $(`#CheckedCTQ-C,#CheckedCTQ-M,#CheckedCTQ-E,#CheckedQDT-Other,#CheckedQDT-NA`).on("change", function () {
            LoadingFun(filterData);
        });
    };

    getCall.call();
}

//************************************* Refactor_datatable Function ***************************************8//

function Refactor_datatable(result) {
    dataTable.clear().draw();
    dataTable.rows.add(result);
    dataTable.columns.adjust().draw();
    var Countmatrix = TotalCount(result);
    CharRiskmatrix(Countmatrix);
}

//************************************* GetCharacteristicsInfo Function ***************************************8//

function GetCharacteristicsInfo(result) {
    var CTQ_C = $(`#CheckedCTQ-C`).is(":checked");
    var CTQ_M = $(`#CheckedCTQ-M`).is(":checked");
    var CTQ_E = $(`#CheckedCTQ-E`).is(":checked");
    var CTQ_Other = $(`#CheckedQDT-Other`).is(":checked");
    var CTQ_NA = $(`#CheckedQDT-NA`).is(":checked");
    $("#charRiskmatrix").html("");

    if (CTQ_C && CTQ_M && CTQ_E && CTQ_NA && CTQ_Other) {
        Refactor_datatable(result);
    } else if (CTQ_C && CTQ_M && CTQ_E && CTQ_Other) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.major || ele.mclass == ObjSetClasses.other || ele.mclass == ObjSetClasses.emission;
        });
        Refactor_datatable(filterArray);
    } if (CTQ_C && CTQ_M && CTQ_E) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.major || ele.mclass == ObjSetClasses.emission;
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_C && CTQ_M && CTQ_NA) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.major || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A");
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_C && CTQ_E && CTQ_NA) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.emission || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A");
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_M && CTQ_E && CTQ_NA) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.major || ele.mclass == ObjSetClasses.emission || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A");
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_C && CTQ_M && CTQ_NA && CTQ_Other) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.major || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A") || ele.mclass == ObjSetClasses.other;
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_C && CTQ_E && CTQ_NA && CTQ_Other) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.emission || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A") || ele.mclass == ObjSetClasses.other;
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_M && CTQ_E && CTQ_NA && CTQ_Other) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.major || ele.mclass == ObjSetClasses.emission || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A") || ele.mclass == ObjSetClasses.other;
        });
        Refactor_datatable(filterArray);
    }
    else if (CTQ_NA && CTQ_Other) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.other;
        });
        Refactor_datatable(filterArray);
    }
    else if (CTQ_C && CTQ_Other) {

        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.other;
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_E && CTQ_Other) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.emission || ele.mclass == ObjSetClasses.other;
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_M && CTQ_Other) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.major || ele.mclass == ObjSetClasses.other;
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_C && CTQ_NA) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A");
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_E && CTQ_NA) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.emission || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A");
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_M && CTQ_NA) {
        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.major || (ele.potIndex == "N/A" && ele.criticalIndex == "N/A");
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_C && CTQ_M) {

        var filterArray = result.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.major;
        });
        Refactor_datatable(filterArray);
    } else if (CTQ_C && CTQ_E) {
        var filterArray = result.filter(
            (ele) => ele.mclass == ObjSetClasses.critical || ele.mclass == ObjSetClasses.emission
        );
        Refactor_datatable(filterArray);
    } else if (CTQ_M && CTQ_E) {
        var filterArray = result.filter(
            (ele) => ele.mclass == ObjSetClasses.major || ele.mclass == ObjSetClasses.emission
        );
        Refactor_datatable(filterArray);
    } else if (CTQ_C) {

        var filterArray = result.filter((ele) => ele.mclass == ObjSetClasses.critical);
        Refactor_datatable(filterArray);
    } else if (CTQ_M) {
        var filterArray = result.filter((ele) => ele.mclass == ObjSetClasses.major);
        Refactor_datatable(filterArray);
    } else if (CTQ_E) {
        var filterArray = result.filter((ele) => ele.mclass == ObjSetClasses.emission);
        Refactor_datatable(filterArray);
    } else if (CTQ_NA) {
        var filterArray = result.filter((ele) => (ele.potIndex == "N/A" && ele.criticalIndex == "N/A"));
        Refactor_datatable(filterArray);
    } else if (CTQ_Other) {
        var filterArray = result.filter((ele) => ele.mclass == "Other");
        Refactor_datatable(filterArray);
    }
    //else {
    // console.log("other");
    // var filterArray = result.filter((ele) => ele.mclass == "Other");
    // Refactor_datatable(filterArray);
    //}
}

//************************************* Loading Function ***************************************8//

function LoadingFun(result) {
    var area = $("#Area_id :selected").val();
    var component = $("#component_id :selected").val();
    var model = $("#model_id :selected").val();
    var oper = $("#oper_id :selected").val();
    var quad = $("#Quad_id :selected").val();
    var plant = $("#plant_id :selected").val();
    var partNo = $("#part_no_id :selected").val();
    var partDesc = $("#part_desc_id :selected").val();

    var filteredResult = result.filter(function (ele) {
        var p = !plant || ele.plant_name === plant;
        var a = !area || ele.area === area;
        var c = !component || ele.component === component;
        var m = !model || ele.model === model;
        var o = !oper || ele.operation === oper;
        var q = !quad || ele.quadrant === quad;
        var p_no = !partNo || ele.part_number === partNo;
        var p_desc = !partDesc || ele.part_desc === partDesc;

        return p && a && c && m && o && q && p_no && p_desc;
    });
    SetClassStatus(filteredResult);
    GetCharacteristicsInfo(filteredResult);

    $(`#CheckedCTQ-C,#CheckedCTQ-M,#CheckedCTQ-E,#CheckedQDT-Other,#CheckedQDT-Other`).on("change", function () {
        GetCharacteristicsInfo(filteredResult);
    });
}

//************************************* Populate Dropdown Function ***************************************8//

populateDropdown = (result) => {
    var area = getQueryStringValue("area");
    var comp = getQueryStringValue("comp");
    var model = getQueryStringValue("model");
    var oper = getQueryStringValue("oper");
    var plant = getQueryStringValue("plant");
    var quad = getQueryStringValue("quad");
    var part_desc = getQueryStringValue("part_desc");
    var part_number = getQueryStringValue("part_no");

    var plant_arr = [];
    $("#plant_id").html("");

    //const areaArr = [...new Set(result.map((x) => x.area))];
    $("#Area_id").html(`<option value="">All</option>`);
    $("#component_id").html(`<option value="">All</option>`);
    $("#model_id").html(`<option value="">All</option>`);
    $("#plant_id").html(`<option value="">All</option>`);

    var plant_arr = [...new Set(result.map((x) => x.plant_name))];

    plant_arr.forEach((ele, index) => {
        $("#plant_id").append(
            `<option ${plant == ele && "selected"}   value="${ele}">${ele}</option>`
        );
    });


    selectArea(result, $("#Area_id").val());
    // After choosing country, showing the state
    $("#plant_id").on("change", function () {
        // Reset the state array and drop down
        selectArea(result, this.value);
    });
    function selectArea(result, plant_value) {
        var plant_name = $("#plant_id").val();
        filterArray = [];
        var areaArr = [];

        var Quadarr = [
            Objquadrants.QuadI,
            Objquadrants.QuadII,
            Objquadrants.QuadIII,
            Objquadrants.QuadIV,

        ];
        $("#Area_id").html("");
        $("#component_id").html("");
        $("#model_id").html("");
        $("#oper_id").html("");
        $("#part_no_id").html("");
        $("#part_desc_id").html("");
        $("#Area_id").html(`<option value="">All</option>`);
        $("#component_id").html(`<option value="">All</option>`);
        $("#model_id").html(`<option value="">All</option>`);
        $("#oper_id").html(`<option value="">All</option>`);
        $("#Quad_id").html(`<option style="background-color:white;color:black" value="">All</option>`);
        $("#part_no_id").html(`<option value="">All</option>`);
        $("#part_desc_id").html(`<option value="">All</option>`);

        var riskquad = quadarantsNew(quad);
        Quadarr.forEach((ele, index) => {
            var quad_color = '';
            if (ele === 'Stable & Capable') {
                quad_color = 'background-color:#6dc96f;color:white';
            } else if (ele === 'Un-Stable & Capable') {
                quad_color = 'background-color:#4296f1;color:white';
            } else if (ele === 'Un-Stable & Un-Capable') {
                quad_color = 'background-color:#f8605a;color:white';
            } else if (ele === 'Stable & Un-Capable') {
                quad_color = 'background-color:#fdb861;color:white';
            }
            $("#Quad_id").append(
                `<option style="${quad_color}" ${riskquad == ele && "selected"
                }   value="${ele}">${ele}</option>`
            );
        });
        filterArray = result;
        if (plant_name) {
            filterArray = filterArray.filter(v => v.plant_name == plant_name);
        }

        filterArray.forEach((ele, idx) => {
            areaArr.push(ele.area);

        }); // End of data forEach function

        areaArr = [...new Set(areaArr.map((x) => x))];
        areaArr.forEach((ele, index) => {
            $("#Area_id").append(
                `<option ${area == ele && "selected"} value="${ele}">${ele}</option>`
            );
        });
        loadSelected(result, $("#Area_id").val());
        $("#Area_id").on("change", function () {
            loadSelected(result, this.value);
        });


        function loadSelected(result, area_value) {
            var plant_name = $("#plant_id").val();

            var area_name = $("#Area_id").val();

            var modelArr = [];
            $("#model_id").html("");
            $("#model_id").html(`<option value="">All</option>`);
            filterArray = result;
            if (plant_name) {
                filterArray = filterArray.filter(v => v.plant_name == plant_name);
            }
            if (area_name) {
                filterArray = filterArray.filter(v => v.area == area_name);
            }
            filterArray.forEach((ele, idx) => {
                modelArr.push(ele.model);
                //if (ele.area == area_value) {
                //    modelArr.push(ele.model);
                //} else {
                //    modelArr.push(ele.model);
                //}
            }); // End of data forEach function

            modelArr = [...new Set(modelArr.map((x) => x))];

            modelArr.forEach((ele, index) => {
                $("#model_id").append(
                    `<option ${model == ele && "selected"} value="${ele}">${ele}</option>`
                );
            });

        }

        selectComponent(result, $("#model_id").val());

        // After choosing the Component, show the population
        $("#model_id").on("change", function () {
            selectComponent(result, this.value);
        });
        function selectComponent(result, model) {
            var plant_name = $("#plant_id").val();
            var area_name = $("#Area_id").val();
            var model_name = $("#model_id").val();
            var filterArray = [];
            var compArr = [];

            $("#component_id").html("");
            $("#component_id").html(`<option value="">All</option>`);
            filterArray = result;
            if (plant_name) {
                filterArray = filterArray.filter(v => v.plant_name == plant_name);
            }
            if (area_name) {
                filterArray = filterArray.filter(v => v.area == area_name);
            }
            if (model_name) {
                filterArray = filterArray.filter(v => v.model == model_name);
            }

            //filterArray_component = result.filter(v => v.area === area_name && v.plant_name == plant_name && v.model == model_name);
            filterArray.forEach((ele, idx) => {
                compArr.push(ele.component);
                //if (ele.model == model) {
                //    compArr.push(ele.component);
                //}
            }); // End of data forEach function

            compArr = [...new Set(compArr.map((x) => x))];
            compArr.forEach((ele, index) => {
                $("#component_id").append(
                    `<option ${comp == ele && "selected"} value="${ele}">${ele}</option>`
                );
            });
            selectOperation(result, $("#component_id").val());
        }
        $("#component_id").on("change", function () {
            selectOperation(result, this.value);
        });

        function selectOperation(result, comp) {
            var plant_name = $("#plant_id").val();
            var area_name = $("#Area_id").val();
            var model_name = $("#model_id").val();
            var componet_name = $("#component_id").val();
            var filterArray_operation = [];
            var operArr = [];

            $("#oper_id").html("");
            //  $("#component_id").html(`<option value="">All</option>`);
            $("#oper_id").html(`<option value="">All</option>`);
            filterArray = result;
            if (plant_name) {
                filterArray = filterArray.filter(v => v.plant_name == plant_name);
            }
            if (area_name) {
                filterArray = filterArray.filter(v => v.area == area_name);
            }
            if (model_name) {
                filterArray = filterArray.filter(v => v.model == model_name);
            }
            if (componet_name) {
                filterArray = filterArray.filter(v => v.component == componet_name);
            }

            //filterArray_operation = result.filter(v => v.area === area_name && v.plant_name == plant_name && v.model == model_name && v.component == componet_name);
            filterArray.forEach((ele, idx) => {
                operArr.push(ele.operation);
                //if (ele.component == comp) {
                //    operArr.push(ele.operation);
                //}
            }); // End of data forEach function
            operArr = [...new Set(operArr.map((x) => x))];
            operArr.forEach((ele, index) => {
                $("#oper_id").append(
                    `<option ${oper == ele && "selected"} value="${ele}">${ele}</option>`
                );
            });
            selectPart_no(result, $("#oper_id").val());

        }

        $("#oper_id").on("change", function () {
            selectPart_no(result, this.value);
        });

        function selectPart_no(result, oper) {
            var plant_name = $("#plant_id").val();
            var area_name = $("#Area_id").val();
            var model_name = $("#model_id").val();
            var componet_name = $("#component_id").val();
            var operation_name = $("#oper_id").val();
            var filterArray = [];
            var part_numberArr = [];

            $("#part_no_id").html("");

            $("#part_no_id").html(`<option value="">All</option>`);
            filterArray = result;

            if (plant_name) {
                filterArray = filterArray.filter(v => v.plant_name == plant_name);
            }
            if (area_name) {
                filterArray = filterArray.filter(v => v.area == area_name);
            }
            if (model_name) {
                filterArray = filterArray.filter(v => v.model == model_name);
            }
            if (componet_name) {
                filterArray = filterArray.filter(v => v.component == componet_name);
            }
            if (operation_name) {
                filterArray = filterArray.filter(v => v.operation == operation_name);
            }
            //filterArray_part_no = result.filter(v => v.area === area_name && v.plant_name == plant_name && v.model == model_name && v.component == componet_name && v.operation == operation_name);
            filterArray.forEach((ele, idx) => {
                part_numberArr.push(ele.part_number);
                //if (ele.operation == oper) {
                //    part_numberArr.push(ele.part_number);
                //}
            }); // End of data forEach function
            part_numberArr = [...new Set(part_numberArr.map((x) => x))];
            part_numberArr.forEach((ele, index) => {
                $("#part_no_id").append(
                    `<option ${part_number == ele && "selected"} value="${ele}">${ele}</option>`
                );
            });
            selectPart_desc(result, $("#part_no_id").val());

        }

        $("#part_no_id").on("change", function () {
            selectPart_desc(result, $("#part_no_id").val());
        });

        function selectPart_desc(result, part_no) {
            var plant_name = $("#plant_id").val();
            var area_name = $("#Area_id").val();
            var model_name = $("#model_id").val();
            var componet_name = $("#component_id").val();
            var operation_name = $("#oper_id").val();
            var part_no_name = $("#part_no_id").val();
            var filterArray = [];

            var part_descArr = [];

            $("#part_desc_id").html("");

            $("#part_desc_id").html(`<option value="">All</option>`);
            filterArray = result;

            if (plant_name) {
                filterArray = filterArray.filter(v => v.plant_name == plant_name);
            }
            if (area_name) {
                filterArray = filterArray.filter(v => v.area == area_name);
            }
            if (model_name) {
                filterArray = filterArray.filter(v => v.model == model_name);
            }
            if (componet_name) {
                filterArray = filterArray.filter(v => v.component == componet_name);
            }
            if (operation_name) {
                filterArray = filterArray.filter(v => v.operation == operation_name);
            }
            if (part_no_name) {
                filterArray = filterArray.filter(v => v.part_number == part_no_name);
            }


            console.log(filterArray);

            filterArray.forEach((ele, idx) => {
                part_descArr.push(ele.part_desc);
                //if (ele.part_number == part_no) {
                //    part_descArr.push(ele.part_desc);
                //}
            }); // End of data forEach function
            part_descArr = [...new Set(part_descArr.map((x) => x))];
            part_descArr.forEach((ele, index) => {
                $("#part_desc_id").append(
                    `<option ${part_desc == ele && "selected"} value="${ele}">${ele}</option>`
                );
            });


        }



    }

}


//************************************* SetClassStatus Function ***************************************8//

function SetClassStatus(array) {
    $("#SetClassStatus").html("");

    const counts = {
        "CTQ-C": 0,
        "CTQ-M": 0,
        "CTQ-E": 0,
        "CTQ-Other": 0,
        "QDT-NA": 0,
    };
    console.log('ctq array');
    console.log(array);
    console.log('obj det class');
    console.log(ObjSetClasses);
    array.forEach((item) => {
        if (item.mclass === ObjSetClasses.emission && item.criticalIndex !== "N/A" & item.potIndex !== "N/A") {
            counts["CTQ-E"]++;
        } else if (item.mclass === ObjSetClasses.major && item.criticalIndex !== "N/A" & item.potIndex !== "N/A") {
            counts["CTQ-M"]++;
        } else if (item.mclass === ObjSetClasses.critical && item.criticalIndex !== "N/A" & item.potIndex !== "N/A") {
            counts["CTQ-C"]++;

        } else if (item.mclass === ObjSetClasses.other && item.criticalIndex !== "N/A" & item.potIndex !== "N/A") {
            counts["CTQ-Other"]++;

        } else if (item.criticalIndex === "N/A" & item.potIndex === "N/A") {
            counts["QDT-NA"]++;
        }
    });
    console.log('count');
    console.log(counts);
    // return counts;
    Object.keys(counts).forEach(function (ele) {
        var html = ` <div class="d-flex justify-content-around  align-items-center">
        <div class="form-check form-switch">
        <input class="form-check-input" type="checkbox" id="Checked${ele}" checked>

      </div>
        <span class="text-black text-capitalize font-weight-bold ${ele}1"  style="margin:  5px 0 ; color: #2f2e2e; text-wrap:nowrap">${ele} </span>
        <span class = "wrap">
        <span class="${ele}1" style="width:100%; background:#FFA726; color: #fff !important;" ${counts[ele] == 0 ? "" : "hidden"
            }>N/A</span>
          <span class="${ele}" title="${counts[ele]}" style="width:${counts[ele] == 0 ? 0 : 100
            }%;">${counts[ele]}</span>

        </span>
        </div>`;
        $("#SetClassStatus").append(html);
        var checkId = `Checked${ele}`;
    });

    function Average(x, y) {
        return (x / (x + y)) * 100 || 0;
    }
}

//************************************* CharRiskmatrix Function ***************************************8//
function CharRiskmatrix(Countmatrix) {
    riskmatrix = `  <div class="riskMatrix_char text-center">
  <div class="box bg-gradient-info py-4" >

    <h5 class="mb-0 text-white ctqSpan " data-bs-toggle="tooltip"  title="Un-Stable & Capable">CTQ = ${Countmatrix.quad2}</h5>
  </div>
  <div class="box bg-gradient-success py-4" >

    <h5 class="mb-0 text-white ctqSpan" data-bs-toggle="tooltip"  title="Stable & Capable">CTQ =  ${Countmatrix.quad1}</h5>
  </div>


    <div class="box bg-gradient-danger py-4" >

        <h5 class="mb-0 text-white ctqSpan" data-bs-toggle="tooltip"  title="Un-Stable & Un-Capable">CTQ =  ${Countmatrix.quad3}</h5>
      </div>
      <div class="box bg-gradient-warning py-4" >

        <h5 class="mb-0 text-white ctqSpan" data-bs-toggle="tooltip"  title="Stable & Un-Capable">CTQ =  ${Countmatrix.quad4}</h5>
      </div>


</div>`;

    $("#charRiskmatrix").append(riskmatrix);
}

//************************************* GetChar_Chart Function ***************************************8//

function GetChar_Chart(part, char_id, plant) {
    $("#myModal").modal("show");
    $("#value_chart").prop("checked", true);

    $("#display").html("");
    var getCall = new ApiGet(
        `/GetCharCharts?partList=&partID=${part}&charID=${char_id}&Plant=${plant}`
    );
    getCall.onSuccess = function onSuccess(result) {
        chartdata = result;
        Show_CharChart();
    };
    //pushparaj
    getCall.onError = function onError(Error) {
        $("#display").html("");
        var html = `<div>License not available</div>`
        $("#display").html(html);
    }
    getCall.call();
}
$("#Type_ctq").on("change", function () {
    let type_value = this.value;
    if (type_value == "CTQ") {
        $(".QDT-NA").show();
        $(".QDT-NA1").show();
        $("#CheckedQDT-NA").show();

        $(".CTQ-C1").show();
        $(".CTQ-C").show();
        $("#CheckedCTQ-C").show();

        $(".CTQ-M1").show();
        $(".CTQ-M").show();
        $("#CheckedCTQ-M").show();

        $(".CTQ-E1").show();
        $(".CTQ-E").show();
        $("#CheckedCTQ-E").show();

        $(".CTQ-Other1").show();
        $(".CTQ-Other").show();
        $("#CheckedCTQ-Other").show();

        $(`#CheckedCTQ-C`).is(":checked");
        $(`#CheckedCTQ-M`).is(":checked");
        $(`#CheckedCTQ-E`).is(":checked");
        $(`#CheckedQDT-Other`).is(":checked");
        $(`#CheckedQDT-NA`).is(":checked");


        $('#CheckedCTQ-C').prop('checked', true).trigger('change');
        $('#CheckedCTQ-M').prop('checked', true).trigger('change');
        $('#CheckedCTQ-E').prop('checked', true).trigger('change');
        $('#CheckedQDT-Other').prop('checked', true).trigger('change');
        $('#CheckedQDT-NA').prop('checked', true).trigger('change');

    } else if (type_value == "CTQ_NA") {
        $(".QDT-NA1").show();
        $(".QDT-NA").show();
        $("#CheckedQDT-NA").show();

        $(".CTQ-C1").hide();
        $(".CTQ-C").hide();
        $("#CheckedCTQ-C").hide();

        $(".CTQ-M1").hide();
        $(".CTQ-M").hide();
        $("#CheckedCTQ-M").hide();

        $(".CTQ-E1").hide();
        $(".CTQ-E").hide();
        $("#CheckedCTQ-E").hide();

        $(".CTQ-Other1").show();
        $(".CTQ-Other").show();
        $("#CheckedCTQ-Other").show();

        $('#CheckedCTQ-C').prop('checked', false).trigger('change');
        $('#CheckedCTQ-M').prop('checked', false).trigger('change');
        $('#CheckedCTQ-E').prop('checked', true).trigger('change');
        $('#CheckedQDT-Other').prop('checked', true).trigger('change');
        $('#CheckedQDT-NA').prop('checked', true).trigger('change');

    } else {
        $(".QDT-NA1").show();
        $(".QDT-NA").show();
        $("#CheckedQDT-NA").show();

        $(".CTQ-C1").show();
        $(".CTQ-C").show();
        $("#CheckedCTQ-C").show();

        $(".CTQ-M1").show();
        $(".CTQ-M").show();
        $("#CheckedCTQ-M").show();

        $(".CTQ-E1").show();
        $(".CTQ-E").show();
        $("#CheckedCTQ-E").show();

        $(".CTQ-Other1").show();
        $(".CTQ-Other").show();
        $("#CheckedCTQ-Other").show();

        $('#CheckedCTQ-C').prop('checked', true).trigger('change');
        $('#CheckedCTQ-M').prop('checked', true).trigger('change');
        $('#CheckedCTQ-E').prop('checked', true).trigger('change');
        $('#CheckedQDT-Other').prop('checked', true).trigger('change');
        $('#CheckedQDT-NA').prop('checked', true).trigger('change');
    }
    //$('#Type_ctq option[value="CTQ"]').prop('selected', true).trigger('change');
});