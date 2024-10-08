ShowPageDetails("page", "Plant Status");

//****************************** Document Ready Function  *************************************//


$(document).ready(function () {
    var plantName = `<h6 class="font-weight-bolder mb-0" id="PlantTitle"></h6>`;
    $("#Plant_name").append(plantName);
    var getCall = new ApiGet(`/userDetails`);
    getCall.onSuccess = function onSuccess(result) {
             processRefresh(result);
    };
    getCall.call();
});

//****************************** processRefresh Function  *************************************//

function processRefresh(UserData) {
    var getCall = new ApiGet(`/Test_char_info`);
    getCall.onSuccess = function onSuccess(result) {
        console.log("Test_char_info", result);

        var filterResult;
        var area = (UserData.area || '').split(',')
        var plant = (UserData.plant_location || '').split(',')



        if (getQueryStringValue('plant')) {
            filterResult =
                UserData.role_id == 2 || UserData.role_id == 14
                    ? result.filter((ele) => ele.plant_name == getQueryStringValue('plant'))
                    : result.filter((ele) => plant.includes(ele.plant_name));

        }
        else {
            filterResult =
                UserData.role_id == 2 || UserData.role_id == 14
                    ? result
                    : filterData = result.filter((ele) => plant.includes(ele.plant_name));

        }
        setClasses(filterResult, UserData);
        $(`#CheckedCTQ-C,#CheckedCTQ-M`).on("change", function () {
            setClasses(filterResult, UserData);
        });

        // DrawChart(FilterArray);
    };
    getCall.call();
}

//****************************** processRefresh Function  *************************************//

function Quad_Details(quad, data, UserData) {
    $("#Quad_container").html("");

    var plantText = getQueryStringValue("plant"); // ? getQueryStringValue('plant'):'All'
    if (plantText) {
        $("#Plant_name").html("");
        var plantName = `<h6 class="font-weight-bolder mb-0" id="PlantTitle"> Plant:${plantText} </h6>`;
        $("#Plant_name").append(plantName);
    }

    var html = `
    <div class="col-lg-3 col-md-4 mt-4 mb-4">
  <!-- Tabs nav -->
  <div class="nav tata-nav flex-column nav-pills nav-pills-custom mt-5"
       id="v-pills-tab"
       role="tablist"
       aria-orientation="vertical">
      <a class="nav-link mb-3 p-3 shadow active"
         id="v-pills-home-tab"
         data-toggle="pill"
         href="#v-pills-home"
         role="tab"
         aria-controls="v-pills-home"
         aria-selected="true">
          <span class="font-weight-bold small text-uppercase">${Objquadrants.QuadIII} = ${quad.quad3}</span>
      </a>

      <a class="nav-link tata-nav-link mb-3 p-3 shadow"
         id="v-pills-profile-tab"
         data-toggle="pill"
         href="#v-pills-profile"
         role="tab"
         aria-controls="v-pills-profile"
         aria-selected="false">
          <span class="font-weight-bold small text-uppercase"> ${Objquadrants.QuadIV} = ${quad.quad4}</span>
      </a>

      <a class="nav-link tata-nav-link mb-3 p-3 shadow"
         id="v-pills-messages-tab"
         data-toggle="pill"
         href="#v-pills-messages"
         role="tab"
         aria-controls="v-pills-messages"
         aria-selected="false">
          <span class="font-weight-bold small text-uppercase"> ${Objquadrants.QuadII} = ${quad.quad2}</span>
      </a>

      <a class="nav-link tata-nav-link mb-3 p-3 shadow"
         id="v-pills-settings-tab"
         data-toggle="pill"
         href="#v-pills-settings"
         role="tab"
         aria-controls="v-pills-settings"
         aria-selected="false">
          <span class="font-weight-bold small text-uppercase">${Objquadrants.QuadI} = ${quad.quad1}</span>
      </a>
  </div>
</div>

<div class="col-lg-9 col-md-9 mt-4 mb-4 card " id="PlantQuadrant">
  </div>
</div>`;

    $("#Quad_container").append(html);
    var plantOverCTQ = JSON.parse(Historical_PlantData().responseText);
    var Plantoverall = GetPlantData(plantOverCTQ, filterPlant=true,plantText);
    var filterQuad = FilterQuadrant(
        $(".tata-nav a").children().html(),
        Plantoverall
    );
    DrawChart(
        format_string($(".tata-nav a").children().html()),
        data,
        filterQuad, UserData
    );

    $(".tata-nav a").on("click", function () {
        $(`#PlantQuadrant`).html("");

        $(this).addClass("active").siblings().removeClass("active");
        $(this)
            .attr("aria-selected", "true")
            .siblings()
            .attr("aria-selected", "false");

        var quadrant = format_string($(this).children().html());
        var filterQuad = FilterQuadrant(quadrant, Plantoverall);

        DrawChart(quadrant, data, filterQuad, UserData);
    });
}

//****************************** processRefresh Function  *************************************//

function FilterQuadrant(quadrant, data) {
    var form_string = format_string(quadrant);
    var quad = quadarants(form_string);
    var filterArray = data.filter((ele) => ele.name == quad);
    return filterArray;
}

//****************************** processRefresh Function  *************************************//


function Historical_PlantData() {
    let overall = [];
    var token = window.localStorage.getItem("token");
    var headers = {
        Authorization: "Bearer " + token,
    };

    var getCall = new ApiGet(`/Test_plant_quadrant`, null, null, false);
    getCall.onSuccess = function onSuccess(result) {

        overall = result;
    };
    getCall.onComplete = function onComplete(result) {
        overall = result;
    };
    getCall.call();
    return overall;
}

//****************************** processRefresh Function  *************************************//

function format_string(string) {
    var res = string.split("=");
    var format = res[0].trim();
    var result = format.replace("&amp;", "&");
    return result;
}


//****************************** processRefresh Function  *************************************//

var data1;
function countOccurrences(data, property) {
    const counts = {};
    var quadrant;
    data.forEach((entry) => {
        const value = entry[property];
        quadrant = entry.quadrant;
        plant = entry.plant_name;

        if (!counts[value]) {
            counts[value] = 0;
        }

        counts[value]++;
    });

    var data1 = Object.keys(counts).map((key) => ({
        [property]: key,
        [`ctqCount`]: counts[key],
        quadrant,
        plant,
    }));
    var finalResult = data1.sort((a, b) => b.ctqCount - a.ctqCount);

    return finalResult;
}


//****************************** processRefresh Function  *************************************//


function setClasses(array, UserData) {
    $("#PlantQuadrant").html("");
    var CTQ_C = $(`#CheckedCTQ-C`).is(":checked");
    var CTQ_M = $(`#CheckedCTQ-M`).is(":checked");

    if (CTQ_C && CTQ_M) {
        var TotalCountabc = TotalCount(array);
        Quad_Details(TotalCountabc, array, UserData);
    } else if (CTQ_C) {
        var filterArray = array.filter((ele) => {
            return ele.mclass == ObjSetClasses.critical;
        });
        var TotalCountabc = TotalCount(filterArray);
        Quad_Details(TotalCountabc, filterArray, UserData);
    } else if (CTQ_M) {
        var filterArray = array.filter((ele) => {
            return ele.mclass == ObjSetClasses.major;
        });

        var TotalCountabc = TotalCount(filterArray);
        Quad_Details(TotalCountabc, filterArray, UserData);
    } else {
        var filterArray = array.filter((ele) => {
            return ele.mclass == ObjSetClasses.major && ele.mclass == ObjSetClasses.critical;
        });
        var TotalCountabc = TotalCount(filterArray);
        Quad_Details(TotalCountabc, filterArray, UserData);
    }
}

//****************************** processRefresh Function  *************************************//

function DrawChart(quadrant, result, filterQuad, UserData) {
    var FilterArray = result.filter((ele) => ele.quadrant == quadrant);


    html = `  <div class="tab-content" id="v-pills-tabContent">
  <div class="tab-pane fade show active"
       id="v-pills-home"
       role="tabpanel"
       aria-labelledby="v-pills-home-tab">
      <div class="row">
          <div class="col-lg-6 col-md-4 mt-4 mb-4">
              <div class="card-header p-0 position-relative mt-2 mx-3 z-index-2">
                 ${getQueryStringValue(`plant`) ? `<a href="../AreaExplorer/index.html?plant=${getQueryStringValue(`plant`)}">` : `<a href="../AreaExplorer/index.html">`}
                      <div class="bg-gradient-dark shadow-primary border-radius-lg py-2">
                          <h6 class="text-white text-capitalize text-center ps-3">
                              Area
                          </h6>
                      </div>
                  </a>
              </div>
              <div class="card-header p-0 position-relative mt-2 mx-3 z-index-2 bg-transparent">
                  <div class="bg-gradient-tata shadow-primary border-radius-lg py-3 pe-1">
                      <div class="chart">
                          <canvas id="chart-bars"
                                  class="chart-canvas"
                                  height="170"></canvas>
                      </div>
                  </div>
              </div>
          </div>
          <div class="col-lg-6 col-md-4 mt-4 mb-4">
              <div class="card-header p-0 position-relative mt-2 mx-3 z-index-2">
                  <div class="bg-gradient-dark shadow-primary border-radius-lg py-2">
                      <h6 class="text-white text-capitalize text-center ps-3">
                          Model
                      </h6>
                  </div>
              </div>
              <div class="card-header p-0 position-relative mt-2 mx-3 z-index-2 bg-transparent">
                  <div class="bg-gradient-tata shadow-primary border-radius-lg py-3 pe-1">
                      <div class="chart">
                          <canvas id="chart-bars1"
                                  class="chart-canvas"
                                  height="170"></canvas>
                      </div>
                  </div>
              </div>
          </div>
          <div class="col-lg-6 col-md-4 mt-4 mb-4">
              <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
                  <div class="bg-gradient-dark shadow-primary border-radius-lg py-2">
                      <h6 class="text-white text-center text-capitalize ps-3">
                          Component
                      </h6>
                  </div>
              </div>
              <div class="card-header p-0 position-relative mt-2 mx-3 z-index-2 bg-transparent">
                  <div class="bg-gradient-tata shadow-primary border-radius-lg py-3 pe-1">
                      <div class="chart">
                          <canvas id="chart-bars2"
                                  class="chart-canvas"
                                  height="170"></canvas>
                      </div>
                  </div>
              </div>
          </div>
          <div class="col-lg-6 col-md-4 mt-4 mb-4">
              <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
                  <div class="bg-gradient-dark shadow-primary border-radius-lg py-2">
                      <h6 class="text-white text-center text-capitalize ps-3">
                          Trailing Trend {All  CTQ Class}
                      </h6>
                  </div>
              </div>
              <div class="card-header p-0 position-relative mt-2 mx-3 z-index-2 bg-transparent">
                  <div class="bg-gradient-tata shadow-success border-radius-lg py-3 pe-1">
                      <div class="chart">
                          <canvas id="chart-line1"
                                  class="chart-canvas"
                                  height="170"></canvas>
                      </div>
                  </div>
              </div>
          </div>
      </div>
  </div>

`;
    $("#PlantQuadrant").append(html);
    const areaCounts = countOccurrences(FilterArray, "area");
    const modelCounts = countOccurrences(FilterArray, "model");
    const componentCounts = countOccurrences(FilterArray, "component");

    createAreaBarChart(areaCounts, UserData);
    createModelBarChart(modelCounts, UserData);
    createComponentBarChart(componentCounts, UserData);
    LineChart(filterQuad, "chart-line1");
    // CreateLineChart();
}

//****************************** processRefresh Function  *************************************//

function CreateLineChart() {
    var ctx3 = document.getElementById("chart-line1").getContext("2d");
    new Chart(ctx3, {
        type: "line",
        data: {
            labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun"],
            datasets: [
                {
                    label: "CTQ Count",
                    tension: 0,
                    borderWidth: 0,
                    pointRadius: 5,
                    pointBackgroundColor: "rgba(255, 255, 255, .8)",
                    pointBorderColor: "transparent",
                    borderColor: "rgba(255, 255, 255, .8)",
                    borderColor: "rgba(255, 255, 255, .8)",
                    borderWidth: 4,
                    backgroundColor: "transparent",
                    fill: true,
                    data: [50, 40, 300, 320, 500, 350, 200, 230, 500],
                    maxBarThickness: 6,
                },
            ],
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false,
                },
            },
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
                        text: "CTQ-Count",
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

//****************************** processRefresh Function  *************************************//

function createBarChart(data, chartID, chartOptions) {
    // Extract data for the chart
    var labels = data.map((entry) => entry[chartOptions.labelKey]);
    var values = data.map((entry) => entry[chartOptions.valueKey]);
    // Create a new Chart.js chart
    var ctx = document.getElementById(chartID).getContext("2d");
    var myChart = new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [
                {
                    label: chartOptions.label,
                    tension: 0.4,
                    borderWidth: 0,
                    borderRadius: 4,
                    borderSkipped: false,
                    backgroundColor: "rgba(255, 255, 255, .8)",
                    data: values,
                    maxBarThickness: 6,
                },
            ],
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false,
                },
                tooltip: {
                    callbacks: {
                        title: function (context) {
                            // Display the label as the tooltip title


                            return context[0].label;
                        },
                        label: function (context) {
                            // Display the ctqCount as the tooltip label
                            return "CTQ Count: " + context.formattedValue;
                        },
                    },
                },
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
                        text: "CTQ-Count",
                        color: "#fff",
                        font: {
                            size: 14,
                            weight: "bold",
                            lineHeight: 1.2,
                        },
                        padding: { left: 0, right: 0, bottom: 0 },
                    },
                    ticks: {
                        suggestedMin: 0,
                        suggestedMax: 500,
                        beginAtZero: true,
                        padding: 10,
                        font: {
                            size: 14,
                            weight: 300,
                            family: "Roboto",
                            style: "normal",
                            lineHeight: 2,
                        },
                        color: "#fff",
                    },
                },
                x: {
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
                        text: chartOptions.label,
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
                        //maxTicksLimit: 2,

                        maxRotation: 90,
                        minRotation: 90,
                        // padding: 10,
                        autoSkip: false,
                        fontSize: 10,

                        font: {
                            size: 12,
                            weight: 300,
                            family: "Roboto",
                            style: "normal",
                            // lineHeight: 2,

                        },
                    },
                },
            },
            onClick: function (evt) {
                if (chartOptions.onClick) {
                    const activePoints = myChart.getElementsAtEventForMode(
                        evt,
                        "nearest",
                        { intersect: true },
                        true
                    );
                    if (activePoints.length > 0) {
                        const index = activePoints[0].index;
                        chartOptions.onClick(labels[index]);
                    }
                }
            },
            // Other chart options here
            ...chartOptions.chartOptions,
        },
    });
}

//******************************  Function to create a bar chart for "model"  *************************************//

function createModelBarChart(data, UserData) {
    var chartOptions = {
        labelKey: "model",
        valueKey: "ctqCount",
        label: "Model",
        chartOptions: {
        },
        onClick: function (label) {
            var qudrant = quadarants(data[0].quadrant);

            var plant = UserData.role_id == 2 || UserData.role_id == 14 ? getQueryStringValue('plant') : data[0].plant


            var url = `../CharacteristicExplorer/index.html?model=${label}&quad=${qudrant}&plant=${plant}`;

            window.location.href = url;

        },
    };

    createBarChart(data, "chart-bars1", chartOptions);
}



//****************************** createComponentBarChart  Function   *************************************//

function createComponentBarChart(data, UserData) {
    var chartOptions = {
        labelKey: "component",
        valueKey: "ctqCount", // Adjust this key based on your data structure
        label: "Component",
        chartOptions: {
            // Customize other chart options here
        },
        onClick: function (label) {
            var qudrant = quadarants(data[0].quadrant);

            var plant = UserData.role_id == 2 || UserData.role_id == 14 ? getQueryStringValue('plant') : data[0].plant
            //var plant = getQueryStringValue('plant') ? getQueryStringValue('plant') : getPlant
            var url = `../CharacteristicExplorer/index.html?comp=${label}&quad=${qudrant}&plant=${plant}`;

            window.location.href = url;


        },
    };

    createBarChart(data, "chart-bars2", chartOptions);
}

//*************************  Function to create a bar chart for "area" ************************//
function createAreaBarChart(data, UserData) {
    var chartOptions = {
        labelKey: "area",
        valueKey: "ctqCount",
        label: "Area",
        chartOptions: {},
        onClick: function (label) {
            var qudrant = quadarants(data[0].quadrant);
            var plant = UserData.role_id == 2 || UserData.role_id == 14 ? getQueryStringValue('plant') : data[0].plant

            var url = `../CharacteristicExplorer/index.html?area=${label}&quad=${qudrant}&plant=${plant}`;

            window.location.href = url;

        },
    };

    createBarChart(data, "chart-bars", chartOptions);
}

//************************* BarChart Function  ************************//

function BarChart(data, chartID, mapVal) {

    var labels = data.map((entry) => entry.area);
    var ctqCounts = data.map((entry) => entry.ctqCount);

    // Create a new Chart.js chart using the extracted data
    var myChart = new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [
                {
                    label: "CTQ Count",
                    tension: 0.4,
                    borderWidth: 0,
                    borderRadius: 4,
                    borderSkipped: false,
                    backgroundColor: "rgba(255, 255, 255, .8)",
                    data: ctqCounts,
                    maxBarThickness: 6,
                },
            ],
        },
        // Rest of your chart options
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false,
                },
            },
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
                        text: "CTQ-Count",
                        color: "#fff",
                        font: {
                            size: 14,
                            weight: "bold",
                            lineHeight: 1.2,
                        },
                        padding: { left: 0, right: 0, bottom: 0 },
                    },
                    ticks: {
                        suggestedMin: 0,
                        suggestedMax: 500,
                        beginAtZero: true,
                        padding: 10,
                        font: {
                            size: 14,
                            weight: 300,
                            family: "Roboto",
                            style: "normal",
                            lineHeight: 2,
                        },
                        color: "#fff",
                    },
                },
                x: {
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
                        text: "Area",
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
                        maxRotation: 100,
                        minRotation: 10,
                        padding: 10,
                        autoSkip: false,
                        fontSize: 10,
                        // padding: 10,
                        font: {
                            size: 12,
                            weight: 300,
                            family: "Roboto",
                            style: "normal",
                            // lineHeight: 2,
                        },
                    },
                },
            },
            onClick: (evt) => {
                const res = mychart.getElementsAtEventForMode(
                    evt,
                    "nearest",
                    { intersect: true },
                    true
                );
                // If didn't click on a bar, `res` will be an empty array
                if (res.length === 0) {
                    return;
                }
                // $("").val(mychart.data.labels[res[0].index]).change();
                $("#Area_id option")
                    .filter(function () {
                        //may want to use $.trim in here
                        return $(this).text() == mychart.data.labels[res[0].index];
                    })
                    .prop("selected", true);
                // Alerts "You clicked on A" if you click the "A" chart
                alert("You clicked on " + mychart.data.labels[res[0].index]);
            },
        },
    });
}
