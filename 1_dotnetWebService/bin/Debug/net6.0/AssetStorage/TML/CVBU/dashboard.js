var quadrant;
ShowPageDetails("page", "CVBU-Overview");


//**************************** Document Ready function *********************************//
$(document).ready(function () {
  var getCall = new ApiGet(`/userDetails`);
  getCall.onSuccess = function onSuccess(result) {
    ProcessRefresh(result);
  };
  getCall.call();

  $("nav li a").click(function () {
    $(this).parent().siblings().children().removeClass("active");
    $(this).addClass("active");
  });
});

//**************************** ProcessRefresh function *********************************//

function ProcessRefresh(UserData) {
  var getCall = new ApiGet(`/Test_CVBU_quadrant`);
  getCall.onSuccess = function onSuccess(result) {
    console.log("/Test_CVBU_quadrant", result);

    CVBU_summary(result, UserData);
  };
  getCall.call();

  var getCall = new ApiGet(`/Test_plant_quadrant`);
  getCall.onSuccess = function onSuccess(result) {
    console.log("/Test_plant_quadrant", result);
    var groupedData = {};

    result.forEach(function (item) {
      var plantName = item.plant;
      if (!groupedData[plantName]) {
        groupedData[plantName] = [];
      }
      groupedData[plantName].push(item);
    });

    for (var plantName in groupedData) {
      var plantData = groupedData[plantName];
      let i = Object.keys(groupedData).indexOf(plantName);

      plant_summary(plantData, i);
    }
  };
  getCall.call();
}

//**************************** CVBU_summary function *********************************//

function CVBU_summary(quadrant, UserData) {
  Cvbuquad = {
    quad1: quadrant[0].plantQuad1,
    quad2: quadrant[0].plantQuad2,
    quad3: quadrant[0].plantQuad3,
    quad4: quadrant[0].plantQuad4,
    plants: quadrant[0].plant,
  };
   var cvbu_summary = `<div class="card">
   
    <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
      <div class="bg-tata-title shadow-primary border-radius-lg pt-4 pb-3">
        <h6 class="text-white text-capitalize ps-3">CVBU</h6>
      </div>
    </div>
   
    <div class="row mt-2 px-4 align-items-center">
    
    
      <div class="col-lg-6 "> 
     
          <div class="riskMatrix_tata  mt-3 text-center">
          ${RiskMatrix_container(Cvbuquad, false)}
     
    </div>
    <div class="col-lg-6 col-md-6 mt-2  ">
      
        <div class="card-header p-0 position-relative  z-index-2 bg-transparent">
          <div class=" bg-gradient-tata shadow-success border-radius-lg py-2 pe-1">
            <div class="chart">
              <canvas id="chart-line" class="chart-canvas" height="250"></canvas>
            </div>
          </div>  
        
      </div>
      <div class="card-body pt-3 px-2 pb-0">
        <h6 class="mb-0 "> CTQ Trailing Trend
      </h6>
        <p class="text-sm ">CTQ Monthly Performance</p>
        <hr class="dark horizontal m-0">
        
      </div>
    </div>
    </div>
    
        <div id="CTQ_Count" class="px-2"> </div>
     </div>
    
    
    `;

  $("#CVBU_container").append(cvbu_summary);

  CTQ_Details(UserData);
  var result = GetPlantData(quadrant);

  LineChart(result, `chart-line`);
  var getCall = new ApiGet(`/Test_waterfall_dia`);
  getCall.onSuccess = function onSuccess(result) {

    WaterFlowChart(result);
  };
  getCall.call();
}

//**************************** plant_summary function *********************************//

function plant_summary(quadrant, i) {
  Plantquad = {
    quad1: quadrant[0].plantQuad1,
    quad2: quadrant[0].plantQuad2,
    quad3: quadrant[0].plantQuad3,
    quad4: quadrant[0].plantQuad4,
    plants: quadrant[0].plant,
  };

  var html = `
        <div class="card plant_card">
            <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
                <a class="nav-link text-white" href="../plantStatus/index.html?plant=${
                  Plantquad.plants
                }&quad1=${Plantquad.quad1}&quad2=${Plantquad.quad2}&quad3=${
    Plantquad.quad3
  }&quad4=${Plantquad.quad4}">
                    <div class="bg-plant-title shadow-primary border-radius-lg pt-4 pb-3 shadow-primary border-radius-lg pt-4 pb-3">
                        <h6 class="text-white text-capitalize ps-3">${
                          Plantquad.plants
                        }</h6>
                    </div>
                </a>
            </div>
            <div class="row mt-4">
                <div class="col-lg-12 px-4">
                    <div class="riskMatrix_tata text-center">
                        ${RiskMatrix_container(Plantquad, true)}
                    </div>
                </div>
                <div class="col-lg-12 col-md-6 ">
                    <div class="card-header p-0 position-relative mx-3 z-index-2 bg-transparent">
                        <div class="bg-gradient-tata shadow-success border-radius-lg py-3 pe-1">
                            <div class="chart">
                                <canvas id="chart-line${
                                  i + 1
                                }" class="chart-canvas" height="230"></canvas>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <h6 class="mb-0">CTQ Trailing Trend</h6>
                        <p class="text-sm">CTQ Monthly Performance</p>
                        <hr class="dark horizontal m-0">
                    </div>
                </div>
            </div>
        </div>
    `;

  $("#plant_container").append(html);

  var result = GetPlantData(quadrant);

  LineChart(result, `chart-line${i + 1}`);
}

//**************************** WaterFlowChart function *********************************//

function WaterFlowChart(jsonData) {
  // Extract the values from the JSON data
  var lastCount = jsonData.last_count;
  var decrCount = jsonData.decr_count;
  var incCount = jsonData.inc_count;
  var totalCount = jsonData.cur_count;
  var lastmonth = jsonData.lastMonth;
  var curmonth = jsonData.currentMonth;

  var ctxBar = document.getElementById("canvas").getContext("2d");

  window.myBar = new Chart(ctxBar, {
    type: "bar",
    data: {
      labels: [lastmonth, "Decr", "Inc", curmonth],

      datasets: [
        {
          label: lastmonth,
          data: [[0, lastCount], [], []],
          backgroundColor: "#9DB2BF",
        },

        {
          label: "Decr",
          data: [[], [lastCount - decrCount, lastCount], []],
          backgroundColor: "#9DB2BF",
        },
        {
          label: "Inc",
          data: [[], [], [lastCount, lastCount + incCount]],
          backgroundColor: "#9DB2BF",
        },
        {
          label: curmonth,
          data: [[], [], [], [0, totalCount]],
          backgroundColor: "#9DB2BF",
        },
      ],
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          display: false,
          // position: "top",
        },
        title: {
          display: true,
          // text: "Chart.js Bar Chart",
        },
        tooltip: {
          callbacks: {
            label: function (context) {
              let label = context.dataset.label || "";
              if (label) {
                label += ": ";
              }
              if (context.formattedValue == "[null, null]") {
                return "";
              } else {
                label +=
                  JSON.parse(context.formattedValue)[1] -
                  JSON.parse(context.formattedValue)[0];
              }
              return label;
            },
          },
        },
      },
    },
  });
}

//**************************** CTQ_Details function *********************************//

function CTQ_Details(UserData) {
  var html = `
        <div class="card px-2 border ">
        <div class="row mt-2 px-4 align-items-center ">
    
        <div class="col-lg-12 ">
        <div class="row mt-2  align-items-center ">
        <div class="col-lg-6">
               <h6 class="mb-2  p-2 border-bottom border-dark "> Movement in Stable-Capable Quadrant </h6>

        </div>
        <div class="col-lg-6 ">
        <h6  class="mb-2  p-2 border-bottom border-dark "> Top-5 Degraded CTQ</h6>
        
        </div>
    </div>
        </div>
        <div class="col-lg-6 ">
               

         <div class="card mb-4 border">
               <div>
               <canvas id="canvas" height="300"></canvas>
           </div>
            </div>
           </div>
          <div class="col-lg-6 " style="height:400px; overflow:auto;">
         <table class="table " style="width:100%">
         <colgroup>
              <col style="width:50%">
              <col style="width:10%">
              <col style="width:10%">
              <col style="width:10%">
          </colgroup>
           <thead style="border-top: 0">
             <tr>
               <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 px-0">characteristics</th>
               <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 px-0 ">plant</th>
               <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 px-0">Prev</th>
               <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 px-0">Cur</th>
               
             </tr>
           </thead>
           <tbody id="Crit_Diffrence">
           
           </tbody></table>
          </div>
          
          </div>
          </div>
          <div class="row mt-2 px-4 align-items-center">
          
          
          <div class="col-lg-12 ">

        <div class="card mb-4">
  <h6> Plantwise Target CTQ v/s Actual CTQ </h6>
     <div class="table-responsive">

    <table class="table align-items-center mb-0">
    <colgroup>
    
  </colgroup>
      <thead>
        <tr>
          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Plant</th>
          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Target</th>
          <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Actual</th>
          ${
            sessionStorage.getItem("role_id") == 2
              ? `<th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Option</th>`
              : ""
          }
          
        </tr>
      </thead>
      <tbody id="Insp_Target">
      </tbody >
      </table>
      </div>
      </div>
     
           </div>`;

  $(`#CTQ_Count`).append(html);
  var getCall = new ApiGet(`/Crit_Diffrence`);
  getCall.onSuccess = function onSuccess(result) {
    //var filterResult =UserData.role_id==2 ? result : result.filter((ele) => ele.plant == UserData.plant_location)

    for (i = 0; i < result.length; i++) {
      Plant_CTQ(result[i]);
    }
    // CVBU_summary(result);
  };
  getCall.call();
  var getCall = new ApiGet(`/Test_plant_quadrant`);
  getCall.onSuccess = function onSuccess(result) {
    var groupedData = {};
    result.forEach(function (item) {
      var plantName = item.plant;
      if (!groupedData[plantName]) {
        groupedData[plantName] = [];
      }
      groupedData[plantName].push(item);
    });
    TargetCtq(groupedData);
  };
  getCall.call();
}

//**************************** TargetCtq function *********************************//

function TargetCtq(plantdata) {
  var getCall = new ApiGet(`/GetPlantData`);
  getCall.onSuccess = function onSuccess(output) {

    const result = [];

    for (const plantCode in output) {
      if (
        output.hasOwnProperty(plantCode) &&
        plantdata.hasOwnProperty(plantCode)
      ) {
        const actualVal = plantdata[plantCode][0].plantTotal;

        result.push({
          plant: plantCode,
          targetVal: output[plantCode],
          actualVal: actualVal,
        });
      }
    }

    $(`#Insp_Target`).html("");

    //
    result.forEach(function (ele) {

      Insp_Target(ele);
    });

  };
  getCall.call(); // This will display the modified data array
}

//**************************** Insp_Target function *********************************//

function Insp_Target(data) {
  var html = `  <tr>
          <td>
           
              <div class="d-flex flex-column justify-content-center">
                <h6 class="mb-0 text-xs">${data.plant}</h6>
              </div>
           
          </td>
          <td>
            <p class="text-xs font-weight-bold mb-0">${data.targetVal}</p>
           
          </td>
          <td class="align-middle text-center text-sm">
          <p class="text-xs font-weight-bold mb-0">${data.actualVal}</p>
          </td>
          ${
            sessionStorage.getItem("role_id") == 2
              ? `<td class="align-middle text-center text-sm">
          <div class="d-flex align-items-center justify-content-center" >
                   <img src="../../img/icons/edit.svg" class="cursor-pointer" alt="image" onclick="EditTarget('${data.plant}',${data.targetVal})"  data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg"  />
          </div>     
            </td>`
              : ""
          } 

        </tr>
       
      
     `;
  $(`#Insp_Target`).append(html);
}

//**************************** EditTarget function *********************************//

function EditTarget(plant, target) {
  $("#myModal").modal("show");

  $("#targetInput").val("");
  $("#targetInput").val(target);
  $("#TargetSave").click(function () {
    //    AddPlantsFromConfiguration http://localhost:7000/AddPlantsFromConfiguration?plant=PNE&targetValue=40
    var ins_Target = $("#targetInput").val();
    var getCall = new ApiGet(
      `/AddPlantsFromConfiguration?plant=${plant}&targetValue=${ins_Target}`
    );
    getCall.onSuccess = function onSuccess(result) {

    };
    getCall.call();
    $("#myModal").modal("hide");
  });
}

//**************************** Plant_CTQ function *********************************//

function Plant_CTQ(data) {
  html = `<tr>
              
        <td>

        <div class="d-flex flex-column justify-content-center">
            <h6 class="mb-0 text-sm">${data.parameter_desc}</h6>
         <p class="text-sm font-weight-normal text-secondary mb-0"><span class="" style="color:#000"> ${data.component} </span> <span class="text-dark fst-italic">  ${data.model}   </span> </p>

         </div>
   
         
        </td>
        <td class="align-middle">
         
        <div class="d-flex flex-column justify-content-center">
          <h6 class="mb-0 text-xs">${data.plant}</h6>
        </div>
     
        </td>
        <td class="align-middle text-center text-sm">
        <p class="text-xs font-weight-bold mb-0">${data.lastMonthcriticalIndex}</p>
        </td>
       
        <td class="align-middle text-center text-sm">
        <p class="text-xs font-weight-bold mb-0">${data.currentMonthcriticalIndex}</p>
        </td>

      </tr>`;
  $(`#Crit_Diffrence`).append(html);
}

//**************************** RiskMatrix_container function *********************************//

function RiskMatrix_container(data, status) {
  plant = status ? data.plants : "";

  var html = `
  
  <div class="boxDetail"></div>
  <div class="boxColumnDetail">
    <h6 class="mb-0 text-sm">Un-Stable</h6>
    <br>
   
  </div>
  <div class="boxColumnDetail">
    <h6 class="mb-0 text-sm">Stable</h6>
    <br>
   
  </div>
  <div class="boxRowDetail">
      <h6 class="mb-0 text-sm">Capable</h6>
      <br>
    
      <br>
    </div>
    <div class="box bg-gradient-info py-2" title="click for more details" onclick="DetailCharacteristics('Quad-II','${plant}')">
       
    <h5 class="mb-0 text-white ctqSpan">CTQ = ${data.quad2}</h5>
  </div>
  <div class="box bg-gradient-success py-2"  title="click for more details"  onclick="DetailCharacteristics('Quad-I','${plant}')">
   
    <h5 class="mb-0 text-white ctqSpan">CTQ = ${data.quad1}</h5>
  </div>
       
      <div class="boxRowDetail">
      <h6 class="mb-0 text-sm">Not-Capable</h6>
      <br>
    
    </div>
    <div class="box bg-gradient-danger py-2" title="click for more details"  onclick="DetailCharacteristics('Quad-III','${plant}')">
       
        <h5 class="mb-0 text-white ctqSpan">CTQ = ${data.quad3}</h5>
      </div>
      <div class="box bg-gradient-warning py-2" title="click for more details"  onclick="DetailCharacteristics('Quad-IV','${plant}')">
       
        <h5 class="mb-0 text-white ctqSpan">CTQ = ${data.quad4}</h5>
      </div>
      
      
    </div>
    <div class="p-3">
      <h6 class="mb-0 "> Risk Matrix </h6>
      <p class="text-sm "> Current 2x2 Matrix KPI status </p>
      <hr class="dark horizontal m-0">
      
    </div>`;
  return html;
  // $('.riskMatrix_tata').append(html);
}

//**************************** DetailCharacteristics function *********************************//

function DetailCharacteristics(quad, plant) {
  var url = `../CharacteristicExplorer/index.html?quad=${quad}&plant=${plant}`;

  window.location.href = url;
}
