//************************ AjAx Loader *********************************//


const Objquadrants = {
  QuadI: "Stable & Capable",
  QuadII: "Un-Stable & Capable",
  QuadIII: "Un-Stable & Un-Capable",
  QuadIV: "Stable & Un-Capable"
};
const ObjSetClasses = {
    critical: "CTQ-C", major: "CTQ-M", emission:"CTQ-E"
}

//************************ AjAx Loader *********************************//

isAuthorized();
try {
  $(document)
    .ajaxStart(function () {
      $("#AjaxLoader").show();
    })
    .ajaxStop(function () {
      $("#AjaxLoader").hide();
    });
} catch (e) {
  $(document).ajaxStop(function () {
    $("#AjaxLoader").hide();
  });
}
var chartdata;

var USER_PERMISSIONS = [];

const navData = [
  {
    id: 1,
    title: "CVBU-Overview",
    imgPath: "dashboard.svg",
    permission_code: "CVBU.VIEW_CVBU",
    navPath: "CVBU",
  },
  {
    id: 2,
    title: "Plant Status",
    imgPath: "inventory.svg",
    permission_code: "PLANT_STATUS.VIEW_PLANT_STATUS",
    navPath: "plantStatus",
  },
  {
    id: 3,
    title: "Area Explorer",
    imgPath: "area_chart.svg",
    permission_code: "AREA_EXPLORER.VIEW_AREA_EXPLORER",
    navPath: "AreaExplorer",
  },

  {
    id: 5,
    title: "Characteristic Explorer",
    imgPath: "data_exploration.svg",
    permission_code: "CHAR_EXPLORER.VIEW_CHAR_EXPLORER",
    navPath: "CharacteristicExplorer",
  },
  {
    id: 6,
    title: "Target Inspection",
    imgPath: "data_exploration.svg",
    permission_code: "TARGET_INSPECTION.VIEW_TARGET_INSPECTION",
    navPath: "TargetInspection",
  },
  // { id: 7, title: "2X2 Risk Matrix", imgPath: "table_view.svg", permission_code: "2X2_MATRIX.VIEW_2x2_MATRIX", navPath: "2x2matrix" }
];

//************************ Documnet Ready Function  *********************************//

$(document).ready(function () {});

function ShowPageDetails(page, pageName) {
  sessionStorage.setItem(page, pageName);
  showSidebar();
  GetUserDetails();
}

//************************ addHeader Function  *********************************//

function addHeader(data, details) {
  $("#displayNAV").html("");
  var showEle = data.title;
  var addNav = `
    <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl" id="navbarBlur" data-scroll="true">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
                        <li class="breadcrumb-item text-sm"><a class="opacity-5 text-dark" href="javascript:;">Pages</a></li>
                        <li class="breadcrumb-item text-sm text-dark active" aria-current="page">Dashboard</li>
                    </ol>
                    <h6 class="font-weight-bolder mb-0">${showEle}</h6>
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
                                <span class="d-sm-inline d-none" id="roleName">${details.role_name} : ${details.full_name} </span>
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
                <a href="../Permissions/manageUser.html"><li  style="display:none" >Configure Users</li></a>
                <a href="../Permissions/manageRole.html"><li  style="display:none" >Configure Roles</li></a>
                <a onclick="LogOut()"><li >Log out</li></a>
            </ul>
        </div>`;
  $("#displayNAV").append(addNav);

  CheckPermission(details.role_name);
  $("#showHiddenMenu").click(function () {
    $(".settingDropDown").toggleClass("active");
  });

  // $(".settingDropDown li").show();
}

//************************ showSidebar Function  *********************************//

function showSidebar() {
  $("#sidenav-main").html("");
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

  $("#sidenav-main").append(html);

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

//************************ getQueryStringValue Function  *********************************//

function getQueryStringValue(key) {
  return decodeURIComponent(
    window.location.search.replace(
      new RegExp(
        "^(?:.*[&\\?]" +
          encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") +
          "(?:\\=([^&]*))?)?.*$",
        "i"
      ),
      "$1"
    )
  );
}

function GetPlants() {
  var plant = "";
  var token = window.localStorage.getItem("token");
  var headers = {
    Authorization: "Bearer " + token,
  };

  var getCall = new ApiGet(`/GetPlants`, null, null, false);
  getCall.onSuccess = function onSuccess(result) {
    plant = result;
  };
  getCall.onComplete = function onComplete(result) {
    plant = result;
  };
  getCall.call();
  return plant;
}

//********************************************* Guage Chart*********************** */
function guage_Chart(obj) {
  var nok = obj == undefined ? 100 : (obj.nok / (obj.ok + obj.nok)) * 100;

  var val = obj == undefined ? 0 : (obj.ok / (obj.ok + obj.nok)) * 100;

  var dom1 = document.getElementById("guage_chart");
  var myChart = echarts.init(dom1, {
    renderer: "canvas",
    useDirtyRect: false,
  });
  var app = {};

  var option;
  option = {
    series: [
      {
        type: "gauge",
        axisLine: {
          lineStyle: {
            width: 12,
            color: [
              [nok / 100, "#f44335"],
              [1, " #4caf50"],
            ],
          },
        },
        pointer: {
          show: false,
          itemStyle: {
            color: "inherit",
          },
        },
        axisTick: {
          distance: -15,
          length: 8,
          lineStyle: {
            color: "#fff",
            width: 0.5,
          },
        },
        splitLine: {
          distance: -30,
          length: 30,
          lineStyle: {
            color: "#fff",
            width: 4,
          },
        },
        axisLabel: {
          color: "inherit",
          distance: 15,
          fontSize: 12,
        },
        detail: {
          fontSize: 15,
          valueAnimation: true,
          formatter: "{value}%",
          color: "inherit",
        },
        data: [
          {
            name: obj == undefined ? "no data found" : obj.area,
            value: Math.trunc(val),
            title: {
              fontWeight: "bold",
              fontSize: 14,
              offsetCenter: ["0", "0"],
              //color: '#9B728E'
            },
          },
        ],
        center: ["50%", "50%"],
        radius: "90%",
      },
    ],
  };
  if (option && typeof option === "object") {
    myChart.setOption(option);
  }

  window.addEventListener("resize", myChart.resize);
}

//********************************************* DataReducer Chart*********************** */

function DataReducer(obj) {
  isExists_comp = [];
  var uniqComponent = [...new Set(obj.map((ele) => ele.component))];
  var values = {};
  var inputs = obj.forEach(function (ele, i) {
    if (isExists_comp.includes(ele.component)) {
      values[ele.component] = {
        sr_no: i,
        component: ele.component,
        area: ele.area,
        ok: values[ele.component].ok + ele.ok,
        nok: values[ele.component].nok + ele.nok,
      };
    } else {
      isExists_comp.push(ele.component);
      values[ele.component] = {
        sr_no: i,
        component: ele.component,
        area: ele.area,
        ok: ele.ok,
        nok: ele.nok,
      };
    }
  });

  var data = Object.values(values);

  return data;
}
//************************************Show Alarm(Capsule)***************************************** *//
function showAlarm(obj, index) {
  //
  var op_status = obj.nok == 0 ? "ACCEPTED" : "REJECTED";

  var Date = getDateValue();
  var FromDate = Date.FromDate;
  var ToDate = Date.ToDate;
  var myvar = `

    <div class="col-sm-4">
                <div class="line-layout row"><span class="line-machine col-sm-4 px-0">${
                  obj.component
                }</span>
                 <div   class=" col-sm-2 overall-status"></div>


                  <div class = "col-sm-4 px-0 wrap cursor-pointer user-select-none" onclick="linkFor_model('${
                    obj.area
                  }','${obj.component}')">
                              <span class="split-green" title="${
                                obj.ok
                              }" style="width:${
    (obj.ok / (obj.ok + obj.nok)) * 100
  }%;">${obj.ok}</span>
                              <span class="split-red"  title="${
                                obj.nok
                              }" style="width:${
    (obj.nok / (obj.ok + obj.nok)) * 100
  }%;">${obj.nok}</span>

                                        </div>
              </div>
    </div>
`;
  // </a>

  $("#machine-layout").append(myvar);

  // write your code here

  var op_status_ele = $(".overall-status")[index];

  if (op_status == "REJECTED") {
    $(op_status_ele).css("background-color", "#f44335");
  } else if (op_status == "WARNING") {
    $(op_status_ele).css("background-color", "#fb8c00");
  } else {
    $(op_status_ele).css("background-color", "#4caf50");
  }
}

// ***************************************  time Chart **********************************************\\
function GetBarColor(status) {
  switch (status) {
    case "OK":
      return "#4caf50";
      break;
    case "NOK":
      return "#f44335";
      break;
    case "WARNING":
      return "#fb8c00";
      break;
  }
}

// ***************************************  showTimechart Chart **********************************************\\

function showTimechart(obj, sr_no, result) {
  //   " onclick="LinkFor_char('${obj.area}','${obj.component}', '${obj.model}','${obj.operation}')"
  var mytimeline = `
  <tr style="cursor: pointer;">
  <td class="text-align-center"> <a href="#""><div class="oper_desc">Model:${
    obj.model
  }</div>
    <div class=" timelineChart text-center" id="timeline-chart${sr_no}"></div></a>
  </td>
  <td class="align-items-center"><div class="desc_statustotal text-center">Total-${
    obj.ok + obj.nok
  }</div></td>
  <td class="align-items-center"><div class="desc_statusok text-center">OK-${
    obj.ok
  }</div></td>
  <td class="align-items-center"> <div class="desc_statusnotok text-center">NOK-${
    obj.nok
  }</div></td>

</tr>`;

  //
  //    <td class="align-items-center"><div class="desc_statustotal text-center">${obj.ftt}</div> </td>
  $("#plant-machine-desc").append(mytimeline);

  var FromDate = FromDate;
  var ToDate = ToDate;
  var index = sr_no;
  var oper = obj.operation;

  var filterArray = result.filter(
    (ele) =>
      ele.model == obj.model &&
      ele.operation == obj.operation &&
      ele.cplan == obj.cplan
  );
  Quality_StackChart(filterArray, index, oper, true, obj.cplan);
}

function randomStatus(obj) {
  var cntCOk = 0,
    cntNOk = 0;
  var cok = obj.conditionalOK;
  var nok = obj.nok;
  var status = [];
  var nokper = (nok / obj.total) * 100;

  for (var i = 0; i < obj.total; i++) {
    if (cntNOk < nok) {
      status.push("REJECTED");
      cntNOk += 1;
    } else if (cntCOk < cok) {
      status.push("WARNING");
      cntCOk += 1;
    } else {
      status.push("ACCEPTED");
    }
  }

  return status;
}
// ***************************************  Quality_StackChart Chart **********************************************\\

function Quality_StackChart(obj, index, oper, showYAxis, cplan) {
  var barseries = [];

  $.each(obj, function (key, item) {
    var dataitem = [];
    dataitem.push(1);
    var series = {
      name: item.part_serial_no,
      type: "bar",
      stack: "count",
      label: {
        show: false,
      },
      emphasis: {
        focus: "series",
      },
      data: dataitem,
      itemStyle: {
        color: GetBarColor(item.status),
        borderWidth: 0.5,
        borderColor: "#fff",
      },
    };
    barseries.push(series);
  });
  var dom = document.getElementById(`timeline-chart${index}`);
  var myChart = echarts.init(dom, null, {
    renderer: "svg",
    useDirtyRect: false,
  });

  var app = {};
  var option;

  option = {
    tooltip: {
      position: "top",
      confine: true,
      extraCssText: "z-index:1000",
      textStyle: {
        overflow: "breakAll",
        width: 40,
        fontSize: 10,
        height: 2,
      },
      formatter: function (params) {
        return params.marker + "PartId: " + params.seriesName;
      },
      axisPointer: {
        // Use axis to trigger tooltip
        type: "shadow", // 'shadow' as default; can also be 'line' or 'shadow'
      },
    },
    grid: {
      height: 60,
      left: 100,
      top: 0,
      right: 0,
      bottom: 0,
      containLabel: false,
    },
    xAxis: {
      type: "value",
      show: false,
    },
    yAxis: {
      type: "category",
      data: [oper],
      show: showYAxis,
    },
    series: barseries,
  };

  if (option && typeof option === "object") {
    myChart.setOption(option);
  }
  myChart.on("click", function (params) {
    var cplan = obj[0].cplan;
    var plant = obj[0].plant;
    PartReport(cplan, params.name, params.seriesName, plant);
  });

  window.addEventListener("resize", myChart.resize);
}

// ***************************************  PartReport Function **********************************************\\

function PartReport(cplan, oper, part, plant) {
  $("#myModal").modal("show");
  // $("#value_chart").prop("checked", true);
  $("#display").html("");
  $("#myModalLabel").html("");

  $("#myModalLabel").html(`Part Report for Serial Number :${part} `);
  var getCall = new ApiGet(
    `/GetPartReport?c_plan=${cplan}&operation=${oper}&serial_no=${part}&plant=${plant}`
  );
  getCall.onSuccess = function onSuccess(result) {
    var img_string = result.part_report_img;
    Base_Converter(img_string);
  };
  getCall.call();
}

// ***************************************  GetChar_Chart Chart **********************************************\\

function GetChar_Chart_archive(part, char_id) {
  var getCall = new ApiGet(
    `/GetCharCharts?partList=&partID=${part}&charID=${char_id}`
  );
  getCall.onSuccess = function onSuccess(result) {
    chartdata = result;
    Show_CharChart();
  };
  getCall.call();
}

//****************************** quadarants  Function   *************************************//

function quadarants(quad) {
  switch (quad) {
      case Objquadrants.QuadI:
          return "Quad-I";
          break;
      case Objquadrants.QuadII:
          return "Quad-II";
          break;
      case Objquadrants.QuadIII:
          return "Quad-III";
          break;
      case Objquadrants.QuadIV:
          return "Quad-IV";
          break;
  }
}

function quadarantsNew(quad) {
  switch (quad) {
      case "Quad-I":
          return Objquadrants.QuadI;
          break;
      case "Quad-II":
          return Objquadrants.QuadII;
          break;
      case "Quad-III":
          return Objquadrants.QuadIII;
          break;
      case "Quad-IV":
          return Objquadrants.QuadIV;
          break;
  }
}

//*************************************** Base64 Image Converter ******************************//
function Base_Converter(base64) {
  "use strict";

  function fixBinary(bin) {
    var length = bin.length;
    var buf = new ArrayBuffer(length);
    var arr = new Uint8Array(buf);
    for (var i = 0; i < length; i++) {
      arr[i] = bin.charCodeAt(i);
    }
    return buf;
  }

  var display = document.getElementById("display");
  display.innerHTML = display.innerHTML || "";
  function log(text) {
    display.innerHTML += "\n" + text;
  }

  var binary = fixBinary(atob(base64));
  var blob = new Blob([binary], { type: "image/jpeg" });
  var url = URL.createObjectURL(blob);
  $("display").html("");
  $("#display").html($("<img>").attr("src", url));

  var xhr = new XMLHttpRequest();
  xhr.open("GET", url);
  xhr.responseType = "arraybuffer";
  xhr.onreadystatechange = function () {
    if (xhr.readyState !== 4) {
      return;
    }

    var returnedBlob = new Blob([xhr.response], { type: "image/png" });
    var reader = new FileReader();
    reader.onload = function (e) {
      var returnedURL = e.target.result;
      var returnedBase64 = returnedURL.replace(/^[^,]+,/, "");
    };
    reader.readAsDataURL(blob); //Convert the blob from clipboard to base64
  };
  xhr.send();
}

function Show_CharChart() {
   //pushparaj
   if (chartdata == null) {
    $("#display").html("");
    var html = `<div>License not available</div>`
    $("#display").html(html);
  }
  else {
    if ($("#value_chart").is(":checked")) {
      $("#display").html("");
      Base_Converter(chartdata.valueChartImg);
    }
    if ($("#qcc_chart").is(":checked")) {
      $("#display").html("");
      Base_Converter(chartdata.qccChartImg);
      }
    if ($("#histo_chart").is(":checked")) {
      $("#display").html("");
      Base_Converter(chartdata.histChartImg);
      }
  }
}
//*********************************Class images******************* */
function getClassification(mclass) {
    if (mclass == ObjSetClasses.emission) {
    return `<div class="rounded  d-flex justify-content-center  align-items-center" style=" box-shadow: #cafbff 1.95px 1.95px 2.6px; border:1px solid #7ce7f0 ;background-color:#9bf6ff33"> <h6  class="d-flex mb-0 py-1 justify-content-center align-items-center" style="color:#31d2e2 !important" >E</h6></div>`;
  } else if (mclass == ObjSetClasses.major) {
    return `<div class="rounded  d-flex justify-content-center align-items-center" style=" box-shadow: #c7dcff 1.95px 1.95px 2.6px; border:1px solid #a8e5f5 ;background-color:#a0c4ff33"> <h6 class="d-flex mb-0 py-1 justify-content-center align-items-center"  style="color:#3475e1 !important" >M</h6></div>`;
  } else if (mclass == ObjSetClasses.critical) {
    return `<div class="rounded  d-flex justify-content-center align-items-center" style=" box-shadow: #ffadff 1.95px 1.95px 2.6px; border:1px solid #f9a4f9 ;background-color:#ffc6ff33"> <h6 class="d-flex mb-0 py-1 justify-content-center align-items-center " style="color:#f500f5 !important" >C</h6></div>`;
  }
  return `Others`;
}

function roundNumber(num) {
  return num;
}

/***************************************** GetPlantData Function *******************************************/

function GetPlantData(quadrant,filterPlant=false,plName="") {
  let result = [];

  const plantNames = Array.from(new Set(quadrant.map((item) => item.plant)));
  const Months = Array.from(new Set(quadrant.map((item) => item.month)));

  // Iterate over each unique plant name and process its data
  plantNames.forEach((plantName) => {
    // Filter data for the current plant
    const plantData = quadrant.filter((item) => item.plant == plantName);

    // Create an object for the current plant
    const plantObject = {
      plant: plantName,
      // month:Months,
      quadrant: [
        { name: "Quad-I", ctqHisto: [], month: Months.reverse() },
        { name: "Quad-II", ctqHisto: [], month: Months.reverse() },
        { name: "Quad-III", ctqHisto: [], month: Months.reverse() },
        { name: "Quad-IV", ctqHisto: [], month: Months.reverse() },
        { name: "Total", ctqHisto: [], month: Months.reverse() },
      ],
    };
    plantData.slice(0, 6).forEach((monthData) => {
      const { plantQuad1, plantQuad2, plantQuad3, plantQuad4, plantTotal } =
        monthData;
      plantObject.quadrant[0].ctqHisto.unshift(plantQuad1);
      plantObject.quadrant[1].ctqHisto.unshift(plantQuad2);
      plantObject.quadrant[2].ctqHisto.unshift(plantQuad3);
      plantObject.quadrant[3].ctqHisto.unshift(plantQuad4);
      plantObject.quadrant[4].ctqHisto.unshift(plantTotal);
    });

    // Calculate ctqCur for each quadrant
    plantObject.quadrant.forEach((quadrant) => {
      quadrant.ctqCur = quadrant.ctqHisto[5]; // Current month (index 5)
    });

    // Add the plantObject to the result
    result.push(plantObject);
  });
  if (filterPlant) {
    if (plName!=""){
      let temp = result.find(plObject=>plObject.plant==plName);
      console.log(temp);
      return temp.quadrant;
    }
    let names = [ "Quad-I", "Quad-II", "Quad-III", "Quad-IV", "Total" ];
    let total_plant = names.map (name=>{
      return {
        name:name,
        ctqHisto: Array(Months.length).fill(0),
        month:Months,
        ctqCur:0,
      };
    });
    result.forEach(plantObject=>{ //check each plant data
      plantObject.quadrant.forEach(quad_data=>{ //get each quadrant data of a plant
        let quad_num= names.indexOf(quad_data.name);
        if (quad_num!=-1){
          let new_ctq_total=
          total_plant[quad_num].ctqHisto.map((val,index)=>{
            return val+quad_data.ctqHisto[index];
          });
          total_plant[quad_num].ctqHisto=new_ctq_total;
          quad_data.ctqCur = new_ctq_total[new_ctq_total.length-1];
        };
        
      });
    }); 
    return total_plant;
  }
  var output = result[0].quadrant;
  return output;
}

/***************************************** LineChart Function *******************************************/

function LineChart(obj, chartID) {
  var ctx1 = document.getElementById("" + chartID + "").getContext("2d");
  var months = obj[0].month;
  var barseries = [];

  function GetLineColor(quad) {
    switch (quad) {
      case "Quad-I":
        return "#43A047";
        break;
      case "Quad-II":
        return "#1A73E8";
        break;
      case "Quad-III":
        return "#E53935";
        break;
      case "Quad-IV":
        return "#FB8C00";
        break;
      case "Total":
        return "#fff";
        break;
    }
  }

  $.each(obj, function (key, item) {
    var dataitem = [];
    var histoCTQ = item.ctqHisto;
    // dataitem.push(item.ctqHisto);
    var series = {
      label: item.name,
      tension: 0,
      borderWidth: 0,
      pointRadius: 5,
      pointBackgroundColor: GetLineColor(item.name),
      pointBorderColor: "transparent",
      borderColor: GetLineColor(item.name),
      borderColor: GetLineColor(item.name),
      borderWidth: 4,
      backgroundColor: "transparent",
      fill: true,
      data: histoCTQ,
      maxBarThickness: 6,
    };

    barseries.push(series);
  });

  // Animation-----
  const totalDuration = 1000;
  const delayBetweenPoints = totalDuration / barseries.length;
  const previousY = (ctx1) =>
    ctx1.index === 0
      ? ctx1.chart.scales.y.getPixelForValue(100)
      : ctx1.chart
          .getDatasetMeta(ctx1.datasetIndex)
          .data[ctx1.index - 1].getProps(["y"], true).y;
  const animation = {
    x: {
      type: "number",
      easing: "linear",
      duration: delayBetweenPoints,
      from: NaN, // the point is initially skipped
      delay(ctx1) {
        if (ctx1.type !== "data" || ctx1.xStarted) {
          return 0;
        }
        ctx1.xStarted = true;
        return ctx1.index * delayBetweenPoints;
      },
    },
    y: {
      type: "number",
      easing: "linear",
      duration: delayBetweenPoints,
      from: previousY,
      delay(ctx1) {
        if (ctx1.type !== "data" || ctx1.yStarted) {
          return 0;
        }
        ctx1.yStarted = true;
        return ctx1.index * delayBetweenPoints;
      },
    },
  };
  // Animation-----
  new Chart(ctx1, {
    type: "line",
    data: {
       labels: months,
      datasets: barseries,
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: true,

          labels: {
            color: "#fff",

            boxWidth: 10,
          },
        },
      },
      animation,

      interaction: {
        intersect: false,
        mode: "index",
      },
      scales: {
        y: {
          grid: {
            drawBorder: false,
            display: true,
            drawOnChartArea: true,
            drawTicks: false,
            borderDash: [5, 5],
            color: "rgba(255, 255, 255, .2)",
          },
          title: {
            display: true,
            text: "CTQ Counts",
            color: "#fff",
            font: {
              size: 14,
              weight: "bold",
              lineHeight: 1.2,
            },
            padding: { left: 0, right: 0, bottom: 0 },
          },
          ticks: {
            display: true,
            color: "#f8f9fa",
            padding: 10,
            font: {
              size: 14,
              weight: 300,
              family: "Roboto",
              style: "normal",
              lineHeight: 2,
            },
          },
        },

        x: {
          grid: {
            drawBorder: false,
            display: false,
            drawOnChartArea: false,
            drawTicks: false,
            borderDash: [5, 5],
          },
          title: {
            display: true,
            text: "Months",
            color: "#fff",
            font: {
              size: 14,
              weight: "bold",
              lineHeight: 1.2,
            },
            padding: { left: 0, right: 0, bottom: 0 },
          },
          ticks: {
            display: true,
            color: "#f8f9fa",
            padding: 10,
            font: {
              size: 14,
              weight: 300,
              family: "Roboto",
              style: "normal",
              lineHeight: 2,
            },
          },
        },
      },
    },
  });
}

/***************************************** GetUserDetails Function *******************************************/

function GetUserDetails() {
  var US_ID = sessionStorage.getItem("userID");
  $("#PlantTitle").html("");
  var getCall = new ApiGet(`/userDetails`);
  getCall.onSuccess = function onSuccess(result) {
    sessionStorage.setItem("role_id", result.role_id);
    USER_PERMISSIONS = [];
    $.each(result.us_permissions, function (key, item) {
      if (item.pr_granted) {
        //userPermissions.push(item.PR_CODE);
        USER_PERMISSIONS.push(item.permission_code);
      }
    });
    var html =
      result.plant_location && result.role_id != 2 && result.role_id != 14
        ? result.plant_location
        : "All";
    $("#PlantTitle").html(`Plant: ${html}`);

    for (i = 0; i < navData.length; i++) {
      if (sessionStorage.getItem("page") == navData[i].title) {
        addHeader(navData[i], result);
      }
    }

    CheckPermission();
  };

  getCall.call();

}

var ROLES = {
  ADMINISTRATOR: "Admin",
};

/***************************************** CheckPermission Function *******************************************/

function CheckPermission(role) {
  var $elements = $("[PERMISSION_CODE]");
  var USER_ROLE_CODE = role;
  if (USER_ROLE_CODE == ROLES.ADMINISTRATOR) {
    $(".settingDropDown li").show();
    return;
  }
  $.each($elements, function (key, item) {
    if ($.inArray($(item).attr("PERMISSION_CODE"), USER_PERMISSIONS) < 0) {
      $(item).hide();
      $(item).remove();
    }
  });
}
/***************************************** isAuthorized Function *******************************************/

function isAuthorized() {
  var pCode = $("body").attr("code");
  var getCall = new ApiPost(`/CheckUserAuthorization?permission_code=${pCode}`);
  getCall.onSuccess = function onSuccess(result) {
    if (!result) {
      history.back();
      alert("Access Denied....! Please Contact Admin.");
    }
  };
  getCall.onError = function onError(errormessage) {
    window.location.href = "../signin/index.html";
  };
  getCall.call();
}

/***************************************** LogOut Function *******************************************/

function LogOut() {

  window.localStorage.removeItem("token");
  window.localStorage.removeItem("landing_page");

  window.location.href = "../signin/index.html";
}

//*******************************************  Total Count of Matrix   **************************************** */
function TotalCount(test) {

  const specifiedQuadrants = [
    Objquadrants.QuadI,
    Objquadrants.QuadII,
    Objquadrants.QuadIII,
    Objquadrants.QuadIV,
  ];

  const quadrantCounts = {};
  specifiedQuadrants.forEach((quadrant, index) => {
    quadrantCounts[`quad${index + 1}`] = 0;
  });

  test.forEach((entry) => {
    const quadrant = entry.quadrant || "other";

    if (specifiedQuadrants.includes(quadrant)) {
      const index = specifiedQuadrants.indexOf(quadrant);
      quadrantCounts[`quad${index + 1}`]++;
    }
  });
  return quadrantCounts;
}
