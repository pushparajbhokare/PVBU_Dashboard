ShowPageDetails("page", "Characteristic Explorer");

//************************************* DataTable for Characteristics Table ***************************************8//

var dataTable = $("#char-data-table").DataTable({
  paging: true,

  searching: true,
  info: false,

  columnDefs: [
    {
      render: function (data, type, full, meta) {
        return ` <div class="d-flex py-1">
                                     <div class="d-flex flex-column justify-content-center">
                                         <h6 class="mb-0 text-sm">${full.parameter_desc}</h6>
                                             <p class="text-sm font-weight-normal text-secondary mb-0"><span class="" style="color:#000"> ${full.component} </span> <span class="text-dark fst-italic"> ${full.model}   </span> </p>
                                      </div>
                                 </div>`;
      },
      targets: 0,
    },

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
        } else if (full.quadrant ==  Objquadrants.QuadII) {
          return `<div class="d-flex py-1 justify-content-center align-items-center">
                                   <p class="text-sm font-weight-normal mb-0 p-2">cp = ${full.potIndex}</p>
                                   <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#1A73E8"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>
                  </div>`;
        } else if (full.quadrant ==  Objquadrants.QuadIII) {
          return `<div class="d-flex py-1 justify-content-center align-items-center">
                       <p class="text-sm font-weight-normal mb-0 p-2">cp = ${full.potIndex}</p>
                       <svg xmlns="http://www.w3.org/2000/svg" height="10" fill="#EF5350"  viewBox="0 0 512 512"><path d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8z"/></svg>
                   </div>`;
        } else if (full.quadrant ==  Objquadrants.QuadIV) {
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
        } else if (full.quadrant ==  Objquadrants.QuadIV) {
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
  language: {
    paginate: {
      next: "&#8594;", // or '→'
      previous: "&#8592;", // or '←'
    },
  },
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

    LoadingFun(filterData);

    $(`#CheckedCTQ-C,#CheckedCTQ-M,#CheckedCTQ-E`).on("change", function () {
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
  $("#charRiskmatrix").html("");

  if (CTQ_C && CTQ_M && CTQ_E) {
    Refactor_datatable(result);
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
  } else {
    var filterArray = result.filter((ele) => ele.mclass == "Other");
    Refactor_datatable(filterArray);
  }
}

//************************************* Loading Function ***************************************8//

function LoadingFun(result) {
  var area = $("#Area_id :selected").val();
  var component = $("#component_id :selected").val();
  var model = $("#model_id :selected").val();
  var oper = $("#oper_id :selected").val();
  var quad = $("#Quad_id :selected").val();
  var plant = $("#plant_id :selected").val();

  var filteredResult = result.filter(function (ele) {
    var p = !plant || ele.plant_name === plant;
    var a = !area || ele.area === area;
    var c = !component || ele.component === component;
    var m = !model || ele.model === model;
    var o = !oper || ele.operation === oper;
    var q = !quad || ele.quadrant === quad;

    return p && a && c && m && o && q;
  });
  SetClassStatus(filteredResult);
  GetCharacteristicsInfo(filteredResult);

  $(`#CheckedCTQ-C,#CheckedCTQ-M,#CheckedCTQ-E`).on("change", function () {
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

  var plant_arr = [];
  $("#plant_id").html("");

  const areaArr = [...new Set(result.map((x) => x.area))];
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

  areaArr.forEach((ele, index) => {
    $("#Area_id").append(
      `<option ${area == ele && "selected"} value="${ele}">${ele}</option>`
    );
  });

  loadSelected(result, $("#Area_id").val());

  // After choosing country, showing the state
  $("#Area_id").on("change", function () {
    // Reset the state array and drop down
    loadSelected(result, this.value);
  });


  function loadSelected(result, value) {
    var compArr = [];

    var Quadarr = [
    Objquadrants.QuadI,
    Objquadrants.QuadII,
    Objquadrants.QuadIII,
    Objquadrants.QuadIV,

    ];
    $("#component_id").html("");
    $("#model_id").html("");
    $("#component_id").html(`<option value="">All</option>`);
    $("#model_id").html(`<option value="">All</option>`);
    $("#oper_id").html(`<option value="">All</option>`);
    $("#Quad_id").html(`<option value="">All</option>`);

    var riskquad = quadarantsNew(quad);
    Quadarr.forEach((ele, index) => {
      $("#Quad_id").append(
        `<option ${
          riskquad == ele && "selected"
        }   value="${ele}">${ele}</option>`
      );
    });

    result.forEach((ele, idx) => {
      if (ele.area == value) {
        compArr.push(ele.component);
      } else {
        compArr.push(ele.component);
      }
    }); // End of data forEach function

    compArr = [...new Set(compArr.map((x) => x))];

    compArr.forEach((ele, index) => {
      $("#component_id").append(
        `<option ${comp == ele && "selected"} value="${ele}">${ele}</option>`
      );
    });
    selectModel(result, $("#component_id").val());
  }
  // After choosing the Component, show the population
  $("#component_id").on("change", function () {
    selectModel(result, this.value);
  });
  function selectModel(result, comp) {
    var modelArr = [];

    $("#model_id").html("");
    $("#model_id").html(`<option value="">All</option>`);

    result.forEach((ele, idx) => {
      if (ele.component == comp) {
        modelArr.push(ele.model);
      } else {
        modelArr.push(ele.model);
      }
    }); // End of data forEach function
    modelArr = [...new Set(modelArr.map((x) => x))];
    modelArr.forEach((ele, index) => {
      $("#model_id").append(
        `<option ${model == ele && "selected"} value="${ele}">${ele}</option>`
      );
    });
    selectOperation(result, $("#model_id").val());
  }
  $("#model_id").on("change", function () {
    selectOperation(result, this.value);
  });
  function selectOperation(result, comp) {
    var operArr = [];

    $("#oper_id").html("");
    //  $("#component_id").html(`<option value="">All</option>`);
    $("#oper_id").html(`<option value="">All</option>`);

    result.forEach((ele, idx) => {
      if (ele.model == comp) {
        operArr.push(ele.operation);
      } else {
        operArr.push(ele.operation);
      }
    }); // End of data forEach function
    operArr = [...new Set(operArr.map((x) => x))];
    operArr.forEach((ele, index) => {
      $("#oper_id").append(
        `<option ${oper == ele && "selected"} value="${ele}">${ele}</option>`
      );
    });
  }
};

//************************************* SetClassStatus Function ***************************************8//

function SetClassStatus(array) {
  $("#SetClassStatus").html("");

  const counts = {
    "CTQ-C": 0,
    "CTQ-M": 0,
    "CTQ-E": 0,
  };

  array.forEach((item) => {
    if (item.mclass === ObjSetClasses.emission) {
      counts["CTQ-E"]++;
    } else if (item.mclass === ObjSetClasses.major) {
      counts["CTQ-M"]++;
    } else if (item.mclass === ObjSetClasses.critical) {
      counts["CTQ-C"]++;
    }
  });

  // return counts;
  Object.keys(counts).forEach(function (ele) {
    var html = ` <div class="d-flex justify-content-around  align-items-center">
        <div class="form-check form-switch">
        <input class="form-check-input" type="checkbox" id="Checked${ele}" checked>

      </div>
        <span class="text-black text-capitalize font-weight-bold "  style="margin:  5px 0 ; color: #2f2e2e;">${ele} </span>
        <span class = "wrap">
        <span class="" style="width:100%; background:#FFA726; color: #fff !important;" ${
          counts[ele] == 0 ? "" : "hidden"
        }>N/A</span>
          <span class="${ele}" title="${counts[ele]}" style="width:${
      counts[ele] == 0 ? 0 : 100
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

    <h5 class="mb-0 text-white ctqSpan">CTQ = ${Countmatrix.quad2}</h5>
  </div>
  <div class="box bg-gradient-success py-4" >

    <h5 class="mb-0 text-white ctqSpan">CTQ =  ${Countmatrix.quad1}</h5>
  </div>


    <div class="box bg-gradient-danger py-4" >

        <h5 class="mb-0 text-white ctqSpan">CTQ =  ${Countmatrix.quad3}</h5>
      </div>
      <div class="box bg-gradient-warning py-4" >

        <h5 class="mb-0 text-white ctqSpan">CTQ =  ${Countmatrix.quad4}</h5>
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
