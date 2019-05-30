#pragma checksum "C:\Users\GIGABYTE\source\repos\ADGRParser\ADGRVizualizer\Views\SingleCluster\ProfitClusterCount.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ec404123ade3581a3d915d219bc50f9df7a54085"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SingleCluster_ProfitClusterCount), @"mvc.1.0.view", @"/Views/SingleCluster/ProfitClusterCount.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/SingleCluster/ProfitClusterCount.cshtml", typeof(AspNetCore.Views_SingleCluster_ProfitClusterCount))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\GIGABYTE\source\repos\ADGRParser\ADGRVizualizer\Views\_ViewImports.cshtml"
using ADGRVizualizer;

#line default
#line hidden
#line 2 "C:\Users\GIGABYTE\source\repos\ADGRParser\ADGRVizualizer\Views\_ViewImports.cshtml"
using ADGRVizualizer.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ec404123ade3581a3d915d219bc50f9df7a54085", @"/Views/SingleCluster/ProfitClusterCount.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c323128f0c16891ac0d69e9679c04cb807985f6a", @"/Views/_ViewImports.cshtml")]
    public class Views_SingleCluster_ProfitClusterCount : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<(List<SimpleReportViewModel>, List<SimpleReportViewModel>)>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\GIGABYTE\source\repos\ADGRParser\ADGRVizualizer\Views\SingleCluster\ProfitClusterCount.cshtml"
  
    var XLabels = Newtonsoft.Json.JsonConvert.SerializeObject(Model.Item2.Select(x => x.DimensionOne).ToList());
    var YValues = Newtonsoft.Json.JsonConvert.SerializeObject(Model.Item2.Select(x => x.Quantity).ToList());
    var XLabelsAbsolute = Newtonsoft.Json.JsonConvert.SerializeObject(Model.Item1.Select(x => x.DimensionOne).ToList());
    var YValuesAbsolute = Newtonsoft.Json.JsonConvert.SerializeObject(Model.Item1.Select(x => x.Quantity).ToList());
    ViewData["Title"] = "ProfitClusterCount";

#line default
#line hidden
            BeginContext(585, 845, true);
            WriteLiteral(@"
<h1>ProfitClusterCount</h1>
<div class=""box-body"">
    <div class=""chart-container"">
        <canvas id=""chart"" style=""width:1920px; height:900px""></canvas>
    </div>
</div>
<div class=""box-body"">
    <div class=""chart-container"">
        <canvas id=""chart1"" style=""width:1920px; height:900px""></canvas>
    </div>
</div>

<script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.2/Chart.bundle.min.js""></script>
<script src=""https://code.jquery.com/jquery-3.3.1.min.js""></script>

<script type=""text/javascript"">

        $(function () {
            var chartName = ""chart"";
            var chartName1 = ""chart1""
            var ctx = document.getElementById(chartName).getContext('2d');
            var ctx1 = document.getElementById(chartName1).getContext('2d');
        var data = {
                labels: ");
            EndContext();
            BeginContext(1431, 17, false);
#line 33 "C:\Users\GIGABYTE\source\repos\ADGRParser\ADGRVizualizer\Views\SingleCluster\ProfitClusterCount.cshtml"
                   Write(Html.Raw(XLabels));

#line default
#line hidden
            EndContext();
            BeginContext(1448, 1429, true);
            WriteLiteral(@",
                datasets: [{
                    label: ""Cluster Chart Relational"",
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)',
                        'rgba(255, 0, 0)',
                        'rgba(0, 255, 0)',
                        'rgba(0, 0, 255)',
                        'rgba(192, 192, 192)',
                        'rgba(255, 255, 0)',
                        'rgba(255, 0, 255)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
   ");
            WriteLiteral(@"                     'rgba(255, 159, 64, 1)',
                        'rgba(255, 0, 0)',
                        'rgba(0, 255, 0)',
                        'rgba(0, 0, 255)',
                        'rgba(192, 192, 192)',
                        'rgba(255, 255, 0)',
                        'rgba(255, 0, 255)'
                    ],
                    borderWidth: 1,
                    data: ");
            EndContext();
            BeginContext(2878, 17, false);
#line 65 "C:\Users\GIGABYTE\source\repos\ADGRParser\ADGRVizualizer\Views\SingleCluster\ProfitClusterCount.cshtml"
                     Write(Html.Raw(YValues));

#line default
#line hidden
            EndContext();
            BeginContext(2895, 85, true);
            WriteLiteral("\r\n    }]\r\n            };\r\n                    var data1 = {\r\n                labels: ");
            EndContext();
            BeginContext(2981, 25, false);
#line 69 "C:\Users\GIGABYTE\source\repos\ADGRParser\ADGRVizualizer\Views\SingleCluster\ProfitClusterCount.cshtml"
                   Write(Html.Raw(XLabelsAbsolute));

#line default
#line hidden
            EndContext();
            BeginContext(3006, 1328, true);
            WriteLiteral(@",
                datasets: [{
                    label: ""Cluster Chart Absolute"",
                    backgroundColor: [
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)',
                        'rgba(255, 0, 0)',
                        'rgba(0, 255, 0)',
                        'rgba(0, 0, 255)',
                        'rgba(192, 192, 192)',
                        'rgba(255, 255, 0)',
                        'rgba(255, 0, 255)'
                    ],
                    borderColor: [
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)',
                        'rgba(255, 0, 0)',
          ");
            WriteLiteral(@"              'rgba(0, 255, 0)',
                        'rgba(0, 0, 255)',
                        'rgba(192, 192, 192)',
                        'rgba(255, 255, 0)',
                        'rgba(255, 0, 255)'
                    ],
                    borderWidth: 1,
                    data: ");
            EndContext();
            BeginContext(4335, 25, false);
#line 99 "C:\Users\GIGABYTE\source\repos\ADGRParser\ADGRVizualizer\Views\SingleCluster\ProfitClusterCount.cshtml"
                     Write(Html.Raw(YValuesAbsolute));

#line default
#line hidden
            EndContext();
            BeginContext(4360, 1079, true);
            WriteLiteral(@"
    }]
            };

var options = {
                maintainAspectRatio: false,
                scales: {
                    yAxes: [{
                        ticks: {
                            min: 0,
                            beginAtZero: true
                        },
                        gridLines: {
                            display: true,
                            color: ""rgba(255,99,164,0.2)""
                        }
}],
                    xAxes: [{
                        ticks: {
                            min: 0,
                            beginAtZero: true
                        },
                        gridLines: {
                            display: false
                        }
                    }]
                }
            };

       var myChart = new  Chart(ctx, {
                options: options,
                data: data,
                type:'bar'

       });
            var myChart1 = new Chart(ctx1, { options: options, d");
            WriteLiteral("ata: data1, type: \'bar\' });\r\n        });\r\n\r\n</script>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<(List<SimpleReportViewModel>, List<SimpleReportViewModel>)> Html { get; private set; }
    }
}
#pragma warning restore 1591
